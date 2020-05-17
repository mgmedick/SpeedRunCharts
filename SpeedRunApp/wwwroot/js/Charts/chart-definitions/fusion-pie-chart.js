function fusionPieChart(elem, height, width, render3d) {
    this._chartContainer = elem; //element that chart will render in

    this._fusionChart = {};
    this._fusionChart.width = width;
    this._fusionChart.height = height;
    this._fusionChart.type = (render3d) ? 'pie3d' : 'pie2d';
    this._fusionChart.dataFormat = 'JSON';

    this._fusionChart.dataSource = {};
    this._fusionChart.dataSource.chart = {};
    this._fusionChart.dataSource.data = [];

    this._fusionChart.events = {};

    fusionPieChart.prototype.setCaption = function (caption, subCaption) {
        this._fusionChart.dataSource.chart.caption = caption;
        this._fusionChart.dataSource.chart.subCaption = subCaption;

        return this;
    };

    fusionPieChart.prototype.setChartOptions = function (showPercentValues, exportEnabled, showLegend, showLabels, theme, numberscalevalue, numberscaleunit, defaultnumberscale, scalerecursively, maxscalerecursion, scaleseparator) {
        this._fusionChart.dataSource.chart.showPercentValues = showPercentValues;
        this._fusionChart.dataSource.chart.showPercentInTooltip = showPercentValues == 0 ? 1 : 0;
        this._fusionChart.dataSource.chart.exportEnabled = exportEnabled;
        this._fusionChart.dataSource.chart.showLegend = showLegend;
        this._fusionChart.dataSource.chart.showLabels = showLabels;
        this._fusionChart.dataSource.chart.theme = theme;

        if (typeof numberscalevalue !== 'undefined')
            this._fusionChart.dataSource.chart.numberscalevalue = numberscalevalue;

        if (typeof numberscaleunit !== 'undefined')
            this._fusionChart.dataSource.chart.numberscaleunit = numberscaleunit;

        if (typeof defaultnumberscale !== 'undefined')
            this._fusionChart.dataSource.chart.defaultnumberscale = defaultnumberscale;

        if (typeof scalerecursively !== 'undefined')
            this._fusionChart.dataSource.chart.scalerecursively = scalerecursively;

        if (typeof maxscalerecursion !== 'undefined')
            this._fusionChart.dataSource.chart.maxscalerecursion = maxscalerecursion;

        if (typeof scaleseparator !== 'undefined')
            this._fusionChart.dataSource.chart.scaleseparator = scaleseparator;

        return this;
    };

    fusionPieChart.prototype.addData = function (label, value, sliceOut) {
        var dataSet = {
            label: label,
            value: value,
        };

        if (sliceOut)
            dataSet.isSliced = sliceOut;

        this._fusionChart.dataSource.data.push(dataSet);

        return this;
    };

    fusionPieChart.prototype.render = function (loader) {
        loader.loadChart(this._chartContainer, this._fusionChart);
    }

    fusionPieChart.prototype.onInitialized = function (func) {
        this._fusionChart.events.initialized = func;
    };

    fusionPieChart.prototype.onRenderComplete = function (func, includeNoDataToDisplay) {
        this._fusionChart.events.renderComplete = func;

        if ((typeof includeNoDataToDisplay == 'undefined') || ((typeof includeNoDataToDisplay !== 'undefined') && (includeNoDataToDisplay)))
            this.onNoDataToDisplay(func);

        return this;
    };

    fusionPieChart.prototype.onNoDataToDisplay = function (func) {
        this._fusionChart.events.noDataToDisplay = func;

        return this;
    };

    //return FusionPieChart;
};