if (!sra) {
    var sra = {};
}

function speedRunGridModel(sender, categoryTypes, games, categories, levels, speedRunRecords) {
    this.sender = sender,
    this.categoryTypes = categoryTypes,
    this.games = games,
    this.categories = categories,
    this.levels = levels,
    this.speedRunRecords = speedRunRecords
}

/**Initialize Event Functions**/
function initializeClient(searchCategoryTypes, searchGames, searchCategories, searchLevels) {
    initalizeConstants(searchCategoryTypes, searchGames, searchCategories, searchLevels);
    initializeEvents();
}

function initalizeConstants(searchCategoryTypes, searchGames, searchCategories, searchLevels, maxElementsPerPage) {
    sra['searchCategoryTypes'] = searchCategoryTypes;
    sra['searchGames'] = searchGames;
    sra['searchCategories'] = searchCategories;
    sra['searchLevels'] = searchLevels;
}

function initializeEvents() {
    $('.chosen').chosen({ width: "250px" });
    $('.date').datepicker();
    $('#divSearch').setupCollapsible({ initialState: "hidden", linkHiddenText: "Show Filters", linkDisplayedText: "Hide Filters" });
    $('#divChartContainer').setupCollapsible({ initialState: "visible", linkHiddenText: "Show Charts", linkDisplayedText: "Hide Charts" });

    $('#btnSearch').click(runSearch);

    $('#drpCategoryTypes').change(function () {
        onCategoryTypeChange(this);
    });

    loadSpeedRunGridTemplate();
    initializeScrollerGlobalEvents();
}

function loadSpeedRunGridTemplate() {
    $('#divSpeedRunGrid').hide();
    $('#divSpeedRunGridLoading').show();

    getSpeedRunGridData().then(function (data) {
        sra['speedRunRecords'] = data;
        var gridModel = new speedRunGridModel("Game", sra.searchCategoryTypes, sra.searchGames, sra.searchCategories, sra.searchLevels, sra.speedRunRecords);
        sra['speedRunGridModel'] = gridModel;

        $.get('../templates/SpeedRunGrid.html?_t=' + (new Date()).getTime(), function (template, status) {
            sra['speedRunGridTemplate'] = template;
            renderAndInitializeSpeedRunGrid($('#divSpeedRunGrid'), sra.speedRunGridTemplate, sra.speedRunGridModel);

            $('#divSpeedRunGrid').show();
            $('#divSpeedRunGridLoading').hide();
        });
    });
}

function getSpeedRunGridData() {
    var def1 = $.Deferred();
    var gameID = $("#hdnGameID").val();
    var elementsPerPage = sra.apiSettings.maxElementsPerPage;
    var elementsOffset = 0;
    var requestCount = 0;

    getAllGameSpeedRunRecords(gameID, elementsPerPage, elementsOffset, requestCount).then(function (data) {
        setGameSpeedRunRank(data);
        data.sort(function (a, b) { return a.PrimaryRunTimeMilliseconds - b.PrimaryRunTimeMilliseconds; });
        def1.resolve(data);
    });

    return def1.promise();
}

function renderAndInitializeSpeedRunGrid(element, speedRunGridTemplate, speedRunGridModel) {
    renderTemplate($('#divSpeedRunGrid'), speedRunGridTemplate, speedRunGridModel);
    initializeSpeedRunGridEvents();

    var $activeCategoryTypeTab = $('.nav-item.categoryType a.active');
    onCategoryTypeTabClick($activeCategoryTypeTab);
}

function getAllGameSpeedRunRecords(gameID, elementsPerPage, elementsOffset, requestCount, items, def) {
    if (!def) {
        var def = new $.Deferred();
    }

    if (!items) {
        var items = [];
    }

    getGameSpeedRunRecords(gameID, elementsPerPage, elementsOffset).then(function (data) {
        requestCount++;
        items = items.concat(data);

        if (data.length == elementsPerPage) {
            return setTimeout(function () { getAllGameSpeedRunRecords(gameID, elementsPerPage, (elementsPerPage * requestCount), requestCount, items, def) }, 5000);
        } else {
            def.resolve(items);
        }
    });

    return def.promise();
}

function getGameSpeedRunRecords(gameID, elementsPerPage, elementsOffset) {
    var def = new $.Deferred();

    return $.get('GetGameSpeedRunRecords?gameID=' + gameID + "&elementsPerPage=" + elementsPerPage + "&elementsOffset=" + elementsOffset, function (data, status) {

        def.resolve(data);
    });

    return def.promise();
}

