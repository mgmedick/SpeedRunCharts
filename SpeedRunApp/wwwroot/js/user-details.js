var _categoryTypes;
var _games;
var _categories;
var _levels;

function initializeClient(categoryTypes, games, categories, levels) {
    _categoryTypes = categoryTypes;
    _games = games;
    _categories = categories;
    _levels = levels;

    var $activeCategoryTypeTab = $('.nav-item.categoryType a.active');

    initializeEvents();
    initializeScroller();

    $activeCategoryTypeTab.trigger('click');
}

function initializeEvents() {
    $('.chosen').chosen({ width: "250px" });
    $('.date').datepicker();
    $('#divSearch').setupCollapsible({ initialState: "visible", linkHiddenText: "Show Filters", linkDisplayedText: "Hide Filters" });

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

    $('#drpCategoryTypes').change(function () {
        onCategoryTypeChange(this);
    });

    $('#drpGames').change(function () {
        onGameChange(this);
    });
}

function runSearch() {
    var formData = new FormData($('#frmSearch')[0]);

    $.ajax({
        url: 'UserDetailsGrid',
        processData: false,
        contentType: false,
        type: 'POST',
        data: formData,
        success: function (data) {
            if (data != null) {
               $('#divUserDetailsGrid').html(data);
            }
        }
    });
}

function onCategoryTypeChange(element) {
    var selectedCategoryTypeIDs = $(element).val();
    //var selectedGameIDs = $('#drpGames').val();

    var $games = $(_games).filter(function () {
        return (selectedCategoryTypeIDs.length == 0 || $(selectedCategoryTypeIDs).filter(this.categoryTypeIDs).length > 0);
    });

    var $categories = $(_categories).filter(function () {
        return (selectedCategoryTypeIDs.length == 0 || selectedCategoryTypeIDs.indexOf(this.type) > -1);
    });

    //var $games = $(_games).filter(function () {
    //    var gameID = this.id;
    //    return $categories.filter(function () { return this.gameID == gameID; }).length > 0
    //});

    //var $categoryTypes = $(_categoryTypes).filter(function () {
    //    var categoryTypeID = this.id;
    //    return $categories.filter(function () { return this.type == categoryTypeID; }).length > 0
    //});


    populateDropDown($('#drpGames'), $games);
    populateDropDown($('#drpCategories'), $categories);

    //if (selectedCategoryTypeIDs.length == 0) {
    //    populateDropDown($('#drpCategoryTypes'), $categoryTypes);
    //}

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
        return (selectedGameIDs.length == 0 || selectedGameIDs.indexOf(this.gameID) > -1) && (selectedCategoryTypeIDs.length == 0 || selectedCategoryTypeIDs.indexOf(this.type) > -1);
    });

    var $levels = $(_levels).filter(function () {
        return (selectedGameIDs.length == 0 || selectedGameIDs.indexOf(this.gameID) > -1);
    });

    //var $categories = $(_categories).filter(function () {
    //    return (selectedGameIDs.length == 0 || selectedGameIDs.indexOf(this.gameID) > -1) && (selectedCategoryTypeIDs.length == 0 || selectedCategoryTypeIDs.indexOf(this.type.toString()) > -1);
    //});

    //var $categoryTypes = $(_categoryTypes).filter(function () {
    //    var categoryTypeID = this.id;
    //    return $categories.filter(function () { return this.type == categoryTypeID; }).length > 0
    //});

    //var $games = $(_games).filter(function () {
    //    var gameID = this.id;
    //    return $categories.filter(function () { return this.gameID == gameID; }).length > 0
    //});

    //var $levels = $(_levels).filter(function () {
    //    var gameID = this.id;
    //    return $games.filter(function () { return this.gameID == gameID; }).length > 0
    //});

    populateDropDown($('#drpCategories'), $categories);
    populateDropDown($('#drpLevels'), $levels);

    //if (selectedGameIDs.length == 0) {
    //    populateDropDown($('#drpGames'), $games);
    //}
}

