
function speedRunSummaryByMonthChart(container, inputs) {
    this.container = container;
    this.inputs = inputs;

    this.chartConfig = {
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

    speedRunSummaryByMonthChart.prototype.generateChart = function () {
        var def = $.Deferred();
        var that = this;

        $(this.container).empty();

        this.preRender().then(function (data) {
            that.postRender(data).then(function () {
                def.resolve();
            });
        });

        return def.promise();
    }

    speedRunSummaryByMonthChart.prototype.preRender = function () {
        var def = $.Deferred();
        var data = {};

        var dates = _.chain(this.inputs.chartData).map(function (item) { return new Date(item.dateSubmitted) }).value();
        //var minDate = new Date(Math.min.apply(null, dates));
        var maxDate = sra.dateHelper.maxDate(dates);
        var minDate = sra.dateHelper.add(maxDate, -6, "months");
        var filteredData = _.chain(this.inputs.chartData).filter(function (x, i) { return new Date(x.dateSubmitted) >= minDate }).value();
        var timePeriods = _.chain(sra.dateHelper.dateDiffList("month", minDate, maxDate)).map(function (x) { return sra.dateHelper.format(x, "MM/YYYY") })

        data["data"] = filteredData;
        data["timePeriods"] = timePeriods;

        def.resolve(data);
        return def.promise();
    };

    speedRunSummaryByMonthChart.prototype.postRender = function (data) {
        var def = $.Deferred();

        this.renderResults(data).then(function () {
            promise.resolve();
        });

        return def.promise();
    };

    speedRunSummaryByMonthChart.prototype.renderResults = function (data) {
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
            //var category = item.categoryID;
            var monthYear = item.monthYearSubmitted;

            groupedObj[monthYear] = groupedObj[monthYear] || [];
            groupedObj[monthYear].push(item.primaryTimeSeconds);
        });

        var chartDataObj = {};
        //for (var key in groupedObj) {
        //    if (groupedObj.hasOwnProperty(key)) {
                var minKey = 'Min Time';
                var maxKey = 'Max Time';
                var averageKey = 'Avg Time';

                chartDataObj[minKey] = {};
                chartDataObj[maxKey] = {};
                chartDataObj[averageKey] = {};
                for (var key in groupedObj) {
                    chartDataObj[minKey][key] = chartDataObj[minKey][key] || [];
                    chartDataObj[maxKey][key] = chartDataObj[maxKey][key] || [];
                    chartDataObj[averageKey][key] = chartDataObj[averageKey][key] || [];

                    var min = sra.mathHelper.getMin(groupedObj[key]);
                    chartDataObj[minKey][key].push(min);

                    var max = sra.mathHelper.getMax(groupedObj[key]);
                    chartDataObj[maxKey][key].push(max);

                    var average = sra.mathHelper.getAverage(groupedObj[key]);
                    chartDataObj[averageKey][key].push(average);
                }

                //_.chain(groupedObj[key]).each(function (x) {
                //    chartDataObj[key][x[0]] = chartDataObj[key][x[0]] || [];
                //    chartDataObj[key][x[0]].push(sra.mathHelper.getAverage(x[1]));
                //});
        //    }
        //}

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

        var chartElem = $(this.container);
        var config = this.chartConfig;

        var lineChart = new fusionMultiSeriesLineChart(new fusionMultiSeriesChart(chartElem, chartElem.height(), chartElem.width()), 'fusion');

        lineChart.setCaption(config.caption, config.subCaption)
            .setAxis(config.xAxis, config.yAxis, true)
            .setChartOptions(config.showValues, config.exportEnabled, config.formatNumberScale, config.numberOfDecimals, undefined, config.numberscalevalue, config.numberscaleunit, config.defaultnumberscale, config.scalerecursively, config.maxscalerecursion, config.scaleseparator, config.connectNullData)
            .setCategories(categories)
            .onRenderComplete(function (evt, d) {
                def.resolve();
            });

        for (var key in chartDataObj) {
            lineChart.addDataSet(key, _.chain(Object.entries(chartDataObj[key])).map(function (x) {
                return {
                    category: x[0],
                    value: x[1]
                }
            }).value());
        }

        lineChart.render();

        return def.promise();
    };
};




