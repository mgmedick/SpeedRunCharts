function GetUniqueValues(data) {
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
        toggleLink: null,
        linkHiddenText: "Show Search",
        linkDisplayedText: "Hide Search",
        initialState: "hidden",
        transitionSpeed: "fast",
        saveToCookie: "false"
    }, callerSettings || {});

    settings.searchContainer = this;

    if (settings.toggleLink == null)
        settings.toggleLink = $("<a></a>").attr({ 'id': 'linkToggle', 'href': '#' }).addClass("linkToggle").insertBefore(this);
    else
        settings.toggleLink = $(settings.toggleLink);

    //init saveToCookie setting
    settings.saveToCookie = (settings.saveToCookie == "true");

    //define cookie name for the current page for storing state of callapsibles
    var cookieName = location.href.substring(location.href.lastIndexOf('/') + 1, location.href.lastIndexOf('.'));
    cookieName += "_collapsible";

    //define callback function for call to slideToggle
    var toggleCallback = function () {
        if (settings.searchContainer.is(":hidden")) {
            settings.toggleLink.text(settings.linkHiddenText).removeClass("expanded");

            if (settings.saveToCookie)
                $.cookie(cookieName, "hidden");
        }
        else {
            settings.toggleLink.text(settings.linkDisplayedText).addClass("expanded");

            if (settings.saveToCookie)
                $.cookie(cookieName, "visible");
        }
    };

    //define function for link's click event
    var toggleClick = function () {
        settings.searchContainer.slideToggle(settings.transitionSpeed, toggleCallback);

        return false;
    };

    //wire-up click event for link
    settings.toggleLink.click(toggleClick);

    //apply desired initial state to controls
    var initCookievalue;

    if (settings.saveToCookie) {
        initCookievalue = $.cookie(cookieName);

        if (initCookievalue == "hidden")
            settings.searchContainer.hide();
        else
            settings.searchContainer.show();
    }
    else {
        if (settings.initialState == "hidden")
            settings.searchContainer.hide();
        else
            settings.searchContainer.show();
    }

    toggleCallback();

    return this;
};

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









