 /*Copyright © 2018 HiFi Ltd - All Rights Reserved
 * This file is part of Positional Content which is released under the HiFi Positional Content License.
 * Please visit : http://www.positionalcontent.com/PositionalContentLicenseAgreement.pdf for full license details
 */

angular.module("umbraco").directive('positionalContentGlobalControls', [
    '$timeout',
    'HiFi.PositionalContent.GlobalControlsService',
    'HiFi.PositionalContent.ItemService',
    'HiFi.PositionalContent.PremiumServices',
    function ($timeout, globalControlsService, itemService, premium) {

    return {
        restrict: 'E',
        replace: false,
        templateUrl: function () {
            if (premium.isPremium())
                return '../App_Plugins/PositionalContentPremium/globalControls/globalControls.html';
            else
                return '../App_Plugins/PositionalContent/globalControls/globalControls.html';
        },
        link: function (scope, element, attr) {

            scope.item = itemService;
            scope.premium = premium;

            var menuItemTimeout;
            scope.openMenuItem = function (menuItem, delay) {
                if (!delay) delay = 0;
                menuItemTimeout = $timeout(function () {
                    scope.menuItem = menuItem;
                }, delay)
            }

            scope.cancelMenuItemTimeout = function () {
                $timeout.cancel(menuItemTimeout);
            }

            scope.clearAll = function () {

                if (window.confirm("Are you sure?")) {
                    globalControlsService.clearAll(scope.modelValue);
                }
            }

        },
        scope: {
            state: '=',
            property: '=',
            config: '=',
            modelValue: '='
        }
    };
}]);


