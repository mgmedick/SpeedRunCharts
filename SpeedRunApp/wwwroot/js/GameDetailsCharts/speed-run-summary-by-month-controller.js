if (!speedRun)
speedRun = {};

if (!speedRun['graphObjects'])
speedRun.graphObjects = {};

speedRun.graphObjects.SpeedRunSummaryByMonthController = (function () {   

   var mapToRequest = function (that, fromDate, toDate) {
       return {
           fromDate: fromDate,
           toDate: toDate
       };
   };

   var renderResults = function (that, data, promise) {
       var _data = that._.chain(data.Data).clone().value();

       var userMonths = that._.chain(_data).groupBy(function (value) { return value.CreatedByUser + ' ' + value.MonthPeriodDisplay; }).map(function (item) { return [item[0].MonthPeriodDisplay, item[0].CreatedByUser] }).value()
       var newData = that._.chain(_data).filter(function (x) { return x.ClaimStatus == 'New' }).groupBy(function (value) { return value.MonthPeriodDisplay + ' - ' + value.CreatedByUser; }).value();
       var evaluatingdData = that._.chain(_data).filter(function (x) { return x.ClaimStatus == 'Evaluating' }).groupBy(function (value) { return value.MonthPeriodDisplay + ' - ' + value.CreatedByUser; }).value();
       var activeData = that._.chain(_data).filter(function (x) { return x.ClaimStatus == 'Active' }).groupBy(function (value) { return value.MonthPeriodDisplay + ' - ' + value.CreatedByUser; }).value();

       var userMonths = [];
       that._.chain(_data).groupBy(function (value) { return value.CreatedByUser + ' ' + value.MonthPeriodDisplay; }).each(function (item) { userMonths.push({ key: item[0].MonthPeriodDisplay, value: item[0].CreatedByUser }); })

       var categories = [];
       that._.chain(data.TimePeriods).each(function (item) {
           var monthYearKey = item.MonthDisplay + ' ' + item.Year;
           if (that._.where(userMonths, { key: monthYearKey }).length > 0) {
               that._.chain(userMonths).filter(function (item2) { return item2.key == monthYearKey }).each(function (item3) {
                   categories.push(monthYearKey + ' - ' + item3.value)
               })
           } else {
               categories.push(monthYearKey)
           }
       });

       var chartElem = that.container.find(that.chartConfig.selector);
       var config = that.chartConfig;

       var columnChart = new FusionStackedColumnLineChart(new FusionMultiSeriesChart(chartElem, chartElem.height(), chartElem.width()), 'vigicore');

       columnChart.setCaption(config.caption, config.subCaption)
                                 .setAxis(config.xAxis, config.yAxis, undefined, undefined, undefined, true)
                   .setChartOptions(config.showValues, config.exportEnabled, config.formatNumberScale, config.numberOfDecimals, undefined, undefined, config.useRoundEdges)
                                 .setCategories(categories)
                   .onRenderComplete(function (evt, d) {
                       promise.resolve();
                   });

       //columnChart.addLineDataSet('Reported Claims', that._.chain(Object.entries(allData)).map(function (x) { return { category: x[0], value: x[1].length } }).value(), 1);
       columnChart.addColumnDataSet('New', that._.chain(Object.entries(newData)).map(function (x) { return { category: x[0], value: x[1].length } }).value());
       columnChart.addColumnDataSet('Evaluating', that._.chain(Object.entries(evaluatingdData)).map(function (x) { return { category: x[0], value: x[1].length } }).value());
       columnChart.addColumnDataSet('Active', that._.chain(Object.entries(activeData)).map(function (x) { return { category: x[0], value: x[1].length } }).value());

       columnChart.render(that.chartLoader);

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

       var parameters = mapToRequest(that, that.inputs.fromDate, that.inputs.toDate);

       that.$ajax.getWithPromise(promise, '../ClaimReporting/GetForDateRange', parameters)
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