//if (!sra)
//    sra = {};

//if (!sra['graphObjects'])
//    sra.graphObjects = {};

//sra.graphObjects['SpeedRunSummaryByMonth'] =
function speedRunSummaryByMonthChart() {
    //this.container = element;
    //this.templatePath = 'SpeedRunSummaryByMonth.html';
    //this.title = 'Speed Runs Per Month';

    this.chartConfig = {
        //selector: '.chart-container-0',
        //container: element,
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

    speedRunSummaryByMonthChart.prototype.controller = function (container, chartData) {
       var _chartLoader = new chartLoader();
       //var _chartConfig = {
       //        selector: '.chart-container-0',
       //        caption: 'Speed Runs Per Month',
       //        subCaption: 'Most recent 6 Months of activity',
       //        xAxis: 'Date',
       //        yAxis: 'Time (Minutes)',
       //        exportEnabled: 1,
       //        showValues: 0,
       //        formatNumberScale: 1,
       //        numberOfDecimals: 0,
       //        useRoundEdges: 1,
       //        numberscalevalue: "60,60",
       //        numberscaleunit: "m,h",
       //        defaultnumberscale: "s",
       //        scalerecursively: "1",
       //        maxscalerecursion: "-1",
       //        scaleseparator: " ",
       //        connectNullData: 1
       //};
      
        return new speedRunSummaryByMonthController(container, {}, chartData, _chartLoader, this.chartConfig);
   }
};




