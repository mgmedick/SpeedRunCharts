if (!sra)
    sra = {};

if (!sra['graphObjects'])
    sra.graphObjects = {};

sra.graphObjects['SpeedRunsByUser'] = {
    templatePath: 'SpeedRunsByUser.html',
    title: 'Top 10 Speed Runs',
    controller: function (container, gameID, categoryType, categoryID, levelID)  {
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
            numberscalevalue: "60,60",
            numberscaleunit: "m,h",
            defaultnumberscale: "s",
            scalerecursively: "1",
            maxscalerecursion: "-1",
            scaleseparator: ""
        };

        return new sra.graphObjects.SpeedRunsByUserController(sra.ajaxHelper, _, container, { gameID: gameID, categoryType: categoryType, categoryID: categoryID, levelID: levelID, topAmount: 10 }, new chartLoader(), chartConfig);
    }
};


