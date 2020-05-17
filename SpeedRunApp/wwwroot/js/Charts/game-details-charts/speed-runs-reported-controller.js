if (!sra)
    sra = {};

if (!sra['graphObjects'])
    sra.graphObjects = {};

sra.graphObjects.SpeedRunsReportedController = (function () {
   var mapToRequest = function (that, gameID, categoryID) {
       return {
           gameID: gameID,
           categoryID: categoryID
       };
   };

    var renderResults = function (that, data, promise) {
        var _data = that._.chain(data.data).clone().value();

        var allSpeedRunTimes = that._.chain(_data).map(function (item) {
            return item;
        }).sortBy(function (item) {
            return item;
        }).value();

        var chartDataObj = {};
        var numCategories = 4;
        var speedRunTimes = that._.chain(allSpeedRunTimes).clone().value();

        var i = 1;
        while (i < numCategories && speedRunTimes.length > 0) {
            var times = that._.chain(speedRunTimes).map(function (item) { return item.primaryRunTimeSeconds; }).value();
            var average = sra.mathHelper.getAverage(times);
            var speedRunTimes = that._.chain(speedRunTimes).filter(function (x) { return x.primaryRunTimeSeconds <= average }).value();
            var key = '<= ' + sra.dateHelper.formatTime("seconds", average, "mm:ss");

            chartDataObj[key] = chartDataObj[key] || {};
            chartDataObj[key] = { items: speedRunTimes, sort: numCategories - i };

            if (i == 1) {
                key = '> ' + sra.dateHelper.formatTime("seconds", average, "mm:ss");
                values = that._.chain(allSpeedRunTimes).filter(function (x) { return x.primaryRunTimeSeconds > average }).value();

                if (values.length > 0) {
                    chartDataObj[key] = chartDataObj[key] || {};
                    chartDataObj[key] = { items: values, sort: numCategories };
                }
            }

            i++;
        }
        /*
        for (var i = 1; i < numCategories; i++) {
            var average = sra.mathHelper.getAverage(speedRunTimes);
            var speedRunTimes = that._.chain(speedRunTimes).filter(function (x) { return x <= average }).value();
            var key = '<= ' + average + ' min';

            chartDataObj[key] = chartDataObj[key] || {};
            chartDataObj[key] = { items: speedRunTimes, sort: numCategories - i };

            if (i == 1) {
                key = '> ' + average + ' min';
                values = that._.chain(allSpeedRunTimes).filter(function (x) { return x > average }).value();

                chartDataObj[key] = chartDataObj[key] || {};
                chartDataObj[key] = { items: values, sort: numCategories };
            }
        }
        */

        var chartElem = that.container.find(that.chartConfig.selector);
        var config = that.chartConfig;

        var pieChart = new fusionPieChart(chartElem, chartElem.height(), chartElem.width(), true);
      
        var subCaption = that.chartConfig.subCaption;
        that._.chain(Object.keys(that.inputs)).each(function (x) { subCaption = subCaption.replace('{{' + x + '}}', that.inputs[x])}).value();

        pieChart.setCaption(that.chartConfig.caption, subCaption)
            .setChartOptions(config.showPercentValues, config.exportEnabled, config.showLegend, config.showLabels, config.theme, config.numberscalevalue, config.numberscaleunit, config.defaultnumberscale, config.scalerecursively, config.maxscalerecursion, config.scaleseparator)
                            .onRenderComplete(function (evt, d) {
                                promise.resolve();
                            });

        that._.chain(Object.entries(chartDataObj))
            .map(function (x) {
                return { label: x[0], value: x[1].items.length, sort: x[1].sort }
            })
            .sortBy('sort')
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

       var parameters = mapToRequest(that, that.inputs.gameID, that.inputs.categoryID);

       that.$ajax.getWithPromise(promise, 'GetSpeedRunsReportedChartData', parameters)
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


