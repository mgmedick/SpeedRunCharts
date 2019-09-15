
function InitializeClient() {
    var $selectedGridContainer = $('.gridContainer.active');
    InitializeGrid($selectedGridContainer);

    InitializeEvents();
}

function InitializeEvents() {
    $('.nav-item a').one('click', OnNavItemSingleClick);
}

function OnNavItemSingleClick() {
    var jqGridContainerID = $(this).attr("href");
    var $gridContainer = $(jqGridContainerID);
    InitializeGrid($gridContainer);
}

function InitializeGrid(element) {
    var $grid = $(element).find('.grid');
    var $pager = $(element).find('.pager');
    var gameid = $(element).data('gameid');
    var categoryid = $(element).data('categoryid');

    $grid.jqGrid({
        url: 'GetLeaderboardRecords?gameID=' + gameid + '&categoryID=' + categoryid,
        datatype: "json",
        mtype: "GET",
        height: '100%',
        autowidth: true,
        resizeStop: resizeGrid,
        rowNum: 10,
        colNames: ["Player", "Category", "Platform", "Time", "Date"],
        colModel: [
            { name: "playerName" },
            { name: "categoryName" },
            { name: "platformName" },
            { name: "primaryRunTimeString", search: false },
            { name: "relativeDateSubmittedString", search: false }
        ],
        iconSet: "fontAwesome",
        guiStyle: "bootstrap4",
        ignoreCase: true,
        viewrecords: true,
        loadonce: true,
        loadComplete: function () {
            setSearchSelect($(this), 'playerName');
            setSearchSelect($(this), 'categoryName');
            setSearchSelect($(this), 'platformName');
            $(this).jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
        }
    });

    function setSearchSelect(grid, columnName) {
        grid.jqGrid("setColProp", columnName, {
            stype: "select",
            searchoptions: {
                value: buildSearchSelect(getUniqueNames(grid, columnName)),
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

    function getUniqueNames(grid, columnName) {
        var texts = grid.jqGrid("getCol", columnName);
        var uniqueTexts = [];
        var textsMap = {}

        for (var i = 0; i < texts.length; i++) {
            var text = texts[i];
            if (text !== undefined && textsMap[text] === undefined) {
                textsMap[text] = true;
                uniqueTexts.push(text);
            }
        }

        return uniqueTexts;
    }
}

















