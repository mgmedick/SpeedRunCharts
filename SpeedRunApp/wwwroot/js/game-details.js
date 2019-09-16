
function InitializeClient() {
    var $activeCategoryTypeTab = $('.nav-item.categoryType a.active');
    OnCategoryTypeTabSingleClick($activeCategoryTypeTab);

    InitializeEvents();
}

function InitializeEvents() {
    $('.nav-item.categoryType a').one('shown.bs.tab', function () {
        OnCategoryTypeTabSingleClick(this);
    });

    $('.nav-item.category a').one('shown.bs.tab', function () {
        OnCategoryTabSingleClick(this);
    });
}

function OnCategoryTypeTabSingleClick(element) {
    var jqCategoryTypeContainerID = $(element).attr("href");
    var $activeCategoryTab = $(jqCategoryTypeContainerID).find('.category a.active');

    OnCategoryTabSingleClick($activeCategoryTab);
}

function OnCategoryTabSingleClick(element) {
    var jqCategoryContainerID = $(element).attr("href");
    var $gridContainer = $(jqCategoryContainerID);
    InitializeGrid($gridContainer);
}

function InitializeGrid(element) {
    var $grid = $(element).find('.grid');
    var pagerID = $(element).find('.pager').attr("id");
    var gameID = $(element).data('gameid');
    var categoryID = $(element).data('categoryid');
    var categoryType = $(element).data('categorytype');

    $grid.jqGrid({
        url: 'GetLeaderboardRecords?gameID=' + gameID + '&categoryID=' + categoryID + '&categoryType=' + categoryType,
        datatype: "json",
        mtype: "GET",
        height: '100%',
        autowidth: true,
        //resizeStop: resizeGrid,
        rowNum: 50,
        pager: pagerID,
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

    //var width = $grid.parent().width();
    //$grid.setGridWidth(width);

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

















