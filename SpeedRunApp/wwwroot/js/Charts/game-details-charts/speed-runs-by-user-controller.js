
function speedRunsByUserController(container, inputs, chartData, chartConfig) {
    this.container = container;
    this.inputs = inputs;
    this.chartData = chartData,
    this.chartConfig = chartConfig;

    SpeedRunsByUserController.prototype.preRender = function () {
        var def = $.Deferred();
        var that = this;
        var sortedData = $(that.chartData).sort(function (a, b) { return a.PrimaryRunTimeMilliseconds - b.PrimaryRunTimeMilliseconds; }).toArray();
        var data = sortedData.slice(0, that.inputs.topAmount);

        def.resolve(data);
        return def.promise();
    };

    SpeedRunsByUserController.prototype.postRender = function (data) {
        var def = $.Deferred();
        var that = this;

        that.renderResults(that, data).then(function () {
            def.resolve();
        });

        return def.promise();
    };

    SpeedRunsByUserController.prototype.renderResults = function (that, data) {
        var def = $.Deferred();
        var _data = _.chain(data).clone().value();

        var chartDataObj = {};
        _.chain(_data).each(function (item) {
            var playerName = item.playerName;

            chartDataObj[playerName] = chartDataObj[playerName] || [];
            chartDataObj[playerName].push(item.primaryRunTimeSeconds);
        });

        var categories = _.chain(_data).map(function (item) { return item.playerName }).value();
        var chartElem = $(that.container).find(that.chartConfig.selector);
        var config = that.chartConfig;
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

        columnChart.render(that.chartLoader);

        return def.promise();
    };
};