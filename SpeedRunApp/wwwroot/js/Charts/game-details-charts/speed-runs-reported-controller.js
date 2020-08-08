
function speedRunsReportedController(container, inputs, chartData, chartConfig) {
    this.container = container;
    this.inputs = inputs;
    this.chartData = chartData;
    this.chartConfig = chartConfig;

    speedRunsReportedController.prototype.preRender = function () {
        var def = $.Deferred();
        var that = this;
        var data = $(that.chartData).sort(function (a, b) { return a.PrimaryRunTimeMilliseconds - b.PrimaryRunTimeMilliseconds; }).toArray();

        def.resolve(data);
        return def.promise();
    };

    speedRunsReportedController.prototype.postRender = function (data) {
        var def = $.Deferred();
        var that = this;

        that.renderResults(that, data).then(function () {
            def.resolve();
        });

        return def.promise();
    };

    speedRunsReportedController.prototype.renderResults = function (that, data) {
        var def = $.Deferred();
        var _data = _.chain(data).clone().value();

        var allSpeedRunTimes = _.chain(_data).map(function (item) {
            return item;
        }).sortBy(function (item) {
            return item.primaryRunTimeSeconds;
        }).value();

        var chartDataObj = {};
        var percIncrement = 5;
        var maxPerc = 35;
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

            var values = _.chain(allSpeedRunTimes).filter(function (x, i) { return (prevIndex == null || i > prevIndex) && i <= index }).value();
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

        for (var i = 0; i < maxNumCategories; i++) {
            var percNum = (i == 0) ? percIncrement : prevPercNum + (percIncrement * showEvery);
            var index = Math.floor((allSpeedRunTimes.length + 1) * (percNum / 100));
            index = ((index > 0) ? index - 1 : 0);

            if (index >= allSpeedRunTimes.length - 1 || percNum > maxPerc || i == (maxNumCategories - 1)) {
                percNum = 100;
                index = allSpeedRunTimes.length - 1;
            }

            var values = _.chain(allSpeedRunTimes).filter(function (x, i) { return i <= index }).value();
            var time = allSpeedRunTimes[index].primaryRunTimeSeconds;
            var key = '<= ' + sra.dateHelper.formatTime("seconds", time, "hh[h] mm[m] ss[s]") + " (" + percNum + "% - " + values.length + "/" + allSpeedRunTimes.length + ")"

            if (index != prevIndex) {
                chartDataObj[key] = values;
            }

            prevPercNum = percNum;
            prevIndex = index;
            prevTime = time;
        }

        /*
        var prevTotal = 0;
        for (var i = 0; i < maxNumCategories; i++) {
            var percNum = (i == 0) ? percIncrement : prevPercNum + (percIncrement * showEvery);
            var index = Math.floor((allSpeedRunTimes.length + 1) * (percNum / 100));
            index = ((index > 0) ? index - 1 : 0);// + ((prevIndex > 0) ? prevIndex - 1 : 0)
            //var sum = _.chain(Object.entries(chartDataObj)).last().map(function (m, x) { return x[1].length; }, 0).value();

            var time;
            var key;
            var values = _.chain(allSpeedRunTimes).filter(function (x, i) { return i <= index }).value();

            if (index >= allSpeedRunTimes.length - 1 || percNum > maxPerc || i == (maxNumCategories - 1)) {
                values = _.chain(allSpeedRunTimes).filter(function (x, i) { return i > prevTotal }).value();
                key = '> ' + sra.dateHelper.formatTime("seconds", prevTime, "hh[h] mm[m] ss[s]") + " (Last " + (100 - maxPerc) + "% - " + values.length + "/" + allSpeedRunTimes.length + ")";
                chartDataObj[key] = values;
                break;
            } else {
                time = allSpeedRunTimes[index].primaryRunTimeSeconds;
                key = '<= ' + sra.dateHelper.formatTime("seconds", time, "hh[h] mm[m] ss[s]") + " (Top " + percNum + "% - " + values.length + "/" + allSpeedRunTimes.length + ")";

                if (index != prevIndex) {
                    chartDataObj[key] = values;
                }
            }

            prevTotal = values.length - 1;
            prevPercNum = percNum;
            prevIndex = index;
            prevTime = time;
        }
        */

        var chartElem = $(that.container);
        var config = that.chartConfig;
        var pieChart = new fusionPieChart(chartElem, chartElem.height(), chartElem.width(), true);
        var subCaption = that.chartConfig.subCaption;

        _.chain(Object.keys(that.inputs)).each(function (x) { subCaption = subCaption.replace('{{' + x + '}}', that.inputs[x]) }).value();

        pieChart.setCaption(that.chartConfig.caption, subCaption)
            .setChartOptions(config.showPercentValues, config.exportEnabled, config.showLegend, config.showLabels, config.theme, config.numberscalevalue, config.numberscaleunit, config.defaultnumberscale, config.scalerecursively, config.maxscalerecursion, config.scaleseparator, config.numberOfDecimals, config.showPercentInTooltip, config.formatNumberScale)
            .onRenderComplete(function (evt, d) {
                def.resolve();
            });

        _.chain(Object.entries(chartDataObj))
            .map(function (x) {
                return { label: x[0], value: x[1].length }
            })
            .each(function (item, idx) {
                pieChart.addData(item.label, item.value, idx == 0);
            })
            .value();

        pieChart.render();

        return def.promise();
    };
};





