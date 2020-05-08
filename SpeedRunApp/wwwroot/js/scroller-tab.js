var hidWidth;
var scrollBarWidths = 25;

function initializeScroller() {
    initializeScrollerEvents();
    reAdjust();
}

function initializeScrollerEvents() {
    $(window).on('resize', function (e) {
        reAdjust();
    });

    $('.scroller-tab-right').click(function () {
        var scrollerTabWrapper = $(this).closest('.scroller-tab-wrapper');
        var scrollerTabList = $(scrollerTabWrapper).find('.scroller-tab-list');        
        var scrollerTabRight = $(scrollerTabWrapper).find('.scroller-tab-right');
        var scrollerTabLeft = $(scrollerTabWrapper).find('.scroller-tab-left');

        $(scrollerTabLeft).fadeIn('slow');
        $(scrollerTabRight).fadeOut('slow');

        $(scrollerTabList).animate({ marginLeft: "+=" + widthOfHidden(scrollerTabWrapper, scrollerTabList) + "px" }, 'slow', function () {
        });
    });

    $('.scroller-tab-left').click(function () {
        var scrollerTabWrapper = $(this).closest('.scroller-tab-wrapper');
        var scrollerTabList = $(scrollerTabWrapper).find('.scroller-tab-list');        
        var scrollerTabRight = $(scrollerTabWrapper).find('.scroller-tab-right');
        var scrollerTabLeft = $(scrollerTabWrapper).find('.scroller-tab-left'); 
    
        $(scrollerTabRight).fadeIn('slow');
        $(scrollerTabLeft).fadeOut('slow');

        $(scrollerTabList).animate({ marginLeft: "-=" + getLeftPositionOfList(scrollerTabList) + "px" }, 'slow', function () {
        });
    });
}

var widthOfHidden = function (scrollerTabWrapper, scrollerTabList) {
    return (($(scrollerTabWrapper).outerWidth()) - widthOfList(scrollerTabList) - getLeftPositionOfList(scrollerTabList)) - scrollBarWidths;
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

var reAdjust = function () {
    $('.scroller-tab-wrapper').each(function () {
        var scrollerTabWrapper = $(this);
        var scrollerTabList = $(scrollerTabWrapper).find('.scroller-tab-list');
        var scrollerTabRight = $(scrollerTabWrapper).find('.scroller-tab-right');
        var scrollerTabLeft = $(scrollerTabWrapper).find('.scroller-tab-left');

        if (($(scrollerTabWrapper).outerWidth()) < widthOfList(scrollerTabList)) {
            $(scrollerTabRight).show();
        }
        else {
            $(scrollerTabRight).hide();
        }

        if (getLeftPositionOfList(scrollerTabList) < 0) {
            $(scrollerTabLeft).show();
        }
        else {
            $(scrollerTabList).animate({ left: "-=" + getLeftPositionOfList(scrollerTabList) + "px" }, 'slow');
            $(scrollerTabLeft).hide();
        }
    });
}






