<template>
    <div class="card">
        <div class="card-header" id="headingOne">
            <h5 class="mb-0">
                <div v-if="showcharts">
                    <a class="btn btn-link font-weight-bold d-flex align-items-end" style="line-height: 15px;" href="#/" @click="$emit('onshowchartsclick', $event)"><i class="fa fa-chevron-down align-self-center"></i><img src="/dist/fonts/bar-chart.svg" class="img-fluid brand-logo align-self-center mx-2" alt="Responsive image">Hide Charts</a>
                </div>
                <div v-else>
                    <a class="btn btn-link font-weight-bold d-flex align-items-end" style="line-height: 15px;" href="#/" @click="$emit('onshowchartsclick', $event)"><i class="fa fa-chevron-right align-self-center"></i><img src="/dist/fonts/bar-chart.svg" class="img-fluid brand-logo align-self-center mx-2" alt="Responsive image">Show Charts</a>
                </div>
            </h5>
        </div>
        <div class="container row chart-container" :style="[ showcharts ? null : { display:'none' } ]">
            <div class="col-sm-4">
                <div id="divChart1"></div>
            </div>
            <div class="col-sm-4">
                <div id="divChart2"></div>
            </div>
            <div class="col-sm-4">
                <div id="divChart3"></div>
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
        emits: ["onshowchartsclick"],
        props: {
            tabledata: Array,
            isgame: Boolean,
            showcharts: Boolean
        },
        computed: {
            isMediaMedium: function () {
                return window.innerWidth > 768;
            }
        },        
        created: function () {
            this.loadData();
        },
        methods: {
            loadData() {
                var that = this;
                FusionCharts.ready(function () {
                    if (that.isgame) {
                        var gameWorldRecordChart = new FusionCharts(that.getWorldRecordPerDayChart('divChart1'));
                        var gameSpeedRunsPercentileChart = new FusionCharts(that.getSpeedRunsPercentileChart('divChart2'));
                        var gameTopSpeedRunChart = new FusionCharts(that.getTopSpeedRunChart('divChart3', that.isgame));

                        gameWorldRecordChart.render();
                        gameSpeedRunsPercentileChart.render();
                        gameTopSpeedRunChart.render();
                    } else {
                        var userSpeedRunsByMonth = new FusionCharts(that.getPersonalPerDayChart('divChart1'));
                        var userSpeedRunsPercentileChart = new FusionCharts(that.getSpeedRunsPercentileChart('divChart2'));
                        var userTopSpeedRunChart = new FusionCharts(that.getTopSpeedRunChart('divChart3', that.isgame));

                        userSpeedRunsByMonth.render();
                        userSpeedRunsPercentileChart.render();
                        userTopSpeedRunChart.render();
                    }
                });
            },
            getWorldRecordPerDayChart(container) {
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
                    filteredData = filteredData.slice(0, 20);

                    var dates = filteredData.map(item => { return new Date(item.dateSubmitted) });
                    var maxDate = moment(Math.max.apply(null, dates)).startOf('day').toDate();//.add(1, "days").toDate();
                    var minDate = moment(Math.min.apply(null, dates)).startOf('day').toDate();
                    //var minDate = moment(maxDate).add(-24, "months");
                    //filteredData = filteredData.filter(x => { return new Date(x.dateSubmitted) >= minDate; });

                    var _timePeriods = getDateDiffList("day", minDate, maxDate).map(x => { return moment(x).format("MM/DD/YYYY") });

                    var groupedObj = {};
                    var chartDataObj = {};
                    var categoryObj = {};

                    filteredData.forEach(item => {
                        var monthDayYear = moment(item.dateSubmitted).format("MM/DD/YYYY")
                        var playerNames = chain(item.players).map(function (user) { return user.name }).value().join("{br}");
                        
                        groupedObj[monthDayYear] = groupedObj[monthDayYear] || [];
                        groupedObj[monthDayYear].push({ primaryTimeMilliseconds: item.primaryTimeMilliseconds, primaryTimeString: item.primaryTimeString, playerNames: playerNames });                        
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
                            labelObj["label"] = item;
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
                                var monthdayyeara = a.category.split("/");
                                var monthdayyearb = b.category.split("/");

                                return new Date(monthdayyeara[2], monthdayyeara[0] - 1, monthdayyeara[1]) - new Date(monthdayyearb[2], monthdayyearb[0] - 1, monthdayyearb[1])
                            });    

                            //data = data.map(x => { return x.tooltext ? x : { category: x.category, value: x.value, anchorAlpha: "0", showToolTip: "0" } });

                            dataset.push({ seriesname: key, data: data });
                        }
                    }
                }

                const chartConfig = {
                    type: "msline",
                    renderAt: container,
                    width: "100%",
                    dataFormat: "json",
                    dataSource: {
                        chart: {
                            caption: 'World Record Progression',
                            subCaption: 'Per Day (Last 20 World Records)',
                            subCaptionFontSize: 12,
                            xAxis: 'Date',
                            yAxis: 'Time (Minutes)',
                            // canvasPadding: 5, 
                            labelDisplay: "ROTATE",
                            labelFontSize: 11,
                            //labelDisplay: "NONE",
                            showLabels: 1,
                            rotateLabels: 1,
                            slantLabels: 1,
                            //labelStep: 5,
                            showToolTip: 1,
                            lineThickness: 2,
                            //anchorAlpha: "0",
                            anchorRadius: this.isMediaMedium ? 5: 8,
                            //plottooltext:null,
                            //plottooltext:"$dataValue",                                                        
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
                            plotBinSize: 8,
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
            getPersonalPerDayChart(container) {
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
                    //filteredData = filteredData.slice(0, 20);

                    var dates = filteredData.map(item => { return new Date(item.dateSubmitted) });
                    var maxDate = moment(Math.max.apply(null, dates)).startOf('day').toDate();//.add(1, "days").toDate();
                    var minDate = moment(Math.min.apply(null, dates)).startOf('day').toDate();
                    //var minDate = moment(maxDate).add(-24, "months");
                    //filteredData = filteredData.filter(x => { return new Date(x.dateSubmitted) >= minDate; });

                    var _timePeriods = getDateDiffList("day", minDate, maxDate).map(x => { return moment(x).format("MM/DD/YYYY") });

                    var groupedObj = {};
                    var chartDataObj = {};
                    var categoryObj = {};

                    filteredData.forEach(item => {
                        var monthDayYear = moment(item.dateSubmitted).format("MM/DD/YYYY")
                        var playerNames = chain(item.players).map(function (user) { return user.name }).value().join("{br}");
                        
                        groupedObj[monthDayYear] = groupedObj[monthDayYear] || [];
                        groupedObj[monthDayYear].push({ primaryTimeMilliseconds: item.primaryTimeMilliseconds, primaryTimeString: item.primaryTimeString, playerNames: playerNames });                        
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
                            labelObj["label"] = item;
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
                                var monthdayyeara = a.category.split("/");
                                var monthdayyearb = b.category.split("/");

                                return new Date(monthdayyeara[2], monthdayyeara[0] - 1, monthdayyeara[1]) - new Date(monthdayyearb[2], monthdayyearb[0] - 1, monthdayyearb[1])
                            });    

                            //data = data.map(x => { return x.tooltext ? x : { category: x.category, value: x.value, anchorAlpha: "0", showToolTip: "0" } });

                            dataset.push({ seriesname: key, data: data });
                        }
                    }
                }

                const chartConfig = {
                    type: "msline",
                    renderAt: container,
                    width: "100%",
                    dataFormat: "json",
                    dataSource: {
                        chart: {
                            caption: 'Personal Best Progression',
                            subCaption: 'Per Day',
                            subCaptionFontSize: 12,
                            xAxis: 'Date',
                            yAxis: 'Time (Minutes)',
                            // canvasPadding: 5, 
                            labelDisplay: "ROTATE",
                            labelFontSize: 11,
                            //labelDisplay: "NONE",
                            showLabels: 1,
                            rotateLabels: 1,
                            slantLabels: 1,
                            //labelStep: 5,
                            showToolTip: 1,
                            lineThickness: 2,
                            //anchorAlpha: "0",
                            anchorRadius: this.isMediaMedium ? 5 : 8,
                            //plottooltext:null,
                            //plottooltext:"$dataValue",                                                        
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
                            plotBinSize: 8,
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
            getSpeedRunsPercentileChart(container) {
                var dataset = [];

                if (this.tabledata?.length > 0) {
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
                    renderAt: container,
                    width: "100%",
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
            getTopSpeedRunChart(container, isGame) {
                var categories = [];
                var dataset = [];

                if (this.tabledata?.length > 0) {
                    var _data = chain(this.tabledata).clone().value();

                    if (isGame) {
                        _data = _data.filter(x => x.rank);
                    }

                    var sortedData = _data.sort((a, b) => { return a?.primaryTimeMilliseconds - b?.primaryTimeMilliseconds; });
                    var data = sortedData.slice(0, 10);

                    var chartDataObj = {};
                    var categoryObj = {};
                    data.forEach(item => {
                        var playerNames = chain(item.players).map(function (item) { return item.name }).value().join("{br}");

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
                        return { value: item.primaryTimeMilliseconds };
                    });

                    dataset.push({ seriesname: '', data: dataValues });
                }

                const chartConfig = {
                    type: "stackedBar2D",
                    renderAt: container,
                    width: "100%",
                    dataFormat: "json",
                    dataSource: {
                        chart: {
                            caption: isGame ? 'Top 10 Ranked' : 'Top 10',
                            subCaption: '',
                            xAxis: '',
                            yAxis: 'Time (Minutes)',
                            labelFontSize: 12,
                            labelVAlign: 'middle',
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
<style scoped>
   :deep(.fusioncharts-container>svg>g:nth-of-type(2)) {
        display:none !important;
    }
</style>




