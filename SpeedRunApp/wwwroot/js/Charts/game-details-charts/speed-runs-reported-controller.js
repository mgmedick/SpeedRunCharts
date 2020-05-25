if (!sra)
    sra = {};

if (!sra['graphObjects'])
    sra.graphObjects = {};

sra.graphObjects.SpeedRunsReportedController = (function () {
    var mapToRequest = function (that, gameID, categoryType, categoryID, levelID) {
       return {
           gameID: gameID,
           categoryType: categoryType,
           categoryID: categoryID,
           levelID: levelID
       };
   };

    var renderResults = function (that, data, promise) {
        var _data = that._.chain(data.data).clone().value();

        var allSpeedRunTimes = that._.chain(_data).map(function (item) {
            return item;
        }).sortBy(function (item) {
            return item.primaryRunTimeSeconds;
        }).value();

        var chartDataObj = {};
        var percIncrement = 5;
        var maxPerc = 55;
        var showEvery = 2;
        var maxNumCategories = Math.round((100 / percIncrement) / showEvery) + 1;

        var prevPercNum = null;
        var prevIndex = null;
        var prevTime = null;

        /*
        for (var i = 0; i < maxNumCategories; i++) {
            var percNum = (i == 0) ? percIncrement : prevPercNum + (percIncrement * showEvery);
            var index = Math.floor((allSpeedRunTimes.length + 1) * (percNum / 100));
            index = ((index > 0) ? index - 1 : 0);

            if (index >= allSpeedRunTimes.length - 1 || percNum > maxPerc || i == (maxNumCategories - 1)) {
                percNum = 100;
                index = allSpeedRunTimes.length - 1;
            }

            var values = that._.chain(allSpeedRunTimes).filter(function (x, i) { return (prevIndex == null || i > prevIndex) && i <= index }).value();
            var time = allSpeedRunTimes[index].primaryRunTimeSeconds;
            var key = '<= ' + sra.dateHelper.formatTime("seconds", time, "hh[h] mm[m] ss[s]") + " (" + percNum + "%)";

            if (index != prevIndex) {
                chartDataObj[key] = values;
            }

            prevPercNum = percNum;
            prevIndex = index;
            prevTime = time;
        }
        */
        /*
        for (var i = 0; i < maxNumCategories; i++) {
            var percNum = (i == 0) ? percIncrement : prevPercNum + (percIncrement * showEvery);
            var index = Math.floor((allSpeedRunTimes.length + 1) * (percNum / 100));
            index = ((index > 0) ? index - 1 : 0);

            if (index >= allSpeedRunTimes.length - 1 || percNum > maxPerc || i == (maxNumCategories - 1)) {
                percNum = 100;
                index = allSpeedRunTimes.length - 1;
            }

            var values = that._.chain(allSpeedRunTimes).filter(function (x, i) { return i <= index }).value();
            var time = allSpeedRunTimes[index].primaryRunTimeSeconds;
            var key = '<= ' + sra.dateHelper.formatTime("seconds", time, "hh[h] mm[m] ss[s]") + " (" + percNum + "%)";

            if (index != prevIndex) {
                chartDataObj[key] = values;
            }

            prevPercNum = percNum;
            prevIndex = index;
            prevTime = time;
        }
        */

        var percTotal = 0;
        for (var i = 0; i < maxNumCategories; i++) {
            var percNum = (i == 0) ? percIncrement : prevPercNum + (percIncrement * showEvery);
            var index = Math.floor((allSpeedRunTimes.length + 1) * (percNum / 100));
            index = ((index > 0) ? index - 1 : 0);// + ((prevIndex > 0) ? prevIndex - 1 : 0)
            var sum = that._.chain(Object.entries(chartDataObj)).reduce(function (m, x) { return m + x[1].length; }, 0).value();

            var time;
            var key;
            var values = that._.chain(allSpeedRunTimes).filter(function (x, i) { return i <= index }).value();

            if ((sum + (values.length - 1)) >= allSpeedRunTimes.length - 1 || index >= allSpeedRunTimes.length - 1 || percNum > maxPerc || i == (maxNumCategories - 1)) {
                key = '> ' + sra.dateHelper.formatTime("seconds", prevTime, "hh[h] mm[m] ss[s]") + " (" + (100 - percTotal) + "%)";
                values = that._.chain(allSpeedRunTimes).filter(function (x, i) { return i > sum - 1 }).value();
                chartDataObj[key] = values;
                break;
            } else {
                time = allSpeedRunTimes[index].primaryRunTimeSeconds;
                key = '<= ' + sra.dateHelper.formatTime("seconds", time, "hh[h] mm[m] ss[s]") + " (" + percNum + "%)";

                if (index != prevIndex) {
                    chartDataObj[key] = values;
                }
            }

            percTotal += percNum;
            prevPercNum = percNum;
            prevIndex = index;
            prevTime = time;
        }

        var chartElem = that.container.find(that.chartConfig.selector);
        var config = that.chartConfig;

        var pieChart = new fusionPieChart(chartElem, chartElem.height(), chartElem.width(), true);
      
        var subCaption = that.chartConfig.subCaption;
        that._.chain(Object.keys(that.inputs)).each(function (x) { subCaption = subCaption.replace('{{' + x + '}}', that.inputs[x])}).value();

        pieChart.setCaption(that.chartConfig.caption, subCaption)
            .setChartOptions(config.showPercentValues, config.exportEnabled, config.showLegend, config.showLabels, config.theme, config.numberscalevalue, config.numberscaleunit, config.defaultnumberscale, config.scalerecursively, config.maxscalerecursion, config.scaleseparator, config.numberOfDecimals, config.showPercentInTooltip, config.formatNumberScale)
                            .onRenderComplete(function (evt, d) {
                                promise.resolve();
                            });

        that._.chain(Object.entries(chartDataObj))
            .map(function (x) {
                return { label: x[0], value: x[1].length }
            })
            .each(function (item, idx) {
                pieChart.addData(item.label, item.value, idx == 0);
            })
            .value();

        pieChart.render(that.chartLoader);

        return promise.promise();
   };

   //constructor
  function SpeedRunsReportedController(ajaxHelper, underscore, container, inputs, chartLoader, chartConfig) {
       this.container = container;
       this.inputs = inputs;
       this.$ajax = ajaxHelper;
       this.chartLoader = chartLoader;
       this.renderResults = renderResults;
       this._ = underscore;
       this.chartConfig = chartConfig;
   }

   SpeedRunsReportedController.prototype.preRender = function (promise) {
       var that = this;

       var parameters = mapToRequest(that, that.inputs.gameID, that.inputs.categoryType, that.inputs.categoryID, that.inputs.levelID);

       that.$ajax.getWithPromise(promise, 'GetSpeedRunsReportedChartData', parameters)
                    .then(function (result) {
                        that.viewModel = result;

                        promise.resolve();
                    }, function () {
                        promise.reject();
                    });

       return promise.promise();
   };

   SpeedRunsReportedController.prototype.postRender = function (promise) {
       var that = this;

       that.renderResults(that, that.viewModel, promise).then(function () {
           promise.resolve();
       });

       return promise.promise();
   };

   return SpeedRunsReportedController;
}());


