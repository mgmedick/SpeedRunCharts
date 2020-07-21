var _categoryTypes;
var _games;
var _categories;
var _levels;

/**Initialize Functions**/
function initializeClient(categoryTypes, games, categories, levels) {
    _categoryTypes = categoryTypes;
    _games = games;
    _categories = categories;
    _levels = levels;

    initializeEvents();

    var $activeCategoryTypeTab = $('.nav-item.categoryType a.active');
    onCategoryTypeTabClick($activeCategoryTypeTab);
}

function initializeEvents() {
    $('.chosen').chosen({ width: "250px" });
    $('.date').datepicker();
    $('[data-toggle="tooltip"]').tooltip();
    $('#divSearch').setupCollapsible({ initialState: "visible", linkHiddenText: "Show Filters", linkDisplayedText: "Hide Filters" });

    $('#btnSearch').click(runSearch);

    $('#drpCategoryTypes').change(function () {
        onCategoryTypeChange(this);
    });

    $('#drpGames').change(function () {
        onGameChange(this);
    });

    initializeGridContainerEvents();
    initializeScrollerGlobalEvents();
}

function initializeGridContainerEvents() {
    $('.nav-item.categoryType a').click(function () {
        onCategoryTypeTabClick(this);
    });

    $('.nav-item.game a').click(function () {
        onGameTabClick(this);
    });

    $('.nav-item.category a').click(function () {
        onCategoryTabClick(this);
    });

    $('.nav-item.level a').click(function () {
        onLevelTabClick(this);
    });
}

/*Event Handlers*/
//Grid Container Handlers
function onCategoryTypeTabClick(element) {
    var categoryTypeContainerID = $(element).attr('href');
    var $container = $(categoryTypeContainerID);

    $('.categoryType-tab-pane').hide();
    $container.fadeIn();

    var $activeGameTab = $container.find('.game a.active');
    onGameTabClick($activeGameTab);
}

function onGameTabClick(element) {
    var gameContainerID = $(element).attr('href');
    var $container = $(gameContainerID);

    $('.game-tab-pane').hide();
    $container.fadeIn();

    var $activeCategoryTab = $container.find('.category a.active');
    onCategoryTabClick($activeCategoryTab);
}

function onCategoryTabClick(element) {
    var categoryContainerID = $(element).attr('href');
    var $container = $(categoryContainerID);

    $('.category-tab-pane').hide();
    $container.fadeIn();

    if ($(element).data('categorytype') == 1) {
        $container.find('.level-tabs').show();
        $container.find('.level-results').show();
        $container.find('.category-results').hide();

        var $activeLevelTab = $container.find('.level a.active');
        onLevelTabClick($activeLevelTab);
    } else {
        var $activeCategoryPane = $container.find('.category-results');
        if (!$activeCategoryPane.find('.grid')[0].grid) {
            initializeGrid($activeCategoryPane);
        }

        $container.find('.level-tabs').hide();
        $container.find('.level-results').hide();
        $activeCategoryPane.show();
    }
}

function onLevelTabClick(element) {
    var levelContainerID = $(element).attr('href');
    var $container = $(levelContainerID);

    if (!$container.find('.grid')[0].grid) {
        initializeGrid($container);
    }

    $('.level-tab-pane').hide();
    $container.fadeIn();
}

//Search Handlers
function onCategoryTypeChange(element) {
    var selectedCategoryTypeIDs = $(element).val();

    var $games = $(_games).filter(function () {
        return (selectedCategoryTypeIDs.length == 0 || $(selectedCategoryTypeIDs).filter(this.categoryTypeIDs).length > 0);
    });

    var $categories = $(_categories).filter(function () {
        return (selectedCategoryTypeIDs.length == 0 || selectedCategoryTypeIDs.indexOf(this.categoryTypeID) > -1);
    });

    repopulateDropDown($('#drpGames'), $games);
    repopulateDropDown($('#drpCategories'), $categories);

    if (selectedCategoryTypeIDs.indexOf("1") > -1) {
        $('#divLevels').show();
    } else {
        $('#divLevels').hide();
        $('#drpLevels').val([]);
    }
}

function onGameChange(element) {
    var selectedGameIDs = $(element).val();
    var selectedCategoryTypeIDs = $('#drpCategoryTypes').val();

    var $categories = $(_categories).filter(function () {
        return (selectedGameIDs.length == 0 || selectedGameIDs.indexOf(this.gameID) > -1) && (selectedCategoryTypeIDs.length == 0 || selectedCategoryTypeIDs.indexOf(this.categoryTypeID) > -1);
    });

    var $levels = $(_levels).filter(function () {
        return (selectedGameIDs.length == 0 || selectedGameIDs.indexOf(this.gameID) > -1);
    });

    repopulateDropDown($('#drpCategories'), $categories);
    repopulateDropDown($('#drpLevels'), $levels);
}

