if (!sra)
    sra = {};

if (!sra['graphObjects'])
    sra.graphObjects = {};

sra.graphObjects['SpeedRunsByUser'] = {
    templatePath: 'SpeedRunsByUser.html',
    title: 'Top 10 Speed Runs',
    controller: function (container, dateHelper, gameID, categoryID)  {
        var chartConfig = {
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
            numberscalevalue: "60",
            numberscaleunit: " :",
            defaultnumberscale: "",
            scalerecursively: "1",
            maxscalerecursion: "-1",
            scaleseparator: " "
        };

        return new sra.graphObjects.SpeedRunsByUserController(sra.ajaxHelper, _, container, { gameID: gameID, categoryID: categoryID, topAmount: 10 }, new chartLoader(), chartConfig);
    }
};


