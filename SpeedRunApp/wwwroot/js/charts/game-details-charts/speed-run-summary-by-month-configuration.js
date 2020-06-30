if (!sra)
    sra = {};

if (!sra['graphObjects'])
    sra.graphObjects = {};

sra.graphObjects['SpeedRunSummaryByMonth'] = {
   templatePath: 'SpeedRunSummaryByMonth.html',
    title: 'Speed Runs Per Month',
    controller: function (container, gameID, categoryType, categoryID, levelID) {
       var chartConfig = {
               selector: 'div#placeholder',
               caption: 'Speed Runs Per Month',
               subCaption: 'Most recent 6 Months of activity',
               xAxis: 'Date',
               yAxis: 'Time (Minutes)',
               exportEnabled: 1,
               showValues: 0,
               formatNumberScale: 1,
               numberOfDecimals: 0,
               useRoundEdges: 1,
               numberscalevalue: "60,60",
               numberscaleunit: "m,h",
               defaultnumberscale: "s",
               scalerecursively: "1",
               maxscalerecursion: "-1",
               scaleseparator: " ",
               connectNullData: 1
       };
      
        return new sra.graphObjects.SpeedRunSummaryByMonthController(sra.ajaxHelper, _, container, { gameID: gameID, categoryType: categoryType, categoryID: categoryID, levelID: levelID }, new chartLoader(), chartConfig);
   }
};


