
function userSpeedRunsPercentileChart(container, inputs) {
    this.container = container;
    this.inputs = inputs;

    this.chartConfig = {
        caption: 'Speed Run Percentiles',
        subCaption: 'All Time',
        showValues: 1,
        formatNumberScale: 0,
        numberOfDecimals: 0,
        showPercentValues: 0,
        showPercentInTooltip: 0,
        exportEnabled: 1,
        showLegend: 1,
        showLabels: 0,
        theme: 'candy'
        //numberscalevalue: "60",
        //numberscaleunit: " mins",
        //defaultnumberscale: "",
        //scalerecursively: "1",
        //maxscalerecursion: "-1",
        //scaleseparator: " "
    };

    userSpeedRunsPercentileChart.prototype.generateChart = function () {
        var def = $.Deferred();
        var that = this;

        $(this.container).empty();

        this.preRender().then(function (data) {
            that.postRender(data).then(function () {
                def.resolve();
            });
        });

        return def.promise();
    }

    userSpeedRunsPercentileChart.prototype.preRender = function () {
        var def = $.Deferred();
        var data = this.inputs.chartData;

        def.resolve(data);
        return def.promise();
    };

    userSpeedRunsPercentileChart.prototype.postRender = function (data) {
        var def = $.Deferred();

        this.renderResults(data).then(function () {
            def.resolve();
        });

        return def.promise();
    };

    userSpeedRunsPercentileChart.prototype.renderResults = function (data) {
        var def = $.Deferred();
        var _data = _.chain(data).clone().value();

        var allSpeedRunTimes = _.chain(_data).map(function (item) {
            return item;
        }).sortBy("primaryTimeSeconds").value();

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
            var time = allSpeedRunTimes[index].primaryTimeSeconds;
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

            var values = _.chain(allSpeedRunTimes).filter(function (x, i) { return i <= index }).value();
            var time = allSpeedRunTimes[index].primaryTimeSeconds;
            var key = '<= ' + sra.dateHelper.formatTime("seconds", time, "hh[h] mm[m] ss[s]") + " (" + percNum + "% - " + values.length + "/" + allSpeedRunTimes.length + ")"

            if (index != prevIndex) {
                chartDataObj[key] = values;
            }

            prevPercNum = percNum;
            prevIndex = index;
            prevTime = time;
        }
        */

        var prevTotal = 0;
        for (var i = 0; i < maxNumCategories; i++) {
            var percNum = (i == 0) ? percIncrement : prevPercNum + (percIncrement * showEvery);
            var index = Math.floor((allSpeedRunTimes.length + 1) * (percNum / 100));
            index = ((index > 0) ? index - 1 : 0);// + ((prevIndex > 0) ? prevIndex - 1 : 0)
            //var sum = _.chain(Object.entries(chartDataObj)).last().map(function (m, x) { return x[1].length; }, 0).value();

            var time;
            var key;
            var percent;
            var values = _.chain(allSpeedRunTimes).filter(function (x, i) { return i <= index }).value();

            if (index >= allSpeedRunTimes.length - 1 || percNum > maxPerc || i == (maxNumCategories - 1)) {
                values = _.chain(allSpeedRunTimes).filter(function (x, i) { return i >= prevTotal }).value();
                percent = Math.round((values.length / allSpeedRunTimes.length) * 100);
                key = '> ' + sra.dateHelper.formatTime("seconds", prevTime, "hh[h] mm[m] ss[s]") + " (" + percent + "% - " + values.length + "/" + allSpeedRunTimes.length + ")";
                chartDataObj[key] = values;
                break;
            } else {
                time = allSpeedRunTimes[index].primaryTimeSeconds;
                percent = Math.round((values.length / allSpeedRunTimes.length) * 100);
                key = '<= ' + sra.dateHelper.formatTime("seconds", time, "hh[h] mm[m] ss[s]") + " (" + percent + "% - " + values.length + "/" + allSpeedRunTimes.length + ")";

                if (index != prevIndex) {
                    chartDataObj[key] = values;
                }
            }

            prevTotal = values.length - 1;
            prevPercNum = percNum;
            prevIndex = index;
            prevTime = time;
        }

        var chartElem = $(this.container);
        var config = this.chartConfig;
        var pieChart = new fusionPieChart(chartElem, chartElem.height(), chartElem.width(), true);
        var subCaption = this.chartConfig.subCaption;

        //_.chain(Object.keys(this.inputs)).each(function (x) { subCaption = subCaption.replace('{{' + x + '}}', this.inputs[x]) }).value();

        pieChart.setCaption(this.chartConfig.caption, subCaption)
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