/**Initialize Component functions**/
//Initialize Grids
function initializeGrid(element) {
    var grid = $(element).find('.grid');
    var pagerID = $(element).find('.pager').attr("id");
    var gameID = $(element).data('gameid');
    var categoryType = $(element).data('categorytype');
    var categoryID = $(element).data('categoryid');
    var levelID = $(element).data('levelid') ? $(element).data('levelid') : '';
    var userID = $('#hdnUserID').val();
    var startDate = $('#txtStartDate').val();
    var endDate = $('#txtEndDate').val();

    grid.jqGrid({
        url: 'UserSpeedRunGrid_Read?userID=' + userID + '&gameID=' + gameID + '&categoryType=' + categoryType + '&categoryID=' + categoryID + '&levelID=' + levelID + '&startDate=' + startDate + '&endDate=' + endDate,
        datatype: "json",
        mtype: "GET",
        height: '100%',
        //autowidth: true,
        //shrinkToFit: true,
        rowNum: 50,
        pager: pagerID,
        colNames: ["", "Level", "Players", "Platform", "Emulated", "Time", "Status", "Examiner", "Reject Reason", "Submitted Date", "Comment", "Hidden", "Hidden", "Hidden"],
        colModel: [
            { name: "id", width: 50, resizable: false, search: false, formatter: optionsFormatter, align: "center" },
            { name: "levelName", width: 125, hidden: categoryType != 1 },
            { name: "playerUsers", width: 160, formatter: playerFormatter },
            { name: "platformName", width: 160 },
            { name: "isEmulated", width: 125 },
            { name: "primaryRunTimeString", width: 160, search: false },
            { name: "statusTypeString", width: 125 },
            { name: "examinerName", width: 160 },
            { name: "rejectedReason", width: 160, hidden: true },
            { name: "dateSubmitted", width: 160, search: false, sorttype: "date", formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y H:i" }, cellattr: dateSubmittedCellAttr },
            { name: "comment", width: 100, search: false, formatter: commentFormatter, align: "center" },
            { name: "relativeDateSubmittedString", hidden: true },
            { name: "relativeVerifyDateString", hidden: true },
            { name: "playerGuests", hidden: true }
        ],
        iconSet: "fontAwesome",
        guiStyle: "bootstrap4",
        ignoreCase: true,
        viewrecords: true,
        loadonce: true,
        loadComplete: gridLoadComplete
    });

    function gridLoadComplete(element) {
        initializeGridEvents(this);
        initializeGridFilters(this);
        initializeGridStyles(this);
        var $gridContainer = $(this).closest('.grid-container');
        initializeScroller($gridContainer);
    }

    function initializeGridEvents(element) {
        $grid = $(element);
        $grid.find('[data-toggle="modal"]').click(function () {
            var $target = $($(this).data("target"));
            $target.find('.modal-body').hide();
            $target.find('.modal-loading').show();

            $.get($(this).attr("href"), function (data) {
                var $header = $(data).find('.header');
                var $body = $(data).find('.header').next();
                $target.find('.modal-header .close').prevAll().remove();
                $target.find('.modal-header').prepend($header);
                $target.find('.modal-body').html($body);

                $target.find('.modal-loading').hide();
                $target.find('.modal-body').show();
            });
        });

        $grid.find('[data-toggle="tooltip"]').tooltip();
    }

    function initializeGridFilters(element) {
        var gridData = $(element).jqGrid("getGridParam", "data");
        var levelNames = GetUniqueValues($.map(gridData, function (item) { return item.levelName; })).sort();
        var rankStrings = GetUniqueValues($.map(gridData, function (item) { return item.rankString; })).sort();
        var categoryNames = GetUniqueValues($.map(gridData, function (item) { return item.categoryName; })).sort();
        var platformNames = GetUniqueValues($.map(gridData, function (item) { return item.platformName; })).sort();
        var emulatorStrings = GetUniqueValues($.map(gridData, function (item) { return item.isEmulated; })).sort();
        var examinerNames = GetUniqueValues($.map(gridData, function (item) { return item.examinerName; })).sort();
        var statuses = GetUniqueValues($.map(gridData, function (item) { return item.statusTypeString; })).sort();
        var playerNames = [];
        $(gridData).each(function () {
            var playerUsers = this.playerUsers;
            var playerGuests = this.playerGuests;

            $(playerUsers).each(function () {
                if (!playerNames.indexOf(this.name) > -1) {
                    playerNames.push(this.name);
                }
            });

            $(playerGuests).each(function () {
                if (!playerNames.indexOf(this.name)) {
                    playerNames.push(this.name);
                }
            });
        });
        playerNames = playerNames.sort();

        setSearchSelect($(element), 'levelName', levelNames);
        setSearchSelect($(element), 'rankString', rankStrings);
        setSearchSelect($(element), 'playerUsers', playerNames);
        setSearchSelect($(element), 'categoryName', categoryNames);
        setSearchSelect($(element), 'platformName', platformNames);
        setSearchSelect($(element), 'isEmulated', emulatorStrings);
        setSearchSelect($(element), 'examinerName', examinerNames);
        setSearchSelect($(element), 'statusTypeString', statuses);
        $(element).jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
    }

    function initializeGridStyles(element) {
        var $grid = $(element);
        var gridData = $grid.jqGrid("getGridParam", "data");
        var $rejectedItems = $(gridData).filter(function (item) { return item.statusTypeString == "Rejected"; })
        if ($rejectedItems.length > 0) {
            $grid.jqGrid("showCol", ["rejectedReason"]);
        }

        var $gridContainer = $grid.closest('.grid-container');
        $gridContainer.css('width', parseInt($gridContainer.find('.ui-jqgrid-view').width()) + parseInt($gridContainer.css('padding-left')));
    }

    function setSearchSelect(grid, columnName, searchData) {
        grid.jqGrid("setColProp", columnName, {
            stype: "select",
            searchoptions: {
                value: buildSearchSelect(searchData),
                sopt: ["eq"]
            }
        });
    }

    function buildSearchSelect(uniqueNames) {
        var values = ":All";
        $.each(uniqueNames, function () {
            values += ";" + this + ":" + this;
        });
        return values;
    }

    function optionsFormatter(value, options, rowObject) {
        return "<a href='../SpeedRun/SpeedRunSummary?speedRunID=" + value + "' data-toggle='modal' data-target='#videoLinkModal' data-backdrop='static'><i class='fas fa-play-circle'></i></a>";
    }

    function playerFormatter(value, options, rowObject) {
        var html = '';
        var users = value;
        var guests = rowObject.playerGuests;
        var currentUserID = $('#hdnUserID').val();

        $(users).each(function () {
            var user = this;
            if (user.id == currentUserID) {
                html += user.name + "<br/>";
            } else {
                html += "<a href='../User/UserDetails?userID=" + user.id + "'>" + user.name + "</a><br/>";
            }
        });

        $(guests).each(function () {
            var guest = this;
            html += guest.name + "<br/>";
        });

        return html;
    }

    function commentFormatter(value, options, rowObject) {
        var html = '';
        if (value != null) {
            html = '<i class="far fa-comment" data-toggle="tooltip" data-placement="bottom" data-html="true" title="' + value + '"></i>'
        }

        return html;
    }

    function dateSubmittedCellAttr(rowId, val, rowObject, cm, rdata) {
        return ' title="' + rowObject.relativeDateSubmittedString + '"';
    }

    function verifyDateCellAttr(rowId, val, rowObject, cm, rdata) {
        return ' title="' + rowObject.relativeVerifyDateString + '"';
    }
}

/**Ajax functions **/
//Search
function runSearch() {
    var formData = new FormData($('#frmSearch')[0]);
    formData.append("userID", $('#hdnUserID').val());

    $.ajax({
        url: 'SearchUserSpeedRunGrid',
        processData: false,
        contentType: false,
        type: 'POST',
        data: formData,
        success: function (data) {
            if (data != null) {
                $('#divUserSpeedRunGrid').html(data);
                initializeGridContainerEvents();

                var $activeCategoryTypeTab = $('.nav-item.categoryType a.active');
                onCategoryTypeTabClick($activeCategoryTypeTab);
            }
        }
    });
}

/*
function initializeCharts(element) {
    var $chartsContainer = $(element);
    var gameID = $(element).data('gameid');
    var categoryType = $(element).data('categorytype');
    var categoryID = $(element).data('categoryid');
    var levelID = $(element).data('levelid');

    var templateLoader = function () {
        return {
            load: function (path, params, callback, failCallback) {
                sra.templateHelper.getTemplateFromUrl(path, params, callback, failCallback);
            },
        };
    }();
 
    var dashload = new dashboardLoader($chartsContainer, 'div[data-index]', _);
 
    sra.ajaxHelper.get('GetGameDetailsCharts', {},
        function (charts) {
            var chartHandler = function(chartLoader, selector, graphObj) {
                var _chartLoader = chartLoader;
                var _selector = $(selector);
                var _graphObj = graphObj;

                _selector.empty();
                templateLoader.load('../templates/ChartPlaceholder.html', {}, function (html) {
                    var controller = _graphObj.controller(_selector, sra.dateHelper, gameID, categoryType, categoryID, levelID);
 
                    _chartLoader.RenderComponent(_selector, html);
 
                    controller.preRender($.Deferred()).then(function () {
                        controller.postRender($.Deferred()).then(function () {
                        });
                    });
                });
            };
 
            var noChartHandler = function (chartLoader, selector) {
                templateLoader.load('../templates/ChartPlaceholder.html', { msg: 'No Chart Found' }, function (html) {
                    chartLoader.RenderComponent(selector, html);
                });
            };
 
            dashload.AddComponents(sra.graphObjects, charts, chartHandler, noChartHandler);
        }
    , function () {
        var html = templateLoader.load('../templates/ChartError.html', undefined, function (html) {
            $chartsContainer.html(html);
        }, $.noop);
    }, $.noop);
}
*/





 


















