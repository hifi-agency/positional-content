 /*Copyright © 2018 HiFi Ltd - All Rights Reserved
 * This file is part of Positional Content which is released under the HiFi Positional Content License.
 * Please visit : http://www.positionalcontent.com/PositionalContentLicenseAgreement.pdf for full license details
 */

angular.module("umbraco").controller("HiFi.PositionalContent.PropertyEditorPicker", [
    '$scope',
    'HiFi.PositionalContent.Resources',
    function ($scope, resources) {

        $scope.model.dataTypes = [];
        $scope.model.value = $scope.model.value;

        resources.getNestedContentDataTypes().then(function (data) {
            $scope.model.dataTypes = data;
        });

    }]
);