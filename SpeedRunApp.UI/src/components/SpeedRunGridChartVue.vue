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
            <div class="col-sm-4" style="min-height:300px;">
                <div v-if="loading" class="d-flex" style="height:100%;">
                    <div class="m-auto">
                        <i class="fas fa-spinner fa-spin fa-lg"></i>
                    </div>
                </div>
                <div id="divChart1" :style="[ loading ? { display:'none' } : null ]"></div>
            </div>
            <div class="col-sm-4" style="min-height:300px;">
                <div v-if="loading" class="d-flex" style="height:100%;">
                    <div class="m-auto">
                        <i class="fas fa-spinner fa-spin fa-lg"></i>
                    </div>
                </div>
                <div id="divChart2" :style="[ loading ? { display:'none' } : null ]"></div>
            </div>
            <div class="col-sm-4" style="min-height:300px;">
                <div v-if="loading" class="d-flex" style="height:100%;">
                    <div class="m-auto">
                        <i class="fas fa-spinner fa-spin fa-lg"></i>
                    </div>
                </div>              
                <div id="divChart3" :style="[ loading ? { display:'none' } : null ]"></div>
            </div>
        </div>
    </div>
</template>
<script>
    import axios from 'axios';
    const dayjs = require('dayjs');
    import { getDateDiffList, formatTime } from '../js/common.js';
    import FusionCharts from 'fusioncharts/core';
    //import Bar2D from 'fusioncharts/viz/bar2d';
    import StackedBar2D from 'fusioncharts/viz/stackedbar2d';
    import Pie2D from 'fusioncharts/viz/pie2d';
    import Line from 'fusioncharts/viz/line';    
    import CandyTheme from "fusioncharts/themes/es/fusioncharts.theme.candy";
    FusionCharts.addDep(StackedBar2D, Pie2D, Line, CandyTheme);
    
    export default {
        name: "SpeedRunGridChartsVue",
        emits: ["onshowchartsclick"],
        props: {
            gameid: String,
            categorytypeid: String,
            categoryid: String,
            levelid: String,
            variablevalues: String,            
            userid: String,            
            isgame: Boolean,
            showcharts: Boolean,
            title: String
        },
        data() {
            return {
                tabledata: [],
                loading: true
            }
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
                this.loading = true;

                axios.get('/SpeedRun/GetSpeedRunGridData', { params: { gameID: this.gameid, categoryID: this.categoryid, levelID: this.levelid, subCategoryVariableValueIDs: this.variablevalues, userID: this.userid, showAllData: true } })
                    .then(res => {
                        that.tabledata = res.data;                                             
                        that.loadCharts();  
                        that.loading = false;  
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            },            
            loadCharts() {
                var that = this;
                FusionCharts.ready(function () {
                    var gameWorldRecordChart = new FusionCharts(that.getWorldRecordPerDayChart('divChart1', that.isgame));
                    var gameSpeedRunsPercentileChart = new FusionCharts(that.getSpeedRunsPercentileChart('divChart2'));
                    var gameTopSpeedRunChart = new FusionCharts(that.getTopSpeedRunChart('divChart3', that.isgame));

                    gameWorldRecordChart.render();
                    gameSpeedRunsPercentileChart.render();
                    gameTopSpeedRunChart.render();                        
                });
            },
            getWorldRecordPerDayChart(container, isGame) {
                var that = this;
                var dataset = [];

                if (this.tabledata?.length > 0) {
                    var _data = JSON.parse(JSON.stringify(this.tabledata)); 
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

                    if (isGame) {
                        filteredData = filteredData.slice(0, 20);
                    }

                    var dates = filteredData.map(item => { return new Date(item.dateSubmitted) });
                    var maxDate = dayjs(Math.max.apply(null, dates)).startOf('day').toDate();//.add(1, "days").toDate();
                    var minDate = dayjs(Math.min.apply(null, dates)).startOf('day').toDate();
                    //var minDate = dayjs(maxDate).add(-24, "months");
                    //filteredData = filteredData.filter(x => { return new Date(x.dateSubmitted) >= minDate; });

                    var _timePeriods = getDateDiffList("day", minDate, maxDate).map(x => { return dayjs(x).format("MM/DD/YYYY") });

                    var groupedObj = {};
                    var chartDataObj = {};

                    filteredData.forEach(item => {
                        var monthDayYear = dayjs(item.dateSubmitted).format("MM/DD/YYYY")
                        var playerNames = item.players?.map(user => user.name).join("{br}");

                        groupedObj[monthDayYear] = groupedObj[monthDayYear] || [];
                        groupedObj[monthDayYear].push({ primaryTimeMilliseconds: item.primaryTimeMilliseconds, primaryTimeString: item.primaryTimeString, playerNames: playerNames });                        
                    });

                    if (Object.keys(groupedObj).length > 0) {
                        for (var key in groupedObj) {
                            chartDataObj[key] = chartDataObj[key] || [];

                            var minItem = groupedObj[key].sort((a, b) => { return a?.primaryTimeMilliseconds - b?.primaryTimeMilliseconds })[0];
                            chartDataObj[key] = { value: minItem.primaryTimeMilliseconds, tooltext: key + "{br}" + minItem.playerNames + "{br}" + minItem.primaryTimeString };                                                  
                        }

                        _timePeriods.forEach(timePeriod => {
                            if (!chartDataObj.hasOwnProperty(timePeriod)) {
                                chartDataObj[timePeriod] = { value: null, tooltext: ' ' };
                            }
                        });
                    }

                    dataset = Object.entries(chartDataObj)?.map(x => ({ label: x[0], value: x[1]?.value, tooltext: x[1]?.tooltext }));                        
                    
                    if (dataset.length > 0) {
                        dataset = dataset.sort((a, b) => {
                            var monthdayyeara = a.label.split("/");
                            var monthdayyearb = b.label.split("/");

                            return new Date(monthdayyeara[2], monthdayyeara[0] - 1, monthdayyeara[1]) - new Date(monthdayyearb[2], monthdayyearb[0] - 1, monthdayyearb[1])
                        });
                    }
                }

                const chartConfig = {
                    type: "line",
                    renderAt: container,
                    width: "100%",
                    dataFormat: "json",
                    dataSource: {
                        chart: {
                            caption: isGame ? 'World Records (Last 20)' : 'Personal Bests',
                            captionFontSize: 14,                           
                            subCaption: that.title,
                            subCaptionFontSize: 11,
                            xAxis: 'Date',
                            yAxis: 'Time (Minutes)',
                            canvasPadding: 5, 
                            labelDisplay: "ROTATE",
                            labelFontSize: 11,
                            showLabels: 1,
                            rotateLabels: 1,
                            slantLabels: 1,
                            showToolTip: 1,
                            lineThickness: 2,
                            anchorRadius: 5,
                            anchorBgColor: "#303030",
                            anchorBorderThickness: 1,                                                   
                            exportEnabled: 1,
                            showValues: 0,
                            formatNumberScale: 1,
                            numberOfDecimals: 0,
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
                        data: dataset
                    }
                };

                return chartConfig;
            },
            getSpeedRunsPercentileChart(container) {
                var that = this;
                var dataset = [];

                if (this.tabledata?.length > 0) {
                    var _data = JSON.parse(JSON.stringify(this.tabledata)); 
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
                            values = allSpeedRunTimes.length == 1 ? allSpeedRunTimes : allSpeedRunTimes.filter((x, i) => { return i > prevTotal });
                            percent = Math.trunc((values.length / allSpeedRunTimes.length) * 100) || 0;
                            key = '> ' + formatTime("millisecond", prevTime) + " (" + percent + "% - " + values.length + "/" + allSpeedRunTimes.length + ")";
                            chartDataObj[key] = values;
                            break;
                        } else {
                            time = allSpeedRunTimes[index].primaryTimeMilliseconds;
                            percent = Math.trunc((values.length / allSpeedRunTimes.length) * 100) || 0;
                            key = '<= ' + formatTime("millisecond", time) + " (" + percent + "% - " + values.length + "/" + allSpeedRunTimes.length + ")";

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
                    type: "pie2d",
                    renderAt: container,
                    width: "100%",
                    dataFormat: "json",
                    dataSource: {
                        chart: {
                            caption: 'Time Percentiles',
                            captionFontSize: 14,                            
                            subCaption: that.title,
                            subCaptionFontSize: 11,
                            showValues: 1,
                            legendItemFontSize: 12,
                            formatNumberScale: 0,
                            numberOfDecimals: 0,
                            showPercentValues: 0,
                            showPercentInTooltip: 0,
                            exportEnabled: 1,
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
            // getTopSpeedRunChart(container, isGame) {
            //     var dataset = [];

            //     if (this.tabledata?.length > 0) {
            //         var _data = JSON.parse(JSON.stringify(this.tabledata)); 

            //         if (isGame) {
            //             _data = _data.filter(x => x.rank);
            //         }

            //         var sortedData = _data.sort((a, b) => { return a?.primaryTimeMilliseconds - b?.primaryTimeMilliseconds; });
            //         var data = sortedData.slice(0, 10);

            //         dataset = data.map(item => {
            //             return { label: item.players?.map(item => { return item.name; }).join("{br}"), 
            //                      value: item.primaryTimeMilliseconds };
            //         });
            //     }

            //     const chartConfig = {
            //         type: "bar2d",
            //         renderAt: container,
            //         width: "100%",
            //         dataFormat: "json",
            //         dataSource: {
            //             chart: {
            //                 caption: isGame ? 'Top 10 Ranked' : 'Top 10',
            //                 subCaption: '',
            //                 xAxis: '',
            //                 yAxis: 'Time (Minutes)',
            //                 labelFontSize: 11,
            //                 labelVAlign: 'middle',
            //                 exportEnabled: 0,
            //                 showValues: 1,
            //                 placeValuesInside: 1,
            //                 valueFontSize: 12,
            //                 formatNumberScale: 1,
            //                 numberOfDecimals: 0,
            //                 useRoundEdges: 0,
            //                 numberscalevalue: "1000,60,60",
            //                 numberscaleunit: "s,m,h",
            //                 defaultnumberscale: "ms",
            //                 scalerecursively: "1",
            //                 maxscalerecursion: "-1",
            //                 scaleseparator: "",
            //                 theme: "candy",
            //                 bgColor: "#303030",
            //                 baseFontColor: "#fff",
            //                 outCnvBaseFontColor: "#fff"
            //             },
            //             data: dataset
            //         }
            //     };

            //     return chartConfig;
            // },
            getTopSpeedRunChart(container, isGame) {
                var that = this;                
                var categories = [];
                var dataset = [];

                if (this.tabledata?.length > 0) {
                    var _data = JSON.parse(JSON.stringify(this.tabledata)); 

                    if (isGame) {
                        _data = _data.filter(x => x.rank);
                    }

                    var sortedData = _data.sort((a, b) => { return a?.primaryTimeMilliseconds - b?.primaryTimeMilliseconds; });
                    var data = sortedData.slice(0, 10);

                    var chartDataObj = {};
                    var categoryObj = {};
                    data.forEach(item => {
                        var playerNames = item.players?.map(user => user.name).join("{br}");

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
                            caption: 'Top 10',
                            captionFontSize: 14,                            
                            subCaption: that.title,
                            subCaptionFontSize: 11,
                            xAxis: '',
                            yAxis: 'Time (Minutes)',
                            labelFontSize: 11,
                            labelVAlign: 'middle',                            
                            exportEnabled: 1,
                            showValues: 1,
                            valueFontSize: 11,
                            formatNumberScale: 1,
                            numberOfDecimals: 0,
                            useRoundEdges: 0,
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
   :deep(.fusioncharts-container>svg>g>g:last-of-type>g>g>rect) {
        fill:#444 !important;
        stroke: #222 !important;
    }    
   :deep(.fusioncharts-container > svg + div) {
        background:#444 !important;
        box-shadow: none !important;
        border-color: #222 !important;
    }      
   :deep(.fusioncharts-container > svg + div div) {
        background:#444 !important;
    }             
   :deep(.fusioncharts-container > svg + div div span) {
        color: #fff !important;        
        background:#444 !important;
    }     
   :deep(.fusioncharts-container > svg + div div span:hover) {
        background-color: rgba(255,255,255,.15) !important;
    }        
</style>




