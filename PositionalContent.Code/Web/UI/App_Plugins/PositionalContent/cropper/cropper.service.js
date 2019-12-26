 /*Copyright © 2018 HiFi Ltd - All Rights Reserved
 * This file is part of Positional Content which is released under the HiFi Positional Content License.
 * Please visit : http://www.positionalcontent.com/PositionalContentLicenseAgreement.pdf for full license details
 */

angular.module('umbraco').factory('HiFi.PositionalContent.CropperService', [
    '$timeout',
    'HiFi.PositionalContent.ImageService',
    'HiFi.PositionalContent.ScaleService',
    'HiFi.PositionalContent.UtilService',
    function ($timeout, imageService, scaleService, util) {

        return {
            element: function (state) {
                return state.element.find('.positional-content__breakpoint--active .positional-content__cropper');
            },
            isActive: function (modelValue, b) {
                return !(modelValue.primaryImage.height == b.cropHeight && modelValue.primaryImage.width == b.cropWidth);
            },
            width: function (state, modelValue) {
                scaleService.set(state, modelValue);
                var pixels = state.active.cropWidth * scaleService.breakpointValue(state);
                return util.percentageOf(pixels, imageService.displayWidth(state))
            },
            height: function (state, modelValue) {
                scaleService.set(state, modelValue);
                var pixels = state.active.cropHeight * scaleService.breakpointValue(state);
                return util.percentageOf(pixels, imageService.displayHeight(state))
            },
            topBorder: function (state) {
                return state.element.find('.positional-content__breakpoint--active .positional-content__cropper-top-border');
            },
            bottomBorder: function (state) {
                return state.element.find('.positional-content__breakpoint--active .positional-content__cropper-bottom-border');
            },
            leftBorder: function (state) {
                return state.element.find('.positional-content__breakpoint--active .positional-content__cropper-left-border');
            },
            rightBorder: function (state) {
                return state.element.find('.positional-content__breakpoint--active .positional-content__cropper-right-border');
            },
            maximise: function (state) {
                state.active.left = 0;
                state.active.top = 0;
                state.active.scale = scaleService.max(state);

                $timeout(function () {
                    this.saveCropPosition(state);
                }.bind(this));
            },
            saveCropPosition: function (state) {

                var imageHeight = imageService.displayHeight(state);
                var imageWidth = imageService.displayWidth(state);

                var positionAndSize = util.getPositionAndSize(this.element(state));

                state.active.left = util.percentageOf(positionAndSize.left, imageWidth, true);
                state.active.right = util.percentageOf(positionAndSize.right, imageWidth, true);
                state.active.top = util.percentageOf(positionAndSize.top, imageHeight, true);
                state.active.bottom = util.percentageOf(positionAndSize.bottom, imageHeight, true);

                state.active.width = util.percentageOf(positionAndSize.width, imageWidth);
                state.active.height = util.percentageOf(positionAndSize.height, imageHeight);

                this.setCropBorderWidth(state);
                imageService.generateCropLink(state.active, state);

            },
            setCropBorderWidth: function (state) {

                $timeout(function () {
                    var imageHeight = imageService.displayHeight(state);
                    var imageWidth = imageService.displayWidth(state);

                    var positionAndSize = util.getPositionAndSize(this.element(state));

                    var leftBorderWidth = positionAndSize.left;
                    var rightBorderWidth = (imageWidth - positionAndSize.width) - positionAndSize.left;

                    var topBorderHeight = positionAndSize.top;
                    var bottomBorderHeight = (imageHeight - positionAndSize.height) - positionAndSize.top;

                    //can't use angular scope here as digest not quick enough for updates on cropper borders
                    this.rightBorder(state).width(util.percentageOf(rightBorderWidth, imageWidth) +'%');
                    this.leftBorder(state).css('width', util.percentageOf(leftBorderWidth, imageWidth) + '%');
                    this.topBorder(state).css('height', util.percentageOf(topBorderHeight, imageHeight) + '%');
                    this.bottomBorder(state).css('height', util.percentageOf(bottomBorderHeight, imageHeight) + '%');
                }.bind(this));

            },
            validateScaleChange: function (state, breakpoint, oldValue) {

                var lastChar = breakpoint.scale.slice(-1);
                if (lastChar != '.' && breakpoint.scale != '') {
                    var newValue = Number(breakpoint.scale);
                    var maxScale = scaleService.max(state);
                    if (!isNaN(newValue) && newValue >= 1 && newValue <= maxScale) {
                        this.setCropBorderWidth(state);
                        $timeout(function () {
                            this.saveCropPosition(state);
                        }.bind(this));
                    }
                    else
                        breakpoint.scale = oldValue;
                }
            }
        };
    }]);