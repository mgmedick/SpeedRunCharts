if (!speedRun)
    speedRun = {};

if (!speedRun['graphObjects'])
    speedRun.graphObjects = {};

speedRun.graphObjects['SpeedRunSummaryByMonth'] = {
   templatePath: 'SpeedRunSummaryByMonth.html',
   title: 'Speed Run Summary by Month Last 6 Months',
   controller: function (container, dateHelper) {
       var chartConfig = {
               selector: 'div#placeholder',
               caption: 'Speed Run Summary by Month',
               subCaption: 'Last 6 Months',
               xAxis: 'Date',
               yAxis: 'Count of claims',
               exportEnabled: 1,
               showValues: 1,
               formatNumberScale: 0,
               numberOfDecimals: 1,
               useRoundEdges: 1,
       };
      
       return new speedRun.graphObjects.SpeedRunSummaryByMonthController(speedRun.ajaxHelper, _, container, { fromDate: dateHelper.MonthsAgo(6), toDate: dateHelper.Today() }, new chartLoader(), chartConfig);
   }
};


