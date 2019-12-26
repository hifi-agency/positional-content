 /*Copyright © 2018 HiFi Ltd - All Rights Reserved
 * This file is part of Positional Content which is released under the HiFi Positional Content License.
 * Please visit : http://www.positionalcontent.com/PositionalContentLicenseAgreement.pdf for full license details
 */

angular.module("umbraco").directive('positionalContentItems', ['$timeout', 'HiFi.PositionalContent.CropperService', 'HiFi.PositionalContent.ItemService', 'HiFi.PositionalContent.ScaleService', function ($timeout, cropperService, itemService, scaleService) {


    return {
        restrict: 'E',
        replace: false,
        templateUrl: '../App_Plugins/PositionalContent/item/item.html',
        link: function (scope, element, attr) {

            scope.scale = scaleService;
            scope.item = itemService;
            scope.cropper = cropperService;

            element.addClass('positional-content__block');

            scope.saveItem = function (element) {
                itemService.save(element, scope.modelValue.items, scope.constrainTo, scope.state);
            }

            scope.dimension = function (item) {
                return item.dimensions[scope.state.active.name];
            }

            scope.inlineEditorPosition = function(dimension, state)
            {
                return dimension.pinToRight ? 'margin-right: calc(-22px * ' + scaleService.inverse(state) + ');' : 'margin-left: calc(-22px * ' + scaleService.inverse(state) + ');';
            }

        },
        scope: {
            property: '=',
            state: '=',
            modelValue: '=',
            constrainTo: '=',
            readOnly: '='
        }
    };
}]);


