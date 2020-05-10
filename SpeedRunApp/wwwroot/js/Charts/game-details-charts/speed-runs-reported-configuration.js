if (!sra)
    sra = {};

if (!sra['graphObjects'])
    sra.graphObjects = {};

sra.graphObjects['SpeedRunsReported'] = {
    templatePath: 'SpeedRunsReported.html',
    title: 'Speed Runs Reported',
    controller: function (container, dateHelper, gameID, categoryID)  {
        var chartConfig = {
            selector: 'div#placeholder',
            caption: 'Speed Runs Reported',
            subCaption: '{{startDate}} to {{endDate}}',
            showValues: 1,
            formatNumberScale: 0,
            numberOfDecimals: 1,
            showPercentValues: 1,
            exportEnabled: 1,
            showLegend: 1,
            showLabels: 0,
            theme: 'fusion',
        };

        return new sra.graphObjects.SpeedRunsReportedController(sra.ajaxHelper, _, container, { gameID: gameID, categoryID: categoryID, startDate: dateHelper.yearsAgo(10), endDate: dateHelper.today() }, new chartLoader(), chartConfig);
    }
};


