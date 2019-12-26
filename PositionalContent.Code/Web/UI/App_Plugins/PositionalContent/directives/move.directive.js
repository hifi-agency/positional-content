 /*Copyright © 2018 HiFi Ltd - All Rights Reserved
 * This file is part of Positional Content which is released under the HiFi Positional Content License.
 * Please visit : http://www.positionalcontent.com/PositionalContentLicenseAgreement.pdf for full license details
 */

angular.module("umbraco").directive('positionalContentMove', ['$document', 'HiFi.PositionalContent.UtilService', function ($document, util) {
    return {
        restrict: 'A',
        replace: false,
        link: function (scope, element, attr) {
            var dimension = {};
            var parentDimension = {};
            var position = {};
            var parent = util.contraintContext(element, attr.moveConstrainTo);

            var bindToElement = element;
            if (attr.moveBoundElement)
                bindToElement = element.find('.' + attr.moveBoundElement);

            var disabled = false;
            if (attr.moveDisabled)
                disabled = attr.moveDisabled;

            var onStop = attr.moveStop ? attr.moveStop : function () { };
            var onDrag = attr.moveDrag ? attr.moveDrag : function () { };

            if(!disabled)
                bindToElement[0].onmousedown = function ($event) {
                    $event.stopImmediatePropagation();
                    position.x = $event.clientX;
                    position.y = $event.clientY;
                    dimension = util.getPositionAndSize(element);
                    parentDimension = util.getPositionAndSize(parent);
                    $document.bind('mousemove', mousemove);
                    $document.bind('mouseup', mouseup);
                    return false;
                };

            var mousemove = function ($event) {
                var newDimensions = {};

                var changeInLeft = position.x - $event.clientX;
                var deltaLeft = dimension.left - changeInLeft;

                var changeInTop = position.y - $event.clientY;
                var deltaTop = dimension.top - changeInTop;

                var deltaBottom = dimension.bottom + changeInTop;
                var deltaRight = dimension.right + changeInLeft;

                var maxY = parentDimension.height - dimension.height;
                var maxX = parentDimension.width - dimension.width;

                if (deltaTop < 0)
                    deltaTop = 0;

                if (deltaTop > maxY)
                    deltaTop = maxY;

                if (deltaBottom < 0)
                    deltaBottom = 0;

                if (deltaBottom > maxY)
                    deltaBottom = maxY;

                if (deltaLeft < 0)
                    deltaLeft = 0;

                if (deltaLeft > maxX)
                    deltaLeft = maxX;

                if (deltaRight < 0)
                    deltaRight = 0;

                if (deltaRight > maxX)
                    deltaRight = maxX;


                if (util.isOriginBottom(element))
                    newDimensions.bottom = deltaBottom + 'px';
                else
                    newDimensions.top = deltaTop + 'px';


                if (util.isOriginRight(element))
                    newDimensions.right = deltaRight + 'px';
                else
                    newDimensions.left = deltaLeft + 'px';

                element.css(newDimensions);

                if (onDrag && scope[onDrag])
                    scope[onDrag].apply(null, [element]);

                return false;
            }
            var mouseup = function ($event) {
                $document.unbind('mousemove', mousemove);
                $document.unbind('mouseup', mouseup);

                if (onStop && scope[onStop])
                    scope[onStop].apply(null, [element]);
            };

        }
    };
}]);