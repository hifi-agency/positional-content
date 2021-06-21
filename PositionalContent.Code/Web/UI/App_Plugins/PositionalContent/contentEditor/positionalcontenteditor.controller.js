 /*Copyright © 2018 HiFi Ltd - All Rights Reserved
 * This file is part of Positional Content which is released under the HiFi Positional Content License.
 * Please visit : http://www.positionalcontent.com/PositionalContentLicenseAgreement.pdf for full license details
 */

angular.module("umbraco").controller("HiFi.PositionalContentDialogController", [
    '$scope',
    'dialogService',
    'HiFi.PositionalContent.ImageService',
    'HiFi.PositionalContent.BreakpointService',
    'HiFi.PositionalContent.CropperService',
    'HiFi.PositionalContent.ItemService',
    function ($scope, dialogService, imageService, breakpointService, cropperService, itemService) {

        $scope.tabs = [],

        //{ id: 1, label: "Content" }, { id: 2, label: "Settings" }, { id: 3, label: "Dimensions" }

        $scope.title = $scope.dialogData.title;

        var state = $scope.dialogData.state;

        $scope.content = {};
        $scope.settings = {};
        $scope.state = state;
        $scope.cropper = cropperService;
        $scope.breakpointContent = {};
        $scope.breakpointSettings = {};
        $scope.dimensionContent = {};
        $scope.dimensionSettings = {};

        var initDataForEditor = function (obj, content, property) {
            obj.value = content;
            obj.config = property.config;
            obj.propertyEditorView = property.viewPath;
        }

        if ($scope.dialogData.property) {

            $scope.tabs.push({ id: 1, label: "Content" });
            initDataForEditor($scope.content, $scope.dialogData.data.content, $scope.dialogData.property);

        }

        if ($scope.dialogData.settingsProperty) {

            $scope.tabs.push({ id: 2, label: "Settings" });
            initDataForEditor($scope.settings, $scope.dialogData.data.settings, $scope.dialogData.settingsProperty);

        }

        if ($scope.dialogData.data.breakpoints) {

            $scope.breakpoint = $scope.dialogData.data.breakpoints[state.active.name];
            $scope.originalValue = angular.copy($scope.breakpoint);

            if ($scope.dialogData.property) {
                initDataForEditor($scope.breakpointContent, $scope.breakpoint.content, $scope.dialogData.property);
            }

            if ($scope.dialogData.settingsProperty) {
                initDataForEditor($scope.breakpointSettings, $scope.breakpoint.settings, $scope.dialogData.settingsProperty);
            }
        }

        if ($scope.dialogData.data.dimensions) {

            $scope.dimension = $scope.dialogData.data.dimensions[state.active.name];
            $scope.originalValue = angular.copy($scope.dimension);

            if ($scope.dialogData.property) {
                initDataForEditor($scope.dimensionContent, $scope.dimension.content, $scope.dialogData.property);
            }

            if ($scope.dialogData.settingsProperty) {
                initDataForEditor($scope.dimensionSettings, $scope.dimension.settings, $scope.dialogData.settingsProperty);
            }
        }

        $scope.tabs.push({ id: 3, label: "Dimensions" });

        $scope.updateWidth = function (dimension, oldvalue) {
            var newValue = Number(dimension['width']);

            var leftValue = Number(dimension.left);

            var newRightValue = 100 - (leftValue + newValue);

            if (newRightValue >= 0) {
                dimension['right'] = newRightValue;
            }
            else {
                dimension['width'] = oldvalue;
            }
        }

        $scope.updateHeight = function (dimension, oldvalue) {
            var newValue = Number(dimension['height']);

            var topValue = Number(dimension.top);

            var newBottomValue = 100 - (topValue + newValue);

            if (newBottomValue >= 0) {
                dimension['bottom'] = newBottomValue;
            }
            else {
                dimension['height'] = oldvalue;
            }
        }

        $scope.updateDimensions = function (dimension, changing) {

            var toChange, changing, orientation;

            if (changing == 'top') {
                toChange = 'bottom';
                changing = 'top';
                orientation = 'height';
            }
            else if (changing == 'bottom') {
                toChange = 'top';
                changing = 'bottom';
                orientation = 'height';
            }
            else if (changing == 'left') {
                toChange = 'right';
                changing = 'left';
                orientation = 'width';
            }
            else if (changing == 'right') {
                toChange = 'left';
                changing = 'right';
                orientation = 'width';
            }

            var changingValue = Number(dimension[changing]);
            var orientationValue = Number(dimension[orientation]);
            var toChangeValue = 100 - (changingValue + orientationValue);

            if (toChangeValue <= 100 && toChangeValue >= 0) {
                dimension[toChange] = toChangeValue;
                cropperService.setCropBorderWidth(state);
                imageService.generateCropLink(state.active, state);
            }
            else {
                dimension[changing] = $scope.originalValue[changing];
                dimension[toChange] = $scope.originalValue[toChange];
            }

        }

        $scope.save = function () {
            $scope.$broadcast("formSubmitting");

            if ($scope.content.save)
                $scope.dialogData.data.content = $scope.content.save();
            if ($scope.settings.save)
                $scope.dialogData.data.settings = $scope.settings.save();

            if ($scope.breakpointContent.save)
                $scope.dialogData.data.breakpoints[state.active.name].content = $scope.breakpointContent.save();
            if ($scope.breakpointSettings.save)
                $scope.dialogData.data.breakpoints[state.active.name].settings = $scope.breakpointSettings.save();

            if ($scope.dimensionContent.save)
                $scope.dialogData.data.dimensions[state.active.name].content = $scope.dimensionContent.save();
            if ($scope.dimensionSettings.save)
                $scope.dialogData.data.dimensions[state.active.name].settings = $scope.dimensionSettings.save();

            itemService.renderAll($scope.dialogData.modelValue, $scope.state);

            $scope.close();
        };

    }]);

angular.module("umbraco").directive('positionalContentSettings', [function () {
    return {
        restrict: 'E',
        template: "<div ng-include='propertyEditorView'></div>",
        link: function (scope, element, attr) {

            scope.model = {};
            scope.model.config = angular.copy(scope.settings.config);
            scope.model.value = scope.settings.value;
            scope.propertyEditorView = scope.settings.propertyEditorView;

            scope.settings.save = function () {
                return scope.model.value;
            }

        },
        scope: {
            dimension: '=',
            settings: '='
        }
    }
}]);
