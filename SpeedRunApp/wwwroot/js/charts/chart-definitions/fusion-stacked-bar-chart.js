
//constructor
function fusionStackedBarChart(multiSeriesChart, theme) {
    this._multiSeriesChart = multiSeriesChart;
    this._multiSeriesChart.setChartType('stackedBar2D', theme);

    fusionStackedBarChart.prototype.setCaption = function (caption, subCaption) {
        this._multiSeriesChart.setCaption(caption, subCaption);

        return this;
    };

    fusionStackedBarChart.prototype.setChartOptions = function (showValues, exportEnabled, formatNumberScale, numberOfDecimals, forceDecimalDisplay, showSum, useRoundEdges, numberscalevalue, numberscaleunit, defaultnumberscale, scalerecursively, maxscalerecursion, scaleseparator, theme, bgColor, baseFontColor, outCnvBaseFontColor) {
        this._multiSeriesChart.setChartOptions(showValues, exportEnabled, formatNumberScale, numberOfDecimals, forceDecimalDisplay, numberscalevalue, numberscaleunit, defaultnumberscale, scalerecursively, maxscalerecursion, scaleseparator, null, null, theme, bgColor, baseFontColor, outCnvBaseFontColor);
        this._multiSeriesChart._fusionChart.dataSource.chart.showSum = (typeof showSum !== 'undefined') ? showSum : 0;
        this._multiSeriesChart._fusionChart.dataSource.chart.useroundedges = (typeof useRoundEdges !== 'undefined') ? useRoundEdges : 0;

        return this;
    };

    fusionStackedBarChart.prototype.setAxis = function (xAxis, yAxis, yAxisMinValue, yAxisMaxValue, makeXAxisVertical, makeXAxisSlanted) {
        this._multiSeriesChart._fusionChart.dataSource.chart.xAxisName = xAxis;
        this._multiSeriesChart._fusionChart.dataSource.chart.yAxisName = yAxis;
        this._multiSeriesChart._fusionChart.dataSource.chart.labelDisplay = ((makeXAxisVertical) || (makeXAxisSlanted)) ? 'ROTATE' : 'AUTO';
        this._multiSeriesChart._fusionChart.dataSource.chart.slantLabels = (makeXAxisSlanted) ? 1 : 0;
        this._multiSeriesChart._fusionChart.dataSource.chart.yAxisMaxValue = (typeof yAxisMaxValue !== 'undefined') ? yAxisMaxValue : undefined;
        this._multiSeriesChart._fusionChart.dataSource.chart.yAxisMinValue = (typeof yAxisMinValue !== 'undefined') ? yAxisMinValue : undefined;

        return this;
    };

    fusionStackedBarChart.prototype.setCategories = function (arrayOfStrings) {
        this._multiSeriesChart.setCategories(arrayOfStrings);

        return this;
    };

    fusionStackedBarChart.prototype.addColumnDataSet = function (seriesName, categoryAndValueArray, color) { //supply categoryName and Value, so the value can be placed in the right order
        var data = _.map(this._multiSeriesChart._fusionChart.dataSource.categories[0].category, function (category) {
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
            renderAs: "column",
        };

        if (typeof color !== 'undefined')
            dataSet.color = color;

        this._multiSeriesChart._fusionChart.dataSource.dataset.push(dataSet);

        return this;
    };

    fusionStackedBarChart.prototype.addPercentageFormat = function () {
        this._multiSeriesChart.addPercentageFormat();

        return this;
    }

    fusionStackedBarChart.prototype.onInitialized = function (func) {

        this._multiSeriesChart.onInitialized(func);

        return this;
    };

    fusionStackedBarChart.prototype.onRenderComplete = function (func) {
        this._multiSeriesChart.onRenderComplete(func);

        return this;
    };

    fusionStackedBarChart.prototype.render = function (loader) {
        this._multiSeriesChart.render(loader);
    }
}

