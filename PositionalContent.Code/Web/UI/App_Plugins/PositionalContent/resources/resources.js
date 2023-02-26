 /*Copyright © 2018 HiFi Ltd - All Rights Reserved
 * This file is part of Positional Content which is released under the HiFi Positional Content License.
 * Please visit : http://www.positionalcontent.com/PositionalContentLicenseAgreement.pdf for full license details
 */

angular.module('umbraco.resources').factory('HiFi.PositionalContent.Resources',
    function ($q, $http, $routeParams, umbRequestHelper) {
        return {
            getNestedContentDataTypes: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get(Umbraco.Sys.ServerVariables.positionalcontent.apiBaseUrl + 'GetBlockListDataTypes'),
                    'Failed to retrieve datatypes'
                );
            },
            getDataTypeById: function (id) {
                return umbRequestHelper.resourcePromise(
                    $http.get(Umbraco.Sys.ServerVariables.positionalcontent.apiBaseUrl + 'GetDataTypeById?id=' + id),
                    'Failed to retrieve datatype'
                );
            },
            getDataTypeByAlias: function (contentType, contentTypeAlias, propertyAlias) {
                return umbRequestHelper.resourcePromise(
                    $http.get(Umbraco.Sys.ServerVariables.positionalcontent.apiBaseUrl + 'GetDataTypeByAlias?contentType=' + contentType + '&contentTypeAlias=' + contentTypeAlias + '&propertyAlias=' + propertyAlias),
                    'Failed to retrieve datatype'
                );
            },
            getPartialViewResultAsHtmlForEditor: function (dtdGuid, item, breakpointName, previewModifierClass) {

                var view = "positionalcontent/base";
                var url = "/umbraco/backoffice/positionalcontent/positionalcontentpreview/GetPartialViewResultAsHtmlForEditor";
                var resultParameters = { item: angular.toJson(item, false), breakpointName: breakpointName, previewModifierClass: previewModifierClass, dtdGuid: dtdGuid, view: view, id: $routeParams.id, doctype: $routeParams.doctype };

                return $http.post(url, resultParameters, {
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8' },
                    transformRequest: function (result) {
                        return $.param(result);
                    }
                });
            }
        };
    }
);