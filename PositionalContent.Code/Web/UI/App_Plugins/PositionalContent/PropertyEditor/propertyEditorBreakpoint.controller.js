 /*Copyright © 2018 HiFi Ltd - All Rights Reserved
 * This file is part of Positional Content which is released under the HiFi Positional Content License.
 * Please visit : http://www.positionalcontent.com/PositionalContentLicenseAgreement.pdf for full license details
 */

angular.module("umbraco").controller("HiFi.PositionalContent.PropertyBreakpoints", [
    '$scope',
    function ($scope) {

        $scope.add = function () {
            $scope.model.value.push({
                name: "",
                cropWidth: "",
                cropHeight: "",
                breakpointUpper: "",
                breakpointLower: ""
            });
        };

        $scope.remove = function (index) {
            if (window.confirm("Are you sure?")) {
                $scope.model.value.splice(index, 1);
            }
        };

        $scope.sortableOptions = {
            axis: 'y',
            cursor: "move",
            handle: ".icon-navigation"
        };

        //if (!$scope.model.value)
        //    $scope.model.value = {};

        if (!$scope.model.value) {
            $scope.model.value = [];
            $scope.add();
        }

    }]
);