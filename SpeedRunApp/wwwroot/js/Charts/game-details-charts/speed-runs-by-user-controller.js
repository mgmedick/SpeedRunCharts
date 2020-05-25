if (!sra)
    sra = {};

if (!sra['graphObjects'])
    sra.graphObjects = {};

sra.graphObjects.SpeedRunsByUserController = (function () {
    var mapToRequest = function (that, gameID, categoryType, categoryID, levelID, topAmount) {
        return {
            gameID: gameID,
            categoryType: categoryType,
            categoryID: categoryID,
            levelID: levelID,
            topAmount: topAmount
        };
    };

    var renderResults = function (that, data, promise) {
        var _data = that._.chain(data.data).clone().value();

        var chartDataObj = {};
        that._.chain(_data).each(function (item) {
            var playerName = item.playerName;

            chartDataObj[playerName] = chartDataObj[playerName] || [];
            chartDataObj[playerName].push(item.primaryRunTimeSeconds);
        });

        var categories = _.chain(_data).map(function (item) { return item.playerName }).value();

        var chartElem = that.container.find(that.chartConfig.selector);
        var config = that.chartConfig;
        var columnChart = new fusionStackedBarChart(new fusionMultiSeriesChart(chartElem, chartElem.height(), chartElem.width()), 'fusion');

        columnChart.setCaption(config.caption, config.subCaption)
            .setAxis(config.xAxis, config.yAxis, undefined, undefined, undefined, true)
            .setChartOptions(config.showValues, config.exportEnabled, config.formatNumberScale, config.numberOfDecimals, undefined, undefined, config.useRoundEdges, config.numberscalevalue, config.numberscaleunit, config.defaultnumberscale, config.scalerecursively, config.maxscalerecursion, config.scaleseparator)
            .setCategories(categories)
            .onRenderComplete(function (evt, d) {
                promise.resolve();
            });

        columnChart.addColumnDataSet('', that._.chain(Object.entries(chartDataObj)).map(function (x) {
            return {
                category: x[0],
                value: x[1]
            }
        }).value());

        columnChart.render(that.chartLoader);

        return promise.promise();
    };

    //constructor
    function SpeedRunsByUserController(ajaxHelper, underscore, container, inputs, chartLoader, chartConfig) {
        this.container = container;
        this.inputs = inputs;
        this.$ajax = ajaxHelper;
        this.chartLoader = chartLoader;
        this.renderResults = renderResults;
        this._ = underscore;
        this.chartConfig = chartConfig;
    }

    SpeedRunsByUserController.prototype.preRender = function (promise) {
        var that = this;

        var parameters = mapToRequest(that, that.inputs.gameID, that.inputs.categoryType, that.inputs.categoryID, that.inputs.levelID, that.inputs.topAmount);

        that.$ajax.getWithPromise(promise, 'GetSpeedRunsByUserChartData', parameters)
            .then(function (result) {
                that.viewModel = result;

                promise.resolve();
            }, function () {
                promise.reject();
            });

        return promise.promise();
    };

    SpeedRunsByUserController.prototype.postRender = function (promise) {
        var that = this;

        that.renderResults(that, that.viewModel, promise).then(function () {
            promise.resolve();
        });

        return promise.promise();
    };

    return SpeedRunsByUserController;
}());