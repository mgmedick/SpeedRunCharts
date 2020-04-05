var hidWidth;
var scrollBarWidths = 40;

function InitializeScroller() {
    InitializeScrollerEvents();
    reAdjust();
}

function InitializeScrollerEvents() {
    $(window).on('resize', function (e) {
        reAdjust();
    });

    $('.scroller-tab-right').click(function () {
        var scrollerTabWapper = $(this).closest('.scroller-tab-wrapper');
        var scrollerTabList = $(scrollerTabWapper).find('.scroller-tab-list');        
        var scrollerTabRight = $(scrollerTabWapper).find('.scroller-tab-right');
        var scrollerTabLeft = $(scrollerTabWapper).find('.scroller-tab-left');

        $(scrollerTabLeft).fadeIn('slow');
        $(scrollerTabRight).fadeOut('slow');

        $(scrollerTabList).animate({ marginLeft: "+=" + widthOfHidden(scrollerTabWapper) + "px" }, 'slow', function () {
        });
    });

    $('.scroller-tab-left').click(function () {
        var scrollerTabWapper = $(this).closest('.scroller-tab-wrapper');
        var scrollerTabList = $(scrollerTabWapper).find('.scroller-tab-list');        
        var scrollerTabRight = $(scrollerTabWapper).find('.scroller-tab-right');
        var scrollerTabLeft = $(scrollerTabWapper).find('.scroller-tab-left'); 
    
        $(scrollerTabRight).fadeIn('slow');
        $(scrollerTabLeft).fadeOut('slow');

        $(scrollerTabList).animate({ marginLeft: "-=" + getLeftPosi(scrollerTabWapper) + "px" }, 'slow', function () {
        });
    });
}

var widthOfHidden = function (element) {
    return (($(element).outerWidth()) - widthOfList(element) - getLeftPosi(element)) - scrollBarWidths;
};

var widthOfList = function (element) {
    var itemsWidth = 0;
    $(element).find('.scroller-tab-list li').each(function () {
        var itemWidth = $(this).outerWidth();
        itemsWidth += itemWidth;
    });
    return itemsWidth;
};

var getLeftPosi = function (element) {
    return $(element).find('.scroller-tab-list').position().left;
};

var reAdjust = function () {
    $('.scroller-tab-wrapper').each(function() {
        var scrollerTabWapper = this;
        var scrollerTabList = $(scrollerTabWapper).find('.scroller-tab-list'); 
        var scrollerTabRight = $(scrollerTabWapper).find('.scroller-tab-right');
        var scrollerTabLeft = $(scrollerTabWapper).find('.scroller-tab-left');

        if (($(this).outerWidth()) < widthOfList(scrollerTabWapper)) {
            $(scrollerTabRight).show();
        }
        else {
            $(scrollerTabRight).hide();
        }

        if (getLeftPosi(scrollerTabWapper) < 0) {
            $(scrollerTabLeft).show();
        }
        else {
            $(scrollerTabList).animate({ left: "-=" + getLeftPosi(scrollerTabWapper) + "px" }, 'slow');
            $(scrollerTabLeft).hide();
        }
    });
}


