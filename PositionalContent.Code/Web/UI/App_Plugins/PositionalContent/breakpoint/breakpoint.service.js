 /*Copyright © 2018 HiFi Ltd - All Rights Reserved
 * This file is part of Positional Content which is released under the HiFi Positional Content License.
 * Please visit : http://www.positionalcontent.com/PositionalContentLicenseAgreement.pdf for full license details
 */

angular.module('umbraco').factory('HiFi.PositionalContent.BreakpointService', [
    '$rootScope',
    '$window',
    'mediaResource',
    'mediaHelper',
    'dialogService',
    'HiFi.PositionalContent.ImageService',
    'HiFi.PositionalContent.UtilService',
    'HiFi.PositionalContent.PremiumServices',
    function ($rootScope, $window, mediaResource, mediaHelper, dialogService, imageService, util, premium) {

        return {
            element: function (state) {
                return state.element.find('.positional-content__breakpoint--active');
            },
            displayWidth: function (state) {
                return util.pxToInt(this.element(state).css('width'));
            },
            asArray: function (items) {
                var result = [];
                angular.forEach(items, function (value, key) {
                    result.push(value);
                });
                return result;
            },
            setup: function (config, value, element, state) {
                if (!value.breakpoints) {
                    value.breakpoints = {};
                }
                else if (!_.isEmpty(value.breakpoints)) {
                    var key = Object.keys(value.breakpoints)[0];
                    angular.forEach(config.breakpoints.items, function (item) {
                        value.breakpoints[item.name].cropWidth = item.cropWidth;
                        value.breakpoints[item.name].cropHeight = item.cropHeight;
                        value.breakpoints[item.name].breakpointUpper = item.breakpointUpper; 
                        value.breakpoints[item.name].breakpointLower = item.breakpointLower;
                    })
                    this.setActive(value.breakpoints[key], state);
                    this.setUpOverrideImages(this.asArray(value.breakpoints), state);
                }
                state.minimumImageSize = imageService.getMinimumImageSize(config);
            },
            setupBreakpointsFromConfig: function (value, config, state) {

                angular.forEach(config.breakpoints.items, function (item, index) {
                    var breakpoint = {
                        name: item.name,
                        cropWidth: item.cropWidth,
                        cropHeight: item.cropHeight,
                        breakpointUpper: item.breakpointUpper,
                        breakpointLower: item.breakpointLower,
                        left: 0,
                        right: util.percentageLeft(item.cropWidth, value.primaryImage.width),
                        top: 0,
                        bottom: util.percentageLeft(item.cropHeight, value.primaryImage.height),
                        scale: 1
                    };
                    value.breakpoints[item.name] = breakpoint;
                    if (index == 0)
                        this.setActive(breakpoint, state);

                    state.breakpoints[item.name] = {};
                }, this);
            },
            
            setActive: function (breakpoint, state) {
                state.active = breakpoint;
                $rootScope.$broadcast('HiFi.PositionalContent.BreakpointChanged.' + state.id);
            },
            getActiveFromModel: function (value, active) {
                return value.breakpoints[active.name];
            },
            sizeLabel: function (current, index) {
                if (!current.breakpointUpper || !current.breakpointLower)
                    return '';
                if (index == 0)
                    return '(above ' + current.breakpointLower + ')';
                else
                    return '(' + current.breakpointUpper + ' to ' + current.breakpointLower + ')';
            },
            setUpOverrideImages: function (breakpoints, state) {
                angular.forEach(breakpoints, function (b) {
                    if (b.imageId) {
                        mediaResource.getById(b.imageId)
                            .then(function (media) {
                                if (media.trashed)
                                    $window.alert("WARNING: The image is in the recycle bin and won't be available on the front end");
                                state.breakpoints[b.name].imageUrl = mediaHelper.resolveFile(media);
                                imageService.generateCropLink(state.active, state);
                            });
                    }
                });
            },
            openMediaPicker: function (breakpoint, modelValue, state) {
                var callback = function (image) {
                    this.setOverrideImage(image, breakpoint, modelValue, state);
                }.bind(this);
                dialogService.mediaPicker({ callback: callback });
            },
            setOverrideImage: function (image, breakpoint, modelValue, state) {
                mediaResource.getById(image.id).then(function (mediaItem) {

                    var originalWidth = _.find(mediaItem.properties, function (item) { return item.label === 'Width'; }).value;
                    var originalHeight = _.find(mediaItem.properties, function (item) { return item.label === 'Height'; }).value;

                    if (originalWidth >= state.minimumImageSize.width && originalHeight >= state.minimumImageSize.height) {
                        state.breakpoints[breakpoint.name].imageUrl = image.image;
                        breakpoint.imageId = image.id;
                        modelValue.editorZoom = 70;
                        if (premium.isPremium()) {
                            premium.breakpointService.calcZoomPercentages(modelValue, state);
                        }
                    }
                    else
                        alert('Minimum image size not met, this image is ' + image.originalWidth + 'px * ' + image.originalHeight + 'px');
                });
            },
            removeImage: function (b, state) {
                state.breakpoints[state.active.name].imageUrl = undefined;
                b.imageId = undefined;
                b.cropLink = undefined;
            }
        };
    }]);