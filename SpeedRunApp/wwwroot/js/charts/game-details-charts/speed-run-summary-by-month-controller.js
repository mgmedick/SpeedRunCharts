
function speedRunSummaryByMonthController(container, inputs, chartData, chartConfig) {
    this.container = container;
    this.inputs = inputs;
    this.chartData = chartData;
    //this.chartLoader = chartLoader;
    //this.renderResults = renderResults;
    this.chartConfig = chartConfig;

    speedRunSummaryByMonthController.prototype.preRender = function () {
        var def = $.Deferred();
        var that = this;
        var data = {};

        var sortedData = $(that.chartData).sort(function (a, b) {
            return a.PrimaryRunTimeMilliseconds - b.PrimaryRunTimeMilliseconds;
        }).toArray();
        var dates = $(that.chartData).map(function () { return this.DateSubmitted }).toArray();
        var minDate = new Date(Math.min.apply(null, dates));
        var maxDate = new Date(Math.max.apply(null, dates));
        var timePeriods = sra.dateHelper.dateDiffList("day", minDate, maxDate);

        data["data"] = sortedData;
        data["timePeriods"] = timePeriods;

        def.resolve(data);
        return def.promise();
    };

    speedRunSummaryByMonthController.prototype.postRender = function (data) {
        var def = $.Deferred();
        var that = this;

        that.renderResults(that, data).then(function () {
            promise.resolve();
        });

        return def.promise();
    };

    speedRunSummaryByMonthController.prototype.renderResults = function (that, data) {
        var def = $.Deferred();
        var _data = _.chain(data.data).clone().value();
        var _timePeriods = _.chain(data.timePeriods).clone().value();

        /*
        var groupedData = _.chain(_data).groupBy('monthYearSubmitted');
        var chartData = {};
        _.chain(groupedData).each(function (x) {
            var average = sra.mathHelper.getAverage(x[1]);
            chartData[x[0]] = average;
        });
 
        var categories = _timePeriods;
        */

        var groupedObj = {};
        _.chain(_data).each(function (item) {
            var category = item.categoryName;
            var monthYear = item.monthYearSubmitted;

            groupedObj[category] = groupedObj[category] || {};
            groupedObj[category][monthYear] = groupedObj[category][monthYear] || [];
            groupedObj[category][monthYear].push(item.primaryRunTimeSeconds);
        });

        var chartDataObj = {};
        for (var key in groupedObj) {
            if (groupedObj.hasOwnProperty(key)) {
                var minKey = key + ' - Min Time';
                var maxKey = key + ' - Max Time';
                var averageKey = key + ' - Avg Time';

                chartDataObj[minKey] = chartDataObj[minKey] || {};
                chartDataObj[maxKey] = chartDataObj[maxKey] || {};
                chartDataObj[averageKey] = chartDataObj[averageKey] || {};
                for (var subkey in groupedObj[key]) {
                    chartDataObj[minKey][subkey] = chartDataObj[minKey][subkey] || [];
                    chartDataObj[maxKey][subkey] = chartDataObj[maxKey][subkey] || [];
                    chartDataObj[averageKey][subkey] = chartDataObj[averageKey][subkey] || [];

                    var min = sra.mathHelper.getMin(groupedObj[key][subkey]);
                    chartDataObj[minKey][subkey].push(min);

                    var max = sra.mathHelper.getMax(groupedObj[key][subkey]);
                    chartDataObj[maxKey][subkey].push(max);

                    var average = sra.mathHelper.getAverage(groupedObj[key][subkey]);
                    chartDataObj[averageKey][subkey].push(average);
                }

                //_.chain(groupedObj[key]).each(function (x) {
                //    chartDataObj[key][x[0]] = chartDataObj[key][x[0]] || [];
                //    chartDataObj[key][x[0]].push(sra.mathHelper.getAverage(x[1]));
                //});
            }
        }

        var categories = _timePeriods;


        /*
        var categories = [];
        _.chain(_data).each(function (item) {
            if (categories.indexOf(item.dateSubmittedString) == -1) {
                categories.push(item.dateSubmittedString);
            }
        });
 
        categories = _.sortBy(categories, function (item) { return moment(item) });
        */

        var chartElem = $(that.container); //$(that.container).find(that.chartConfig.selector);
        var config = that.chartConfig;

        var lineChart = new fusionMultiSeriesLineChart(new fusionMultiSeriesChart(chartElem, chartElem.height(), chartElem.width()), 'fusion');

        lineChart.setCaption(config.caption, config.subCaption)
            .setAxis(config.xAxis, config.yAxis, true)
            .setChartOptions(config.showValues, config.exportEnabled, config.formatNumberScale, config.numberOfDecimals, undefined, config.numberscalevalue, config.numberscaleunit, config.defaultnumberscale, config.scalerecursively, config.maxscalerecursion, config.scaleseparator, config.connectNullData)
            .setCategories(categories)
            .onRenderComplete(function (evt, d) {
                def.resolve();
            });

        for (var key in chartDataObj) {
            if (chartDataObj.hasOwnProperty(key)) {
                lineChart.addDataSet(key, _.chain(Object.entries(chartDataObj[key])).map(function (x) {
                    return {
                        category: x[0],
                        value: x[1]
                    }
                }).value());
            }
        }

        lineChart.render();

        return def.promise();
    };

    //return SpeedRunSummaryByMonthController;
}


