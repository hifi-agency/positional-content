 /*Copyright © 2018 HiFi Ltd - All Rights Reserved
 * This file is part of Positional Content which is released under the HiFi Positional Content License.
 * Please visit : http://www.positionalcontent.com/PositionalContentLicenseAgreement.pdf for full license details
 */

angular.module('umbraco').factory('HiFi.PositionalContent.UtilService', [
    function () {

        return {
            getPositionAndSize: function ($current) {

                var output = {};

                output.top = this.pxToInt($current.css('top'));
                output.left = this.pxToInt($current.css('left'))
                output.right = this.pxToInt($current.css('right'));
                output.bottom = this.pxToInt($current.css('bottom'));

                //correct due to transform sizing issues;
                var rect = $($current)[0].getBoundingClientRect();
                output.originalHeight = $current.prop('offsetHeight');
                output.originalWidth = $current.prop('offsetWidth');

                output.height = rect.height;
                output.width = rect.width;

                output.widthCorrection = output.originalWidth - output.width;
                output.heightCorrection = output.originalHeight - output.height;

                //correct due to transform sizing changes;
                if (this.isOriginBottom($current))
                    output.top = output.top + output.heightCorrection;
                else
                    output.bottom = output.bottom + output.heightCorrection;

                if (this.isOriginRight($current))
                    output.left = output.left + output.widthCorrection;
                else
                    output.right = output.right + output.widthCorrection;

                return output;
            },
            isOriginRight: function ($current) {
                var transformOrigin = this.getTranformOriginAsArray($current);
                return transformOrigin[0] == '100%';
            },
            isOriginBottom: function ($current) {
                var transformOrigin = this.getTranformOriginAsArray($current);
                if (transformOrigin.length > 0)
                    return transformOrigin[1] == '100%';
                else
                    return false;
            },
            getTranformOriginAsArray: function ($current)
            {
                var output = $current[0].style.transformOrigin;
                if (output)
                    return output.split(' ');
                else
                    return [];
            },
            percentageOf: function (pixels, total, makeSafe) {
                var pixValue = this.convertFromPixels(pixels);
                var totalValue = this.convertFromPixels(total);

                var output = (pixValue / totalValue) * 100;
                return makeSafe ? this.safeSize(output) : output;
            },
            percentageLeft: function (pixels, total) {
                var pixValue = this.convertFromPixels(pixels);
                var totalValue = this.convertFromPixels(total);

                if (pixValue > 0)
                    return ((totalValue - pixValue) / totalValue) * 100;
                else
                    return 0;
            },
            convertFromPixels: function (val) {
                if (!val)
                    return 0;
                else if (val.replace)
                    return val.replace('px', '')
                else
                    return val;
            },
            safeSize: function (value) {
                if (value < 1)
                    value = 0;
                return value;
            },
            pxToInt: function (val) {
                if (val) {
                    var output = val.replace('px', '');
                    return Number(output)
                }
                return val;
            },
            contraintContext: function (element, constrainTo) {
                var $parent = element.parent();
                if (constrainTo)
                    $parent = element.parents('.' + constrainTo);
                return $parent;
            }
        };
    }]);