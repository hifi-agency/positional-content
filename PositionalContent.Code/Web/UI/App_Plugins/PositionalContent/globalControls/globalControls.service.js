 /*Copyright © 2018 HiFi Ltd - All Rights Reserved
 * This file is part of Positional Content which is released under the HiFi Positional Content License.
 * Please visit : http://www.positionalcontent.com/PositionalContentLicenseAgreement.pdf for full license details
 */

angular.module('umbraco').factory('HiFi.PositionalContent.GlobalControlsService', [
    'HiFi.PositionalContent.PremiumServices',
    function (premium) {

        return {
            clearAll: function (modelValue) {
                modelValue.items = [];
                modelValue.breakpoints = {};
                modelValue.primaryImage = undefined;
                modelValue.content = '';
                modelValue.settings = '';
                modelValue.primaryImage = undefined;
                modelValue.active = undefined;
                if (premium.isPremium()) {
                    premium.globalControlsService.calcZoomPercentages(modelValue, state);
                }
            }
        };
    }]);