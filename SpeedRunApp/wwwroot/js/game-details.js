﻿function initializeClient() {
    var $activeCategoryTypeTab = $('.nav-item.categoryType a.active');

    initializeEvents();
    initializeScroller();

    $activeCategoryTypeTab.trigger('click');
}

function initializeEvents() {
    $('.nav-item.categoryType a').click(function () {
        onCategoryTypeTabClick(this);
    });

    $('.nav-item.category a').click(function () {
        onCategoryTabClick(this);
    });

    $('.nav-item.level a').click(function () {
        onLevelTabClick(this);
    });
}

function onCategoryTypeTabClick(element) {
    var categoryTypeContainerID = $(element).attr('href');
    var $container = $(categoryTypeContainerID);
    var categoryTypeChartContainerID = categoryTypeContainerID + '-charts';
    var $chartContainer = $(categoryTypeChartContainerID);

    $('.categoryType-tab-pane').hide();
    $container.fadeIn();

    $('.categoryType-tab-pane-charts').hide();
    $chartContainer.fadeIn();

    var $activeCategoryTab = $container.find('.category a.active');
    onCategoryTabClick($activeCategoryTab);
}

function onCategoryTabClick(element) {
    var categoryContainerID = $(element).attr('href');
    var $container = $(categoryContainerID);
    var categoryChartContainerID = categoryContainerID + '-charts';
    var $chartContainer = $(categoryChartContainerID);

    $('.category-tab-pane').hide();
    $container.fadeIn();

    $('.category-tab-pane-charts').hide();
    $chartContainer.fadeIn();

    if ($(element).data('categorytype') == 1) {
        $container.find('.level-tabs').show();
        $container.find('.level-results').show();
        $container.find('.category-results').hide();

        $chartContainer.find('.level-results-charts').show();

        var $activeLevelTab = $container.find('.level a.active');
        onLevelTabClick($activeLevelTab);
    } else {
        var $activeCategoryPane = $container.find('.category-results');
        var $activeCategoryChartsPane = $chartContainer.find('.category-results-charts');

        if (!$activeCategoryPane.find('.grid')[0].grid) {
            initializeGrid($activeCategoryPane);
            initializeCharts($activeCategoryChartsPane);
        }

        $container.find('.level-tabs').hide();
        $container.find('.level-results').hide();
        $activeCategoryPane.show();

        $chartContainer.find('.level-results-charts').hide();
        $activeCategoryChartsPane.show();
    }
}

function onLevelTabClick(element) {
    var levelContainerID = $(element).attr('href');
    var $container = $(levelContainerID);
    var levelChartContainerID = levelContainerID + '-charts';
    var $chartContainer = $(levelChartContainerID);

    if (!$container.find('.grid')[0].grid) {
        initializeGrid($container);
        initializeCharts($chartContainer);
    }

    $('.level-tab-pane').hide();
    $container.fadeIn();

    $('.level-tab-pane-charts').hide();
    $chartContainer.fadeIn();
}

function initializeGrid(element) {
    var grid = $(element).find('.grid');
    var pagerID = $(element).find('.pager').attr("id");
    var gameID = $(element).data('gameid');
    var categoryType = $(element).data('categorytype');
    var categoryID = $(element).data('categoryid');
    var levelID = $(element).data('levelid');

    grid.jqGrid({
        url: 'GameDetails_Read?gameID=' + gameID + '&categoryType=' + categoryType + '&categoryID=' + categoryID + '&levelID=' + levelID,
        datatype: "json",
        mtype: "GET",
        height: '100%',
        //autowidth: true,
        //shrinkToFit: true,
        rowNum: 50,
        pager: pagerID,
        colNames: ["", "Level", "Rank", "Player", "Platform", "Time", "Examiner", "Date", "Hidden"],
        colModel: [
            { name: "id", width: 50, resizable: false, search: false, formatter: optionsFormatter, align: "center" },
            { name: "levelName", width: 125, hidden: categoryType != 1},
            { name: "rankString", width: 75, sorttype: "number" },
            { name: "playerName", width: 160 },
            { name: "platformName", width: 160 },
            { name: "primaryRunTimeString", width: 160 , search: false },
            { name: "examinerName", width: 160  },
            { name: "dateSubmitted", width: 160, search: false, sorttype: "date", formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y H:i" }, cellattr: dateSubmittedCellAttr },
            { name: "relativeDateSubmittedString", hidden: true }
        ],
        iconSet: "fontAwesome",
        guiStyle: "bootstrap4",
        ignoreCase: true,
        viewrecords: true,
        loadonce: true,
        loadComplete: gridLoadComplete
    });

    function gridLoadComplete() {
        initializeGridEvents();
        initializeGridFilters(this);
        initializeGridStyles(this);
        //initializeScroller();
    }

    function initializeGridEvents() {
        $('[data-toggle="modal"]').click(function () {
            $($(this).data("target") + ' .modal-body').load($(this).attr("href"));
        });
    }

    function initializeGridFilters(element) {
        var gridData = $(element).jqGrid("getGridParam", "data");
        var levelNames = GetUniqueValues($.map(gridData, function (item) { return item.levelName; }));
        var rankStrings = GetUniqueValues($.map(gridData, function (item) { return item.rankString; }));
        var playerNames = GetUniqueValues($.map(gridData, function (item) { return item.playerName; }));
        var categoryNames = GetUniqueValues($.map(gridData, function (item) { return item.categoryName; }));
        var platformNames = GetUniqueValues($.map(gridData, function (item) { return item.platformName; }));
        var examinerNames = GetUniqueValues($.map(gridData, function (item) { return item.examinerName; }));

        setSearchSelect($(element), 'levelName', levelNames);
        setSearchSelect($(element), 'rankString', rankStrings);
        setSearchSelect($(element), 'playerName', playerNames);
        setSearchSelect($(element), 'categoryName', categoryNames);
        setSearchSelect($(element), 'platformName', platformNames);
        setSearchSelect($(element), 'examinerName', examinerNames);
        $(element).jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
    }

    function initializeGridStyles(element) {
        var $grid = $(element);
        var $gridContainer = $grid.closest('.grid-container');
        $gridContainer.css('width', $gridContainer.find('.ui-jqgrid-view').width());
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

    function optionsFormatter(cellvalue, options, rowObject) {
        return "<a href='../SpeedRun/SpeedRunSummary?speedRunID=" + cellvalue + "' data-toggle='modal' data-target='#videoLinkModal' data-backdrop='static'><i class='fas fa-play-circle'></i></a>";
    }

    function dateSubmittedFormatter(cellvalue, options, rowObject) {
        return rowObject.relativeDateSubmittedString;
    }

    function dateSubmittedCellAttr(rowId, val, rowObject, cm, rdata) {
        return ' title="' + rowObject.relativeDateSubmittedString + '"';
    }
}

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






 


















