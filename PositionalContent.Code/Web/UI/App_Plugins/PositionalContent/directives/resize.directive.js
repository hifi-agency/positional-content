 /*Copyright © 2018 HiFi Ltd - All Rights Reserved
 * This file is part of Positional Content which is released under the HiFi Positional Content License.
 * Please visit : http://www.positionalcontent.com/PositionalContentLicenseAgreement.pdf for full license details
 */

angular.module("umbraco").directive('positionalContentResize', ['$document', '$rootScope', '$timeout', 'HiFi.PositionalContent.ScaleService', 'HiFi.PositionalContent.UtilService', function ($document, $rootScope, $timeout, scaleService, util) {

    var cssRoot = 'positional-content__resize-control';

    var createButton = function (btn, isHorizontal, isReverseSide)
    {
        return {
            element : btn,
            isHorizontal: isHorizontal,
            isReverseSide: isReverseSide
        };
    }

    var buildButtons = function (element) {
        var btns = [];

        btns.push(createButton($(element).find('.positional-content__resize-control-left')[0], true, true));
        btns.push(createButton($(element).find('.positional-content__resize-control-right')[0], true, false));
        btns.push(createButton($(element).find('.positional-content__resize-control-top')[0], false, true));
        btns.push(createButton($(element).find('.positional-content__resize-control-bottom')[0], false, false));

        return btns;
    }

    var setButtonVisibility = function (dimension, btn) {

        var makeVisible = false;

        if(btn.isHorizontal)
        {
            if (!dimension.widthAuto)
                if (dimension.pinToRight && btn.isReverseSide)
                    makeVisible = true;
                else if (!dimension.pinToRight && !btn.isReverseSide)
                    makeVisible = true;
        }

        if (!btn.isHorizontal) {
            if (!dimension.heightAuto)
                if (dimension.pinToBottom && btn.isReverseSide)
                    makeVisible = true;
                else if (!dimension.pinToBottom && !btn.isReverseSide)
                    makeVisible = true;
        }

        $(btn.element).removeClass('positional-content__resize-control-hidden');
        if(!makeVisible)
            $(btn.element).addClass('positional-content__resize-control-hidden');
    }

    var setButtonsAllVisibility = function (currentItemDimension, btns) {
        angular.forEach(btns, function (btn) {
            setButtonVisibility(currentItemDimension, btn)
        });
    }

    var setButtonsAllHidden = function (btns) {
        angular.forEach(btns, function (btn) {
            $(btn.element).addClass('positional-content__resize-control-hidden');
        });
    }

    return {
        restrict: 'A',
        replace: false,
        link: function (scope, element, attr) {
            var dimension = {};
            var position = {};
            var parent = util.contraintContext(element, attr.resizeConstrainTo);

            var disabled = false;
            if (attr.moveDisabled)
                disabled = attr.moveDisabled;

            var currentItemDimension = scope.dimension(scope.i);
            var onStop = attr.resizeStop ? attr.resizeStop : function () { };
            var btns = buildButtons(element);

            if (!disabled) {

                scope.$watch('i', function () {
                    currentItemDimension = scope.dimension(scope.i);
                    setButtonsAllVisibility(currentItemDimension, btns);
                }, true);

                $rootScope.$on('HiFi.PositionalContent.BreakpointChanged.' + scope.state.id, function () {
                    $timeout(function () {
                        currentItemDimension = scope.dimension(scope.i);
                        setButtonsAllVisibility(currentItemDimension, btns);
                    });
                });

                angular.forEach(btns, function (btn) {

                    btn.element.onmousedown = function ($event) {
                        $event.stopImmediatePropagation();

                        dimension = util.getPositionAndSize(element);
                        parentDimension = util.getPositionAndSize(parent);

                        if (btn.isHorizontal) {
                            position.x = $event.clientX;
                            dimension.width = element.prop('offsetWidth');
                            dimension.left = element.prop('offsetLeft');
                        }
                        else {
                            position.y = $event.clientY;
                            dimension.height = element.prop('offsetHeight');
                            dimension.top = element.prop('offsetTop');
                        }

                        $document.bind('mousemove', btn.mousemove);
                        $document.bind('mouseup', btn.mouseup);
                        return false;
                    };

                    btn.mousemove = function ($event) {
                        var newDimensions = {};
                        var distanceFromEdge = 0;

                        if (btn.isHorizontal) {
                            var changeInWidth = (position.x - $event.clientX) / attr.resizeScale;

                            if (btn.isReverseSide) {
                                var deltaWidth = dimension.width + changeInWidth;
                                var deltaLeft = dimension.left - changeInWidth;

                                if (deltaWidth > parent.width)
                                    deltaWidth = parent.width;

                                newDimensions.width = deltaWidth + 'px';
                                newDimensions.left = deltaLeft + 'px';

                                distanceFromEdge = parentDimension.width - (dimension.right + (deltaWidth * attr.resizeScale));
                            }
                            else {
                                var deltaWidth = dimension.width - changeInWidth;
                                newDimensions.width = deltaWidth + 'px';

                                distanceFromEdge = parentDimension.width - (dimension.left + (deltaWidth * attr.resizeScale));
                            }
                        }
                        else {
                            var changeInHeight = (position.y - $event.clientY) / attr.resizeScale;
                            if (btn.isReverseSide) {
                                var deltaHeight = dimension.height + changeInHeight;
                                var deltatop = dimension.top - changeInHeight;

                                newDimensions.height = deltaHeight + 'px';
                                newDimensions.top = deltatop + 'px';

                                distanceFromEdge = parentDimension.height - (dimension.bottom + (deltaHeight * attr.resizeScale));
                            }
                            else {
                                var deltaHeight = dimension.height - changeInHeight;
                                newDimensions.height = deltaHeight + 'px';

                                distanceFromEdge = parentDimension.height - (dimension.top + (deltaHeight * attr.resizeScale));
                            }
                        }

                        if (distanceFromEdge > 0)
                            element.css(newDimensions);

                        return false;
                    }
                    btn.mouseup = function ($event) {
                        $document.unbind('mousemove', btn.mousemove);
                        $document.unbind('mouseup', btn.mouseup);

                        if (onStop)
                            scope[onStop].apply(null, [element]);
                    };

                    element.append(btn.element);

                    setButtonVisibility(currentItemDimension, btn);
                });

            }
            else {
                setButtonsAllHidden(btns);
            }
            

        }
    };
}]);