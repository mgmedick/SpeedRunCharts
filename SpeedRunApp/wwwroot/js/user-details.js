if (!sra) {
    var sra = {};
}

function speedRunGridModel(sender, categoryTypes, games, categories, levels, variables) {
    this.sender = sender,
        this.categoryTypes = categoryTypes,
        this.games = games,
        this.categories = categories,
        this.levels = levels,
        this.variables = variables
}

function speedRunGridVariableModel(variables, classPrefix, categoryTypeID, gameID, categoryID, levelID, categoryTypeIndex, gameIndex, categoryIndex, levelIndex, prevID, prevData, count) {
    this.variables = variables,
        this.classPrefix = classPrefix,
        this.categoryTypeID = categoryTypeID,
        this.gameID = gameID,
        this.categoryID = categoryID,
        this.levelID = levelID,
        this.categoryTypeIndex = categoryTypeIndex,
        this.gameIndex = gameIndex,
        this.categoryIndex = categoryIndex,
        this.levelIndex = levelIndex,
        this.prevID = prevID,
        this.prevData = prevData,
        this.count = count
}

/**Initialize Functions**/
function initializeClient() {
    initalizeConstants();
    initializeEvents();
}

function initalizeConstants() {
    sra['gridData'] = [];
}

function initializeEvents() {
    $('.chosen').chosen({ width: "250px" });
    $('.date').datepicker();
    $('[data-toggle="tooltip"]').tooltip();
    $('#divSearch').setupCollapsible({ initialState: "visible", linkHiddenText: "Show Filters", linkDisplayedText: "Hide Filters" });

    $('#btnSearch').click(filterCategories);

    $('#drpCategoryTypes').change(function () {
        onCategoryTypeChange(this);
    });

    $('#drpGames').change(function () {
        onGameChange(this);
    });

    loadSpeedRunGridTemplate();
    initializeScrollerGlobalEvents();
}

function loadSpeedRunGridTemplate() {
    $('#divSpeedRunGridContainer').hide();
    $('#divSpeedRunGridLoading').show();

    getSpeedRunGridData().then(function (data) {
        cacheGridData(data);
        var gridModel = getSpeedRunGridModel(data);
        $.get('../templates/SearchSpeedRunGrid.html?_t=' + (new Date()).getTime(), function (searchTemplate, status) {
            renderAndInitializeSearchSpeedRunGrid($('#divSearchSpeedRunGridContainer'), searchTemplate, gridModel);

            $.get('../templates/SpeedRunGrid.html?_t=' + (new Date()).getTime(), function (gridTemplate, status) {
                $.get('../templates/SpeedRunGridVariable.html?_t=' + (new Date()).getTime(), function (gridVariableTemplate, status) {
                    $.get('../templates/SpeedRunGridVariableChart.html?_t=' + (new Date()).getTime(), function (chartVariableTemplate, status) {
                        sra['speedRunGridTemplate'] = gridTemplate;
                        sra['renderSpeedRunGridVariableTemplate'] = _.template(gridVariableTemplate);
                        sra['renderSpeedRunGridVariableChartTemplate'] = _.template(chartVariableTemplate);

                        renderAndInitializeSpeedRunGrid($('#divSpeedRunGridContainer'), sra.speedRunGridTemplate, gridModel, sra.renderSpeedRunGridVariableTemplate, sra.renderSpeedRunGridVariableChartTemplate);

                        $('#divSpeedRunGridContainer').show();
                        $('#divSpeedRunGridLoading').hide();
                    });
                });
            });
        });
    });
}

function cacheGridData(data) {
    $(data).each(function () {
        var categoryTypeID = this.categoryType.id;
        var gameID = this.gameID;
        var categoryID = this.categoryID;
        var levelID = this.levelID ? this.levelID : '';
        //var variables = formatVariables(this.variables);
        
        //$(this.variables).each(function () { this.})
        var variableValues = $(this.variables).filter(function () { return !levelID && this.gameID == gameID && this.categoryID == categoryID }).map(function () {
            var that = this;
            return $(this.variableValues).map(function () { return that.id + "|" + this.id }).get().join(",");
        }).get().join(",");

        sra.gridData[categoryTypeID] = sra.gridData[categoryTypeID] || [];
        sra.gridData[categoryTypeID][gameID] = sra.gridData[categoryTypeID][gameID] || [];
        sra.gridData[categoryTypeID][gameID][categoryID] = sra.gridData[categoryTypeID][gameID][categoryID] || [];
        sra.gridData[categoryTypeID][gameID][categoryID][levelID] = sra.gridData[categoryTypeID][gameID][categoryID][levelID] || [];
        sra.gridData[categoryTypeID][gameID][categoryID][levelID][variableValues] = sra.gridData[categoryTypeID][gameID][categoryID][levelID][variableValues] || [];

        sra.gridData[categoryTypeID][gameID][categoryID][levelID][variableValues].push(this);
    });
}

