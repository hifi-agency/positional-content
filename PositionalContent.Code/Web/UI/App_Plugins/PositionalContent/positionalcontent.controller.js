/*Copyright © 2018 HiFi Ltd - All Rights Reserved
 * This file is part of Positional Content which is released under the HiFi Positional Content License.
 * Please visit : http://www.positionalcontent.com/PositionalContentLicenseAgreement.pdf for full license details
 */

var app = angular.module("umbraco");

angular.module("umbraco").controller("HiFi.PositionalContent.Controller", [
    '$scope',
    '$rootScope',
    '$q',
    '$timeout',
    '$element',
    'dialogService',
    'umbPropEditorHelper',
    'appState',
    'editorState',
    'HiFi.PositionalContent.Resources',
    'HiFi.PositionalContent.CropperService',
    'HiFi.PositionalContent.ItemService',
    'HiFi.PositionalContent.BreakpointService',
    'HiFi.PositionalContent.ImageService',
    'HiFi.PositionalContent.ScaleService',
    function ($scope, $rootScope, $q, $timeout, $element, dialogService, umbPropEditorHelper, appState, editorState, resources, cropperService, itemService, breakpointService, imageService, scaleService) {

        if (!$scope.model.value)
            $scope.model.value = {};

        $scope.cropper = cropperService;
        $scope.item = itemService;
        $scope.breakpoint = breakpointService;
        $scope.image = imageService;
        $scope.scale = scaleService;

        $scope.model.hideLabel = $scope.model.config.hideLabel == 1;
        $scope.model.value.cssNamespace = $scope.model.config.cssNamespace;

        $scope.sortableOptions = {
            axis: 'x',
            cursor: "move"
        };

        $scope.state = {};
        $scope.state.element = $element;
        $scope.state.id = Date.now();
        $scope.state.breakpoints = {};
        $scope.state.previewModifierClass = $scope.model.config.previewModifierClass;
        angular.forEach($scope.model.value.breakpoints, function (b) {
            $scope.state.breakpoints[b.name] = {};
        });

        breakpointService.setup($scope.model.config, $scope.model.value, $element, $scope.state);
        imageService.setup($scope.model.value, $scope.state);


        //setup for property editor
        var currentSection = appState.getSectionState("currentSection");
        var parentScope = $scope;
        var nodeContext = undefined;
        while (!nodeContext && parentScope.$id !== $rootScope.$id) {
            parentScope = parentScope.$parent;
            nodeContext = parentScope.nodeContext;
        }
        if (!nodeContext) {
            nodeContext = editorState.current;
        }

        if (!$scope.property) {
            $scope.property = {};
        }

        $scope.model.value.dtdGuid = '00000000-0000-0000-0000-000000000000';
        //specific to le blender / grids
        if ($scope.property.dataTypeGuid)
            $scope.model.value.dtdGuid = $scope.property.dataTypeGuid;

        var setupContentConfig = function (contentDataType, config, property) {

            if (config[contentDataType] && config[contentDataType].guid != '00000000-0000-0000-0000-000000000000')
            {
                resources.getDataTypeById(config[contentDataType].guid).then(function (dataType) {

                    property[contentDataType] = {};
                    property[contentDataType].config = dataType.preValues;
                    property[contentDataType].viewPath = umbPropEditorHelper.getViewPath(dataType.view);

                });
            }
        }

        setupContentConfig('itemContentDataType', $scope.model.config, $scope.property);
        setupContentConfig('itemSettingsDataType', $scope.model.config, $scope.property);
        setupContentConfig('imageContentDataType', $scope.model.config, $scope.property);
        setupContentConfig('imageSettingsDataType', $scope.model.config, $scope.property);

        var propAlias = $scope.model.propertyAlias || $scope.model.alias;

        resources.getDataTypeByAlias(currentSection, nodeContext.contentTypeAlias, propAlias)
            .then(function (dataType2) {
                if (dataType2.guid)
                    $scope.model.value.dtdGuid = dataType2.guid;
            })
            .then(function () {
                //has to happen after we finish getting the dtd guid
                itemService.renderAll($scope.model.value, $scope.state);
            });

        var expandModel = function () {
            var model = $element.parents('.umb-modal');
            if (model.length > 0 && !$(model)[0].style['width']) {
                $(model)[0].style.setProperty('width', '50%', 'important');
                $(model)[0].style.setProperty('margin-left', '-50%');
            }

            //var panel = $element.parents('.umb-panel-body.with-footer');
            //if (panel.length > 0) {
            //    $(panel)[0].style.setProperty('overflow', 'hidden');
            //}
        }
        expandModel();

    }]);