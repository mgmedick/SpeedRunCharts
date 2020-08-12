if (!sra) {
    var sra = {};
}

function speedRunGridModel(sender, categoryTypes, games, categories, levels) {
    this.sender = sender,
    this.categoryTypes = categoryTypes,
    this.games = games,
    this.categories = categories,
    this.levels = levels
}

/**Initialize Event Functions**/
function initializeClient(searchCategoryTypes, searchGames, searchCategories, searchLevels) {
    initalizeConstants(searchCategoryTypes, searchGames, searchCategories, searchLevels);
    initializeEvents();
}

function initalizeConstants(searchCategoryTypes, searchGames, searchCategories, searchLevels) {
    sra['searchCategoryTypes'] = searchCategoryTypes;
    sra['searchGames'] = searchGames;
    sra['searchCategories'] = searchCategories;
    sra['searchLevels'] = searchLevels;
}

function initializeEvents() {
    $('.chosen').chosen({ width: "250px" });
    $('.date').datepicker();

    $('#drpCategoryTypes').change(function () {
        onCategoryTypeChange(this);
    });

    $('#drpCategories').change(function () {
        onCategoryChange(this);
    });

    loadSpeedRunGridTemplate();
    initializeScrollerGlobalEvents();
}

function loadSpeedRunGridTemplate() {
    $('#divSpeedRunGridContainer').hide();
    $('#divSpeedRunGridLoading').show();

    var gridModel = new speedRunGridModel("Game", sra.searchCategoryTypes, sra.searchGames, sra.searchCategories, sra.searchLevels);

    $.get('../templates/SearchSpeedRunGrid.html?_t=' + (new Date()).getTime(), function (searchTemplate, status) {
        renderAndInitializeSearchSpeedRunGrid($('#divSearchSpeedRunGridContainer'), searchTemplate, gridModel);

        $.get('../templates/SpeedRunGrid.html?_t=' + (new Date()).getTime(), function (gridTemplate, status) {
            sra['speedRunGridTemplate'] = gridTemplate;
            renderAndInitializeSpeedRunGrid($('#divSpeedRunGridContainer'), sra.speedRunGridTemplate, gridModel);

            $('#divSpeedRunGridContainer').show();
            $('#divSpeedRunGridLoading').hide();
        });
    });
}

function renderAndInitializeSearchSpeedRunGrid(element, searchSpeedRunGridTemplate, speedRunGridModel) {
    renderTemplate(element, searchSpeedRunGridTemplate, speedRunGridModel).then(function () {
        initializeSearchSpeedRunGridEvents(element);
    });
}

function renderAndInitializeSpeedRunGrid(element, speedRunGridTemplate, speedRunGridModel) {
    renderTemplate(element, speedRunGridTemplate, speedRunGridModel).then(function () {
        initializeSpeedRunGridEvents(element)

        var $activeCategoryTypeTab = $('.nav-item.categoryType a.active');
        onCategoryTypeTabClick($activeCategoryTypeTab);
    });
}

function renderTemplate(element, template, data) {
    var def = $.Deferred();
    var _template = _.template(template);

    var html = _template({
        item: data
    });

    $(element).html(html);
    def.resolve();

    return def.promise();
}

function initializeSearchSpeedRunGridEvents(element) {
    $(element).find('.chosen').chosen({ width: "250px" });
    $('#divSearchSpeedRunGrid').setupCollapsible({ initialState: "hidden", linkHiddenText: "Show Filters", linkDisplayedText: "Hide Filters" });

    $('#drpCategoryTypes').change(function () {
        onCategoryTypeChange(this);
    });

    $('#drpCategories').change(function () {
        onCategoryChange(this);
    });
}

