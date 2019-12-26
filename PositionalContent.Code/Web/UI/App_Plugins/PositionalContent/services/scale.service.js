 /*Copyright © 2018 HiFi Ltd - All Rights Reserved
 * This file is part of Positional Content which is released under the HiFi Positional Content License.
 * Please visit : http://www.positionalcontent.com/PositionalContentLicenseAgreement.pdf for full license details
 */

angular.module('umbraco').factory('HiFi.PositionalContent.ScaleService', [
    'HiFi.PositionalContent.ImageService',
    'HiFi.PositionalContent.UtilService',
    function (imageService, util) {

        return {
            element: function (state) {
                return state.element.find('.positional-content__breakpoint--active input[type=range]');
            },
            breakpointValue: function (state) {
                if (state.scale)
                    return state.scale * state.active.scale
                else
                    return 1;
            },
            inverse: function (state) {
                return 1 / this.breakpointValue(state);
            },
            set: function (state, modelValue) {
                var displayWidth = imageService.displayWidth(state);
                if (modelValue.primaryImage.width > displayWidth) {
                    state.scale = util.percentageOf(displayWidth, modelValue.primaryImage.width) / 100;
                }
                else
                    state.scale = 1;
            },
            max: function (state) {
                var maxHeightScale = imageService.displayHeight(state) / (state.active.cropHeight * state.scale);
                var maxWidthScale = imageService.displayWidth(state) / (state.active.cropWidth * state.scale);

                if (maxHeightScale < maxWidthScale)
                    return maxHeightScale.toFixed(2);
                else
                    return maxWidthScale.toFixed(2);
            },
            setMaxScaleOnSlider: function (state) {
                //we have to set this via jquery as max doesn't work as an angular expression
                this.element(state).prop('max', this.max(state));
            }
        };
    }]);