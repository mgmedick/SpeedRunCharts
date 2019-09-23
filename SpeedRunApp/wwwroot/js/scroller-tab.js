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
        $('.scroller-tab-left').fadeIn('slow');
        $('.scroller-tab-right').fadeOut('slow');

        $('.scroller-tab-list').animate({ left: "+=" + widthOfHidden() + "px" }, 'slow', function () {
        });
    });

    $('.scroller-tab-left').click(function () {
        $('.scroller-tab-right').fadeIn('slow');
        $('.scroller-tab-left').fadeOut('slow');

        $('.scroller-tab-list').animate({ left: "-=" + getLeftPosi() + "px" }, 'slow', function () {
        });
    });
}

var widthOfList = function () {
    var itemsWidth = 0;
    $('.scroller-tab-list li').each(function () {
        var itemWidth = $(this).outerWidth();
        itemsWidth += itemWidth;
    });
    return itemsWidth;
};

var widthOfHidden = function () {
    return (($('.scroller-tab-wrapper').outerWidth()) - widthOfList() - getLeftPosi()) - scrollBarWidths;
};

var getLeftPosi = function () {
    return $('.scroller-tab-list').position().left;
};

var reAdjust = function () {
    if (($('.scroller-tab-wrapper').outerWidth()) < widthOfList()) {
        $('.scroller-tab-right').show();
    }
    else {
        $('.scroller-tab-right').hide();
    }

    if (getLeftPosi() < 0) {
        $('.scroller-tab-left').show();
    }
    else {
        $('.item').animate({ left: "-=" + getLeftPosi() + "px" }, 'slow');
        $('.scroller-tab-left').hide();
    }
}