function initializeSpeedRunGridEvents(element) {
    $(element).find('.nav-item.categoryType a').click(function () {
        onCategoryTypeTabClick(this);
    });

    $(element).find('.nav-item.category a').click(function () {
        onCategoryTabClick(this);
    });

    $(element).find('.nav-item.level a').click(function () {
        onLevelTabClick(this);
    });

    $('#divChartContainer').setupCollapsible({ initialState: "visible", linkHiddenText: "Show Charts", linkDisplayedText: "Hide Charts" });
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

        $chartContainer.find('.category-results-charts').hide();
        $chartContainer.find('.level-results-charts').show();

        var $activeLevelTab = $container.find('.level a.active');
        onLevelTabClick($activeLevelTab);
    } else {
        var $activeCategoryPane = $container.find('.category-results');
        var $activeCategoryChartsPane = $chartContainer.find('.category-results-charts');

        if (!$activeCategoryPane.find('.grid')[0].grid) {
            initializeGrid($activeCategoryPane).then(function (data) {
                initializeCharts($activeCategoryChartsPane, data);
            });
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
        initializeGrid($container).then(function (data) {
            initializeCharts($chartContainer, data);
        });
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

    var $levels = $(sra.searchLevels).filter(function () {
        return (selectedCategoryTypeIDs.length == 0 || selectedCategoryTypeIDs.indexOf("1") > -1);
    });

    repopulateDropDown($('#drpCategories'), $categories);
    repopulateDropDown($('#drpLevels'), $levels);
}

function onCategoryChange(element) {
    var selectedCategoryIDs = $(element).val();

    var $selectedLevelCategories = $(sra.searchCategories).filter(function () {
        return (selectedCategoryIDs.indexOf(this.id) > -1 && this.categoryTypeID == "1");
    });

    var $levels = $(sra.searchLevels).filter(function () {
        return (selectedCategoryIDs.length == 0 || $selectedLevelCategories.length > 0);
    });

    repopulateDropDown($('#drpLevels'), $levels);
}

/**Initialize component functions**/
//Initialize Grids
function initializeGrid(element) {
    var def = $.Deferred();
    var grid = $(element).find('.grid');
    var gameID = $(element).data('gameid');
    var categoryType = $(element).data('categorytype');
    var categoryID = $(element).data('categoryid');
    var levelID = $(element).data('levelid');
    var pagerID = $(element).find('.pager').attr("id");

    grid.jqGrid({
        url: 'GetGameSpeedRunRecords?gameID=' + gameID + "&categoryType=" + categoryType + "&categoryID=" + categoryID + "&levelID=" + levelID,
        datatype: "json",
        mtype: "GET",
        height: '100%',
        //autowidth: true,
        //shrinkToFit: true,
        rowNum: 50,
        pager: pagerID,
        colNames: ["", "Rank", "Players", "Platform", "Emulated", "Primary Time", "Real Time", "Real Time (No Load)", "Game Time", "Submitted Date", "Comment", "Hidden", "Hidden", "Hidden", "Hidden", "Hidden", "Hidden", "Hidden", "Hidden", "Hidden"],
        colModel: [
            { name: "id", width: 75, resizable: false, search: false, formatter: optionsFormatter, align: "center", classes: 'options' },
            { name: "rankString", width: 75, sorttype: "number" },
            { name: "playerUsers", width: 160, formatter: playerFormatter },
            { name: "platformName", width: 160 },
            { name: "isEmulated", width: 125 },
            { name: "primaryTimeString", width: 160, search: false },
            { name: "realTimeString", width: 160, search: false },
            { name: "realTimeWithoutLoadsString", width: 160, search: false },
            { name: "gameTimeString", width: 160, search: false },
            {
                name: "dateSubmitted", width: 160, sorttype: "date", formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y H:i" }, searchoptions: {
                    sopt: ["deq", "dge", "dle"],
                    dataInit: function (element, options) {
                        var self = this;
                        var selfOptions = options;
                        $(element).datepicker({
                            dateFormat: 'mm/dd/yy',
                            showButtonPanel: true,
                            onSelect: function () {
                                if (selfOptions.mode === "filter") {
                                    setTimeout(function () {
                                        self.triggerToolbar();
                                    }, 0);
                                } else {
                                    $(this).trigger("change");
                                }
                            }
                        });
                    }
                }, cellattr: dateSubmittedCellAttr
            },
            { name: "comment", width: 100, search: false, formatter: commentFormatter, align: "center" },
            { name: "relativeDateSubmittedString", hidden: true },
            { name: "relativeVerifyDateString", hidden: true },
            { name: "playerGuests", hidden: true },
            { name: "categoryType", hidden: true },
            { name: "gameID", hidden: true },
            { name: "categoryID", hidden: true },
            { name: "levelID", hidden: true },
            { name: "primaryTimeSeconds", hidden: true },
            { name: "monthYearSubmitted", hidden: true }
        ],
        postData: {
            filters: '{"groupOp":"AND","rules":[{"field":"dateSubmitted","op":"dge","data":""},{"field":"dateSubmitted","op":"dle","data":""}]}'
        },
        iconSet: "fontAwesome",
        guiStyle: "bootstrap4",
        ignoreCase: true,
        viewrecords: true,
        loadonce: true,
        loadComplete: gridLoadComplete,
        customSortOperations: {
            deq: {
                operand: "==",
                text: "Date equal",
                filter: function (options) {
                    var fieldData = new Date(options.item[options.cmName]);
                    var searchValue = new Date(options.searchValue);

                    return fieldData.getFullYear() === searchValue.getFullYear()
                        && fieldData.getMonth() === searchValue.getMonth()
                        && fieldData.getDate() === searchValue.getDate();
                }
            },
            dge: {
                operand: ">=",
                text: "Date greater or equal",
                filter: function (options) {
                    var fieldData = new Date(options.item[options.cmName]);
                    var searchValue = new Date(options.searchValue);

                    return fieldData >= searchValue;
                }
            },
            dle: {
                operand: "<=",
                text: "Date less or equal",
                filter: function (options) {
                    var fieldData = new Date(options.item[options.cmName]);
                    var searchValue = new Date(options.searchValue);

                    return fieldData <= searchValue;
                }
            }
        }
    });

    grid.jqGrid('navGrid', '#' + pagerID, { add: false, del: false, search: true, refresh: false }, {}, {}, {}, { multipleSearch: true });

    function gridLoadComplete() {
        initializeGridEvents(this);
        initializeGridFilters(this);
        initializeGridStyles(this);
        var $gridContainer = $(this).closest('.grid-container');
        initializeScroller($gridContainer);

        var data = $(this).jqGrid("getGridParam", "data");
        def.resolve(data);
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
        var rankStrings = _.chain(gridData).map(function (item) { return item.rankString }).uniq("rankString").value().sort();
        var categoryNames = _.chain(gridData).map(function (item) { return item.categoryName }).uniq("categoryName").value().sort();
        var platformNames = _.chain(gridData).map(function (item) { return item.platformName }).uniq("platformName").value().sort();
        var emulatorStrings = _.chain(gridData).map(function (item) { return item.isEmulated }).uniq("platformName").value().sort();

        var playerNames = [];
        $(gridData).each(function () {
            var users = this.playerUsers;
            var guests = this.playerGuests;

            $(users).each(function () {
                if (playerNames.indexOf(this.name) == -1) {
                    playerNames.push(this.name);
                }
            });

            $(guests).each(function () {
                if (playerNames.indexOf(this.name) == -1) {
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
        var html = "<div>"
        html += "<table style='border:none; border-collapse:collapse; border-spacing:0;'>";
        html += "<tr>";
        html += "<td style='border:none; padding:0px;'>";
        html += "<a href='../SpeedRun/SpeedRunSummary?speedRunID=" + cellvalue + "' data-toggle='modal' data-target='#videoLinkModal' data-backdrop='static'><i class='fas fa-play-circle'></i></a>";
        html += "</td>";
        html += "<td style='border:none; padding:0px;'>";
        html += (rowObject.splitsLink) ? "<a href='" + rowObject.splitsLink + "' class='options-link'><img src='/images/SplitsLogo.svg' style='width:20px;'></img></a>" : "";
        html += "</td>";
        html += "</tr>";
        html += "</table>";
        html += "</div>";

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

    return def.promise();
}

//Initialize Charts
function initializeCharts(element, data) {
    var def = $.Deferred();
    var charts = [
        new speedRunsByUserChart($(element).find('.chart-container-0'), { chartData: data, topAmount: 10 }),
        new speedRunsReportedChart($(element).find('.chart-container-1'), { chartData: data }),
        new speedRunSummaryByMonthChart($(element).find('.chart-container-2'), { chartData: data })
    ];

    var promises = $(charts).map(function () { return this.generateChart() });

    $.when.apply(null, promises).then(function () {
        def.resolve();
    })

    //$(charts).each(function () {
    //    var self = this;
    //    //var container = $(element).find(this.selector)
    //    //$(container).empty();

    //    //var controller = graphObj.controller(container, data);

    //    self.preRender().then(function (data) {
    //        self.postRender(data).then(function () {
    //        });
    //    });
    //});
    return def.promise();
}

//function getGameDetailsCharts(element, data) {
//    var charts = [];
//    charts.push(new speedRunsByUserChart($(element).find('.chart-container-0'), { chartData: data, topAmount: 10 }));
//    //charts.push({ selector: ".chart-container-1", chart: new speedRunSummaryByMonthChart() });
//    //charts.push({ selector: ".chart-container-2", chart: new speedRunSummaryByMonthChart() });

//    return charts;
//}

/**Ajax functions **/
//Search
function filterCategories() {
    var categoryTypeIDs = $('#drpCategoryTypes').val();
    var gameIDs = $('#drpGames').val();
    var categoryIDs = $('#drpCategories').val();
    var levelIDs = $('#drpLevels').val();

    var categoryTypes = $(sra.searchCategoryTypes).filter(function () { return categoryTypeIDs.length == 0 || categoryTypeIDs.indexOf(this.id) > -1 })
    var games = $(sra.searchGames).filter(function () { return gameIDs.length == 0 || gameIDs.indexOf(this.id) > -1 })
    var categories = $(sra.searchCategories).filter(function () { return categoryIDs.length == 0 || categoryIDs.indexOf(this.id) > -1 })
    var levels = $(sra.searchLevels).filter(function () { return levelIDs.length == 0 || levelIDs.indexOf(this.id) > -1 })

    $('#divSpeedRunGridContainer').hide();
    $('#divSpeedRunGridLoading').show();

    var gridModel = new speedRunGridModel("Game", categoryTypes, games, categories, levels);
    renderAndInitializeSpeedRunGrid($('#divSpeedRunGridContainer'), sra.speedRunGridTemplate, gridModel);

    $('#divSpeedRunGridContainer').show();
    $('#divSpeedRunGridLoading').hide();
}















 


















