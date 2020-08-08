if (!sra)
    sra = {};

if (!sra['graphObjects'])
    sra.graphObjects = {};

sra.graphObjects['SpeedRunsByUser'] = {
    templatePath: 'SpeedRunsByUser.html',
    title: 'Top 10 Speed Runs',
    controller: function (container, chartData) {
        var _chartLoader = new chartLoader();
        var _chartConfig = {
            selector: 'div#placeholder',
            caption: 'Top 10 Speed Runs',
            subCaption: '',
            xAxis: '',
            yAxis: 'Time (Minutes)',
            exportEnabled: 0,
            showValues: 1,
            formatNumberScale: 1,
            numberOfDecimals: 0,
            useRoundEdges: 1,
            numberscalevalue: "60,60",
            numberscaleunit: "m,h",
            defaultnumberscale: "s",
            scalerecursively: "1",
            maxscalerecursion: "-1",
            scaleseparator: ""
        };

        return new sra.graphObjects.SpeedRunsByUserController(container, { topAmount: 10 }, chartData, _chartLoader, _chartConfig);
    }
};


