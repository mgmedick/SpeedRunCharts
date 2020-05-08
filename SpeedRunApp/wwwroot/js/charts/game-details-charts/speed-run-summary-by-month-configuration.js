﻿if (!sra)
    sra = {};

if (!sra['graphObjects'])
    sra.graphObjects = {};

sra.graphObjects['SpeedRunSummaryByMonth'] = {
   templatePath: 'SpeedRunSummaryByMonth.html',
   title: 'Speed Run Summary',
    controller: function (container, gameID, categoryID, startDate, endDate) {
       var chartConfig = {
               selector: 'div#placeholder',
               caption: 'Speed Run Summary',
               subCaption: 'Last 6 Months',
               xAxis: 'Date',
               yAxis: 'Time (Minutes)',
               exportEnabled: 1,
               showValues: 1,
               formatNumberScale: 0,
               numberOfDecimals: 1,
               useRoundEdges: 1,
       };
      
        return new sra.graphObjects.SpeedRunSummaryByMonthController(sra.ajaxHelper, _, container, { gameID: gameID, categoryID: categoryID, startDate: startDate, endDate: endDate }, new chartLoader(), chartConfig);
   }
};


