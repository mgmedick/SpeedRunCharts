if (!speedRun)
speedRun = {};

if (!speedRun['graphObjects'])
speedRun.graphObjects = {};

speedRun.graphObjects.SpeedRunSummaryByMonthController = (function () {
    var mapToRequest = function (that, gameID, categoryIDs, top) {
       return {
           gameID: gameID,
           categoryIDs: categoryIDs,
           top: top
       };
   };

   var renderResults = function (that, data, promise) {
       var _data = that._.chain(data).clone().value();

       var allCategoryData = that._.chain(_data).groupBy('categoryName').value();
       var categories = that._.chain(_data).groupBy('dateSubmitted').map(function(item) { return item[0].dateSubmitted }).value();

       var chartElem = that.container.find(that.chartConfig.selector);
       var config = that.chartConfig;

       var lineChart = new fusionMultiSeriesLineChart(new fusionMultiSeriesChart(chartElem, chartElem.height(), chartElem.width()), '');

       lineChart.setCaption(config.caption, config.subCaption)
                  .setAxis(config.xAxis, config.yAxis, undefined, undefined, undefined, true)
                  .setChartOptions(config.showValues, config.exportEnabled, config.formatNumberScale, config.numberOfDecimals, undefined, undefined, config.useRoundEdges)
                  .setCategories(categories)
                  .onRenderComplete(function (evt, d) {
                        promise.resolve();
                   });

    //    lineChart.addDataSet("test", that._.chain(Object.entries(allCategoryData)).map(function (x) 
    //    { return { category: x[0], value: x[1].length } }).value(), 1);
                   
       that._.chain(allCategoryData).each(function(item) {
            lineChart.addDataSet(item[0].categoryName, that._.chain(Object.entries(item)).map(function (x) 
            { 
                return { category: x[1].dateSubmitted, value: x[1].primaryRunTimeString } 
            }).value(), 1);
       });

       lineChart.render(that.chartLoader);

       return promise.promise();
   };

   //constructor
   function SpeedRunSummaryByMonthController(ajaxHelper, underscore, container, inputs, chartLoader, chartConfig) {
       this.container = container;
       this.inputs = inputs;
       this.$ajax = ajaxHelper;
       this.chartLoader = chartLoader;
       this.renderResults = renderResults;
       this._ = underscore;
       this.chartConfig = chartConfig;
   }

   SpeedRunSummaryByMonthController.prototype.preRender = function (promise) {
       var that = this;

       var parameters = mapToRequest(that, that.inputs.gameID, that.inputs.categoryIDs, that.inputs.top);

       that.$ajax.getWithPromise(promise, 'GetLeaderboardChartData', parameters)
                    .then(function (result) {
                        that.viewModel = result;

                        promise.resolve();
                    }, function () {
                        promise.reject();
                    });

       return promise.promise();
   };

   SpeedRunSummaryByMonthController.prototype.postRender = function (promise) {
       var that = this;

       that.renderResults(that, that.viewModel, promise).then(function() {
           promise.resolve();    
       });

       return promise.promise();
   };

   return SpeedRunSummaryByMonthController;
}());