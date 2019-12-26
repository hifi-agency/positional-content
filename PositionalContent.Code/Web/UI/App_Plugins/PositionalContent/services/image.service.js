 /*Copyright © 2018 HiFi Ltd - All Rights Reserved
 * This file is part of Positional Content which is released under the HiFi Positional Content License.
 * Please visit : http://www.positionalcontent.com/PositionalContentLicenseAgreement.pdf for full license details
 */

angular.module('umbraco').factory('HiFi.PositionalContent.ImageService', [
    '$rootScope',
    '$window',
    'mediaResource',
    'mediaHelper',
    'editorService',
    'HiFi.PositionalContent.UtilService',
    function ($rootScope, $window, mediaResource, mediaHelper, editorService, util) {

        return {
            element: function (state) {
                return state.element.find('.positional-content__breakpoint--active .positional-content__image');
            },
            displayWidth: function (state) {
                return util.pxToInt(this.element(state).css('width'));
            },
            displayHeight: function (state) {
                return util.pxToInt(this.element(state).css('height'));
            },
            setup: function (value, state) {
                if (value.primaryImage) {
                    state.primaryImage = {};
                    mediaResource.getById(value.primaryImage.imageId)
                        .then(function (media) {
                            this.setupPrimaryImageUrl(media, value, state);
                        }.bind(this));                }
            },
            setupBreakpointImages: function (breakpoints, state) {
                angular.forEach(breakpoints, function (breakpoint) {
                    if (breakpoint.imageId) {
                        mediaResource.getById(breakpoint.imageId).then(function (media)
                        {
                            this.setupBreakpointImageUrl(media, breakpoint, state);
                        }.bind(this));
                    }
                    else
                        this.generateCropLink(breakpoint, state);
                }.bind(this));
            },
            setupPrimaryImageUrl: function (media, value, state) {
                if (media.trashed)
                    $window.alert("WARNING: The image is in the recycle bin and won't be available on the front end");
                state.primaryImage.imageUrl = mediaHelper.resolveFile(media);
                this.setupBreakpointImages(value.breakpoints, state);
            },
            setupBreakpointImageUrl: function (media, breakpoint, state) {
                if (media.trashed)
                    $window.alert("WARNING: The image is in the recycle bin and won't be available on the front end");
                breakpoint.imageUrl = mediaHelper.resolveFile(media);
                this.generateCropLink(breakpoint, state);
            },
            openMediaPicker: function (modelValue, state, isNew) {
                var callback = function (editor) {
                    this.setPrimaryImage(editor, modelValue, state, isNew);
                }.bind(this);
                editorService.mediaPicker({
                    multiPicker: false,
                    onlyImages: true,
                    submit: callback,
                    close: function () {
                        editorService.close();
                    }
                });
            },
            setPrimaryImage: function (editor, modelValue, state, isNew) {
                var image = editor.selection[0];
                mediaResource.getById(image.id).then(function (mediaItem) {

                    var originalWidth = Number(_.find(mediaItem.tabs[0].properties, function (item) { return item.label === 'Width'; }).value);
                    var originalHeight = Number(_.find(mediaItem.tabs[0].properties, function (item) { return item.label === 'Height'; }).value);

                    if (originalWidth >= state.minimumImageSize.width && originalHeight >= state.minimumImageSize.height) {

                        modelValue.primaryImage = {};
                        modelValue.primaryImage.width = originalWidth;
                        modelValue.primaryImage.height = originalHeight;
                        modelValue.primaryImage.imageId = image.id;

                        state.primaryImage = {};
                        state.primaryImage.imageUrl = mediaItem.mediaLink;

                        modelValue.editorZoom = 70;
                        if (isNew)
                            $rootScope.$broadcast('HiFi.PositionalContent.PrimaryImageSet.' + state.id);

                        editor.close();
                    }
                    else
                        alert('Minimum image size not met, this image is ' + originalWidth + 'px * ' + originalHeight + 'px');

                });
            },
            getPreviewImage: function (breakpoint, state, config) {
                return this.getForBreakpoint(breakpoint, state) + '?quality=' + config.previewImageQuality
            },
            getForBreakpoint: function (breakpoint, state) {
                if (state.breakpoints[breakpoint.name].imageUrl)
                    return state.breakpoints[breakpoint.name].imageUrl;
                else
                    return state.primaryImage.imageUrl;
            },
            edit: function (image, dataProperty, settingsProperty, state) {
                editorService.open({
                    view: '../App_Plugins/PositionalContent/contenteditor/positionalcontenteditor.breakpoint.html',
                    dialogData: {
                        title: 'Image content',
                        state: state,
                        data: image,
                        property: dataProperty,
                        settingsProperty: settingsProperty
                    },
                    size: 'large',
                    submit: function (data) {
                        image = data.data;
                        editorService.close();
                    },
                    close: function () {
                        editorService.close();
                    }
                });
            },
            generateCropLink: function (b, state) {
                var left = b.left < 1 ? b.left : 0;
                state.breakpoints[b.name].cropLink = this.getForBreakpoint(b, state) + '?crop=' + b.left + ',' + b.top + ',' + b.right + ',' + b.bottom + '&cropmode=percentage&width=' + b.cropWidth + '&height=' + b.cropHeight + '&mode=crop';
            },
            getMinimumImageSize: function (config) {
                var widthBreakpoint = _.max(config.breakpoints, function (item) { return Number(item.cropWidth) });
                var heightBreakpoint = _.max(config.breakpoints, function (item) { return Number(item.cropHeight) });

                return {
                    width: Number(widthBreakpoint.cropWidth) * config.imageSizeMultiplier,
                    height: Number(heightBreakpoint.cropHeight) * config.imageSizeMultiplier
                }
            },
        };
    }]);