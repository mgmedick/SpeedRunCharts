﻿if (!speedRun)
speedRun = {};

if (!speedRun['graphObjects'])
speedRun.graphObjects = {};

speedRun.graphObjects.SpeedRunSummaryByMonthController = (function () {
    var mapToRequest = function (that, gameID, categoryID, startDate, endDate) {
       return {
           gameID: gameID,
           categoryID: categoryID,
           startDate: startDate,
           endDate: endDate
       };
   };

   var renderResults = function (that, data, promise) {
       var _data = that._.chain(data).clone().value();

       var categoryData = {};
       that._.chain(_data).each(function (item) {
           var category = item.categoryName;
           var date = item.dateSubmittedString;

           categoryData[category] = categoryData[category] || {};
           categoryData[category][date] = categoryData[category][date] || [];
           categoryData[category][date].push(item);
       });

       var categories = [];
       that._.chain(_data).each(function (item) {
           if (categories.indexOf(item.dateSubmittedString) == -1) {
               categories.push(item.dateSubmittedString);
           }
       });

       categories = that._.sortBy(categories, function (item) { return moment(item) });

       var chartElem = that.container.find(that.chartConfig.selector);
       var config = that.chartConfig;

       var lineChart = new fusionMultiSeriesLineChart(new fusionMultiSeriesChart(chartElem, chartElem.height(), chartElem.width()), '');

       lineChart.setCaption(config.caption, config.subCaption)
                  .setAxis(config.xAxis, config.yAxis, true)
           .setChartOptions(config.showValues, config.exportEnabled, config.formatNumberScale, config.numberOfDecimals, undefined)
                  .setCategories(categories)
                  .onRenderComplete(function (evt, d) {
                        promise.resolve();
                   });


       for (var key in categoryData) {
           if (categoryData.hasOwnProperty(key)) {
               lineChart.addDataSet(key, that._.chain(Object.entries(categoryData[key])).map(function (x) {
                   return {
                       category: x[0],
                       value: Math.min.apply(Math, that._.chain(x[1]).map(function (i) { return i.primaryRunTimeMinutes }).value())
                   }
               }).value());
           }
       }

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

   SpeedRunSummaryByMonthController.prototype.postRender = function (promise) {
       var that = this;

       that.renderResults(that, that.viewModel, promise).then(function() {
           promise.resolve();    
       });

       return promise.promise();
   };

   return SpeedRunSummaryByMonthController;
}());