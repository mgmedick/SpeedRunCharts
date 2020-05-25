function fusionMultiSeriesLineChart(multiSeriesChart, theme) {
    //constructor
    this._multiSeriesChart = multiSeriesChart;
    this._multiSeriesChart.setChartType('msline', theme);

    fusionMultiSeriesLineChart.prototype.setCaption = function (caption, subCaption) {
        this._multiSeriesChart.setCaption(caption, subCaption);

        return this;
    };

    fusionMultiSeriesLineChart.prototype.setChartOptions = function (showValues, exportEnabled, formatNumberScale, numberOfDecimals, forceDecimalDisplay, numberscalevalue, numberscaleunit, defaultnumberscale, scalerecursively, maxscalerecursion, scaleseparator, connectNullData) {
        this._multiSeriesChart.setChartOptions(showValues, exportEnabled, formatNumberScale, numberOfDecimals, forceDecimalDisplay, numberscalevalue, numberscaleunit, defaultnumberscale, scalerecursively, maxscalerecursion, scaleseparator, connectNullData);

        //if (typeof scalerecursively !== 'undefined')
        //    this._multiSeriesChart._fusionChart.dataSource.chart.scaleRecursively = scalerecursively;

        //if (typeof maxscalerecursion !== 'undefined')
        //    this._multiSeriesChart._fusionChart.dataSource.chart.maxScaleRecursion = maxscalerecursion;

        //if (typeof scaleseparator !== 'undefined')
        //    this._multiSeriesChart._fusionChart.dataSource.chart.scaleSeparator = scaleseparator;

        return this;
    };

    fusionMultiSeriesLineChart.prototype.setAxis = function (xAxis, yAxis, makeXAxisSlanted) {
        this._multiSeriesChart.setAxis(xAxis, yAxis, makeXAxisSlanted);
        //this._multiSeriesChart._fusionChart.dataSource.chart.xAxisLabelMode = 'AUTO';
        
        return this;
    };

    fusionMultiSeriesLineChart.prototype.setCategories = function (arrayOfStrings) {
        this._multiSeriesChart.setCategories(arrayOfStrings);

        return this;
    };

    fusionMultiSeriesLineChart.prototype.addDataSet = function (seriesName, categoryAndValueArray) { //supply categoryName and Value, so the value can be placed in the right order
        this._multiSeriesChart.addDataSet(seriesName, categoryAndValueArray);

        return this;
    };

    fusionMultiSeriesLineChart.prototype.addPercentageFormat = function () {
        this._multiSeriesChart.addPercentageFormat();

        return this;
    }

    fusionMultiSeriesLineChart.prototype.onInitialized = function (func) {
        this._multiSeriesChart.OnInitialized(func);
    };

    fusionMultiSeriesLineChart.prototype.onRenderComplete = function (func) {
        this._multiSeriesChart.onRenderComplete(func);
    };

    fusionMultiSeriesLineChart.prototype.render = function (loader) {
        this._multiSeriesChart.render(loader);
    }

    //return fusionMultiSeriesLineChart;
};





