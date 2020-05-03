if (!speedRun)
    speedRun = {};

if (!speedRun['graphObjects'])
    speedRun.graphObjects = {};

speedRun.graphObjects['SpeedRunSummaryByMonth'] = {
   templatePath: 'SpeedRunSummaryByMonth.html',
   title: 'Speed Run Summary by Month Last 1 Months',
    controller: function (container, gameID, categoryIDs, startDate, endDate) {
       var chartConfig = {
               selector: 'div#placeholder',
               caption: 'Speed Run Summary by Month',
               subCaption: 'Last 6 Months',
               xAxis: 'Date',
               yAxis: 'Time',
               exportEnabled: 1,
               showValues: 1,
               formatNumberScale: 0,
               numberOfDecimals: 1,
               useRoundEdges: 1,
       };
      
        return new speedRun.graphObjects.SpeedRunSummaryByMonthController(speedRun.ajaxHelper, _, container, { gameID: gameID, categoryIDs: categoryIDs, startDate: startDate, endDate: endDate }, new chartLoader(), chartConfig);
   }
};


