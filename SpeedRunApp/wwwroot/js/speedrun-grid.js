if (!sra) {
    var sra = {};
}

function speedRunGridModel(sender, tabItems) {
    this.sender = sender,
        this.tabItems = tabItems
}

function speedRunGridVariableModel(subCategoryVariables, classPrefix, gameID, categoryTypeID, categoryID, levelID, gameIndex, categoryTypeIndex, categoryIndex, levelIndex, prevID, prevData, count) {
    this.subCategoryVariables = subCategoryVariables,
    this.classPrefix = classPrefix,
    this.gameID = gameID,
    this.categoryTypeID = categoryTypeID,
    this.categoryID = categoryID,
    this.levelID = levelID,
    this.gameIndex = gameIndex,
    this.categoryTypeIndex = categoryTypeIndex,
    this.categoryIndex = categoryIndex,
    this.levelIndex = levelIndex,
    this.prevID = prevID,
    this.prevData = prevData,
    this.count = count
}

/**Initialize Functions**/
function initalizeSpeedRunGrid(sender, id) {
    $('#divSpeedRunGridContainer').hide();
    $('#divSpeedRunGridLoading').show();

    loadSpeedRunGridTemplate(sender, id);
}

/*Load Functions*/
function loadSpeedRunGridTemplate(sender, id) {
    $.get('../' + sender + '/GetSpeedRunGrid?ID=' + id + '&_t=' + (new Date()).getTime(), function (data, status) {
        $.get('../templates/SpeedRunGridSearch.html?_t=' + (new Date()).getTime(), function (searchTemplate, status) {
            $.get('../templates/SpeedRunGrid.html?_t=' + (new Date()).getTime(), function (gridTemplate, status) {
                $.get('../templates/SpeedRunGridVariable.html?_t=' + (new Date()).getTime(), function (gridVariableTemplate, status) {
                    $.get('../templates/SpeedRunGridVariableChart.html?_t=' + (new Date()).getTime(), function (chartVariableTemplate, status) {
                        sra['sender'] = sender;
                        sra['speedRunGridModel'] = data.gridModel;
                        sra['speedRunGridData'] = data.gridData; //getFormattedData(data.gridData);
                        sra['speedRunGridTemplate'] = gridTemplate;
                        sra['renderSpeedRunGridVariableTemplate'] = _.template(gridVariableTemplate);
                        sra['renderSpeedRunGridVariableChartTemplate'] = _.template(chartVariableTemplate);

                        renderAndInitializeSearchSpeedRunGrid($('#divSearchSpeedRunGridContainer'), searchTemplate, sra.speedRunGridModel);
                        renderAndInitializeSpeedRunGrid($('#divSpeedRunGridContainer'), sra.speedRunGridTemplate, sra.speedRunGridModel, sra.renderSpeedRunGridVariableTemplate, sra.renderSpeedRunGridVariableChartTemplate);

                        $('#divSpeedRunGridContainer').show();
                        $('#divSpeedRunGridLoading').hide();
                    });
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

        var $activeGameTab = $('.nav-item.game a.active');
        onGameTabClick($activeGameTab);
    });
}

function initializeSearchSpeedRunGridEvents(element) {
    $(element).find('.select2').select2({ dropdownAutoWidth: true, width: "element" });
    $('#divSearchSpeedRunGrid').setupCollapsible({ initialState: "visible", linkHiddenText: "Show Search", linkDisplayedText: "Hide Search" });

    $('#drpGames').change(function () {
        onGameChange(this);
    });

    $('#drpCategoryTypes').change(function () {
        onCategoryTypeChange(this);
    });

    $('#drpCategories').change(function () {
        onCategoryChange(this);
    });
}

function onGameChange(element) {
    var selectedGameIDs = $(element).val();
    var selectedGames = $(sra.speedRunGridModel.tabItems).filter(function () {
        return selectedGameIDs.indexOf(this.id) > -1
    }).get();

    var categories = $(selectedGames).map(function () { return this.categories }).get();
    var categoryTypes = $(selectedGames).map(function () { return this.categoryTypes }).get();
    var levels = $(selectedGames).map(function () { return this.levels }).get();

    var selectedCategoryTypeID = $('#drpCategoryTypes').val();
    if (selectedCategoryTypeID) {
        categories = $(categories).filter(function () {
            return selectedCategoryTypeID == -1 || this.categoryTypeID == selectedCategoryTypeID;
        }).get();
        categoryTypes = $(categoryTypes).filter(function () {
            return selectedCategoryTypeID == -1 || this.id == selectedCategoryTypeID;
        }).get();
    }

    populateDropDown($('#drpCategories'), categories);
    populateDropDown($('#drpCategoryTypes'), categoryTypes);
    populateDropDown($('#drpLevels'), levels);
}

function onCategoryTypeChange(element) {
    var selectedCategoryTypeID = $(element).val();

    var games = $(sra.speedRunGridModel.tabItems).filter(function () {
        return $(this.categoryTypeIDs).filter(function () { return selectedCategoryTypeID == -1 || this.categoryTypeID == selectedCategoryTypeID }).length > 0
    }).get();
    var categories = $(sra.speedRunGridModel.tabItems).map(function () { return this.categories }).filter(function () {
        return selectedCategoryTypeID == -1 || this.categoryTypeID == selectedCategoryTypeID;
    }).get();

    var selectedGameIDs = $('#drpGames').val();
    if (selectedGameIDs.length > 0) {
        games = $(games).filter(function () { return selectedGameIDs.indexOf(this.id) > -1 });
        var selectedGameCategoryIDs = $(games).map(function () { return this.categories }).map(function () { return this.id; }).get()
        categories = $(categories).filter(function () { return selectedGameCategoryIDs.indexOf(this.id) > -1 }).get();
    }

    if (selectedCategoryTypeID == 0) {
        $('#divLevels').hide();
    } else {
        $('#divLevels').show();
    }

    populateDropDown($('#drpGames'), games);
    populateDropDown($('#drpCategories'), categories);
}

function onCategoryChange(element) {
    var selectedCategoryIDs = $(element).val();

    $('.variable-search').each(function () {
        var categoryID = $(this).data("categoryid");
        if (selectedCategoryIDs.indexOf(categoryID)) {
            $(this).show();
        } else {
            $(this).hide();
        }
    });
}

function populateDropDown(element, items) {
    $(element).empty();
    $(items).each(function () {
        $(element).append($('<option>').text(this.name).attr('value', this.id));
    });
}

function initializeSpeedRunGridEvents(element) {
    $('#divChartContainer').setupCollapsible({ initialState: "visible", linkHiddenText: "Show Charts", linkDisplayedText: "Hide Charts" });

    $(element).find('.nav-item.game a').click(function () {
        onGameTabClick(this);
    });

    $(element).find('.nav-item.categoryType a').click(function () {
        onCategoryTypeTabClick(this);
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

    $(element).find('.nav-item.level-variable-value a').click(function () {
        onLevelVariableValueTabClick(this);
    });
}

/*Event Handlers*/
function onGameTabClick(element) {
    var gameContainerID = $(element).attr('href');
    var $container = $(gameContainerID);
    var gameChartContainerID = gameContainerID + '-charts';
    var $chartContainer = $(gameChartContainerID);

    $('.game-tab-pane').hide();
    $container.fadeIn();

    $('.game-tab-pane-charts').hide();
    $chartContainer.fadeIn();

    var $activeCategoryTypeTab = $container.find('.categoryType a.active');
    onCategoryTypeTabClick($activeCategoryTypeTab);
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
                initializeGridStyles($container);
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
            initializeGridStyles($container);
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

    if ($container.find('.level-variable-tabs').length > 0) {
        var $activeVariableValueTab = $container.find('.level-variable-tabs:first .level-variable-value a.active')
        onLevelVariableValueTabClick($activeVariableValueTab);
    }
    else {
        if (!$container.find('.grid')[0].grid) {
            configureAndInitializeGrid($container.find('.grid-container')).then(function (data) {
                initializeCharts($chartContainer.find('.charts-container'), data);
            });
        } else {
            initializeGridStyles($container);
        }
    }
}

function onLevelVariableValueTabClick(element) {
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

    if ($container.find('.level-variable-tabs').length > 0) {
        var $activeVariableValueTab = $container.find('.level-variable-tabs:first .level-variable-value a.active')
        onLevelVariableValueTabClick($activeVariableValueTab);
    } else {
        if (!$container.find('.grid')[0].grid) {
            configureAndInitializeGrid($container.find('.grid-container')).then(function (data) {
                initializeCharts($chartContainer, data);
            });
        } else {
            initializeGridStyles($container);
        }
    }
}



//Initialize Grids
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
    var data = $(sra.speedRunGridData).filter(function () {
        return this.game.id == gameID
            && this.category.id == categoryID
            && (this.level ? this.level.id : '') == levelID
            && (this.subCategoryVariableValues ? $(this.subCategoryVariableValues).map(function () { return this.item1 + '|' + this.item2 }).get().join(',') : '') == variableValues
    }).get();
    
    $loading.show();
    $grid.hide();

    var perc = 1;
    if (window.matchMedia) {
        var mq = window.matchMedia("(min-width: 1366px) and (max-width: 1920px)");
        if (mq.matches) {
            perc = $(window).width() / 1950.0;
        }
    }

    var columnNames = ["", "Rank", "Players", "Platform", "Emulated", "Primary Time", "Status", "Reject Reason", "Submitted Date", "Verified Date", "Hidden", "Hidden", "Hidden"];

    var columnModel = [
        { name: "id", width: 100 * perc, resizable: false, search: false, formatter: optionsFormatter, align: "center" },
        { name: "rank", width: 75 * perc, sorttype: "number", formatter: rankFormatter, search: true, searchoptions: { sopt: ["nIn"] }, hidden: isSenderUser },
        { name: "players", width: 160 * perc, formatter: playerFormatter, search: true, searchoptions: { sopt: ["aIn"] } },
        { name: "platform.name", width: 160 * perc, search: true, searchoptions: { sopt: ["in"] } },
        { name: "isEmulatedString", width: 125 * perc, search: true, searchoptions: { sopt: ["in"] } },
        { name: "primaryTimeString", width: 160 * perc, search: false },
        { name: "statusType.name", width: 125 * perc, search: true, searchoptions: { sopt: ["in"] }, hidden: !isSenderUser },
        { name: "rejectedReason", width: 160 * perc, hidden: !showRejectedReason(isSenderUser, data) },
        { name: "dateSubmitted", width: 160 * perc, sorttype: "date", formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y H:i" }, cellattr: dateSubmittedCellAttr, search: true, searchoptions: { sopt: ["deq", "dge", "dle"] } },
        { name: "verifyDate", width: 160 * perc, sorttype: "date", formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y H:i" }, cellattr: verifyDateCellAttr, search: true, searchoptions: { sopt: ["deq", "dge", "dle"] } },
        { name: "relativeDateSubmittedString", hidden: true },
        { name: "relativeVerifyDateString", hidden: true },
        { name: "game", hidden: true }
    ];

    var game = $(sra.speedRunGridModel.tabItems).filter(function () { return this.id == gameID }).get(0);

    $(data).each(function () {
        var item = this;
        $(item.variableValues).each(function () {
            var variableValue = this;
            var gameVariable = $(game.variables).filter(function () { return this.id == variableValue.item1 }).get(0);
            var gameVariableValue = $(gameVariable.variableValues).filter(function () { return this.id == variableValue.item2 }).get(0);
            item[gameVariable.id] = gameVariableValue.name;
        });
    });

    $(game.variables).filter(function () {
        var variable = this;
        return !variable.isSubCategory && $(data).filter(function () {
            return this.variableValues
                && $(this.variableValues).filter(function () {
                    return this.item1 == variable.id
                }).length > 0
        }).length > 0
    }).each(function () {
        columnNames.push(this.name);
        var variable = { name: this.id, width: "100%", search: true, searchoptions: { sopt: ["in"] } };
        columnModel.push(variable);
    });

    columnNames.push("Comment");
    columnModel.push({ name: "comment", width: 100, search: false, formatter: commentFormatter, align: "center" });

    initializeGrid($grid, pagerID, data, columnModel, columnNames).then(function (gridData) {
        def.resolve(gridData);
        $grid.show();
        $loading.hide();
    });

    //Column Formatters
    function optionsFormatter(cellvalue, options, rowObject) {
        var html = "<div>"
        html += "<div class='d-table' style='border:none; border-collapse:collapse; border-spacing:0; margin:auto;'>";
        html += "<div class='d-table-row'>";
        html += "<div class='d-table-cell pl-1' style='border:none; padding:0px; width:30px;'>";
        html += "<a href=\"javascript:showSpeedRunSummary('" + this.id + "','" + options.rowId + "');\"><i class='fas fa-play-circle'></i></a>";
        html += "</div>";
        html += "<div class='d-table-cell pl-1 ' style='border:none; padding:0px; width:30px;'>";
        html += "<a href=\"javascript:showSpeedRunDetails('" + this.id + "','" + options.rowId + "');\"><i class='fas fa-edit'></i></a>";
        html += "</div>";
        html += "<div class='d-table-cell pl-1' style='border:none; padding:0px; width:30px;'>";
        html += (rowObject.splitsLink) ? "<a href=\"javascript:showSpilts('" + this.id + "','" + options.rowId + "');\" class='options-link'><img src='/images/SplitsLogo.svg' style='width:20px;'></img></a>" : "";
        html += "</div>";
        html += "</div>";
        html += "</div>";
        html += "</div>";

        return html;
    }

    function rankFormatter(value, options, rowObject) {
        var num = parseInt(value);
        var html = (num) ? sra.mathHelper.getIntOrdinalString(num) : '-';

        return html;
    }

    function playerFormatter(value, options, rowObject) {
        var html = '';

        $(value).each(function () {
            html += "<a href='../User/UserDetails?userID=" + this.id + "'>" + this.name + "</a><br/>";
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

    function showRejectedReason(isSenderUser, data) {
        return isSenderUser && $(data).filter(function () { return this.rejectedReason }).length > 0;
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
        //postData: {
        //    filters: '{"groupOp":"AND","rules":[{"field":"rank","op":"nIn","data":""},{"field":"dateSubmitted","op":"dge","data":""},{"field":"dateSubmitted","op":"dle","data":""}]}'
        //},
        iconSet: "fontAwesome",
        guiStyle: "bootstrap4",
        ignoreCase: true,
        viewrecords: true,
        loadonce: true,
        altRows: true,
        altClass: 'alt-row',
        loadComplete: gridLoadComplete,
        customSortOperations: {
            deq: {
                operand: "==",
                text: "equal",
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
                text: "greater or equal",
                filter: function (options) {
                    var fieldData = new Date(options.item[options.cmName]);
                    var searchValue = new Date(options.searchValue);

                    return fieldData >= searchValue;
                }
            },
            dle: {
                operand: "<=",
                text: "less or equal",
                filter: function (options) {
                    var fieldData = new Date(options.item[options.cmName]);
                    var searchValue = new Date(options.searchValue);

                    return fieldData <= searchValue;
                }
            },
            aIn: {
                operand: "aIN",
                text: "is in",
                filter: function (options) {
                    var fieldData = options.item[options.cmName];
                    var searchValues = options.searchValue.split(',');

                    return $(fieldData).filter(function () { return searchValues.indexOf(this.name) > -1 }).length > 0;
                }
            },
            nIn: {
                operand: "nIN",
                text: "is in",
                filter: function (options) {
                    var fieldData = options.item[options.cmName];
                    var searchValues = options.searchValue.split(',');

                    return $(searchValues).filter(function () { return this == fieldData }).length > 0;
                }
            }
        }
    });

    grid.jqGrid('navGrid', '#' + pagerID, { add: false, del: false, search: true, refresh: false }, {}, {}, {}, { multipleSearch: true });

    function gridLoadComplete() {
        initializeGridEvents(this);
        initializeGridFilters(this);
        initializeGridStyles(this);

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
        var columns = $(element).jqGrid("getGridParam", "colModel");

        $(columns).each(function () {
            var column = this;
            if (column.search && !column.hidden) {
                var colSearchData = [];
                if (column.sorttype == "date") {
                    searchOptions = []
                    setSearchDate(element, column.name, column.searchoptions.sopt);
                } else {
                    switch (column.name) {
                        case "rank":
                            colSearchData = _.chain(data).map(function (item) { return parseInt(item.rank) }).uniq().sortBy(function (item) { return item; }).value();
                            break;
                        case "players":
                            colSearchData = _.chain(data).pluck('players').flatten().filter(function (item) { return item }).uniq(function (item) { return [item.id, item.name].join(); }).sortBy(function (item) { return item.name }).map(function (value) { return value.name }).value();
                            break;
                        case "platform.name":
                            colSearchData = _.chain(data).filter(function (item) { return item.platform }).map(function (item) { return item.platform.name }).uniq().sortBy(function (item) { return item }).value();
                            break;
                        default:
                            colSearchData = _.chain(data).filter(function (item) { return item[column.name] }).map(function (item) { return item[column.name] }).uniq().sortBy(function (item) { return item }).value();
                            break;
                    }

                    setSearchSelect($(element), column.name, colSearchData, column.searchoptions.sopt);
                }
            }
        });

        $(element).jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
    }

    function setSearchDate(element, columnName, sortOptions) {
        $(element).jqGrid("setColProp", columnName, {
            searchoptions: {
                sopt: sortOptions,
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
            }
        });
    }

    function setSearchSelect(element, columnName, searchData, sortOptions) {
        if (searchData.length > 0) {
            $(element).jqGrid("setColProp", columnName, {
                stype: "select",
                searchoptions: {
                    value: buildSearchSelect(searchData),
                    sopt: sortOptions,
                    attr: {
                        multiple: "multiple",
                        attr: { style: "width:100%;" }//,
                        //size: 4
                    },
                    dataInit: function (element) {
                        setTimeout(function () {
                            $(element).select2({ width: "element" });
                        }, 0);
                    }
                }
            });
        } else {
            $(element).jqGrid("setColProp", columnName, { search: false });
        }
    }

    function buildSearchSelect(items) {
        var values = $(items).map(function () {
            return this + ":" + this;
        }).get().join(";");
        return values;
    }

    return def.promise();
}

function initializeGridStyles(element) {
    var $tabgridContainer = $(element).closest('.tab-grid-container');
    $tabgridContainer.css('width', parseInt($tabgridContainer.find('.ui-jqgrid-view:visible').width()) + parseInt($tabgridContainer.css('padding-left')));
    initializeScroller($tabgridContainer);

    //if (getCookie("theme")== "theme-dark") {
    //    $tabgridContainer.find('.table').removeClass('table-active').addClass('table-dark');
    //    $tabgridContainer.find('.ui-pg-table').removeClass('table-active').addClass('table-dark');
    //} else {
    //    $tabgridContainer.find('.table').removeClass('table-dark').addClass('table-active');
    //    $tabgridContainer.find('.ui-pg-table').removeClass('table-dark').addClass('table-active');
    //}
}

//Initialize Charts
function initializeCharts(element, data) {
    var def = $.Deferred();
    if (sra.sender == "User") {
        sra['charts'] = [
            new userTopSpeedRunsChart($(element).find('.chart-container-0'), { chartData: data, topAmount: 10 }),
            new userSpeedRunsPercentileChart($(element).find('.chart-container-1'), { chartData: data }),
            new userSpeedRunsByDateChart($(element).find('.chart-container-2'), { chartData: data })
        ];
    } else {
        sra['charts'] = [
            new gameTopSpeedRunsChart($(element).find('.chart-container-0'), { chartData: data, topAmount: 10 }),
            new gameSpeedRunsPercentileChart($(element).find('.chart-container-1'), { chartData: data }),
            new gameSpeedRunsByMonthChart($(element).find('.chart-container-2'), { chartData: data })
        ];
    }

    var promises = $(sra.charts).map(function () { return this.generateChart() });

    $.when.apply(null, promises).then(function () {
        def.resolve();
    });

    return def.promise();
}

/**Search functions **/
function search() {
    var gameIDs = $('#drpGames').val();
    var categoryTypeID = $('#drpCategoryTypes').val();
    var categoryIDs = $('#drpCategories').val();
    var levelIDs = $('#drpLevels').val();
    var hideEmpty = $('#chkHideEmpty').prop("checked");
    var tabItems = sra.speedRunGridModel.tabItems.map(a => Object.assign({}, a));
    $(tabItems).each(function () {
        this.subCategoryVariables = cloneVariables(this.subCategoryVariables);
    });

    if (gameIDs.length > 0) {
        tabItems = $(gridModel.tabItems).filter(function () { return gameIDs.indexOf(this.id) > -1 }).get();
    }

    if (categoryTypeID != -1) {
        $(tabItems).each(function () {
            this.categoryTypes = $(this.categoryTypes).filter(function () { return categoryTypeID == this.id }).get();
            //this.categories = $(this.categories).filter(function () { return categoryTypeID = this.categoryTypeID }).value();
            //this.subCategoryVariables = $(this.subCategoryVariables).filter(function () { return (categoryTypeID == 0 && (this.scopeTypeID == 0 || this.scopeTypeID == 1)) || (categoryTypeID == 1 && (this.scopeTypeID == 2 || this.scopeTypeID == 3)) }).value();
        });
    }

    if (categoryIDs.length > 0) {
        $(tabItems).each(function () {
            this.categories = $(this.categories).filter(function () { return categoryIDs.indexOf(this.id) > -1 }).get();
            //var filteredCategoryIDs = $(this.categories).map(function () { return this.id; })
            //this.subCategoryVariables = $(this.subCategoryVariables).filter(function () { return filteredCategoryIDs.indexOf(this.categoryID) > -1 }).value();
        });
    }

    if (levelIDs.length > 0) {
        $(tabItems).each(function () {
            this.levels = $(this.levels).filter(function () { return levelIDs.indexOf(this.id) > -1 }).get();
            //var filteredLevelIDs = $(this.categories).map(function () { return this.id; })
            //this.subCategoryVariables = $(this.subCategoryVariables).filter(function () { return filteredLevelIDs.indexOf(this.levelID) > -1 }).value();
        });
    }

    $('.variable-select').each(function () {
        var variableName = $(this).data('variablename');
        var variableValueNames = $(this).val();
        if (variableValueNames.length > 0) {
            var variables = $(tabItems).map(function () { return this.subCategoryVariables }).get();
            filterVariableValues(variables, variableName, variableValueNames);
        }
    });

    if (hideEmpty) {
        $(tabItems).each(function () {
            this.categories = $(this.categories).filter(function () { return this.hasData }).get();
            this.levels = $(this.levels).filter(function () { return this.hasData }).get();
            hideEmptyVariableValues(this.subCategoryVariables);
        });
    }

    $('#divSpeedRunGridContainer').hide();
    $('#divSpeedRunGridLoading').show();

    var gridModel = new speedRunGridModel(sra.sender, tabItems);
    renderAndInitializeSpeedRunGrid($('#divSpeedRunGridContainer'), sra.speedRunGridTemplate, gridModel, sra.renderSpeedRunGridVariableTemplate, sra.renderSpeedRunGridVariableChartTemplate);

    $('#divSpeedRunGridContainer').show();
    $('#divSpeedRunGridLoading').hide();
}

function cloneVariables(variables) {
    var clonedVariables = variables.map(a => Object.assign({}, a));

    $(clonedVariables).each(function () {
        this.variableValues = this.variableValues.map(a => Object.assign({}, a));
        $(this.variableValues).each(function () {
            if (this.subVariables.length > 0) {
                this.subVariables = cloneVariables(this.subVariables);
            }
        });
    });

    return clonedVariables;
}

function filterVariableValues(variables, variableName, variableValueNames) {
    $(variables).each(function () {
        if (this.name == variableName) {
            this.variableValues = $(this.variableValues).filter(function () { return variableValueNames.indexOf(this.name) > -1 }).get();
        }

        var subVariables = $(this.variableValues).map(function () { return this.subVariables; }).get();
        if (subVariables.length > 0) {
            filterVariableValues(subVariables, variableName, variableValueNames);
        }
    });
}

function hideEmptyVariableValues(variables) {
    $(variables).each(function () {
        this.variableValues = $(this.variableValues).filter(function () { return this.hasData }).get();

        var subVariables = $(this.variableValues).map(function () { return this.subVariables; }).get();
        if (subVariables.length > 0) {
            hideEmptyVariableValues(subVariables);
        }
    });
}

function showSpeedRunSummary(gridID, rowID) {
    var currentItem = $('#' + gridID).jqGrid("getLocalRow", rowID);
    var $modal = $('#editModal');
    var $modalTitle = $('#editModal').find('.modal-title');
    var $modalBody = $('#editModal').find('.modal-body');
    var $modalLoading = $('#editModal').find('.modal-loading');

    $modalBody.hide();
    $modalLoading.show();
    $modal.modal('show');
    $.get('../templates/SpeedRunSummary.html?_t=' + (new Date()).getTime(), function (summaryTemplate, status) {
        renderTemplate(null, summaryTemplate, currentItem).then(function (result) {
            var $header = $(result).find('.header');
            var $body = $(result).find('.body');

            $header.find('.details').remove();
            $modalTitle.html($header);
            $modalBody.html($body);
            $modalBody.show();
            $modalLoading.hide();
        });
    });
}

function showSpeedRunDetails(gridID, rowID) {
    var currentItem = $('#' + gridID).jqGrid("getLocalRow", rowID);
    var $modal = $('#editModal');
    var $modalTitle = $('#editModal').find('.modal-title');
    var $modalBody = $('#editModal').find('.modal-body');
    var $modalLoading = $('#editModal').find('.modal-loading');
    $modalTitle.html("<h5>Details</h5>");

    $modalBody.hide();
    $modalLoading.show();
    $modal.modal('show');
    $.get('../templates/SpeedRunEdit.html?_t=' + (new Date()).getTime(), function (detailsTemplate, status) {
        $.get('../SpeedRun/GetEditSpeedRun?gameID=' + currentItem.game.id + '&speedRunID=' + currentItem.id + '&isReadOnly=true', function (data, status) {
            data.speedRunVM = currentItem;
            renderTemplate($modalBody, detailsTemplate, data).then(function () {
                initializeSpeedRunEdit(data.isReadOnly);
                $modalBody.show();
                $modalLoading.hide();
            });
        });
    });
}

function showSpilts(gridID, rowID) {
    var $modal = $('#editModal');
    var $modalTitle = $('#editModal').find('.modal-title');
    var $modalBody = $('#editModal').find('.modal-body');
    var $modalLoading = $('#editModal').find('.modal-loading');
    $modalTitle.html("<h5>Splits</h5>");

    $modalLoading.hide();
    $modalBody.html("<div class='row'><div class='col-auto m-auto'><h4>Coming Soon</h4></div></div>");
    $modalBody.show();
    $modal.modal('show');
}










