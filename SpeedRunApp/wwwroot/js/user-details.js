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

/**Initialize Functions**/
function initializeClient() {
    initializeEvents();

    var $activeCategoryTypeTab = $('.nav-item.categoryType a.active');
    onCategoryTypeTabClick($activeCategoryTypeTab);
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
    $('#divSpeedRunGrid').hide();
    $('#divSpeedRunGridLoading').show();

    getSpeedRunGridData().then(function (data) {
        cacheGridData(data);
        var gridModel = getSpeedRunGridModel(data);
        $.get('../templates/SpeedRunGrid.html?_t=' + (new Date()).getTime(), function (template, status) {
            renderAndInitializeSpeedRunGrid($('#divSpeedRunGrid'), template, gridModel);

            $('#divSpeedRunGrid').show();
            $('#divSpeedRunGridLoading').hide();
        });
    });
}

function cacheGridData(data) {
    $(data).each(function () {
        var categoryTypeID = this.cateogryType.ID;
        var gameID = this.gameID;
        var categoryID = this.categoryID;
        var levelID = this.levelID;

        sra.gridData[categoryTypeID] = sra.gridData[categoryTypeID] || [];
        sra.gridData[cateogryTypeID][gameID] = sra.gridData[cateogryTypeID][gameID] || [];
        sra.gridData[cateogryTypeID][gameID][categoryID] = sra.gridData[cateogryTypeID][gameID][categoryID] || [];
        sra.gridData[cateogryTypeID][gameID][categoryID][levelID] = sra.gridData[cateogryTypeID][gameID][categoryID][levelID] || [];

        sra.gridData[cateogryTypeID][gameID][categoryID][levelID].push(this);
    });
}

function getSpeedRunGridModel(data) {
    var groupedCategoryTypes = _.groupBy(data, function (value) {
        return value.categoryType.id + '#' + value.categoryType.name;
    });
    sra["searchCategoryTypes"] = $(groupedCategoryTypes).map(function () { return { id: this[0].id, name: this[0].name } }).toArray()

    var groupedCategories = _.groupBy(data, function (value) {
        return value.categoryID + '#' + value.categoryName + '#' + value.gameID + '#' + value.categoryType.id;
    });
    sra["searchCategories"] = $(groupedCategories).map(function () { return { id: this[0].categoryID, name: this[0].categoryName, gameID: this[0].gameID, categoryTypeID: this[0].id } }).value();

    var groupedGames = _.groupBy(data, function (value) {
        return value.gameID + '#' + value.gameName;
    });
    sra["searchGames"] = $(groupedGames).map(function () {
        var that = this;
        return { id: this[0].gameID, name: this[0].gameName, categoryTypeIDs: $(sra.searchCategories).filter(function () { this.gameID == that.gameID }).map(function () { return this }).toArray() }
    }).toArray();

    var groupedLevels = _.groupBy(data, function (value) {
        return value.levelID + '#' + value.levelName + "#" + value.gameID;
    });
    sra["searchLevels"] = $(groupedLevels).map(function () { return { id: this[0].levelID, name: this[0].levelName, gameID: this[0].gameID } }).toArray();

    return new speedRunGridModel("User", sra.searchCategoryTypes, sra.searchGames, sra.searchCategories, sra.searchLevels)
}

function getSpeedRunGridData() {
    var def1 = $.Deferred();
    var gameID = $("#hdnUserID").val();
    var elementsPerPage = sra.apiSettings.maxElementsPerPage;
    var elementsOffset = 0;
    var requestCount = 0;

    getAllUserSpeedRuns(gameID, elementsPerPage, elementsOffset, requestCount).then(function (data) {
        data.sort(function (a, b) { return a.PrimaryRunTimeMilliseconds - b.PrimaryRunTimeMilliseconds; });
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
            return setTimeout(function () { getUserSpeedRuns(userID, elementsPerPage, (elementsPerPage * requestCount), requestCount, items, def) }, 5000);
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

function renderAndInitializeSpeedRunGrid(element, speedRunGridTemplate, speedRunGridModel) {
    renderTemplate(element, speedRunGridTemplate, speedRunGridModel).then(function () {
        initializeSpeedRunGridEvents();

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

function initializeSpeedRunGridEvents() {
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
    var def = $.Deferred();
    var grid = $(element).find('.grid');
    var pagerID = $(element).find('.pager').attr("id");
    var categoryType = $(element).data('categorytype');
    var gameID = $(element).data('gameid');
    var categoryID = $(element).data('categoryid');
    var levelID = $(element).data('levelid') ? $(element).data('levelid') : '';
    var localData = sra.gridData[categoryType][gameID][categoryID][levelID];

    grid.jqGrid({
        datatype: "local",
        data: localData,
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

    function gridLoadComplete(element) {
        initializeGridEvents(this);
        initializeGridFilters(this);
        initializeGridStyles(this);
        var $gridContainer = $(this).closest('.grid-container');
        initializeScroller($gridContainer);

        var data = $(this).jqGrid("getGridParam", "data");
        def.resolve(data)
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

    function optionsFormatter(cellvalue, options, rowObject) {
        var html = "<a href='../SpeedRun/SpeedRunSummary?speedRunID=" + cellvalue + "' data-toggle='modal' data-target='#videoLinkModal' data-backdrop='static'><i class='fas fa-play-circle'></i></a>";

        if (rowObject.splitsLink) {
            html += "<a href='" + rowObject.splitsLink + "' class='options-link'><img src='/images/SplitsLogo.svg' style='width:20px;'></img></a>";
        }

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

    $('#divSpeedRunGrid').hide();
    $('#divSpeedRunGridLoading').show();

    var gridModel = new speedRunGridModel("Game", categoryTypes, games, categories, levels);
    renderAndInitializeSpeedRunGrid($('#divSpeedRunGrid'), sra.speedRunGridTemplate, gridModel);

    $('#divSpeedRunGrid').show();
    $('#divSpeedRunGridLoading').hide();
}









 


















