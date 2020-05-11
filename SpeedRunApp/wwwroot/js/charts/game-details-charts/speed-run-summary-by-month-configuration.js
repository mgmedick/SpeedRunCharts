if (!sra)
    sra = {};

if (!sra['graphObjects'])
    sra.graphObjects = {};

sra.graphObjects['SpeedRunSummaryByMonth'] = {
   templatePath: 'SpeedRunSummaryByMonth.html',
    title: 'Speedrun Monthly Average',
    controller: function (container, dateHelper, gameID, categoryID) {
       var chartConfig = {
               selector: 'div#placeholder',
               caption: 'Speedrun Monthly Average',
               subCaption: 'Last 6 Months',
               xAxis: 'Date',
               yAxis: 'Time (Minutes)',
               exportEnabled: 1,
               showValues: 1,
               formatNumberScale: 0,
               numberOfDecimals: 1,
               useRoundEdges: 1,
       };
      
        return new sra.graphObjects.SpeedRunSummaryByMonthController(sra.ajaxHelper, _, container, { gameID: gameID, categoryID: categoryID, startDate: dateHelper.monthsAgo(6), endDate: dateHelper.today() }, new chartLoader(), chartConfig);
   }
};