function getSpeedRunGridModel(data) {
    sra["searchCategoryTypes"] = _.chain(data).map(function (value) {
        return { id: value.categoryType.id, name: value.categoryType.name }
    }).filter(function (item) { return item.id }).uniq("id").sortBy(function (item) { return item.name; }).value();

    sra["searchCategories"] = _.chain(data).map(function (value) {
        return { id: value.categoryID, name: value.categoryName, gameID: value.gameID, categoryTypeID: value.categoryType.id }
    }).uniq(function (item) { return [item.id, item.categoryTypeID].join(); }).sortBy(function (item) { return item.name; }).value();

    sra["searchGames"] = _.chain(data).map(function (value) {
        return { id: value.gameID, name: value.gameName, categoryTypeIDs: _.chain(sra.searchCategories).filter(function (category) { return category.gameID == value.gameID }).map(function (category) { return category.categoryTypeID }).uniq().value() }
    }).filter(function (item) { return item.id }).uniq("id").sortBy(function (item) { return item.name; }).value();

    sra["searchLevels"] = _.chain(data).map(function (value) {
        return { id: value.levelID, name: value.levelName, gameID: value.gameID, categoryID: value.categoryID }
    }).filter(function (item) { return item.id }).uniq(function (item) { return [item.id, item.categoryID, item.gameID].join(); }).sortBy(function (item) { return item.name; }).value();

    //.uniq(function (item) { return [item.levelID, item.categoryID].join() })
    var variables = _.flatten(_.pluck(data, 'variables')).filter(function (item) { return item != null });
    var groupedVariables = _.uniq(variables, function (item) { return [item.id, item.gameID, item.categoryID].join(); });
    sra["searchVariables"] = getNestedVariables(groupedVariables, 0);

    return new speedRunGridModel("User", sra.searchCategoryTypes, sra.searchGames, sra.searchCategories, sra.searchLevels, sra.searchVariables)
}

function getNestedVariables(variables, count) {
    var results = _.chain(variables).filter(function (item, index) { return index >= count }).map(function (variable) {
        return {
            id: variable.id, name: variable.name, gameID: variable.gameID, categoryID: variable.categoryID, variableValues: _.map(variable.variableValues, function (variableValue) {
                return { id: variableValue.id, name: variableValue.name, variables: getNestedVariables(_.filter(variables, function (item) { return item.gameID == variable.gameID && item.categoryID == variable.categoryID }), count + 1) }
            })
        }
    }).value();

    return _.uniq(results, function (item) { return [item.gameID, item.categoryID].join(); });
}

function getSpeedRunGridData() {
    var def1 = $.Deferred();
    var userID = $("#hdnUserID").val();
    var elementsPerPage = sra.apiSettings.maxElementsPerPage;
    var elementsOffset = 0;
    var requestCount = 0;

    getAllUserSpeedRuns(userID, elementsPerPage, elementsOffset, requestCount).then(function (data) {
        data.sort(function (a, b) { return a.primaryTimeMilliseconds - b.primaryTimeMilliseconds; });
        def1.resolve(data);
    });

    return def1.promise();
}

function getAllUserSpeedRuns(userID, elementsPerPage, elementsOffset, requestCount, items, def) {
    if (!def) {
        var def = new $.Deferred();
    }

    if (!items) {
        var items = [];
    }

    getUserSpeedRuns(userID, elementsPerPage, elementsOffset).then(function (data) {
        requestCount++;
        items = items.concat(data);

        if (data.length == elementsPerPage) {
            return setTimeout(function () { getAllUserSpeedRuns(userID, elementsPerPage, (elementsPerPage * requestCount), requestCount, items, def) }, 2000);
        } else {
            def.resolve(items);
        }
    });

    return def.promise();
}

