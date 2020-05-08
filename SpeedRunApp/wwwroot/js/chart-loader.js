function chartLoader() {
    chartLoader.prototype.loadChart = function (elem, chartObj) {
        if (typeof chartObj == 'undefined') return;
        if (typeof elem == 'undefined') return;

        $(elem).insertFusionCharts(chartObj);
        return;
    };
}









