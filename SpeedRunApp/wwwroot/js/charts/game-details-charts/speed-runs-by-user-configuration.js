if (!sra)
    sra = {};

if (!sra['graphObjects'])
    sra.graphObjects = {};

sra.graphObjects['SpeedRunsByUser'] = {
    templatePath: 'SpeedRunsByUser.html',
    title: 'Speeruns By User',
    controller: function (container, dateHelper, gameID, categoryID)  {
        var chartConfig = {
            selector: 'div#placeholder',
            caption: 'Speeruns By User',
            subCaption: 'Last 6 Months',
            xAxis: '',
            yAxis: 'Time (Minutes)',
            exportEnabled: 1,
            showValues: 1,
            formatNumberScale: 0,
            numberOfDecimals: 1,
            useRoundEdges: 1,
        };

        return new sra.graphObjects.SpeedRunsByUserController(sra.ajaxHelper, _, container, { gameID: gameID, categoryID: categoryID, startDate: dateHelper.monthsAgo(6), endDate: dateHelper.today() }, new chartLoader(), chartConfig);
    }
};