/*
function onCategoryChange(element) {
    var selectedCategoryIDs = $(element).val();
    var selectedGameIDs = $('#drpGames').val();
    var selectedCategoryTypeIDs = $('#drpCategoryTypes').val();

    var $selectedCategories = $(_categories).filter(function () {
        return (selectedCategoryIDs.length == 0 || selectedCategoryIDs.indexOf(this.id) > -1);
    });

    var $categories = $(_categories).filter(function () {
        return (selectedGameIDs.length == 0 || selectedGameIDs.indexOf(this.gameID) > -1) && (selectedCategoryTypeIDs.length == 0 || selectedCategoryTypeIDs.indexOf(this.type.toString()) > -1);
    });

    var $categoryTypes = $(_categoryTypes).filter(function () {
        var categoryTypeID = this.id;
        return $selectedCategories.filter(function () { return this.type == categoryTypeID; }).length > 0
    });

    var $games = $(_games).filter(function () {
        var gameID = this.id;
        return $selectedCategories.filter(function () { return this.gameID == gameID; }).length > 0
    });

    populateDropDown($('#drpCategoryTypes'), $categoryTypes);
    populateDropDown($('#drpGames'), $games);

    if (selectedCategoryIDs.length == 0) {
        populateDropDown($('#drpCategories'), $categories);
    }
}
*/

/*
function onSearchListChange() {
    var selectedCategoryIDs = $('#drpCategories').val();
    var selectedCategoryTypeIDs = $('#drpCategoryTypes').val();
    var selectedGameIDs = $('#drpGames').val();

    var $categories = $(_categories).filter(function () {
        return (selectedCategoryIDs.length == 0 || selectedCategoryIDs.indexOf(this.id) > -1) && (selectedCategoryTypeIDs.length == 0 || selectedCategoryTypeIDs.indexOf(this.type.toString()) > -1) && (selectedGameIDs.length == 0 || selectedGameIDs.indexOf(this.gameID) > -1);
    });

    var $categoryTypes = $(_categoryTypes).filter(function () {
        var categoryTypeID = this.id;
        return $categories.filter(function () { return this.type == categoryTypeID }).length > 0
    });

    var $games = $(_games).filter(function () {
        var gameID = this.id;
        return $categories.filter(function () { return this.gameID == gameID }).length > 0
    });

    //var $levels = $(_levels).filter(function () {
    //    var gameID = this.gameID;
    //    return selectedCategoryTypeID == 1 && $games.filter(function () { return this.id == gameID; }).length > 0
    //});

    populateDropDown($('#drpCategories'), $categories);
    populateDropDown($('#drpCategoryTypes'), $categoryTypes);
    populateDropDown($('#drpGames'), $games);
}
*/

function populateDropDown(element, items) {
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

function initializeGrid(element) {
    var grid = $(element).find('.grid');
    var pagerID = $(element).find('.pager').attr("id");
    var userID = $(element).data('userid');
    var gameID = $(element).data('gameid');
    var categoryType = $(element).data('categorytype');
    var categoryID = $(element).data('categoryid');
    var levelID = $(element).data('levelid') ? $(element).data('levelid') : '';

    grid.jqGrid({
        url: 'UserDetailsGrid_Read?userID=' + userID + '&gameID=' + gameID + '&categoryType=' + categoryType + '&categoryID=' + categoryID + '&levelID=' + levelID,
        datatype: "json",
        mtype: "GET",
        height: '100%',
        //autowidth: true,
        //shrinkToFit: true,
        rowNum: 50,
        pager: pagerID,
        colNames: ["", "Level", "Player", "Platform", "Time", "Examiner", "Date", "Hidden"],
        colModel: [
            { name: "id", width: 50, resizable: false, search: false, formatter: optionsFormatter, align: "center" },
            { name: "levelName", width: 125, hidden: categoryType != 1},
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





 


