function setGameSpeedRunRank(data) {
    var groupedObj = [];
    $(data).each(function () {
        var gameID = this.gameID;
        var categoryID = this.categoryID;
        var levelID = this.levelID;

        groupedObj[gameID] = groupedObj[gameID] || [];
        groupedObj[gameID][categoryID] = groupedObj[gameID][categoryID] || [];
        groupedObj[gameID][categoryID][levelID] = groupedObj[gameID][categoryID][levelID] || [];

        groupedObj[gameID][categoryID][levelID].push(this);
    });

    for (var gameID in groupedObj) {
        for (var categoryID in groupedObj[gameID]) {
            for (var levelID in groupedObj[gameID][categoryID]) {
                var groupData = $(groupedObj[gameID][categoryID][levelID]).sort(function (a, b) { return a.PrimaryRunTimeMilliseconds - b.PrimaryRunTimeMilliseconds; })

                var ranks = $.grep(groupData, function (item, idx) {
                    index = idx > 0 ? idx - 1 : 0;
                    return item.primaryRunTimeSeconds != groupData[index].primaryRunTimeSeconds;
                }).map(function (item) { return item.primaryRunTimeSeconds; });

                $.each(groupData, function (idx, item) {
                    var rank = $.inArray(item.primaryRunTimeSeconds, ranks) + 1;
                    item.rankString = rank + 1;
                });
            }
        }
    }
}

function renderTemplate(element, template, data) {
    var _template = _.template(template);

    var html = _template({
        item: data
    });

    $(element).html(html);
}

function initializeSpeedRunGridEvents() {
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

/*Event Handlers*/
//Grid Container Handlers
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
            //initializeCharts($activeCategoryChartsPane);
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
        //initializeCharts($chartContainer);
    }

    $('.level-tab-pane').hide();
    $container.fadeIn();

    $('.level-tab-pane-charts').hide();
    $chartContainer.fadeIn();
}

//Search Handlers
function onCategoryTypeChange(element) {
    var selectedCategoryTypeIDs = $(element).val();

    var $categories = $(sra.searchCategories).filter(function () {
        return (selectedCategoryTypeIDs.length == 0 || selectedCategoryTypeIDs.indexOf(this.categoryTypeID) > -1);
    });

    repopulateDropDown($('#drpCategories'), $categories);

    if (selectedCategoryTypeIDs.indexOf("1") > -1) {
        $('#divLevels').show();
    } else {
        $('#divLevels').hide();
        $('#drpLevels').val([]);
    }
}

