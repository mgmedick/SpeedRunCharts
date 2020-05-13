if (!sra)
    sra = {};

if (!sra['graphObjects'])
    sra.graphObjects = {};

sra.graphObjects.SpeedRunsByUserController = (function () {

    var mapToRequest = function (that, fromDate, toDate) {
        return {
            fromDate: fromDate,
            toDate: toDate
        };
    };

    var renderResults = function (that, data, promise) {
        var _data = that._.chain(data.data).clone().value();

        var allData = that._.chain(_data).groupBy('playerName').value();

        _data = _data.filter(function (x) { return !that._.isUndefined(x.NextSteps) && !that._.isNull(x.NextSteps) });
        that._.each(_data, function (x) { that._.extend(x, { NextStepsArray: x.NextSteps.split(', ') }) });
        that._.each(_data, function (x) { that._.extend(x, { IsActionPlan: x.NextStepsArray.indexOf('Action Plan') >= 0, IsRCA: x.NextStepsArray.indexOf('Root Cause') >= 0 }) });

        var actionPlanOpenData = that._.chain(_data).filter(function (x) { return x.IsActionPlan == true && (!x.ActionExists || !x.ActionIsComplete) }).groupBy('MonthPeriodDisplay').value();
        var actionPlanComplete = that._.chain(_data).filter(function (x) { return x.IsActionPlan == true && (x.ActionExists && x.ActionIsComplete) }).groupBy('MonthPeriodDisplay').value();
        var rcaOpenData = that._.chain(_data).filter(function (x) { return x.IsRCA == true && x.EventStatus !== 'Complete' }).groupBy('MonthPeriodDisplay').value();
        var rcaClosedData = that._.chain(_data).filter(function (x) { return x.IsRCA == true && x.EventStatus == 'Complete' }).groupBy('MonthPeriodDisplay').value();

        var categories = _.chain(data.TimePeriods)
            .map(function (item) {
                return item.MonthDisplay + ' ' + item.Year
            })
            .value();

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

        columnChart.addLineDataSet('Reported Events', that._.chain(Object.entries(allData)).map(function (x) { return { category: x[0], value: x[1].length } }).value(), 1);
        columnChart.addColumnDataSet('Action Plan Open', that._.chain(Object.entries(actionPlanOpenData)).map(function (x) { return { category: x[0], value: x[1].length } }).value());
        columnChart.addColumnDataSet('Action Plan Complete', that._.chain(Object.entries(actionPlanComplete)).map(function (x) { return { category: x[0], value: x[1].length } }).value());
        columnChart.addColumnDataSet('RCA Open', that._.chain(Object.entries(rcaOpenData)).map(function (x) { return { category: x[0], value: x[1].length } }).value());
        columnChart.addColumnDataSet('RCA Closed', that._.chain(Object.entries(rcaClosedData)).map(function (x) { return { category: x[0], value: x[1].length } }).value());

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

        var parameters = mapToRequest(that, that.inputs.fromDate, that.inputs.toDate);

        that.$ajax.getWithPromise(promise, '../EventReporting/GetForDateRange', parameters)
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