function getUserSpeedRuns(userID, elementsPerPage, elementsOffset) {
    var def = new $.Deferred();

    return $.get('GetUserSpeedRuns?userID=' + userID + "&elementsPerPage=" + elementsPerPage + "&elementsOffset=" + elementsOffset, function (data, status) {

        def.resolve(data);
    });

    return def.promise();
}

function renderAndInitializeSearchSpeedRunGrid(element, searchSpeedRunGridTemplate, speedRunGridModel) {
    renderTemplate(element, searchSpeedRunGridTemplate, speedRunGridModel).then(function () {
        initializeSearchSpeedRunGridEvents(element);
    });
}

function renderAndInitializeSpeedRunGrid(element, speedRunGridTemplate, speedRunGridModel, renderVariableGridTemplate, renderVariableChartemplate) {
    var functions = {
        renderSpeedRunGridVariableTemplate: renderVariableGridTemplate,
        renderSpeedRunGridVariableChartTemplate: renderVariableChartemplate
    };

    renderTemplate(element, speedRunGridTemplate, speedRunGridModel, functions).then(function () {
        initializeSpeedRunGridEvents(element)

        var $activeCategoryTypeTab = $('.nav-item.categoryType a.active');
        onCategoryTypeTabClick($activeCategoryTypeTab);
    });
}