/**Initialize component functions**/
//Initialize Grids
function initializeGrid(element) {
    var grid = $(element).find('.grid');
    var pagerID = $(element).find('.pager').attr("id");
    var gameID = $(element).data('gameid');
    var categoryType = $(element).data('categorytype');
    var categoryID = $(element).data('categoryid');
    var levelID = $(element).data('levelid');
    var localData = $(sra.speedRunGridModel.speedRunRecords).filter(function () { return this.gameID == gameID && this.categoryID == categoryID && this.levelID == levelID }).toArray();

    grid.jqGrid({
        datatype: "local",
        data: localData,
        height: '100%',
        //autowidth: true,
        //shrinkToFit: true,
        rowNum: 50,
        pager: pagerID,
        colNames: ["", "Rank", "Players", "Platform", "Emulated", "Time", "Examiner", "Submitted Date", "Comment", "Hidden", "Hidden", "Hidden"],
        colModel: [
            { name: "id", width: 75, resizable: false, search: false, formatter: optionsFormatter, align: "center", classes: 'options' },
            { name: "rankString", width: 75, sorttype: "number" },
            { name: "playerUsers", width: 160, formatter: playerFormatter },
            { name: "platformName", width: 160 },
            { name: "isEmulated", width: 125 },
            { name: "primaryRunTimeString", width: 160, search: false },
            { name: "examinerName", width: 160 },
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

    function gridLoadComplete() {
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
        var rankStrings = GetUniqueValues($.map(gridData, function (item) { return item.rankString; })).sort();
        var categoryNames = GetUniqueValues($.map(gridData, function (item) { return item.categoryName; })).sort();
        var platformNames = GetUniqueValues($.map(gridData, function (item) { return item.platformName; })).sort();
        var emulatorStrings = GetUniqueValues($.map(gridData, function (item) { return item.isEmulated; })).sort();
        var examinerNames = GetUniqueValues($.map(gridData, function (item) { return item.examinerName; })).sort();
        var playerNames = [];
        $(gridData).each(function () {
            var users = this.playerUsers;
            var guests = this.playerGuests;

            $(users).each(function () {
                if (!playerNames.indexOf(this.name) > -1) {
                    playerNames.push(this.name);
                }
            });

            $(guests).each(function () {
                if (!playerNames.indexOf(this.name) > -1) {
                    playerNames.push(this.name);
                }
            });
        });
        playerNames = playerNames.sort();

        setSearchSelect($(element), 'rankString', rankStrings);
        setSearchSelect($(element), 'playerUsers', playerNames);
        setSearchSelect($(element), 'categoryName', categoryNames);
        setSearchSelect($(element), 'platformName', platformNames);
        setSearchSelect($(element), 'isEmulated', emulatorStrings);
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
        var html = "<a href='../SpeedRun/SpeedRunSummary?speedRunID=" + cellvalue + "' data-toggle='modal' data-target='#videoLinkModal' data-backdrop='static'><i class='fas fa-play-circle'></i></a>";

        if (rowObject.splitsLink) {
            html += "<a href='" + rowObject.splitsLink + "' class='options-link'><img src='/images/SplitsLogo.svg' style='width:20px;'></img></a>";
        }

        return html;
    }

    function playerFormatter(value, options, rowObject) {
        var html = '';
        var isUser = value != null;
        var users = value;
        var guests = rowObject.playerGuests;

        $(users).each(function () {
            var user = this;
            html += "<a href='../User/UserDetails?userID=" + user.id + "'>" + user.name + "</a><br/>";
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

//Initialize Charts
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
                    var controller = _graphObj.controller(_selector, gameID, categoryType, categoryID, levelID);
 
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

/**Ajax functions **/
//Search
function runSearch() {
    var startDate = Date.parse($('#txtStartDate').val());
    var endDate = Date.parse($('#txtEndDate').val());
    var categoryTypeIDs = $('#drpCategoryTypes').val();
    var gameIDs = $('#drpGames').val();
    var categoryIDs = $('#drpCategories').val();
    var levelIDs = $('#drpLevels').val();

    var records = $(sra.speedRunRecords).filter(function () {
        return ((!startDate || (Date.parse(this.dateSubmitted) >= startDate))
            && (!endDate || (Date.parse(this.dateSubmitted) <= endDate))
            && (categoryTypeIDs.length == 0 || categoryTypeIDs.indexOf(this.categoryType.id))
            && (gameIDs.length == 0 || games.indexOf(this.gameID))
            && (categoryIDs.length == 0 || categoryIDs.indexOf(this.categoryID))
            && (levelIDs.length == 0 || levelIDs.indexOf(this.levelID)))
    }).toArray();

    sra.speedRunGridModel.speedRunRecords = records;

    if (startDate || endDate) {
        var games = $(sra.searchGames).filter(function () {
            var gameID = this.id;
            return $(records).filter(function () { return this.gameID == gameID }).length > 0;
        });

        var categories = $(sra.searchCategories).filter(function () {
            var categoryID = this.id;
            return $(records).filter(function () { return this.categoryID == categoryID }).length > 0;
        });

        var categoryTypes = $(categories).filter(function () {
            var categoryTypeID = this.categoryType.id;
            return $(records).filter(function () { return this.categoryType.id == categoryTypeID }).length > 0;
        });

        var levels = $(sra.searchLevels).filter(function () {
            var levelID = this.id;
            return $(records).filter(function () { return this.levelID == levelID }).length > 0;
        });

        sra.speedRunGridModel.categoryTypes = categoryTypes;
        sra.speedRunGridModel.games = games;
        sra.speedRunGridModel.categories = categories;
        sra.speedRunGridModel.levels = levels;
    }

    $('#divSpeedRunGrid').hide();
    $('#divSpeedRunGridLoading').show();

    renderAndInitializeSpeedRunGrid($('#divSpeedRunGrid'), sra.speedRunGridTemplate, sra.speedRunGridModel);

    $('#divSpeedRunGrid').show();
    $('#divSpeedRunGridLoading').hide();
}










 


















