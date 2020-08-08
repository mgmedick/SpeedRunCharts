if (!sra)
    sra = {};

if (!sra['graphObjects'])
    sra.graphObjects = {};

sra.graphObjects['SpeedRunsReported'] = {
    templatePath: 'SpeedRunsReported.html',
    title: 'Speed Runs Per Average',
    controller: function (container, chartData) {
        var _chartLoader = new chartLoader();
        var _chartConfig = {
            selector: 'div#placeholder',
            caption: 'Speed Run Percentiles',
            subCaption: 'All Time',
            showValues: 1,
            formatNumberScale: 0,
            numberOfDecimals: 0,
            showPercentValues: 0,
            showPercentInTooltip: 1,
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

        return new sra.graphObjects.SpeedRunsReportedController(container, {}, chartData, _chartLoader, _chartConfig);
    }
};


