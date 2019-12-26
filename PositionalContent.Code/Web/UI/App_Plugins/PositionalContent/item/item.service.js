 /*Copyright © 2018 HiFi Ltd - All Rights Reserved
 * This file is part of Positional Content which is released under the HiFi Positional Content License.
 * Please visit : http://www.positionalcontent.com/PositionalContentLicenseAgreement.pdf for full license details
 */

angular.module('umbraco').factory('HiFi.PositionalContent.ItemService', [
    '$timeout',
    '$sce',
    'editorService',
    'HiFi.PositionalContent.UtilService',
    'HiFi.PositionalContent.ScaleService',
    'HiFi.PositionalContent.Resources',
    function ($timeout, $sce, editorService, util, scaleService, resources) {

        return {
            nextId: function (value, state) {
                if (value.items && value.items.length > 0) {
                    var maxIdItem = _.max(value.items, function (o) { return o.id });
                    return Number(maxIdItem.id) + 1;
                }
                else
                    return 0;
            },
            add: function (value, config, state) {
                if (!value.items) {
                    value.items = [];
                }
                var breakpointDimensions = {};
                angular.forEach(config.breakpoints, function (breakpoint) {                  
                    breakpointDimensions[breakpoint.name] = angular.copy(config.initialItemDimensions);
                });
                value.items.push({
                    id: this.nextId(value, state),
                    dimensions: breakpointDimensions
                });
            },
            edit: function (item, dataProperty, settingsProperty, state, modelValue) {
                editorService.open({
                    view: '../App_Plugins/PositionalContent/contenteditor/positionalcontenteditor.dimension.html',
                    dialogData: {
                        title: 'Item ' + item.id + ' content',
                        isItem: true,
                        state: state,
                        data: item,
                        modelValue: modelValue,
                        property: dataProperty,
                        settingsProperty: settingsProperty
                    },
                    size: 'large',
                    submit: function (data) {
                        item = data.data;
                        editorService.close();
                    },
                    close: function () {
                        editorService.close();
                    }
                });
            },
            remove: function (index, modelValue) {
                if (index > -1) {
                    if (window.confirm("Are you sure?")) {
                        modelValue.items.splice(index, 1);
                    }
                }
            },
            renderAll: function (modelValue, state) {
                if (state.active && modelValue && modelValue.dtdGuid != '00000000-0000-0000-0000-000000000000' && !state.recentlyRendered)
                {
                    //stop rendered from loading multiple time on initial start
                    state.recentlyRendered = true;
                    $timeout(function () { state.recentlyRendered = false; }, 200);

                    angular.forEach(modelValue.items, function (i) {
                        $timeout(function () { if (!i.preview) { i.preview = "<div class='item-preview-loading'>Loading...</div>"; } }, 1000);
                        resources.getPartialViewResultAsHtmlForEditor(modelValue.dtdGuid, i, state.active.name, state.previewModifierClass).then(function (response) {
                            if (response.data.trim().length > 0) {
                                i.preview = response.data;
                            }
                        });
                    });

                }
            },
            save: function (element, items, constrainTo, state) {
                var $item = $(element);
                var $parent = util.contraintContext(element, constrainTo);

                var id = $item.data('id');

                var positionAndSize = util.getPositionAndSize($item);
                var parentPositionAndSize = util.getPositionAndSize($parent);

                var parentWidth = parentPositionAndSize.width;
                var parentHeight = parentPositionAndSize.height;

                var breakpointName = state.active.name;

                angular.forEach(items, function (data, key) {
                    if (data.id == id) {

                        data.dimensions[breakpointName].left = util.percentageOf(positionAndSize.left, parentWidth, true);
                        data.dimensions[breakpointName].right = util.percentageOf(positionAndSize.right, parentWidth, true);

                        data.dimensions[breakpointName].top = util.percentageOf(positionAndSize.top, parentHeight, true);
                        data.dimensions[breakpointName].bottom = util.percentageOf(positionAndSize.bottom, parentHeight, true);

                        data.dimensions[breakpointName].width = util.percentageOf(positionAndSize.width, parentWidth, true);
                        data.dimensions[breakpointName].height = util.percentageOf(positionAndSize.height, parentHeight, true);
                    }
                });
            },
            positionalStyles: function(item, dimension, state)
            {
                var styles = {};
                var origin = '';

                if (dimension.widthAuto)
                    styles.width = 'auto';
                else
                    styles.width = dimension.width / scaleService.breakpointValue(state) + '%';

                if (dimension.heightAuto)
                    styles.height = 'auto';
                else
                    styles.height = dimension.height / scaleService.breakpointValue(state) + '%';

                if (dimension.pinToRight) {
                    styles.right = dimension.right + '%';
                    origin = '100%';
                }
                else {
                    styles.left = dimension.left + '%';
                    origin = '0';
                }

                if (dimension.pinToBottom) {
                    styles.bottom = dimension.bottom + '%';
                    origin = origin + ' 100%';
                }
                else {
                    styles.top = dimension.top + '%';
                    origin = origin + ' 0';
                }

                styles['transform-origin'] = origin;
                styles['transform'] = 'scale(' + scaleService.breakpointValue(state) + ')';

                var d = _.map(Object.keys(styles), function(key) { return key +':'+ styles[key] +';' }).join('');
                
                return d;
            },
            centerHorizontal: function (dimension) {

                var value = (100 - dimension.width) / 2;
                dimension.left = value;
                dimension.right = value;

            },
            centerVertical: function (dimension, state, $event) {

                var value = (100 - dimension.height) / 2;
                dimension.top = value;
                dimension.bottom = value;

            },
            trustedHtml: function (plainText) {
                return $sce.trustAsHtml(plainText);
            }
        };
    }]);