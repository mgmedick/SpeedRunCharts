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

    $('#txtMenuSearch').autocomplete({
        minlength: 3,
        source: '../Menu/Search',
        search: function (event, ui) {
            $(this).parent().addClass("loading-icon");
        },
        response: function (event, ui) {
            $(this).parent().removeClass("loading-icon");
        },
        focus: function (event, ui) {
            event.preventDefault();
            this.value = ui.item.label;
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
        if ($(items).filter(function () { return this.subItems && this.subItems.length > 0 }).length == 0) {
            ul.append("<div class='dropdown-divider'></div><div class='ui-autocomplete-category'>No results found</div>");
        } else {
            $(items).each(function () {
                if (this.subItems.length > 0) {
                    var category = this.label;
                    ul.append("<div class='dropdown-divider'></div><div class='ui-autocomplete-category'>" + category + "</div>");

                    $(this.subItems).each(function () {
                        this.category = category;
                        var li = that._renderItemData(ul, this);
                    });
                }
            });
        }
    };
}

function showLogin() {
    var $modal = $('#loginModal');
    var $modalBody = $modal.find('.modal-body');
    var $modalTitle = $modal.find('.modal-title');
    $modalTitle.html("<h5>Log In</h5>");

    $.get('/SpeedRun/Login', function (data) {
        $modalBody.html(data);
        $modal.modal('show');
    }, "html");
}

function login() {
    var $modal = $('#loginModal');
    var $modalBody = $modal.find('.modal-body');
    var formData = new FormData($('#frmLogin')[0]);

    $.ajax({
        url: "/SpeedRun/Login",
        processData: false,
        contentType: false,
        type: "POST",
        data: formData,
        success: function (data) {
            if (data.success) {
                $modal.modal('hide');
                location.reload();
            } else if (data.success === false) {
                $modalBody.html(data.message);
            } else {
                $modalBody.html(data);
            }
        }
    });
}

function showSignUp() {
    var $modal = $('#loginModal');
    var $modalBody = $modal.find('.modal-body');
    var $modalTitle = $modal.find('.modal-title');
    $modalTitle.html("<h5>Sign Up</h5>");

    $.get('/SpeedRun/SignUp', function (data) {
        $modalBody.html(data);
        $modal.modal('show');
    }, "html");
}

function signUp() {
    var $modal = $('#loginModal');
    var $modalBody = $modal.find('.modal-body');
    var $loading = $modal.find('.signup-loading');
    var $successmsg = $modal.find('.signup-successmsg');
    var formData = new FormData($('#frmSignUp')[0]);

    $successmsg.hide();
    $loading.show();

    $.ajax({
        url: "/SpeedRun/SignUp",
        processData: false,
        contentType: false,
        type: "POST",
        data: formData,
        success: function (data) {
            if (data.success) {
                $loading.hide();
                $successmsg.show();
            } else if (data.success === false) {
                $modalBody.html(data.message);
            } else {
                $modalBody.html(data);
            }
        }
    });
}


















