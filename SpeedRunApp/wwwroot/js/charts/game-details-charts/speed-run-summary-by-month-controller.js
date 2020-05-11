if (!sra)
    sra = {};

if (!sra['graphObjects'])
    sra.graphObjects = {};

sra.graphObjects.SpeedRunSummaryByMonthController = (function () {
    var mapToRequest = function (that, gameID, categoryID, startDate, endDate) {
        return {
           gameID: gameID,
           categoryID: categoryID,
           startDate: startDate,
           endDate: endDate
       };
   };

   var renderResults = function (that, data, promise) {
       var _data = that._.chain(data.data).clone().value();
       var _timePeriods = that._.chain(data.timePeriods).clone().value();

       /*
       var groupedData = that._.chain(_data).groupBy('monthYearSubmitted');
       var chartData = {};
       that._.chain(groupedData).each(function (x) {
           var average = sra.mathHelper.getAverage(x[1]);
           chartData[x[0]] = average;
       });

       var categories = _timePeriods;
       */

       var groupedObj = {};
       that._.chain(_data).each(function (item) {
           var category = item.categoryName;
           var monthYear = item.monthYearSubmitted;

           groupedObj[category] = groupedObj[category] || {};
           groupedObj[category][monthYear] = groupedObj[category][monthYear] || [];
           groupedObj[category][monthYear].push(item.primaryRunTimeMinutes);
       });

       var chartDataObj = {};
       for (var key in groupedObj) {
           if (groupedObj.hasOwnProperty(key)) {
               chartDataObj[key] = chartDataObj[key] || {};
               for (var subkey in groupedObj[key]) {
                   chartDataObj[key][subkey] = chartDataObj[key][subkey] || [];
                   chartDataObj[key][subkey].push(sra.mathHelper.getAverage(groupedObj[key][subkey]));
               }

               //that._.chain(groupedObj[key]).each(function (x) {
               //    chartDataObj[key][x[0]] = chartDataObj[key][x[0]] || [];
               //    chartDataObj[key][x[0]].push(sra.mathHelper.getAverage(x[1]));
               //});
           }
       }

       var categories = _timePeriods;


       /*
       var categories = [];
       that._.chain(_data).each(function (item) {
           if (categories.indexOf(item.dateSubmittedString) == -1) {
               categories.push(item.dateSubmittedString);
           }
       });

       categories = that._.sortBy(categories, function (item) { return moment(item) });
       */

       var chartElem = that.container.find(that.chartConfig.selector);
       var config = that.chartConfig;

       var lineChart = new fusionMultiSeriesLineChart(new fusionMultiSeriesChart(chartElem, chartElem.height(), chartElem.width()), 'fusion');

       lineChart.setCaption(config.caption, config.subCaption)
           .setAxis(config.xAxis, config.yAxis, true)
           .setChartOptions(config.showValues, config.exportEnabled, config.formatNumberScale, config.numberOfDecimals, undefined)
           .setCategories(categories)
           .onRenderComplete(function (evt, d) {
               promise.resolve();
           });

       for (var key in chartDataObj) {
           if (chartDataObj.hasOwnProperty(key)) {
               lineChart.addDataSet(key, that._.chain(Object.entries(chartDataObj[key])).map(function (x) {
                   return {
                       category: x[0],
                       value: x[1]
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