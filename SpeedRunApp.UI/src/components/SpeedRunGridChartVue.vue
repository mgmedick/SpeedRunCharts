<template>
    <div class="card">
        <div class="card-header" id="headingOne">
            <h5 class="mb-0">
                <div v-if="showCharts">
                    <a class="btn btn-link font-weight-bold" href="#/" @click="showCharts = !showCharts"><i class="fa fa-chevron-down"></i>&nbsp;&nbsp;Hide Charts</a>
                </div>
                <div v-else>
                    <a class="btn btn-link font-weight-bold" href="#/" @click="showCharts = !showCharts"><i class="fa fa-chevron-right"></i>&nbsp;&nbsp;Show Charts</a>               
                </div>
            </h5>
        </div>
        <div class="container row" :style="[ showCharts ? null : { display:'none' } ]">
            <div class="col-sm-4">
                <div id="divGameSpeedRunsByMonthChart"></div>
            </div>
            <div class="col-sm-4">
                <div id="divGameSpeedRunsPercentileChart"></div>
            </div>
            <div class="col-sm-4">
                <div id="divGameTopSpeedRunChart"></div>
            </div>
        </div>
    </div>
</template>
<script>
    import moment from 'moment';
    //import { chain, clone, map, value, join } from 'lodash';
    import { chain } from 'lodash';    
    import { getDateDiffList, formatTime } from '../js/common.js';

    export default {
        name: "SpeedRunGridChartsVue",
        props: {
            tabledata: Array
        },
        data() {
            return {
                showCharts: false
            }
        },
        created: function () {
            this.loadData();
        },
        methods: {
            loadData() {
                var that = this;
                FusionCharts.ready(function () {
                    var gameWorldRecordChart = new FusionCharts(that.getGameWorldRecordChart2());
                    var gameSpeedRunsPercentileChart = new FusionCharts(that.getGameSpeedRunsPercentileChart());
                    var gameTopSpeedRunChart = new FusionCharts(that.getGameTopSpeedRunChart());

                    gameWorldRecordChart.render();
                    gameSpeedRunsPercentileChart.render();
                    gameTopSpeedRunChart.render();
                });
            },               
            getGameWorldRecordChart2() {
                var that = this;
                var categories = [];
                var dataset = [];

                if (this.tabledata?.length > 0) {
                    var _data = chain(this.tabledata).clone().value();
                    _data = _data.sort((a, b) => { 
                        return a?.primaryTimeMilliseconds - b?.primaryTimeMilliseconds
                    });                    

                    var filteredData = [];
                    while (_data.length > 0) {
                        var item = _data[0];
                        filteredData.push(item);

                        _data = _data.filter(x => x.dateSubmitted < item.dateSubmitted)
                                     .sort((a, b) => { return a?.primaryTimeMilliseconds - b?.primaryTimeMilliseconds });
                    }

                    var dates = filteredData.map(item => { return new Date(item.dateSubmitted) });
                    var maxDate = moment(Math.max.apply(null, dates)).toDate();
                    var minDate = moment(Math.min.apply(null, dates)).toDate();

                    var _timePeriods = getDateDiffList("month", minDate, maxDate).map(x => { return moment(x).format("MM/YYYY") });

                    var groupedObj = {};
                    var chartDataObj = {};
                    var categoryObj = {};

                    filteredData.forEach(item => {
                        var monthYear = moment(item.dateSubmitted).format("MM/YYYY")

                        var playerNames = chain(item.players).map(function (user) { return user.name }).value().join("{br}");
                        groupedObj[monthYear] = groupedObj[monthYear] || [];
                        groupedObj[monthYear].push({ primaryTimeMilliseconds: item.primaryTimeMilliseconds, primaryTimeString: item.primaryTimeString, playerNames: playerNames });                  
                    });

                    if (Object.keys(groupedObj).length > 0) {
                        var minKey = 'Min Time';
                        chartDataObj[minKey] = {};
                        for (var key in groupedObj) {
                            chartDataObj[minKey][key] = chartDataObj[minKey][key] || [];

                            var minItem = groupedObj[key].sort((a, b) => { return a?.primaryTimeMilliseconds - b?.primaryTimeMilliseconds })[0];
                            chartDataObj[minKey][key] = { value: minItem.primaryTimeMilliseconds, tooltext: key + "{br}" + minItem.playerNames + "{br}" + minItem.primaryTimeString };
                        }

                        categoryObj["category"] = _timePeriods.map(item => {
                            var labelObj = {};
                            labelObj["label"] = item
                            return labelObj;
                        });
                        categories.push(categoryObj);

                        for (var key in chartDataObj) {
                            _timePeriods.forEach(timePeriod => {
                                if (!chartDataObj[key].hasOwnProperty(timePeriod)) {
                                    chartDataObj[key][timePeriod] = { value: null, tooltext: ' ' };
                                }
                            })
                        }
                    }

                    for (var key in chartDataObj) {
                        var data = chain(Object.entries(chartDataObj[key])).map(function (x) { return { category: x[0], value: x[1]?.value, tooltext: x[1]?.tooltext } }).value();
                        if (data.length > 0) {
                            data = data.sort((a, b) => {
                                var monthyeara = a.category.split("/");
                                var monthyearb = b.category.split("/");

                                return new Date(monthyeara[1], monthyeara[0] - 1) - new Date(monthyearb[1], monthyearb[0] - 1)
                            });

                            dataset.push({ seriesname: key, data: data });
                        }
                    }
                }

                const chartConfig = {
                    type: "msline",
                    renderAt: "divGameSpeedRunsByMonthChart",
                    width: "100%",
                    dataFormat: "json",
                    dataSource: {
                        chart: {
                            caption: 'World Records',
                            subCaption: 'Per Month',
                            subcaptionFontSize: 12,
                            xAxis: 'Date',
                            yAxis: 'Time (Minutes)',
                            labelDisplay: "rotate",
                            labelFontSize: 12,
                            exportEnabled: 0,
                            showValues: 0,
                            formatNumberScale: 1,
                            numberOfDecimals: 0,
                            useRoundEdges: 1,
                            numberscalevalue: "1000,60,60",
                            numberscaleunit: "s,m,h",
                            defaultnumberscale: "ms",
                            scalerecursively: "1",
                            maxscalerecursion: "-1",
                            scaleseparator: " ",
                            connectNullData: 1,
                            plotBinSize: 1,
                            setAdaptiveYMin: 1,
                            theme: "candy",
                            bgColor: "#303030",
                            baseFontColor: "#fff",
                            outCnvBaseFontColor: "#fff"
                        },
                        categories: categories,
                        dataset: dataset
                    }
                };

                return chartConfig;
            },                      
            getGameSpeedRunsByMonthChart() {
                var that = this;
                var categories = [];
                var dataset = [];

                if (this.tabledata?.length > 0) {
                    //var _data = _.chain(this.tabledata).clone().value();
                    var _data = chain(this.tabledata).clone().value();                    
                    var dates = _data.map(item => { return new Date(item.dateSubmitted) });
                    var maxDate = moment(Math.max.apply(null, dates)).toDate();
                    var minDate = moment(maxDate).add(-24, "months").toDate();
                    var minDataDate = moment(Math.min.apply(null, dates)).toDate();
                    minDate = minDataDate > minDate ? minDataDate : minDate;

                    var filteredData = _data.filter((x, i) => {
                        return new Date(x.dateSubmitted) >= minDate
                    }).sort((a, b) => { return new Date(a.dateSubmitted) - new Date(b.dateSubmitted) });

                    var _timePeriods = getDateDiffList("month", minDate, maxDate).map(x => { return moment(x).format("MM/YYYY") });

                    //var _timePeriods = _.chain(timePeriods).clone().value();
                    //var _timePeriods = chain(timePeriods).clone().value();
                    var groupedObj = {};
                    var chartDataObj = {};
                    var categoryObj = {};

                    filteredData.forEach(item => {
                        var monthYear = item.monthYearSubmitted;

                        groupedObj[monthYear] = groupedObj[monthYear] || [];
                        groupedObj[monthYear].push(item.primaryTimeMilliseconds);
                    });

                    if (Object.keys(groupedObj).length > 0) {
                        var minKey = 'Min Time';
                        chartDataObj[minKey] = {};
                        for (var key in groupedObj) {
                            chartDataObj[minKey][key] = chartDataObj[minKey][key] || [];

                            var min = Math.min.apply(null, groupedObj[key]);
                            chartDataObj[minKey][key] = min;
                        }

                        categoryObj["category"] = _timePeriods.map(item => {
                            var labelObj = {};
                            labelObj["label"] = item
                            return labelObj;
                        });
                        categories.push(categoryObj);

                        for (var key in chartDataObj) {
                            _timePeriods.forEach(timePeriod => {
                                if (!chartDataObj[key].hasOwnProperty(timePeriod)) {
                                    chartDataObj[key][timePeriod] = null;
                                }
                            })
                        }
                    }

                    for (var key in chartDataObj) {
                        //var data = _.chain(Object.entries(chartDataObj[key])).map(function (x) { return { category: x[0], value: x[1] } }).value();
                        var data = chain(Object.entries(chartDataObj[key])).map(function (x) { return { category: x[0], value: x[1] } }).value();                        
                        if (data.length > 0) {
                            data = data.sort((a, b) => {
                                var monthyeara = a.category.split("/");
                                var monthyearb = b.category.split("/");

                                return new Date(monthyeara[1], monthyeara[0] - 1) - new Date(monthyearb[1], monthyearb[0] - 1)
                            });
                            dataset.push({ seriesname: key, data: data });
                        }
                    }
                }

                const chartConfig = {
                    type: "msline",
                    renderAt: "divGameSpeedRunsByMonthChart",
                    width: "100%",
                    //height: "350",
                    dataFormat: "json",
                    dataSource: {
                        chart: {
                            caption: 'Fastest Per Month',
                            subCaption: 'Last 2 Years',
                            xAxis: 'Date',
                            yAxis: 'Time (Minutes)',
                            exportEnabled: 0,
                            showValues: 0,
                            formatNumberScale: 1,
                            numberOfDecimals: 0,
                            useRoundEdges: 1,
                            numberscalevalue: "1000,60,60",
                            numberscaleunit: "s,m,h",
                            defaultnumberscale: "ms",
                            scalerecursively: "1",
                            maxscalerecursion: "-1",
                            scaleseparator: " ",
                            connectNullData: 1,
                            setAdaptiveYMin: 1,
                            theme: "candy",
                            bgColor: "#303030",
                            baseFontColor: "#fff",
                            outCnvBaseFontColor: "#fff"
                        },
                        categories: categories,
                        dataset: dataset
                    }
                };

                return chartConfig;
            },
            getGameSpeedRunsPercentileChart() {
                var that = this;
                var dataset = [];

                if (this.tabledata?.length > 0) {
                    //var _data = _.chain(this.tabledata).clone().value();
                    var _data = chain(this.tabledata).clone().value();
                    var allSpeedRunTimes = _data.sort((a, b) => { return a?.PrimaryTimeMilliseconds - b?.PrimaryTimeMilliseconds; });

                    var chartDataObj = {};
                    var percIncrement = 5;
                    var maxPerc = 25;
                    var showEvery = 2;
                    var maxNumCategories = Math.round((100 / percIncrement) / showEvery) + 1;

                    var prevPercNum = null;
                    var prevIndex = null;
                    var prevTime = null;

                    var prevTotal = 0;
                    for (var i = 0; i < maxNumCategories; i++) {
                        var percNum = (i == 0) ? percIncrement : prevPercNum + (percIncrement * showEvery);
                        var index = Math.ceil((allSpeedRunTimes.length + 1) * (percNum / 100));
                        index = ((index > 0) ? index - 1 : 0);// + ((prevIndex > 0) ? prevIndex - 1 : 0)

                        var time;
                        var key;
                        var percent;
                        var values = allSpeedRunTimes.filter((x, i) => { return i <= index });

                        if (index >= allSpeedRunTimes.length - 1 || percNum > maxPerc || i == (maxNumCategories - 1)) {
                            values = allSpeedRunTimes.filter((x, i) => { return i >= prevTotal });
                            percent = Math.trunc((values.length / allSpeedRunTimes.length) * 100) || 0;
                            key = '> ' + formatTime("milliseconds", prevTime) + " (" + percent + "% - " + values.length + "/" + allSpeedRunTimes.length + ")";
                            chartDataObj[key] = values;
                            break;
                        } else {
                            time = allSpeedRunTimes[index].primaryTimeMilliseconds;
                            percent = Math.trunc((values.length / allSpeedRunTimes.length) * 100) || 0;
                            key = '<= ' + formatTime("milliseconds", time) + " (" + percent + "% - " + values.length + "/" + allSpeedRunTimes.length + ")";

                            if (index != prevIndex) {
                                chartDataObj[key] = values;
                            }
                        }

                        prevTotal = values.length - 1;
                        prevPercNum = percNum;
                        prevIndex = index;
                        prevTime = time;
                    }

                    dataset = Object.entries(chartDataObj)
                        .map(x => {
                            return { label: x[0], value: x[1].length }
                        });
                }

                const chartConfig = {
                    type: "pie3d",
                    renderAt: "divGameSpeedRunsPercentileChart",
                    width: "100%",
                    //height: "350",
                    dataFormat: "json",
                    dataSource: {
                        chart: {
                            caption: 'Time Percentiles',
                            subCaption: '',
                            showValues: 1,
                            legendItemFontSize: 12,
                            formatNumberScale: 0,
                            numberOfDecimals: 0,
                            showPercentValues: 0,
                            showPercentInTooltip: 0,
                            exportEnabled: 0,
                            showLegend: 1,
                            showLabels: 0,
                            theme: "candy",
                            bgColor: "#303030",
                            baseFontColor: "#fff",
                            outCnvBaseFontColor: "#fff"
                        },
                        data: dataset
                    }
                };

                return chartConfig;
            },
            getGameTopSpeedRunChart() {
                var that = this;
                var categories = [];
                var dataset = [];

                if (this.tabledata?.length > 0) {
                    //var _data = _.chain(this.tabledata).clone().value();
                    var _data = chain(this.tabledata).clone().value();
                    _data = _data.filter(x => x.rank);
                    var sortedData = _data.sort((a, b) => { return a?.primaryTimeMilliseconds - b?.primaryTimeMilliseconds; });
                    var data = sortedData.slice(0, 10);

                    var chartDataObj = {};
                    var categoryObj = {};
                    data.forEach(item => {
                        //var playerNames = _.chain(item.players).map(function (item) { return item.name }).value().join(",");
                        var playerNames = chain(item.players).map(function (item) { return item.name }).value().join("{br}");

                        //chartDataObj[playerNames] = chartDataObj[playerNames] || [];
                        chartDataObj[playerNames] = item.primaryTimeMilliseconds;
                    });

                    categoryObj["category"] = data.map(item => {
                        var labelObj = {};
                        labelObj["label"] = item.players?.map(item => {
                            return item.name;
                        }).join("{br}");
                        return labelObj;
                    });
                    categories.push(categoryObj);

                    var dataValues = data.map(item => {
                        return { value: item.primaryTimeMilliseconds};
                    });                    

                    dataset.push({ seriesname: '', data: dataValues });
                }

                const chartConfig = {
                    type: "stackedBar2D",
                    renderAt: "divGameTopSpeedRunChart",
                    width: "100%",
                    //height: "350",
                    dataFormat: "json",
                    dataSource: {
                        chart: {
                            caption: 'Top 10 Ranked',
                            subCaption: '',
                            xAxis: '',
                            yAxis: 'Time (Minutes)',
                            labelFontSize: 12,
                            labelVAlign: 'middle',
                            //plotSpacePercent: 10,
                            exportEnabled: 0,
                            showValues: 1,
                            formatNumberScale: 1,
                            numberOfDecimals: 0,
                            useRoundEdges: 1,
                            numberscalevalue: "1000,60,60",
                            numberscaleunit: "s,m,h",
                            defaultnumberscale: "ms",
                            scalerecursively: "1",
                            maxscalerecursion: "-1",
                            scaleseparator: "",
                            theme: "candy",
                            bgColor: "#303030",
                            baseFontColor: "#fff",
                            outCnvBaseFontColor: "#fff"
                        },
                        categories: categories,
                        dataset: dataset
                    }
                };

                return chartConfig;
            }
        }
    }
</script>
<style>
   .fusioncharts-container>svg>g:nth-of-type(2){
        display:none !important;
    }
</style>




