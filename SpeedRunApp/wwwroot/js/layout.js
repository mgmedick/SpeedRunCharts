if (!sra) {
    var sra = {};
}

function apiSettings(maxPerPage, reqLimit, tLimitMS) {
    this.maxElementsPerPage = maxPerPage;
    this.requestLimit = reqLimit;
    this.timeLimitMS = tLimitMS;
}

function initializeGlobalClient(maxElementsPerPage, requestLimit, timeLimitMS) {
    initializeGlobalConstants(maxElementsPerPage, requestLimit, timeLimitMS);
    initializeGlobalEvents();
}

function initializeGlobalConstants(maxElementsPerPage, requestLimit, timeLimitMS) {
    sra["apiSettings"] = new apiSettings(maxElementsPerPage, requestLimit, timeLimitMS);
}

function initializeGlobalEvents() {
    $('#txtGameUserSearch').autocomplete({
        minlength: 3,
        source: '../SpeedRun/SearchGamesAndUsers',
        search: function (event, ui) {
            $(this).parent().addClass("loading-icon");
        },
        response: function (event, ui) {
            $(this).parent().removeClass("loading-icon");
        },
        select: function (event, ui) {
            var item = ui.item;
            if (item) {
                $(this).val(item.label);

                var controller;
                var action;
                var params;

                switch (item.category) {
                    case "Games":
                        controller = "Game";
                        action = "GameDetails"
                        params = "gameID=" + item.value;
                        break;
                    case "Users":
                        controller = "User";
                        action = "UserDetails"
                        params = "userID=" + item.value;
                        break;
                }

                location.href = encodeURI('../' + controller + "/" + action + "?" + params);
            }

            return false;
        }
    }).data('ui-autocomplete')._renderMenu = function (ul, items) {
        var that = this;
        var currentCategory;

        $.each(items, function (index, item) {
            if (item.category != currentCategory) {
                ul.append("<div class='dropdown-divider'></div><div class='ui-autocomplete-category'>" + item.category + "</div>");
                currentCategory = item.category;
            }

            var li = that._renderItemData(ul, item);
            //if (item.category) {
            //    li.find(".ui-menu-item-wrapper").addClass("dropdown-item");
            //}
        });
    };
}




