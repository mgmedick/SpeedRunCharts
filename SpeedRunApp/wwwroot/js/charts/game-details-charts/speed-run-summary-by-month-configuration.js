if (!sra)
    sra = {};

if (!sra['graphObjects'])
    sra.graphObjects = {};

sra.graphObjects['SpeedRunSummaryByMonth'] = {
   templatePath: 'SpeedRunSummaryByMonth.html',
    title: 'Speed Run Monthly Average',
    controller: function (container, dateHelper, gameID, categoryID) {
       var chartConfig = {
               selector: 'div#placeholder',
               caption: 'Speed Run Monthly Average',
               subCaption: 'Last 6 Months',
               xAxis: 'Date',
               yAxis: 'Time (Minutes)',
               exportEnabled: 1,
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
      
        return new sra.graphObjects.SpeedRunSummaryByMonthController(sra.ajaxHelper, _, container, { gameID: gameID, categoryID: categoryID, startDate: dateHelper.monthsAgo(6), endDate: dateHelper.today() }, new chartLoader(), chartConfig);
   }
};


