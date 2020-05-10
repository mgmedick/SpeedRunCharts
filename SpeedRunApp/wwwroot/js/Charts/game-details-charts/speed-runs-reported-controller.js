if (!sra)
    sra = {};

if (!sra['graphObjects'])
    sra.graphObjects = {};

sra.graphObjects.SpeedRunsReportedController = (function () {
   var mapToRequest = function (that, gameID, categoryID, startDate, endDate) {
       return {
           gameID: gameID,
           categoryID: categoryID,
           startDate: startDate,
           endDate: endDate
       };
   };

   var renderResults = function (that, data, promise) {
       var speedRunTimes = that._.chain(data).map(function (item) {
           return item.primaryRunTimeMinutes
       }).sortBy(function (item) {
           return item;
       }).value();

       var midIndex = Math.ceil(speedRunTimes.length / 2);
       var median = sra.mathHelper.getAverage(speedRunTimes);
       var firstHalfMedian = sra.mathHelper.getAverage(speedRunTimes.slice(0, midIndex));
       var secondHalfMedian = sra.mathHelper.getAverage(speedRunTimes.slice(midIndex, speedRunTimes.length));

       var firstQtrData = that._.chain(data).filter(function (item) { return item.primaryRunTimeMinutes <= firstHalfMedian }).value();
       var secondQtrData = that._.chain(data).filter(function (item) { return item.primaryRunTimeMinutes > firstHalfMedian && item.primaryRunTimeMinutes <= median }).value();
       var thirdQtrData = that._.chain(data).filter(function (item) { return item.primaryRunTimeMinutes > median && item.primaryRunTimeMinutes <= secondHalfMedian }).value();
       var fourthQtrData = that._.chain(data).filter(function (item) { return item.primaryRunTimeMinutes > secondHalfMedian }).value();

       var categoryData = {};
       categoryData['<= ' + firstHalfMedian + ' min'] = firstQtrData;
       categoryData['<= ' + median + ' min'] = secondQtrData;
       categoryData['<= ' + secondHalfMedian + ' min'] = thirdQtrData;
       categoryData['> ' + secondHalfMedian + ' min'] = fourthQtrData;

       var chartElem = that.container.find(that.chartConfig.selector);
       var config = that.chartConfig;

       var pieChart = new fusionPieChart(chartElem, chartElem.height(), chartElem.width(), true);
      
       var subCaption = that.chartConfig.subCaption;
       that._.chain(Object.keys(that.inputs)).each(function (x) { subCaption = subCaption.replace('{{' + x + '}}', that.inputs[x])}).value();

       pieChart.setCaption(that.chartConfig.caption, subCaption)
                           .setChartOptions(config.showPercentValues, config.exportEnabled, config.showLegend, config.showLabels, config.theme)
                           .onRenderComplete(function (evt, d) {
                               promise.resolve();
                           });

       that._.chain(Object.entries(categoryData))
           .map(function (x) { return { label: x[0], value: x[1].length } })
           .each(function (item, idx) {
               pieChart.addData(item.label, item.value, idx == 0);
           })
           .value();

       pieChart.render(that.chartLoader);

       return promise.promise();
   };

   //constructor
  function SpeedRunsReportedController(ajaxHelper, underscore, container, inputs, chartLoader, chartConfig) {
       this.container = container;
       this.inputs = inputs;
       this.$ajax = ajaxHelper;
       this.chartLoader = chartLoader;
       this.renderResults = renderResults;
       this._ = underscore;
       this.chartConfig = chartConfig;
   }

   SpeedRunsReportedController.prototype.preRender = function (promise) {
       var that = this;

       var parameters = mapToRequest(that, that.inputs.gameID, that.inputs.categoryID, that.inputs.startDate, that.inputs.endDate);

       that.$ajax.getWithPromise(promise, 'GetLeaderboardChartData', parameters)
                    .then(function (result) {
                        that.viewModel = result;

                        promise.resolve();
                    }, function () {
                        promise.reject();
                    });

       return promise.promise();
   };

   SpeedRunsReportedController.prototype.postRender = function (promise) {
       var that = this;

       that.renderResults(that, that.viewModel, promise).then(function () {
           promise.resolve();
       });

       return promise.promise();
   };

   return SpeedRunsReportedController;
}());


