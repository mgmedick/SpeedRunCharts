<template>
    <div style="height:100%;">
        <div v-if="loading" class="d-flex" style="height:100%;">
            <div class="m-auto">
                <i class="fas fa-spinner fa-spin fa-lg"></i>
            </div>
        </div>
        <div :id="chartconainerid" style="height:100%;"></div>
        <div v-if="!loading && !ismodal" style="cursor:pointer !important;" @click="$emit('onexpandchartclick', $event)">
            <i class="fas fa-expand" style="position:absolute; bottom:20px; right:20px;"></i>
        </div>        
    </div>
</template>
<script>
    const dayjs = require('dayjs');
    import { getDateDiffList } from '../../js/common.js';
    import FusionCharts from 'fusioncharts/core';
    import MSLine from 'fusioncharts/viz/msline';    
    import InverseMSLine from 'fusioncharts/viz/inversemsline';    
    import CandyTheme from "fusioncharts/themes/es/fusioncharts.theme.candy";
    FusionCharts.addDep(MSLine, InverseMSLine, CandyTheme);

    export default {
        name: "LeaderboardTopLineChart",
        emits: ["onexpandchartclick"],
        props: {  
            tabledata: Array,
            title: String,
            istimerasc: Boolean,
            showmilliseconds: Boolean,
            ismodal: Boolean,
            chartconainerid: String
        },
        data() {
            return {
                loading: true
            }
        },        
        computed: {    
            isMediaLarge: function () {
                return this.$el.clientWidth > 992;
            },                             
            captionFontSize: function () {
                return this.isMediaLarge ? 14 : 12;
            },                              
            subCaptionFontSize: function () {
                return this.isMediaLarge ? 12 : 10;
            },  
            labelFontSize: function () {
                return this.isMediaLarge ? 13 : 11;
            },                    
            yAxisValueFontSize: function () {
                return this.isMediaLarge ? 13 : 11;
            },      
            legendItemFontSize: function () {
                return this.isMediaLarge ? 12 : 10;
            },     
            legendIconScale: function () {
                return this.isMediaLarge ? .8 : .5;
            },                                                                             
            bgColor: function () {
                return document.body.classList.contains('theme-dark') ? "#303030" : "#f8f9fa";
            },
            fontColor: function () {
                return document.body.classList.contains('theme-dark') ? "#fff" : "#000";
            },
            paletteColors: function() {
                var colors = ['36b5d8','f0dc46','f066ac','6ec85a','6e80ca','e09653','e1d7ad','61c8c8','ebe4f4','e60049','0bb4ff','50e991','ffee00','9b19f5','ffa300','dc0ab4','b3d4ff','00bfa0','fd7f6f','7eb0d5','b2e061','bd7ebe','ffb55a','fff6b3','beb9db','fdcce5','8bd3c7','3366cc','dc3912','ff9900','109618','990099','0099c6','dd4477','b9d2d5','efd39e','efa7a7','bbf2d5','7db8b9','ffc197'];
                return colors;
            }                                                                
        },              
        mounted: function () {
            this.loadChart();
        },
        methods: {                      
            loadChart() {
                var that = this;
                this.loading = true;
                FusionCharts.ready(function () {
                    new FusionCharts(that.initChart()).render();
                    that.loading = false;                                          
                });
            },
            initChart() {
                var that = this;
                var categoryObj = {};
                var timePeriods = [];
                var categories = [];
                var dataset = [];
                var ymax = 0;
                var ymin = 0;
                var labelStep = 0;

                if (this.tabledata?.length > 0) {
                    var _alldata = JSON.parse(JSON.stringify(this.tabledata)); 
                    var topdata = _alldata.filter(x => x.rank);

                    var sortedData = [];
                    if (this.istimerasc){
                        sortedData = topdata.sort((a, b) => { return b?.primaryTimeMilliseconds - a?.primaryTimeMilliseconds; });
                    } else {
                        sortedData = topdata.sort((a, b) => { return a?.primaryTimeMilliseconds - b?.primaryTimeMilliseconds; });
                    }                    
                    topdata = sortedData.slice(0, 10);

                    var players = [];
                    topdata.forEach(item => {
                        var playerNames = item.players?.map(user => user.name).join("{br}");
                        players.push(playerNames);
                    });

                    var filteredData = _alldata.filter(i => !isNaN(Date.parse(i.dateSubmitted)) && i.players && players.indexOf(i.players.map(user => user.name).join("{br}")) > -1);

                    var dates = filteredData.map(item => { return new Date(item.dateSubmitted) });
                    var maxDate = dayjs(Math.max.apply(null, dates)).startOf('day').toDate();      
                    var minDate = dayjs(Math.min.apply(null, dates)).startOf('day').toDate();
                    // var minDate = dayjs(maxDate).add(-12, "months").toDate();
                    // minDate = minDataDate > minDate ? minDataDate : minDate;
                    //filteredData = filteredData.filter(x => { return new Date(x.dateSubmitted) >= minDate; });

                    var timePeriods = getDateDiffList("day", minDate, maxDate).map(x => { return dayjs(x).format("MM/DD/YYYY") });

                    categoryObj["category"] = timePeriods.map(item => {
                        var labelObj = {};
                        labelObj["label"] = item;
                        return labelObj;
                    });
                    categories.push(categoryObj);

                    var times = filteredData.map(i => that.showmilliseconds ? i.primaryTimeMilliseconds : i.primaryTimeSeconds);
                    ymin = Math.min.apply(null, times);
                    ymax = Math.max.apply(null, times);
                    var maxLabelCount = this.ismodal ? 20 : 10;
                    labelStep = Math.floor(timePeriods.length / maxLabelCount);

                    players.forEach(player => {                                   
                        var playerNames = player;                            
                        var _data = filteredData.filter(i => i.players.map(user => user.name).join("{br}") == playerNames).sort((a, b) => { 
                            return new Date(b.dateSubmitted) - new Date(a.dateSubmitted);
                        });   

                        if (_data.length > 0) {
                            var groupedObj = {};
                            var chartDataObj = {};
                            _data.forEach(item => {
                                var monthDayYear = dayjs(item.dateSubmitted).format("MM/DD/YYYY");

                                groupedObj[monthDayYear] = groupedObj[monthDayYear] || [];
                                if(that.showmilliseconds){
                                    groupedObj[monthDayYear].push({ primaryTime: item.primaryTimeMilliseconds, primaryTimeString: item.primaryTimeString, playerNames: playerNames });                        
                                } else {
                                    groupedObj[monthDayYear].push({ primaryTime: item.primaryTimeSeconds, primaryTimeString: item.primaryTimeSecondsString, playerNames: playerNames });                        
                                }
                            });

                            if (Object.keys(groupedObj).length > 0) {
                                for (var key in groupedObj) {
                                    chartDataObj[key] = chartDataObj[key] || [];

                                    var minItem = {};
                                    if (this.istimerasc) {
                                        minItem = groupedObj[key].sort((a, b) => { return b?.primaryTime - a?.primaryTime })[0];
                                    } else {
                                        minItem = groupedObj[key].sort((a, b) => { return a?.primaryTime - b?.primaryTime })[0];
                                    }

                                    chartDataObj[key] = { value: minItem.primaryTime, tooltext: key + "{br}" + minItem.playerNames + "{br}" + minItem.primaryTimeString };                                                  
                                }

                                timePeriods.forEach(timePeriod => {
                                    if (!chartDataObj.hasOwnProperty(timePeriod)) {
                                        chartDataObj[timePeriod] = { value: null, tooltext: ' ' };
                                    }
                                });

                                var chartData = Object.entries(chartDataObj)?.map(x => ({ label: x[0], value: x[1]?.value, tooltext: x[1]?.tooltext }));                        
                                            
                                if (chartData.length > 0) {
                                    chartData = chartData.sort((a, b) => {
                                        var monthdayyeara = a.label.split("/");
                                        var monthdayyearb = b.label.split("/");

                                        return new Date(monthdayyeara[2], monthdayyeara[0] - 1, monthdayyeara[1]) - new Date(monthdayyearb[2], monthdayyearb[0] - 1, monthdayyearb[1])
                                    });      
                                }

                                dataset.push({ seriesname: playerNames, data: chartData });
                            }
                        }
                    });
                }

                var chartConfig = {
                    type: this.istimerasc ? "inversemsline" : "msline",
                    renderAt: this.chartconainerid,
                    width: "100%",
                    height: "100%",
                    dataFormat: "json",
                    dataSource: {
                        chart: {
                            caption: 'Top 10 Players Run History',
                            captionFontSize: this.captionFontSize, 
                            captionAlignment:"center",
                            captionFontColor: this.fontColor,                            
                            alignCaptionWithCanvas: 0,                                                      
                            subCaption: this.title,
                            subCaptionFontSize: this.subCaptionFontSize,
                            subCaptionFontColor: "#888",
                            xAxis: 'Date',
                            yAxis: 'Time',
                            canvasTopPadding: 5,
                            canvasBottomPadding: 5,
                            canvasLeftPadding: 10,
                            canvasRightPadding: 10,
                            labelFontSize: this.labelFontSize,
                            showLabels: 1,
                            labelStep: labelStep,
                            rotateLabels: 1,
                            slantLabels: 1,
                            showToolTip: 1,
                            hoverOnEmpty: 0,
                            showLegend: 1,
                            plotHighlightEffect: 'fadeout|anchorBgColor=7f7f7f, alpha=5',
                            legendItemFontSize: this.legendItemFontSize,
                            legendIconScale: this.legendIconScale,
                            legendItemFontColor: this.fontColor,
                            // legendNumRows: 3,
                            // legendNumColumns: 4,
                            alignLegendWithCanvas: 1,
                            lineThickness: 2,
                            anchorRadius: 2,
                            anchorBgColor: this.bgColor,
                            //anchorAlpha: 0,
                            anchorBgAlpha: 100,
                            anchorBorderThickness: 1,                                                   
                            exportEnabled: 1,
                            showValues: 0,
                            formatNumberScale: 1,
                            numberOfDecimals: 0,
                            numberscalevalue: this.showmilliseconds ? "1000,60,60" : "60,60",                           
                            numberscaleunit: this.showmilliseconds ? "s,m,h" : "m,h",
                            defaultnumberscale: this.showmilliseconds ? "ms" : "s",
                            scalerecursively: "1",
                            maxscalerecursion: "-1",                     
                            scaleseparator: " ",
                            connectNullData: 1,
                            //plotBinSize: 1.5,
                            //setAdaptiveYMin: 1,
                            yAxisMaxValue: ymax,
                            yAxisMinValue: ymin,
                            yAxisValueFontSize: this.yAxisValueFontSize,                        
                            theme: "candy",
                            palettecolors: this.paletteColors.join(','),                                                       
                            bgColor: this.bgColor,
                            baseFontColor: this.fontColor,
                            outCnvBaseFontColor: this.fontColor
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






