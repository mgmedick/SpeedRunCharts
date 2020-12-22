if (!sra) {
    var sra = {};
}

function apiSettings(maxPerPage, reqLimit, tLimitMS) {
    this.maxElementsPerPage = maxPerPage;
    this.requestLimit = reqLimit;
    this.timeLimitMS = tLimitMS;
}

function initializeGlobalClient(maxElementsPerPage, requestLimit, timeLimitMS, defaultTheme) {
    initializeGlobalConstants(maxElementsPerPage, requestLimit, timeLimitMS, defaultTheme);
    initializeGlobalEvents();
}

function initializeGlobalConstants(maxElementsPerPage, requestLimit, timeLimitMS, defaultTheme) {
    if (!sra.apiSettings) {
        sra["apiSettings"] = new apiSettings(maxElementsPerPage, requestLimit, timeLimitMS);
    }

    if (!getCookie('theme')) {
        setCookie('theme', defaultTheme);
    }
}

function initializeGlobalEvents() {
    $('#chkNightMode').click(function () {
        if ($(this).is(":checked")) {
            setCookie('theme', "theme-dark");
            $(document.body).removeClass("theme-light").addClass("theme-dark");
            //$(document.body).find('.table').removeClass('table-active').addClass('table-dark');
            //$(document.body).find('.speedRunSummary').removeClass('bg-light').addClass('bg-dark');

            $(sra.charts).each(function () {
                this.chartConfig.bgColor = "#303030";
                this.generateChart();
            });
        } else {
            setCookie('theme', "theme-light");
            $(document.body).removeClass("theme-dark").addClass("theme-light");
            //$(document.body).find('.table').removeClass('table-dark').addClass('table-active');
            //$(document.body).find('.speedRunSummary').removeClass('bg-dark').addClass('bg-light');

            $(sra.charts).each(function () {
                this.chartConfig.bgColor = "#2c3e50";
                this.generateChart();
            });
        }
    });

    $('#txtGameUserSearch').autocomplete({
        minlength: 3,
        source: '../Menu/Search',
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












