function initializeClient() {
    var $activeCategoryTypeTab = $('.nav-item.categoryType a.active');

    initializeEvents();
    initializeScroller();

    $activeCategoryTypeTab.trigger('click');
}

function initializeEvents() {
    //shown.bs.tab
    $('.nav-item.categoryType a').one('click', function () {
        onCategoryTypeTabSingleClick(this);
    });

    $('.nav-item.categoryType a').click(function () {
        onCategoryTypeTabClick(this);
    });

    $('.nav-item.category a').one('click', function () {
        onCategoryTabSingleClick(this);
    });

    $('.nav-item.category a').click(function () {
        onCategoryTabClick(this);
    });

    $('.nav-item.level a').one('click', function () {
        onLevelTabSingleClick(this);
    });

    $('.nav-item.level a').click(function () {
        onLevelTabClick(this);
    });

    /*
    $('.nav-item.category a').click(function (e) {
        e.preventDefault();
        var gridContainerID = $(this).attr('href');

        $('.charts-container').hide();
        $(gridContainerID + '-Charts').fadeIn();

        $(this).tab('show');
    });
    */

}

function onCategoryTypeTabSingleClick(element) {
    var categoryTypeContainerID = $(element).attr('href');
    var chartCategoryTypeContainerID = categoryTypeContainerID + '-charts';
    var $container = $(categoryTypeContainerID);
    var $chartContainer = $(chartCategoryTypeContainerID);

    var $activeCategoryTab = $container.find('.category a.active');

    if ($(element).data('categorytype') == 1) {
        $container.find('.level.first-item a').removeClass('active');
        $container.find('.level:not(.first-item):first a').addClass('active');
        $container.find('.level-tabs').show();

        $chartContainer.find('.level.first-item a').removeClass('active');
        $chartContainer.find('.level:not(.first-item):first a').addClass('active');
        $chartContainer.find('.level-tabs').show();
    } else {
        $container.find('.level:not(.first-item):first a').removeClass('active');
        $container.find('.level.first-item a').addClass('active');
        $container.find('.level-tabs').hide();

        $chartContainer.find('.level:not(.first-item):first a').removeClass('active');
        $chartContainer.find('.level.first-item a').addClass('active');
        $chartContainer.find('.level-tabs').hide();
    }

    var $activeCategoryTab = $container.find('.category a.active');

    $activeCategoryTab.trigger('click');
}

function onCategoryTypeTabClick(element) {
    var categoryTypeContainerID = $(element).attr('href');
    var chartCategoryTypeContainerID = categoryTypeContainerID + '-charts';
    var $container = $(categoryTypeContainerID);
    var $chartContainer = $(chartCategoryTypeContainerID);

    var $activeCategoryTab = $container.find('.category a.active');

    if ($(element).data('categorytype') == 1) {
        $container.find('.level.first-item a').removeClass('active');
        $container.find('.level:not(.first-item):first a').addClass('active');
        $container.find('.level-tabs').show();

        $chartContainer.find('.level.first-item a').removeClass('active');
        $chartContainer.find('.level:not(.first-item):first a').addClass('active');
        $chartContainer.find('.level-tabs').show();
    } else {
        $container.find('.level:not(.first-item):first a').removeClass('active');
        $container.find('.level.first-item a').addClass('active');
        $container.find('.level-tabs').hide();

        $chartContainer.find('.level:not(.first-item):first a').removeClass('active');
        $chartContainer.find('.level.first-item a').addClass('active');
        $chartContainer.find('.level-tabs').hide();
    }

    $('.categoryType-tab-pane').hide();
    $container.fadeIn();

    $('.categoryType-tab-pane-charts').hide();
    $chartContainer.fadeIn();

    onCategoryTabClick($activeCategoryTab);
}

function onCategoryTabSingleClick(element) {
    var categoryContainerID = $(element).attr("href");
    var $activeLevelTab = $(categoryContainerID).find('.level a.active');

    $activeLevelTab.trigger('click');

    //var containerID = $(element).attr("href");
    //var $container = $(containerID);

    //if ($(element).data('categorytype') == 1) {
    //    var $activeLevelTab = $('.level a.active');
    //    $activeLevelTab.trigger('click');
    //} else {
    //    initializeGrid($container);
    //}

    //var $chartsContainer = $(gridContainerID + "-Charts");
    //initializeCharts($chartsContainer);
}

function onCategoryTabClick(element) {
    var categoryContainerID = $(element).attr('href');
    var chartCategoryContainerID = categoryContainerID + '-charts';
    var $activeLevelTab = $(categoryContainerID).find('.level a.active');

    $('.category-tab-pane').hide();
    $(categoryContainerID).fadeIn();

    $('.category-tab-pane-charts').hide();
    $(chartCategoryContainerID).fadeIn();

    onLevelTabClick($activeLevelTab);
}

function onLevelTabSingleClick(element) {
    var gridContainerID = $(element).attr("href");
    var chartsContainerID = gridContainerID + '-charts'
    var $gridContainer = $(gridContainerID);
    var $chartsContainer = $(chartsContainerID);

    initializeGrid($gridContainer);
    initializeCharts($chartsContainer);
}

function onLevelTabClick(element) {
    var levelContainerID = $(element).attr('href');
    var chartLevelContainerID = levelContainerID + '-charts';

    $('.level-tab-pane').hide();
    $(levelContainerID).fadeIn();

    $('.level-tab-pane-charts').hide();
    $(chartLevelContainerID).fadeIn();
}

function initializeGrid(element) {
    var grid = $(element).find('.grid');
    var pagerID = $(element).find('.pager').attr("id");
    var gameID = $(element).data('gameid');
    var categoryType = $(element).data('categorytype');
    var categoryID = $(element).data('categoryid');
    var levelID = $(element).data('levelid');

    grid.jqGrid({
        url: 'GetLeaderboardRecords?gameID=' + gameID + '&categoryType=' + categoryType + '&categoryID=' + categoryID + '&levelID=' + levelID,
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
            { name: "levelName", width: 50, hidden: categoryType != 1},
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

        //if (categoryType != 1) {
        //    $(this).jqGrid('hideCol', ["levelName"]);
        //}

        //$('.grid-container').show();
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
        return "<a href='SpeedRunSummary?speedRunID=" + cellvalue + "' data-toggle='modal' data-target='#videoLinkModal' data-backdrop='static'><i class='fas fa-play-circle'></i></a>";
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
    var categoryID = $(element).data('categoryid');

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
                    var controller = _graphObj.controller(_selector, sra.dateHelper, gameID, categoryID);
 
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






 


















