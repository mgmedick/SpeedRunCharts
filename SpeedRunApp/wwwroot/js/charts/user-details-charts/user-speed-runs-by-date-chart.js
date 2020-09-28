
function userSpeedRunsByDateChart(container, inputs) {
    this.container = container;
    this.inputs = inputs;

    this.chartConfig = {
        caption: 'Speed Runs By Date',
        subCaption: 'All Time',
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
        connectNullData: 1,
        setAdaptiveYMin: 1,
        theme: sra.userSettings.isDarkMode ? "candy" : "fusion"
    };

    userSpeedRunsByDateChart.prototype.generateChart = function () {
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

    userSpeedRunsByDateChart.prototype.preRender = function () {
        var def = $.Deferred();
        var data = {};
        //var data = this.inputs.chartData;
        var timePeriods = _.chain(this.inputs.chartData).sortBy("dateSubmitted").map(function (item) { return item.dateSubmittedString; }).uniq().value();

        //var dates = _.chain(this.inputs.chartData).map(function (item) { return new Date(item.dateSubmitted) }).value();
        //var maxDate = sra.dateHelper.maxDate(dates);
        //var minDate = sra.dateHelper.minDate(dates);
        //var timePeriods = _.chain(sra.dateHelper.dateDiffList("month", minDate, maxDate)).map(function (x) { return sra.dateHelper.format(x, "MM/YYYY") });

        data["data"] = this.inputs.chartData;
        data["timePeriods"] = timePeriods;

        def.resolve(data);
        return def.promise();
    };

    userSpeedRunsByDateChart.prototype.postRender = function (data) {
        var def = $.Deferred();

        this.renderResults(data).then(function () {
            def.resolve();
        });

        return def.promise();
    };

    userSpeedRunsByDateChart.prototype.renderResults = function (data) {
        var def = $.Deferred();
        var _data = _.chain(data.data).clone().value();
        var _timePeriods = _.chain(data.timePeriods).clone().value();

        var groupedObj = {};
        _.chain(_data).each(function (item) {
            //var category = item.categoryID;
            var dateString = item.dateSubmittedString;

            groupedObj[dateString] = groupedObj[dateString] || [];
            groupedObj[dateString].push(item.primaryTimeSeconds);
        });

        var chartDataObj = {};
        var minKey = 'Min Time';
        chartDataObj[minKey] = {};
        for (var key in groupedObj) {
            chartDataObj[minKey][key] = chartDataObj[minKey][key] || [];

            var min = sra.mathHelper.getMin(groupedObj[key]);
            chartDataObj[minKey][key].push(min);
        }

        /*
        var groupedObj = {};
        _.chain(_data).each(function (item) {
            var monthYear = item.monthYearSubmitted;

            groupedObj[monthYear] = groupedObj[monthYear] || [];
            groupedObj[monthYear].push(item.primaryTimeSeconds);
        });

        var chartDataObj = {};
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
        */

        var categories = _timePeriods;

        var chartElem = $(this.container);
        var config = this.chartConfig;

        var lineChart = new fusionMultiSeriesLineChart(new fusionMultiSeriesChart(chartElem, chartElem.height(), chartElem.width()), 'fusion');

        if (Object.keys(chartDataObj).length > 0) {
            lineChart.setCaption(config.caption, config.subCaption)
                .setAxis(config.xAxis, config.yAxis, true)
                .setChartOptions(config.showValues, config.exportEnabled, config.formatNumberScale, config.numberOfDecimals, undefined, config.numberscalevalue, config.numberscaleunit, config.defaultnumberscale, config.scalerecursively, config.maxscalerecursion, config.scaleseparator, config.connectNullData, config.setAdaptiveYMin, config.theme)
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
        }

        //lineChart.addDataSet('', _.chain(Object.entries(chartDataObj)).map(function (x) {
        //    return {
        //        category: x[0],
        //        value: x[1]
        //    }
        //}).value());

        lineChart.render();

        return def.promise();
    };
};




