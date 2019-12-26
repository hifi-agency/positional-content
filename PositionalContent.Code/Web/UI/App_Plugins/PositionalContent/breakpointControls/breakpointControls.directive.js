 /*Copyright © 2018 HiFi Ltd - All Rights Reserved
 * This file is part of Positional Content which is released under the HiFi Positional Content License.
 * Please visit : http://www.positionalcontent.com/PositionalContentLicenseAgreement.pdf for full license details
 */

angular.module("umbraco").directive('positionalContentBreakpointControls', [
    '$rootScope',
    '$timeout',
    'HiFi.PositionalContent.ImageService',
    'HiFi.PositionalContent.CropperService',
    'HiFi.PositionalContent.ItemService',
    'HiFi.PositionalContent.ScaleService',
    'HiFi.PositionalContent.BreakpointService',
    'HiFi.PositionalContent.PremiumServices',
    function ($rootScope, $timeout, imageService, cropperService, itemService, scaleService, breakpointService, premium) {

    return {
        restrict: 'E',
        replace: false,
        templateUrl: function () {
            if (premium.isPremium())
                return '../App_Plugins/PositionalContentPremium/breakpointControls/breakpointControls.html';
            else
                return '../App_Plugins/PositionalContent/breakpointControls/breakpointControls.html';
        },
        link: function (scope, element, attr) {

            scope.image = imageService;
            scope.cropper = cropperService;
            scope.item = itemService;
            scope.scale = scaleService;
            scope.breakpointService = breakpointService;
            scope.premium = premium;

        },
        scope: {
            breakpoint: '=',
            state: '=',
            modelValue: '=',
            config: '=',
            property: '='
        }
    };
}]);


