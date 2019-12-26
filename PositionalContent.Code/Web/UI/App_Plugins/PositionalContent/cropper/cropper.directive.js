 /*Copyright © 2018 HiFi Ltd - All Rights Reserved
 * This file is part of Positional Content which is released under the HiFi Positional Content License.
 * Please visit : http://www.positionalcontent.com/PositionalContentLicenseAgreement.pdf for full license details
 */

angular.module("umbraco").directive('positionalContentCropper', [
    '$document',
    '$rootScope',
    '$timeout',
    'HiFi.PositionalContent.CropperService',
    'HiFi.PositionalContent.ScaleService',
    'HiFi.PositionalContent.PremiumServices',
    function ($document, $rootScope, $timeout, cropperService, scaleService, premium) {


    return {
        restrict: 'E',
        replace: false,
        templateUrl: function () {
            if (premium.isPremium())
                return '../App_Plugins/PositionalContentPremium/cropper/cropper.html';
            else
                return '../App_Plugins/PositionalContent/cropper/cropper.html';
        },
        link: function (scope, element, attr) {

            scope.scale = scaleService;
            scope.cropper = cropperService;
            scope.premium = premium;

            element.addClass('positional-content__block');

            scope.saveCropPosition = function () {
                return cropperService.saveCropPosition(scope.state);
            }

            scope.onDrag = function () {
                cropperService.setCropBorderWidth(scope.state);
            }

        },
        scope: {
            breakpoint: '=',
            property: '=',
            state: '=',
            modelValue: '='
        }
    };
}]);


