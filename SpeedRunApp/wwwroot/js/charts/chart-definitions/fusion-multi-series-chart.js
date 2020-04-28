﻿var fusionMultiSeriesChart = (function () {
    //constructor
    function fusionMultiSeriesChart(elem, height, width) {
        this._chartContainer = elem; //element that chart will render in

        this._fusionChart = {};
        this._fusionChart.width = width;
        this._fusionChart.height = height;
        this._fusionChart.dataFormat = 'JSON';

        this._fusionChart.dataSource = {};
        this._fusionChart.dataSource.chart = {};
        this._fusionChart.dataSource.categories = [];
        this._fusionChart.dataSource.dataset = [];
        this._fusionChart.events = {};
    }

    fusionMultiSeriesChart.prototype.setChartType = function (chartType, theme) {
        this._fusionChart.type = chartType;

        if (typeof theme !== 'undefined')
            this._fusionChart.dataSource.chart.theme = theme;

        return this;
    };

    fusionMultiSeriesChart.prototype.setCaption = function (caption, subCaption) {
        this._fusionChart.dataSource.chart.caption = caption;
        this._fusionChart.dataSource.chart.subCaption = subCaption;


        return this;
    };

    fusionMultiSeriesChart.prototype.setChartOptions = function (showValues, exportEnabled, formatNumberScale, numberOfDecimals, forceDecimalDisplay) {
        this._fusionChart.dataSource.chart.showValues = showValues;
        this._fusionChart.dataSource.chart.exportEnabled = exportEnabled;
        this._fusionChart.dataSource.chart.formatNumberScale = formatNumberScale;

        if (typeof numberOfDecimals !== 'undefined')
            this._fusionChart.dataSource.chart.decimals = numberOfDecimals;

        if (typeof forceDecimalDisplay !== 'undefined')
            this._fusionChart.dataSource.chart.forceDecimals = forceDecimalDisplay;

        return this;
    };

    fusionMultiSeriesChart.prototype.setAxis = function (xAxis, yAxis) {
        this._fusionChart.dataSource.chart.xAxisName = xAxis;
        this._fusionChart.dataSource.chart.yAxisName = yAxis;

        return this;
    };

    fusionMultiSeriesChart.prototype.setCategories = function (arrayOfStrings) {

        var category = {
            category: _.map(arrayOfStrings, function (item) {
                return { "label": item };
            })
        };

        this._fusionChart.dataSource.categories = [category];

        return this;
    };

    fusionMultiSeriesChart.prototype.addDataSet = function (seriesName, categoryAndValueArray) { //supply categoryName and Value, so the value can be placed in the right order
        var data = _.map(this._fusionChart.dataSource.categories[0].category, function (category) {
            var summary = _.find(categoryAndValueArray, function (item) {
                return item.category == category.label;
            });

            if (typeof summary !== 'undefined')
                return {
                    value: summary.value,
                    toolText: summary.toolText,
                }
            else
                return { value: '' }
        });

        var dataSet = {
            seriesName: seriesName,
            data: data,
        };

        this._fusionChart.dataSource.dataset.push(dataSet);

        return this;
    };

    fusionMultiSeriesChart.prototype.addPercentageFormat = function () {
        this._fusionChart.dataSource.chart.numberSuffix = '%';

        return this;
    }

    fusionMultiSeriesChart.prototype.onInitialized = function (func) {
        this._fusionChart.events.initialized = func;
    };

    fusionMultiSeriesChart.prototype.onRenderComplete = function (func) {
        this._fusionChart.events.renderComplete = func;
    };

    fusionMultiSeriesChart.prototype.render = function (loader) {

        loader.LoadChart(this._chartContainer, this._fusionChart);
    }

    return fusionMultiSeriesChart;
}());
