if (!sra)
    sra = {};

if (!sra['graphObjects'])
    sra.graphObjects = {};

sra.graphObjects['SpeedRunsReported'] = {
    templatePath: 'SpeedRunsReported.html',
    title: 'Speed Runs Per Average',
    controller: function (container, dateHelper, gameID, categoryID)  {
        var chartConfig = {
            selector: 'div#placeholder',
            caption: 'Speed Runs Per Average',
            subCaption: 'All Time',
            showValues: 1,
            formatNumberScale: 0,
            numberOfDecimals: 1,
            showPercentValues: 1,
            exportEnabled: 1,
            showLegend: 1,
            showLabels: 0,
            theme: 'fusion'
            //numberscalevalue: "60",
            //numberscaleunit: " mins",
            //defaultnumberscale: "",
            //scalerecursively: "1",
            //maxscalerecursion: "-1",
            //scaleseparator: " "
        };

        return new sra.graphObjects.SpeedRunsReportedController(sra.ajaxHelper, _, container, { gameID: gameID, categoryID: categoryID }, new chartLoader(), chartConfig);
    }
};


