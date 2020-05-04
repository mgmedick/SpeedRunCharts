var chartLoader = (function () {
    function chartLoader() { };
    
    chartLoader.prototype.LoadChart = function (elem, chartObj) {
        if (typeof chartObj == 'undefined') return;
        if (typeof elem == 'undefined') return;

        //elem.parent().empty();
        elem.insertFusionCharts(chartObj);
        return;
    };
 
    return chartLoader;
 }());




