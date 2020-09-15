if (!sra) {
    var sra = {};
}

function speedRunGridModel(sender, categoryTypes, games, categories, levels, subCategoryVariables) {
    this.sender = sender,
    this.categoryTypes = categoryTypes,
    this.games = games,
    this.categories = categories,
    this.levels = levels,
    this.subCategoryVariables = subCategoryVariables
}

function speedRunGridVariableModel(subCategoryVariables, classPrefix, categoryTypeID, gameID, categoryID, levelID, categoryTypeIndex, gameIndex, categoryIndex, levelIndex, prevID, prevData, count) {
    this.subCategoryVariables = subCategoryVariables,
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
function initalizeSpeedRunGrid(sender, params) {
    $('#divSpeedRunGridContainer').hide();
    $('#divSpeedRunGridLoading').show();

    if (sender == "User") {
        getUserSpeedRunGridData().then(function (data) {
            var cachedData = getFormattedData(data);
            var params = getParamsFromGridData(data);
            initalizeConstants(sender, params.categoryTypes, params.games, params.categories, params.levels, params.subCategoryVariables, params.variables, cachedData);
            var gridModel = new speedRunGridModel(sender, sra.categoryTypes, sra.games, sra.categories, sra.levels, sra.subCategoryVariables); 
            loadSpeedRunGridTemplate(gridModel);
        });
    } else {
        initalizeConstants(sender, params.categoryTypes, params.games, params.categories, params.levels, params.subCategoryVariables, params.variables, null);
        var gridModel = new speedRunGridModel(sender, sra.categoryTypes, sra.games, sra.categories, sra.levels, sra.subCategoryVariables);
        loadSpeedRunGridTemplate(gridModel);
    }
}

function initalizeConstants(sender, categoryTypes, games, categories, levels, subCategoryVariables, variables, cachedData) {
    sra['sender'] = sender;
    sra['categoryTypes'] = categoryTypes;
    sra['games'] = games;
    sra['categories'] = categories;
    sra['levels'] = levels;
    sra['subCategoryVariables'] = subCategoryVariables;
    sra['variables'] = variables;
    sra['cachedData'] = cachedData;
}

/*Load Functions*/
function loadSpeedRunGridTemplate(gridModel) {
    $.get('../templates/SpeedRunGridSearch.html?_t=' + (new Date()).getTime(), function (searchTemplate, status) {
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
                configureAndInitializeGrid($activeCategoryPane.find('.grid-container')).then(function (data) {
                    initializeCharts($activeCategoryChartsPane, data);
                });
            } else {
                configureAndInitializeScroller($container);
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
            configureAndInitializeGrid($container.find('.grid-container')).then(function (data) {
                initializeCharts($chartContainer, data);
            });
        } else {
            configureAndInitializeScroller($container);
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
        configureAndInitializeGrid($container.find('.grid-container')).then(function (data) {
            initializeCharts($chartContainer.find('.charts-container'), data);
        });
    } else {
        configureAndInitializeScroller($container);
    }
}

/**Data Functions**/
function getFormattedData(data) {
    var gridData = {};

    $(data).each(function () {
        var categoryTypeID = this.categoryType.id;
        var gameID = this.game.id;
        var categoryID = this.category.id;
        var levelID = this.level ? this.level.id : '';
        var variableValues = $(this.subCategoryVariableValues).filter(function () { return !levelID && this.variable.gameID == gameID && this.variable.categoryID == categoryID }).map(function () {
            return this.variable.id + "|" + this.id;
        }).get().join(",");

        gridData[categoryTypeID] = gridData[categoryTypeID] || [];
        gridData[categoryTypeID][gameID] = gridData[categoryTypeID][gameID] || [];
        gridData[categoryTypeID][gameID][categoryID] = gridData[categoryTypeID][gameID][categoryID] || [];
        gridData[categoryTypeID][gameID][categoryID][levelID] = gridData[categoryTypeID][gameID][categoryID][levelID] || [];
        gridData[categoryTypeID][gameID][categoryID][levelID][variableValues] = gridData[categoryTypeID][gameID][categoryID][levelID][variableValues] || [];

        gridData[categoryTypeID][gameID][categoryID][levelID][variableValues].push(this);
    });

    return gridData;
}

function getParamsFromGridData(data) {
    var categoryTypes = _.chain(data).map(function (value) {
        return { id: value.categoryType.id, name: value.categoryType.name }
    }).uniq("id").sortBy(function (item) { return item.name; }).value();

    var categories = _.chain(data).map(function (value) {
        return { id: value.category.id, name: value.category.name, gameID: value.game.id, categoryTypeID: value.categoryType.id }
    }).uniq(function (item) { return [item.id, item.categoryTypeID].join(); }).sortBy(function (item) { return item.name; }).value();

    var games = _.chain(data).map(function (value) {
        return { id: value.game.id, name: value.game.name, categoryTypeIDs: _.chain(categories).filter(function (category) { return category.gameID == value.game.id }).map(function (category) { return category.categoryTypeID }).uniq().value() }
    }).uniq("id").sortBy(function (item) { return item.name; }).value();

    var levels = _.chain(data).filter(function (value) { return value.level }).map(function (value) {
        return { id: value.level.id, name: value.level.name, gameID: value.game.id, categoryID: value.category.id }
    }).uniq(function (item) { return [item.id, item.categoryID, item.gameID].join(); }).sortBy(function (item) { return item.name; }).value();

    var subCategoryVariableValues = _.chain(data).pluck('subCategoryVariableValues').flatten().value();
    var subCategoryVariables1 = _.chain(subCategoryVariableValues).pluck('variable').flatten().uniq(function (item) { return [item.id, item.gameID, item.categoryID].join(); }).value();
    _.chain(subCategoryVariables1).each(function (variable) {
        variable.variableValues = _.chain(subCategoryVariableValues).filter(function (variableValue) { return variableValue.variable.id == variable.id }).uniq("id").value();
    });
    var subCategoryVariables = getNestedVariables(subCategoryVariables1, 0);

    var variableValues = _.chain(data).pluck('variableValues').flatten().value();
    var variables = _.chain(variableValues).pluck('variable').flatten().uniq(function (item) { return [item.id, item.gameID, item.categoryID].join(); }).value();
    _.chain(variables).each(function (variable) {
        variable.variableValues = _.chain(variableValues).filter(function (variableValue) { return variableValue.variable.id == variable.id }).uniq("id").value();
    });

    var params = { categoryTypes, categories, games, levels, subCategoryVariables, variables };

    return params;
}

function getNestedVariables(variables, count) {
    var results = _.chain(variables).filter(function (item, index) { return index >= count }).map(function (variable) {
        return {
            id: variable.id, name: variable.name, gameID: variable.gameID, categoryID: variable.categoryID, variableValues: _.map(variable.variableValues, function (variableValue) {
                return { id: variableValue.id, name: variableValue.name, subVariables: getNestedVariables(_.filter(variables, function (item) { return item.gameID == variable.gameID && item.categoryID == variable.categoryID }), count + 1) }
            })
        }
    }).value();

    return _.uniq(results, function (item) { return [item.gameID, item.categoryID].join(); });
}

function getUserSpeedRunGridData() {
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

//Initialize Grids
function getGridData(categoryType, gameID, categoryID, levelID, variableValues) {
    var def = $.Deferred();
    var isSenderUser = sra.sender == "User";

    if (isSenderUser) {
        var gridData = sra.cachedData[categoryType][gameID][categoryID][levelID][variableValues];
        def.resolve(gridData);
    } else {
        $.get('GetGameSpeedRunRecords?gameID=' + gameID + "&categoryType=" + categoryType + "&categoryID=" + categoryID + "&levelID=" + levelID + "&variableValues=" + variableValues, function (data, status) {
            def.resolve(data);
        });
    }

    return def.promise();
}

function configureAndInitializeGrid(element) {
    var def = $.Deferred();
    var $grid = $(element).find('.grid');
    var $loading = $(element).find('.loading');
    var pagerID = $(element).find('.pager').attr("id");
    var categoryType = $(element).data('categorytype');
    var gameID = $(element).data('gameid');
    var categoryID = $(element).data('categoryid');
    var levelID = $(element).data('levelid') ? $(element).data('levelid') : '';
    var variableValues = $(element).data('variablevalues') ? $(element).data('variablevalues') : '';
    var isSenderUser = sra.sender == "User";

    $loading.show();
    $grid.hide();
    getGridData(categoryType, gameID, categoryID, levelID, variableValues).then(function (data) {
        $(data).each(function () {
            var item = this;
            $(item.variableValues).each(function () {
                item[this.variable.id] = this.name;
            });
        });

        var columnNames = ["", "Rank", "Players", "Platform", "Emulated", "Primary Time", "Status", "Reject Reason", "Submitted Date", "Hidden", "Hidden", "Hidden", "Hidden", "Hidden", "Hidden", "Hidden", "Hidden", "Hidden", "Hidden"];

        var columnModel = [
            { name: "id", width: 75, resizable: false, search: false, formatter: optionsFormatter, align: "center" },
            { name: "rankString", width: 75, sorttype: "number", hidden: isSenderUser },
            { name: "playerUsers", width: 160, formatter: playerFormatter },
            { name: "platform.name", width: 160 },
            { name: "isEmulatedString", width: 125 },
            { name: "primaryTimeString", width: 160, search: false },
            { name: "statusTypeString", width: 125, hidden: !isSenderUser },
            { name: "rejectedReason", width: 160, hidden: true },
            { name: "dateSubmitted", width: 160, sorttype: "date", formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y H:i" }, searchoptions: {
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
            { name: "relativeDateSubmittedString", hidden: true },
            { name: "relativeVerifyDateString", hidden: true },
            { name: "playerGuests", hidden: true },
            { name: "categoryType", hidden: true },
            { name: "gameID", hidden: true },
            { name: "categoryID", hidden: true },
            { name: "levelID", hidden: true },
            { name: "primaryTimeSeconds", hidden: true },
            { name: "monthYearSubmitted", hidden: true },
            { name: "subCategoryVariables", hidden: true }
        ];

       $(sra.variables).filter(function () {
            var variable = this;
           return $(data).filter(function () { return !levelID && variable.gameID == gameID && variable.categoryID == categoryID && this.hasOwnProperty(variable.id) }).length > 0
        }).each(function () {
            columnNames.push(this.name);
            var variable = { name: this.id, width: 160, search: false };
            columnModel.push(variable);
        });

        columnNames.push("Comment");
        columnModel.push({ name: "comment", width: 100, search: false, formatter: commentFormatter, align: "center" });

        initializeGrid($grid, pagerID, data, columnModel, columnNames).then(function (gridData) {
            def.resolve(gridData);
            $grid.show();
            $loading.hide();
        });
    });

    //Column Formatters
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

    function platformFormatter(value, options, rowObject) {
        return value.id;
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

    function buildSearchSelect(items) {
        var values = ":All";
        $.each(items, function () {
            values += ";" + this.id + ":" + this.name;
        });
        return values;
    }

    return def.promise();
}

function initializeGrid(grid, pagerID, localData, columnModel, columnNames) {
    var def = $.Deferred();

    grid.jqGrid({
        datatype: "local",
        data: localData,
        height: '100%',
        //autowidth: true,
        //shrinkToFit: true,
        rowNum: 50,
        pager: pagerID,
        colNames: columnNames,
        colModel: columnModel,
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
            },
            nIn: {
                operand: "nIN",
                text: "In",
                filter: function (options) {
                    var fieldData = options.item[options.cmName];
                    var searchValue = options.searchValue;

                    return $(fieldData).filter(function () { return this.name == searchValue; }).length > 0;
                }
            }
        }
    });

    grid.jqGrid('navGrid', '#' + pagerID, { add: false, del: false, search: true, refresh: false }, {}, {}, {}, { multipleSearch: true });

    function gridLoadComplete() {
        initializeGridEvents(this);
        initializeGridFilters(this);
        initializeGridStyles(this);
        configureAndInitializeScroller(this);

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
        var data = $(element).jqGrid("getGridParam", "data");
        var rankNumbers = _.chain(data).map(function (item) { return parseInt(item.rankString) }).uniq().sortBy(function (item) { return item; }).value();
        var platformNames = _.chain(data).map(function (item) { return item.platform.name }).uniq().sortBy(function (item) { return item.name }).value();
        var playerUsers = _.chain(data).pluck('playerUsers').flatten().uniq("id").sortBy(function (item) { return item.name }).map(function (value) { return value.name }).value();
        var playerGuests = _.chain(data).pluck('playerGuests').flatten().uniq("id").sortBy(function (item) { return item.name }).map(function (value) { return value.name }).value();
        var playerNames = _.union(playerUsers, playerGuests);
        var emulatedNames = _.chain(data).map(function (item) { return item.isEmulatedString }).uniq().sortBy(function (item) { return item }).value();

        setSearchSelect($(element), 'rankString', rankNumbers, ["eq"]);
        setSearchSelect($(element), 'platform.name', platformNames, ["eq"]);
        setSearchSelect($(element), 'playerUsers', playerNames, ["nIn"]);
        setSearchSelect($(element), 'isEmulatedString', emulatedNames, ["eq"]);
        $(element).jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
    }

    function setSearchSelect(grid, columnName, searchData, sortOptions) {
        grid.jqGrid("setColProp", columnName, {
            stype: "select",
            searchoptions: {
                value: buildSearchSelect(searchData),
                sopt: sortOptions
            }
        });
    }

    function buildSearchSelect(items) {
        var values = ":All";
        $.each(items, function () {
            values += ";" + this + ":" + this;
        });
        return values;
    }

    function initializeGridStyles(element) {
        var $grid = $(element);
        var data = $grid.jqGrid("getGridParam", "data");
        var $rejectedItems = $(data).filter(function (item) { return item.statusTypeString == "Rejected"; })
        if ($rejectedItems.length > 0) {
            $grid.jqGrid("showCol", ["rejectedReason"]);
        }

        var $tabgridContainer = $grid.closest('.tab-grid-container');
        var $gridContainer = $grid.closest('.tab-grid-container');
        $tabgridContainer.css('width', parseInt($gridContainer.find('.ui-jqgrid-view').width()) + parseInt($gridContainer.css('padding-left')));
    }

    return def.promise();
}

function configureAndInitializeScroller(element) {
    var $tabgridContainer = $(element).closest('.tab-grid-container');
    initializeScroller($tabgridContainer);

    $tabgridContainer.css('width', parseInt($tabgridContainer.find('.ui-jqgrid-view:visible').width()) + parseInt($tabgridContainer.css('padding-left')));

    //var maxValue = 130;
    //var maxWidth = Math.max.apply(Math, $tabgridContainer.find('.tab-row-name:visible').map(function () { return $(this).width(); }).get());
    //if (maxWidth > maxValue) {
    //    maxWidth = maxValue;
    //}

    //$tabgridContainer.find('.tab-row-name-container:visible').each(function () { $(this).width(maxWidth); })
}

//Initialize Charts
function initializeCharts(element, data) {
    var def = $.Deferred();
    var charts = [];
    if (sra.sender == "User") {
        charts = [
            new userTopSpeedRunsChart($(element).find('.chart-container-0'), { chartData: data, topAmount: 10 }),
            new userSpeedRunsByDateChart($(element).find('.chart-container-1'), { chartData: data }),
            new userSpeedRunsPercentileChart($(element).find('.chart-container-2'), { chartData: data })
        ];
    } else {
        charts = [
            new gameTopSpeedRunsChart($(element).find('.chart-container-0'), { chartData: data, topAmount: 10 }),
            new gameSpeedRunsPercentileChart($(element).find('.chart-container-1'), { chartData: data }),
            new gameSpeedRunsByMonthChart($(element).find('.chart-container-2'), { chartData: data })
        ];
    }

    var promises = $(charts).map(function () { return this.generateChart() });

    $.when.apply(null, promises).then(function () {
        def.resolve();
    });

    return def.promise();
}

/**Search functions **/
function filterCategories() {
    var categoryTypeIDs = $('#drpCategoryTypes').val();
    var gameIDs = $('#drpGames').val();
    var categoryIDs = $('#drpCategories').val();
    var levelIDs = $('#drpLevels').val();

    var categoryTypes = $(sra.categoryTypes).filter(function () { return categoryTypeIDs.length == 0 || categoryTypeIDs.indexOf(this.id) > -1 })
    var games = $(sra.games).filter(function () { return gameIDs.length == 0 || gameIDs.indexOf(this.id) > -1 })
    var categories = $(sra.categories).filter(function () { return categoryIDs.length == 0 || categoryIDs.indexOf(this.id) > -1 });
    var levels = $(sra.searchLevels).filter(function () { return levelIDs.length == 0 || levelIDs.indexOf(this.id) > -1 })

    $('#divSpeedRunGridContainer').hide();
    $('#divSpeedRunGridLoading').show();

    var gridModel = new speedRunGridModel("Game", categoryTypes, games, categories, levels);
    renderAndInitializeSpeedRunGrid($('#divSpeedRunGridContainer'), sra.speedRunGridTemplate, gridModel);

    $('#divSpeedRunGridContainer').show();
    $('#divSpeedRunGridLoading').hide();
}










