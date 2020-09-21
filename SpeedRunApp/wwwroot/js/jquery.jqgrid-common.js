function autoresizeGrid() {
    var $grid = $(this);
    var columns = $grid.jqGrid('getGridParam', 'colModel');

    var colsTotalWidth = 0;
    for (var i = 0; columns[i]; i++) {

        if (columns[i].hidden == null || !columns[i].hidden) {
            colsTotalWidth += columns[i].width;
            $grid.setColProp(columns[i].name, { width: columns[i].width, widthOrg: columns[i].width });
        }
    }

    colsTotalWidth += 50;
    $(this).jqGrid('setGridWidth', colsTotalWidth, true);
}


