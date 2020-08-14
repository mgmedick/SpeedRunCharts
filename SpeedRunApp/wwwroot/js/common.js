﻿function GetUniqueValues(data) {
    var uniqueTexts = [];
    var textsMap = {};

    for (var i = 0; i < data.length; i++) {
        var text = data[i];
        if (text !== undefined && textsMap[text] === undefined) {
            textsMap[text] = true;
            uniqueTexts.push(text);
        }
    }

    return uniqueTexts;
}

$.fn.setupCollapsible = function (callerSettings) {
    var settings = $.extend({
        linkHiddenText: "Show Search",
        linkDisplayedText: "Hide Search",
        initialState: "hidden",
        transitionSpeed: "fast"
    }, callerSettings || {});

    settings.searchContainer = this;

    settings.toggleLink = $("<a></a>").attr({ 'id': 'linkToggle', 'href': '#' }).addClass("linkToggle").insertBefore(this);

    var toggleCallback = function () {
        if (settings.searchContainer.is(":hidden")) {
            settings.toggleLink.text(settings.linkHiddenText).removeClass("expanded");
        }
        else {
            settings.toggleLink.text(settings.linkDisplayedText).addClass("expanded");
        }
    };

    var toggleClick = function () {
        settings.searchContainer.slideToggle(settings.transitionSpeed, toggleCallback);
        return false;
    };

    //wire-up click event for link
    settings.toggleLink.click(toggleClick);

    if (settings.initialState == "hidden")
        settings.searchContainer.hide();
    else
        settings.searchContainer.show();

    toggleCallback();

    return this;
};

$.widget("custom.categorycomplete", $.ui.autocomplete, {
    _renderMenu: function (ul, items) {
        var that = this;
        var currentCategory;

        $.each(items, function (index, item) {
            if (item.category != currentCategory) {
                ul.append("<li class='ui-autocomplete-category'>" + item.category + "</li>");
                currentCategory = item.category;
            }
            that._renderItem(ul, item);
        });
    }
});

function repopulateDropDown(element, items) {
    var selectedItems = $(element).val();

    $(element).empty();
    $(items).each(function () {
        if (selectedItems.length > 0 && selectedItems.indexOf(this.id.toString()) > -1) {
            $(element).append($('<option>').text(this.name).attr('value', this.id).attr('selected', 'selected'));
        } else {
            $(element).append($('<option>').text(this.name).attr('value', this.id));
        }
    });

    if ($(element).hasClass('chosen')) {
        $(element).trigger('chosen:updated');
    }
}









