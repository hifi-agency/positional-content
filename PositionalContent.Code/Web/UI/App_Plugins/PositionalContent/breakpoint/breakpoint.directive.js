 /*Copyright © 2018 HiFi Ltd - All Rights Reserved
 * This file is part of Positional Content which is released under the HiFi Positional Content License.
 * Please visit : http://www.positionalcontent.com/PositionalContentLicenseAgreement.pdf for full license details
 */

angular.module("umbraco").directive('positionalContentBreakpoints', [
    '$rootScope',
    '$timeout',
    'HiFi.PositionalContent.ImageService',
    'HiFi.PositionalContent.BreakpointService',
    'HiFi.PositionalContent.CropperService',
    'HiFi.PositionalContent.ItemService',
    'HiFi.PositionalContent.ScaleService',
    'HiFi.PositionalContent.PremiumServices',
    function ($rootScope, $timeout, imageService, breakpointService, cropperService, itemService, scaleService, premium) {

    return {
        restrict: 'E',
        replace: false,
        templateUrl: function () {
            if (premium.isPremium())
                return '../App_Plugins/PositionalContentPremium/breakpoint/breakpoint.html';
            else
                return '../App_Plugins/PositionalContent/breakpoint/breakpoint.html';
        },
        link: function (scope, element, attr) {

            scope.image = imageService;
            scope.breakpoint = breakpointService;
            scope.cropper = cropperService;
            scope.item = itemService;
            scope.scale = scaleService;
            scope.premium = premium;

            scope.breakpointsAsArray = function () {
                return breakpointService.asArray(scope.modelValue.breakpoints);
            }

            scope.hasBreakpoints = function () {
                return scope.breakpointsAsArray().length > 1;
            }

            if (!scope.tabs && scope.state)
            {
                scope.onImageLoaded = function () {
                    element.find('.positional-content__image-loader').hide();
                    element.find('.positional-content__image-wrapper').css('display', 'inline-block');
                    element.find('.positional-content__image-footer').show();
                    if (scope.state.active) {
                        var activeBeakpoint = breakpointService.getActiveFromModel(scope.modelValue, scope.state.active);
                        breakpointService.setActive(activeBeakpoint, scope.state);
                        itemService.renderAll(scope.modelValue, scope.state);
                        if (premium.isPremium()) {
                            premium.breakpointService.calcZoomPercentages(scope.modelValue, scope.state);
                        }
                    }
                }

                $rootScope.$on('HiFi.PositionalContent.BreakpointChanged.' + scope.state.id, function () {
                    $timeout(function () {
                        scaleService.setMaxScaleOnSlider(scope.state);
                        cropperService.setCropBorderWidth(scope.state);
                        itemService.renderAll(scope.modelValue, scope.state);
                    }.bind(this));
                });

                $rootScope.$on('HiFi.PositionalContent.PrimaryImageSet.' + scope.state.id, function () {
                    breakpointService.setupBreakpointsFromConfig(scope.modelValue, scope.config, scope.state);
                });
            }


        },
        scope: {
            state: '=',
            modelValue: '=',
            config: '=',
            property: '=',
            tabs: '='
        }
    };
}]);


