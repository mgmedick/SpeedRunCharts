var hidWidth;
var scrollBarWidths = 25;
var scrollDistance = 300;

function initializeScrollerGlobalEvents() {
    $(window).on('resize', function (e) {
        reAdjust(this);
    });
}

function initializeScroller(element) {
    initializeScrollerEvents(element);
    reAdjust(element);
}

function initializeScrollerEvents(element) {
    $(element).find('.scroller-tab-wrapper').each(function () {
        var scrollerTabWrapper = this;
        var scrollerTabList = $(scrollerTabWrapper).find('.scroller-tab-list');
        var scrollerTabRight = $(scrollerTabWrapper).find('.scroller-tab-right');
        var scrollerTabRightEnd = $(scrollerTabWrapper).find('.scroller-tab-right-end');
        var scrollerTabLeft = $(scrollerTabWrapper).find('.scroller-tab-left');
        var scrollerTabLeftEnd = $(scrollerTabWrapper).find('.scroller-tab-left-end');

        $(scrollerTabRight).off('click');
        $(scrollerTabRight).on('click', function () {
            $(scrollerTabLeft).fadeIn('slow');
            $(scrollerTabLeftEnd).fadeIn('slow');

            var hiddenWidth = widthOfHiddenEnd(scrollerTabWrapper, scrollerTabList);
            var scrollWidth = (scrollDistance * -1);
            if (hiddenWidth >= scrollWidth) {
                scrollWidth = hiddenWidth;
                $(scrollerTabRight).fadeOut('slow');
                $(scrollerTabRightEnd).fadeOut('slow');
            }

            $(scrollerTabList).animate({ marginLeft: "+=" + scrollWidth + "px" }, 'slow', function () { });
        });

        $(scrollerTabRightEnd).off('click');
        $(scrollerTabRightEnd).on('click', function () {
            $(scrollerTabLeft).fadeIn('slow');
            $(scrollerTabLeftEnd).fadeIn('slow');
            $(scrollerTabRight).fadeOut('slow');
            $(scrollerTabRightEnd).fadeOut('slow');

            $(scrollerTabList).animate({ marginLeft: "+=" + widthOfHiddenEnd(scrollerTabWrapper, scrollerTabList) + "px" }, 'slow', function () { });
        });

        $(scrollerTabLeft).off('click');
        $(scrollerTabLeft).on('click', function () {
            $(scrollerTabRight).fadeIn('slow');
            $(scrollerTabRightEnd).fadeIn('slow');

            var leftPosition = getLeftPositionOfList(scrollerTabList);
            var scrollWidth = (scrollDistance * -1);
            if (leftPosition >= scrollWidth) {
                scrollWidth = leftPosition;
                $(scrollerTabLeft).fadeOut('slow');
                $(scrollerTabLeftEnd).fadeOut('slow');
            }

            $(scrollerTabList).animate({ marginLeft: "-=" + scrollWidth + "px" }, 'slow', function () { });
        });

        $(scrollerTabLeftEnd).off('click');
        $(scrollerTabLeftEnd).on('click', function () {
            $(scrollerTabRight).fadeIn('slow');
            $(scrollerTabRightEnd).fadeIn('slow');
            $(scrollerTabLeft).fadeOut('slow');
            $(scrollerTabLeftEnd).fadeOut('slow');

            $(scrollerTabList).animate({ marginLeft: "-=" + getLeftPositionOfList(scrollerTabList) + "px" }, 'slow', function () { });
        });

        if ($.isFunction($.fn.mousewheel)) {
            $(scrollerTabWrapper).off('mousewheel');
            $(scrollerTabWrapper).on('mousewheel', function (event, delta) {
                var hiddenWidth = widthOfHiddenEnd(scrollerTabWrapper, scrollerTabList);
                var leftPosition = getLeftPositionOfList(scrollerTabList);
                var scrollWidth = (delta * 30);

                if (hiddenWidth >= scrollWidth && delta == -1) {
                    scrollWidth = hiddenWidth;
                } else if (leftPosition >= scrollWidth && delta == 1) {
                    scrollWidth = leftPosition;
                }

                if (hiddenWidth >= 0) {
                    $(scrollerTabRight).fadeOut('slow');
                    $(scrollerTabRightEnd).fadeOut('slow');

                } else {
                    $(scrollerTabRight).fadeIn('slow');
                    $(scrollerTabRightEnd).fadeIn('slow');
                }

                if (leftPosition >= 0) {
                    $(scrollerTabLeft).fadeOut('slow');
                    $(scrollerTabLeftEnd).fadeOut('slow');
                } else if (!$(scrollerTabLeft).is(':visible')) {
                    $(scrollerTabLeft).fadeIn('slow');
                    $(scrollerTabLeftEnd).fadeIn('slow');
                }

                if (hiddenWidth < 0 && delta == -1) {
                    $(scrollerTabList).animate({ marginLeft: "+=" + scrollWidth + "px" }, 'fast', function () { });
                }

                if (leftPosition < 0 && delta == 1) {
                    $(scrollerTabList).animate({ marginLeft: "+=" + scrollWidth + "px" }, 'fast', function () { });
                }

                event.preventDefault();
            });
        }
    });
}

var widthOfHiddenEnd = function (scrollerTabWrapper, scrollerTabList) {
    return (($(scrollerTabWrapper).outerWidth()) - widthOfList(scrollerTabList) - getLeftPositionOfList(scrollerTabList)) - (scrollBarWidths * 2);
};

var widthOfList = function (scrollerTabList) {
    var itemsWidth = 0;
    $(scrollerTabList).find('li').each(function () {
        var itemWidth = $(this).outerWidth();
        itemsWidth += itemWidth;
    });
    return itemsWidth;
};

var getLeftPositionOfList = function (scrollerTabList) {
    return parseInt($(scrollerTabList).css("marginLeft"));
};

function reAdjust(element) {
    $(element).find('.scroller-tab-wrapper').each(function () {
        var scrollerTabWrapper = $(this);
        var scrollerTabList = $(scrollerTabWrapper).find('.scroller-tab-list');
        var scrollerTabRight = $(scrollerTabWrapper).find('.scroller-tab-right');
        var scrollerTabRightEnd = $(scrollerTabWrapper).find('.scroller-tab-right-end');
        var scrollerTabLeft = $(scrollerTabWrapper).find('.scroller-tab-left');
        var scrollerTabLeftEnd = $(scrollerTabWrapper).find('.scroller-tab-left-end');

        if (($(scrollerTabWrapper).outerWidth()) < widthOfList(scrollerTabList)) {
            $(scrollerTabRight).show();
            $(scrollerTabRightEnd).show();
        }
        else {
            $(scrollerTabRight).hide();
            $(scrollerTabRightEnd).hide();
        }

        if (getLeftPositionOfList(scrollerTabList) < 0) {
            $(scrollerTabLeft).show();
            $(scrollerTabLeftEnd).show();
        }
        else {
            $(scrollerTabList).animate({ left: "-=" + getLeftPositionOfList(scrollerTabList) + "px" }, 'slow');
            $(scrollerTabLeft).hide();
            $(scrollerTabLeftEnd).hide();
        }
    });
}







