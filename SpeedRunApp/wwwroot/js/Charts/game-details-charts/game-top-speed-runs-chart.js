﻿
function gameTopSpeedRunsChart(container, inputs) {
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

    gameTopSpeedRunsChart.prototype.generateChart = function () {
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

    gameTopSpeedRunsChart.prototype.preRender = function () {
        var def = $.Deferred();
        var sortedData = $(this.inputs.chartData).sort(function (a, b) { return a.PrimaryTimeMilliseconds - b.PrimaryTimeMilliseconds; }).toArray();
        var data = sortedData.slice(0, this.inputs.topAmount);

        def.resolve(data);
        return def.promise();
    };

    gameTopSpeedRunsChart.prototype.postRender = function (data) {
        var def = $.Deferred();

        this.renderResults(data).then(function () {
            def.resolve();
        });

        return def.promise();
    };

    gameTopSpeedRunsChart.prototype.renderResults = function (data) {
        var def = $.Deferred();
        var _data = _.chain(data).clone().value();

        var chartDataObj = {};
        _.chain(_data).each(function (item) {
            var playerNames = _.chain(item.playerUsers).map(function (item) { return item.name }).value().join(",");

            chartDataObj[playerNames] = chartDataObj[playerNames] || [];
            chartDataObj[playerNames].push(item.primaryTimeSeconds);
        });

        var categories = _.chain(_data).map(function (item) { return _.chain(item.playerUsers).map(function (item) { return item.name }).value().join(",") }).value();
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