function renderTemplate(element, template, data, functions) {
    var def = $.Deferred();
    var _template = _.template(template);

    var html = _template({
        item: data,
        fn: functions
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

    $(element).find('.nav-item.game a').click(function () {
        onGameTabClick(this);
    });

    $(element).find('.nav-item.category a').click(function () {
        onCategoryTabClick(this);
    });

    $(element).find('.nav-item.category-variable-value a').click(function () {
        onCategoryVariableValueTabClick(this);
    });

    $(element).find('.nav-item.level a').click(function () {
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

    var $activeGameTab = $container.find('.game a.active');
    onGameTabClick($activeGameTab);
}

function onGameTabClick(element) {
    var gameContainerID = $(element).attr('href');
    var $container = $(gameContainerID);
    var gameChartContainerID = gameContainerID + '-charts';
    var $chartContainer = $(gameChartContainerID);

    $('.game-tab-pane').hide();
    $container.fadeIn();

    $('.game-tab-pane-charts').hide();
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
        $container.find('.level-results').show();
        $container.find('.category-results').hide();

        $chartContainer.find('.category-results-charts').hide();
        $chartContainer.find('.level-results-charts').show();

        var $activeLevelTab = $container.find('.level a.active');
        onLevelTabClick($activeLevelTab);
    } else {
        var $activeCategoryPane = $container.find('.category-results');
        var $activeCategoryChartsPane = $chartContainer.find('.category-results-charts');

        $container.find('.level-results').hide();
        $activeCategoryPane.show();

        $chartContainer.find('.level-results-charts').hide();
        $activeCategoryChartsPane.show();

        if ($container.find('.category-variable-tabs').length > 0) {
            var $activeVariableValueTab = $activeCategoryPane.find('.category-variable-tabs:first .category-variable-value a.active')
            onCategoryVariableValueTabClick($activeVariableValueTab);
        }
        else {
            if (!$activeCategoryPane.find('.grid')[0].grid) {
                initializeGrid($activeCategoryPane.find('.grid-container')).then(function (data) {
                    initializeCharts($activeCategoryChartsPane, data);
                });
            }
        }
    }
}

function onCategoryVariableValueTabClick(element) {
    var variableValueContainerID = $(element).attr('href');
    var $container = $(variableValueContainerID);
    var containerClass = $container.data("class");
    var variableValueChartContainerID = variableValueContainerID + '-charts';
    var $chartContainer = $(variableValueChartContainerID);
    var chartContainerClass = $chartContainer.data("class");

    $('.' + containerClass).hide();
    $container.fadeIn();

    $('.' + chartContainerClass).hide();
    $chartContainer.fadeIn();

    if ($container.find('.category-variable-tabs').length > 0) {
        var $activeVariableValueTab = $container.find('.category-variable-tabs:first .category-variable-value a.active')
        onCategoryVariableValueTabClick($activeVariableValueTab);
    } else {
        if (!$container.find('.grid')[0].grid) {
            initializeGrid($container.find('.grid-container')).then(function (data) {
                initializeCharts($chartContainer, data);
            });
        }
    }
}

function onLevelTabClick(element) {
    var levelContainerID = $(element).attr('href');
    var $container = $(levelContainerID);
    var levelChartContainerID = levelContainerID + '-charts';
    var $chartContainer = $(levelChartContainerID);

    $('.level-tab-pane').hide();
    $container.fadeIn();

    $('.level-tab-pane-charts').hide();
    $chartContainer.fadeIn();

    if (!$container.find('.grid')[0].grid) {
        initializeGrid($container.find('.grid-container')).then(function (data) {
            initializeCharts($chartContainer.find('.charts-container'), data);
        });
    }
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
    var def = $.Deferred();
    var grid = $(element).find('.grid');
    var pagerID = $(element).find('.pager').attr("id");
    var categoryType = $(element).data('categorytype');
    var gameID = $(element).data('gameid');
    var categoryID = $(element).data('categoryid');
    var levelID = $(element).data('levelid') ? $(element).data('levelid') : '';
    var variableValues = $(element).data('variablevalues') ? $(element).data('variablevalues') : '';
    var localData = sra.gridData[categoryType][gameID][categoryID][levelID][variableValues];

    grid.jqGrid({
        datatype: "local",
        data: localData,
        height: '100%',
        //autowidth: true,
        //shrinkToFit: true,
        rowNum: 50,
        pager: pagerID,
        colNames: ["", "Players", "Platform", "Emulated", "Primary Time", "Real Time", "Real Time (No Load)", "Game Time", "Status", "Reject Reason", "Submitted Date", "Comment", "Hidden", "Hidden", "Hidden", "Hidden", "Hidden", "Hidden", "Hidden", "Hidden", "Hidden", "Hidden"],
        colModel: [
            { name: "id", width: 75, resizable: false, search: false, formatter: optionsFormatter, align: "center" },
            { name: "playerUsers", width: 160, formatter: playerFormatter },
            { name: "platformName", width: 160 },
            { name: "isEmulated", width: 125 },
            { name: "primaryTimeString", width: 160, search: false },
            { name: "realTimeString", width: 160, search: false },
            { name: "realTimeWithoutLoadsString", width: 160, search: false },
            { name: "gameTimeString", width: 160, search: false },
            { name: "statusTypeString", width: 125 },
            { name: "rejectedReason", width: 160, hidden: true },
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
            { name: "monthYearSubmitted", hidden: true },
            { name: "variables", hidden: true }
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
        var rankStrings = _.chain(gridData).map(function (item) { return item.rankString }).uniq().value().sort();
        var categoryNames = _.chain(gridData).map(function (item) { return item.categoryName }).uniq().value().sort();
        var platformNames = _.chain(gridData).map(function (item) { return item.platformName }).uniq().value().sort();
        var emulatorStrings = _.chain(gridData).map(function (item) { return item.isEmulated }).uniq().value().sort();
        var statuses = _.chain(gridData).map(function (item) { return item.statusTypeString }).uniq().value().sort();
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

    function optionsFormatter(cellvalue, options, rowObject) {
        var html = "<div>"
        html += "<table style='border:none; border-collapse:collapse; border-spacing:0; margin:auto;'>";
        html += "<tr>";
        html += "<td style='border:none; padding:0px; width:30px;'>";
        html += "<a href='../SpeedRun/SpeedRunSummary?speedRunID=" + cellvalue + "' data-toggle='modal' data-target='#videoLinkModal' data-backdrop='static'><i class='fas fa-play-circle'></i></a>";
        html += "</td>";
        html += "<td style='border:none; padding:0px; width:30px;'>";
        html += (rowObject.splitsLink) ? "<a href='" + rowObject.splitsLink + "' class='options-link'><img src='/images/SplitsLogo.svg' style='width:20px;'></img></a>" : "";
        html += "</td>";
        html += "</tr>";
        html += "</table>";
        html += "</div>";

        return html;
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

    return def.promise();
}

//Initialize Charts
function initializeCharts(element, data) {
    var def = $.Deferred();
    var charts = [
        new userTopSpeedRunsChart($(element).find('.chart-container-0'), { chartData: data, topAmount: 10 }),
        new userSpeedRunsByDateChart($(element).find('.chart-container-1'), { chartData: data }),
        new userSpeedRunsPercentileChart($(element).find('.chart-container-2'), { chartData: data })
    ];

    var promises = $(charts).map(function () { return this.generateChart() });

    $.when.apply(null, promises).then(function () {
        def.resolve();
    });

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









 


















