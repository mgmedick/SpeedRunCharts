function InitializeClient() {
    var $activeCategoryTypeTab = $('.nav-item.categoryType a.active');
    OnCategoryTypeTabSingleClick($activeCategoryTypeTab);

    InitializeEvents();
    InitializeCharts();
    InitializeScroller();
}

function InitializeEvents() {
    $('.nav-item.categoryType a').one('shown.bs.tab', function () {
        OnCategoryTypeTabSingleClick(this);
    });

    $('.nav-item.category a').one('shown.bs.tab', function () {
        OnCategoryTabSingleClick(this);
    });
}

function OnCategoryTypeTabSingleClick(element) {
    var jqCategoryTypeContainerID = $(element).attr("href");
    var $activeCategoryTab = $(jqCategoryTypeContainerID).find('.category a.active');

    OnCategoryTabSingleClick($activeCategoryTab);
}

function OnCategoryTabSingleClick(element) {
    var jqCategoryContainerID = $(element).attr("href");
    var $gridContainer = $(jqCategoryContainerID);
    InitializeGrid($gridContainer);
}

function InitializeGrid(element) {
    var $grid = $(element).find('.grid');
    var pagerID = $(element).find('.pager').attr("id");
    var gameID = $(element).data('gameid');
    var categoryType = $(element).data('categorytype');
    var categoryID = $(element).data('categoryid');
    var levelIDs = '';
    $(_levels).each(function () {
        levelIDs += this.id + ',';
    })
    levelIDs = levelIDs.replace(/,\s*$/, "");

    $grid.jqGrid({
        url: 'GetLeaderboardRecords?gameID=' + gameID + '&categoryType=' + categoryType + '&categoryID=' + categoryID + '&levelIDs=' + levelIDs,
        datatype: "json",
        mtype: "GET",
        height: '100%',
        autowidth: true,
        shrinkToFit: true,
        rowNum: 50,
        pager: pagerID,
        colNames: ["", "Level", "Rank", "Player", "Platform", "Time", "Examiner", "Date", "Hidden"],
        colModel: [
            { name: "id", width: 50, resizable:false, search: false, formatter: OptionsFormatter, align: "center" },
            { name: "levelName" },
            { name: "rankString", sorttype: "number" },
            { name: "playerName" },
            { name: "platformName" },
            { name: "primaryRunTimeString", search: false },
            { name: "examinerName" },
            { name: "dateSubmitted", search: false, sorttype: "date", formatter: "date", formatoptions: {srcformat: "ISO8601Long", newformat: "m/d/Y H:i"}, cellattr: DateSubmittedCellAttr },
            { name: "relativeDateSubmittedString", hidden: true }
        ],
        iconSet: "fontAwesome",
        guiStyle: "bootstrap4",
        ignoreCase: true,
        viewrecords: true,
        loadonce: true,
        loadComplete: GridLoadComplete
    });

    function GridLoadComplete() {
        InitializeGridEvents();
        InitializeGridFilters(this);

        if (categoryType != 1) {
            $(this).jqGrid('hideCol', ["levelName"]);
        }
    }

    function InitializeGridEvents() {
        $('[data-toggle="modal"]').click(function () {
            $($(this).data("target") + ' .modal-body').load($(this).attr("href"));
        });
    }

    function InitializeGridFilters(element) {
        var gridData = $(element).jqGrid("getGridParam", "data");
        var levelNames = GetUniqueValues($.map(gridData, function (item) { return item.levelName; }));
        var rankStrings = GetUniqueValues($.map(gridData, function (item) { return item.rankString; }));
        var playerNames = GetUniqueValues($.map(gridData, function (item) { return item.playerName; }));
        var categoryNames = GetUniqueValues($.map(gridData, function (item) { return item.categoryName; }));
        var platformNames = GetUniqueValues($.map(gridData, function (item) { return item.platformName; }));
        var examinerNames = GetUniqueValues($.map(gridData, function (item) { return item.examinerName; }));

        SetSearchSelect($(element), 'levelName', levelNames);
        SetSearchSelect($(element), 'rankString', rankStrings);
        SetSearchSelect($(element), 'playerName', playerNames);
        SetSearchSelect($(element), 'categoryName', categoryNames);
        SetSearchSelect($(element), 'platformName', platformNames);
        SetSearchSelect($(element), 'examinerName', examinerNames);
        $(element).jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
    }

    function SetSearchSelect(grid, columnName, searchData) {
        grid.jqGrid("setColProp", columnName, {
            stype: "select",
            searchoptions: {
                value: BuildSearchSelect(searchData),
                sopt: ["eq"]
            }
        });
    }

    function BuildSearchSelect(uniqueNames) {
        var values = ":All";
        $.each(uniqueNames, function () {
            values += ";" + this + ":" + this;
        });
        return values;
    }

    function OptionsFormatter(cellvalue, options, rowObject) {
        return "<a href='SpeedRunSummary?speedRunID=" + cellvalue + "' data-toggle='modal' data-target='#videoLinkModal' data-backdrop='static'><i class='fas fa-play-circle'></i></a>";
        //return "<a href='' data-toggle='modal' data-target='#videoLinkModal'><i class='fas fa-video'></i></a>";
    }

    function DateSubmittedFormatter(cellvalue, options, rowObject) {
        return rowObject.relativeDateSubmittedString;
        //return "<a href='' data-toggle='modal' data-target='#videoLinkModal'><i class='fas fa-video'></i></a>";
    }

    function DateSubmittedCellAttr(rowId, val, rowObject, cm, rdata) {
        return ' title="' + rowObject.relativeDateSubmittedString + '"';
    }

    //var data = $grid.getGridParam('data');
    //return data;
}

function InitializeCharts() {
    var gameID = _gameID;
    var top = 20; 
    var gameCategoryIDs = '';
    $(_categories).each(function () {
        if(this.type == "0") {
            gameCategoryIDs += this.id + ',';
        }
    });
    gameCategoryIDs = gameCategoryIDs.replace(/,\s*$/, "");

    
    var templateLoader = function () {
        return {
            load: function (path, params, callback, failCallback) {
                speedRun.templateHelper.getTemplateFromUrl(path, params, callback, failCallback);
            },
        };
    }();
 
    var dashload = new dashboardLoader($('div#summaryCharts'), 'div[data-index]', _);
 
    speedRun.ajaxHelper.get('GetGameDetailsCharts', {},
        function (charts) {
            var chartHandler = function(chartLoader, selector, graphObj) {
                var _selector = $(selector);
                var _graphObj = graphObj;
                var _chartLoader = chartLoader;
 
                templateLoader.load('../templates/ChartPlaceholder.html', { }, function(html) {
                    var controller = _graphObj.controller(_selector, gameID, gameCategoryIDs, top);
 
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
 
            dashload.AddComponents(speedRun.graphObjects, charts, chartHandler, noChartHandler);
        }
    , function () {
        var html = templateLoader.load('../templates/ChartError.html', undefined, function (html) {
            $('div#summaryCharts').html(html);
        }, $.noop);
    }, $.noop);
}




 


















