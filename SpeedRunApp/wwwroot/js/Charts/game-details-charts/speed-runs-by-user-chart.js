
function speedRunsByUserChart(container, inputs) {
    this.container = container;
    this.inputs = inputs;

    this.chartConfig = {
        caption: 'Top 10 Speed Runs',
        subCaption: '',
        xAxis: '',
        yAxis: 'Time (Minutes)',
        exportEnabled: 0,
        showValues: 1,
        formatNumberScale: 1,
        numberOfDecimals: 0,
        useRoundEdges: 1,
        numberscalevalue: "60,60",
        numberscaleunit: "m,h",
        defaultnumberscale: "s",
        scalerecursively: "1",
        maxscalerecursion: "-1",
        scaleseparator: ""
    };

    speedRunsByUserChart.prototype.generateChart = function () {
        var def = $.Deferred();
        var that = this;

        that.preRender().then(function (data) {
            that.postRender(data).then(function () {
                def.resolve();
            });
        });

        return def.promise();
    }

    speedRunsByUserChart.prototype.preRender = function () {
        var def = $.Deferred();
        var sortedData = $(this.inputs.chartData).sort(function (a, b) { return a.PrimaryRunTimeMilliseconds - b.PrimaryRunTimeMilliseconds; }).toArray();
        var data = sortedData.slice(0, this.inputs.topAmount);

        def.resolve(data);
        return def.promise();
    };

    speedRunsByUserChart.prototype.postRender = function (data) {
        var def = $.Deferred();

        this.renderResults(data).then(function () {
            def.resolve();
        });

        return def.promise();
    };

    speedRunsByUserChart.prototype.renderResults = function (data) {
        var def = $.Deferred();
        var _data = _.chain(data).clone().value();

        var chartDataObj = {};
        _.chain(_data).each(function (item) {
            var playerName = item.playerName;

            chartDataObj[playerName] = chartDataObj[playerName] || [];
            chartDataObj[playerName].push(item.primaryRunTimeSeconds);
        });

        var categories = _.chain(_data).map(function (item) { return item.playerName }).value();
        var chartElem = $(this.container);
        var config = this.chartConfig;
        var columnChart = new fusionStackedBarChart(new fusionMultiSeriesChart(chartElem, chartElem.height(), chartElem.width()), 'fusion');

        columnChart.setCaption(config.caption, config.subCaption)
            .setAxis(config.xAxis, config.yAxis, undefined, undefined, undefined, true)
            .setChartOptions(config.showValues, config.exportEnabled, config.formatNumberScale, config.numberOfDecimals, undefined, undefined, config.useRoundEdges, config.numberscalevalue, config.numberscaleunit, config.defaultnumberscale, config.scalerecursively, config.maxscalerecursion, config.scaleseparator)
            .setCategories(categories)
            .onRenderComplete(function (evt, d) {
                def.resolve();
            });

        columnChart.addColumnDataSet('', _.chain(Object.entries(chartDataObj)).map(function (x) {
            return {
                category: x[0],
                value: x[1]
            }
        }).value());

        columnChart.render();

        return def.promise();
    };
};


