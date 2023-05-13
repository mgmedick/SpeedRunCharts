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
    import StackedColumn2DLine from 'fusioncharts/viz/stackedcolumn2dline';
    import CandyTheme from "fusioncharts/themes/es/fusioncharts.theme.candy";
    FusionCharts.addDep(StackedColumn2DLine, CandyTheme);

    export default {
        name: "UserSpeedRunCountLineChart",
        emits: ["onexpandchartclick"],
        props: {  
            tabledata: Array,
            games: Array,
            categorytypeid: String,
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
            caption: function () {
                return (this.categorytypeid == 0 ? 'Game' : 'Level') + ' Run Counts (Last 12 Months)';
            },                            
            captionFontSize: function () {
                return this.isMediaLarge ? 14 : 12;
            },                             
            labelFontSize: function () {
                return this.isMediaLarge ? 12 : 10;
            },                    
            outCnvBaseFontSize: function () {
                return this.isMediaLarge ? 13 : 11;
            },            
            valueFontSize: function () {
                return this.isMediaLarge ? 12 : 10;
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
                var categories = [];
                var dataset = [];
                var totaldata = [];

                if (this.tabledata?.length > 0) {
                    var _alldata = JSON.parse(JSON.stringify(this.tabledata));
                    var _alldates = _alldata.map(i => new Date(i.dateSubmitted));
                    var maxDate = new Date();                    
                    var minDataDate = new Date(Math.min.apply(null, _alldates));
                    var minDate = dayjs(maxDate).add(-12, "months").toDate();
                    minDate = minDataDate > minDate ? minDataDate : minDate;                   
                    var timePeriods = getDateDiffList("month", minDate, maxDate).map(x => { return dayjs(x).format("MM/YYYY") });

                    categoryObj["category"] = timePeriods.map(item => {
                        var labelObj = {};
                        labelObj["label"] = item;
                        return labelObj;
                    });
                    categories.push(categoryObj);

                    _alldata = _alldata.filter(i => new Date(i.dateSubmitted)>= minDate);

                    if (this.categorytypeid == 0) {
                        this.games.filter(i => i.categories.filter(i=>i.categoryTypeID == that.categorytypeid).length > 0).forEach(game => {
                            var categories = game.categories.filter(i=>i.categoryTypeID == that.categorytypeid).map(i => { return i.id });
                            var _data = _alldata.filter(i => i.gameID == game.id && categories.indexOf(i.categoryID) > -1);
                           
                            if (_data.length > 0) {
                                totaldata = totaldata.concat(_data);
                                var chartObj = that.getChartObj(_data);
                                var chartDataObj = that.getChartData(chartObj, game.name);

                                timePeriods.forEach(timePeriod => {
                                    if (!chartDataObj.hasOwnProperty(timePeriod)) {
                                        chartDataObj[timePeriod] = { value: 0, tooltext: timePeriod + ', ' + game.name + ', ' + ', 0 runs' };
                                    }
                                });

                                var chartData = Object.entries(chartDataObj)?.map(x => ({ label: x[0], value: x[1]?.value, showValue: x[1]?.value > 0, tooltext: x[1]?.tooltext }));                        
                                            
                                if (chartData.length > 0) {
                                    chartData = chartData.sort((a, b) => {
                                        var monthyeara = a.label.split("/");
                                        var monthyearb = b.label.split("/");

                                        return new Date(monthyeara[1], 1, monthyeara[0]) - new Date(monthyearb[1], 1, monthyearb[0])
                                    });                                       
                                }

                                dataset.push({ seriesname: game.name, data: chartData });                 
                            }                        
                        });
                    } else {
                        this.games.filter(i => i.categories.filter(i=>i.categoryTypeID == that.categorytypeid).length > 0).forEach(game => {
                            game.levels.forEach(level => {  
                                var _data = _alldata.filter(i => i.gameID == game.id && i.levelID == level.id);

                                if (_data.length > 0) {
                                    totaldata = totaldata.concat(_data);
                                    var chartObj = that.getChartObj(_data);
                                    var chartDataObj = that.getChartData(chartObj, game.name + ", " + level.name);

                                    timePeriods.forEach(timePeriod => {
                                        if (!chartDataObj.hasOwnProperty(timePeriod)) {
                                            chartDataObj[timePeriod] = { value: 0, tooltext: timePeriod + ', ' + game.name + ", " + level.name + ', ' + ', 0 runs' };
                                        }
                                    });

                                    var chartData = Object.entries(chartDataObj)?.map(x => ({ label: x[0], value: x[1]?.value, showValue: x[1]?.value > 0, tooltext: x[1]?.tooltext }));                        
                                                
                                    if (chartData.length > 0) {
                                        chartData = chartData.sort((a, b) => {
                                            var monthyeara = a.label.split("/");
                                            var monthyearb = b.label.split("/");

                                            return new Date(monthyeara[1], 1, monthyeara[0]) - new Date(monthyearb[1], 1, monthyearb[0])
                                        });                                       
                                    }

                                    dataset.push({ seriesname: game.name + ", " + level.name, data: chartData });                 
                                }    
                            });                    
                        });              
                    }

                    if (totaldata.length > 0) {
                        var chartObj = that.getChartObj(totaldata);
                        var chartDataObj = that.getChartData(chartObj, 'Total');

                        timePeriods.forEach(timePeriod => {
                            if (!chartDataObj.hasOwnProperty(timePeriod)) {
                                chartDataObj[timePeriod] = { value: 0, tooltext: timePeriod + ' Total, ' + ', 0 runs' };
                            }
                        });

                        var chartData = Object.entries(chartDataObj)?.map(x => ({ label: x[0], value: x[1]?.value, tooltext: x[1]?.tooltext, color: "#FF0000" }));                        
                                    
                        if (chartData.length > 0) {
                            chartData = chartData.sort((a, b) => {
                                var monthyeara = a.label.split("/");
                                var monthyearb = b.label.split("/");

                                return new Date(monthyeara[1], 1, monthyeara[0]) - new Date(monthyearb[1], 1, monthyearb[0])
                            });                                       
                        }

                        dataset.push({ seriesname: 'Total', data: chartData, renderAs: 'line', showValues: 0, includeInLegend: 0 }); 
                    }
                }

                var chartConfig = {
                    type: "stackedcolumn2dline",
                    renderAt: this.chartconainerid,
                    width: "100%",
                    height: "100%",
                    dataFormat: "json",
                    dataSource: {
                        chart: {
                            caption: this.caption,
                            captionFontSize: this.captionFontSize,   
                            captionAlignment:"center",
                            captionFontColor: this.fontColor,
                            alignCaptionWithCanvas: 0,                                                     
                            xAxis: '',
                            yAxis: 'Total Runs',  
                            showZeroPlaneValue: 0,
                            yAxisMinValue: 0,                                                       
                            labelFontSize: this.labelFontSize,
                            labelVAlign: 'middle',
                            rotateLabels: 1,
                            slantLabels: 1,                        
                            exportEnabled: 1,
                            showValues: 1,
                            plotTooltext: "$label, $value run(s)",
                            valueFontSize: this.valueFontSize,
                            outCnvBaseFontSize: this.outCnvBaseFontSize,                                                                            
                            numberOfDecimals: 0,
                            useRoundEdges: 0,
                            useEllipsesWhenOverflow: 1,
                            showLegend: 1,
                            legendItemFontSize: this.legendItemFontSize,
                            legendIconScale: this.legendIconScale,
                            legendItemFontColor: this.fontColor,
                            // legendNumRows: 3,
                            // legendNumColumns: 4,   
                            palettecolors: this.paletteColors.join(','),                                                   
                            theme: "candy",
                            bgColor: this.bgColor,
                            valueFontColor: "#fff",
                            textOutline: 1,
                            baseFontColor: this.fontColor,
                            labelFontColor: this.fontColor,
                            outCnvBaseFontColor: this.fontColor                            
                        },
                        categories: categories,
                        dataset: dataset
                    }
                };

                return chartConfig;
            },
            getChartObj(data) {
                var that = this;
                var chartObj = {};

                data.forEach(item => {
                    var monthYear = dayjs(item.dateSubmitted).format("MM/YYYY");
                    chartObj[monthYear] = chartObj[monthYear] || {};
                    chartObj[monthYear] = { count: (chartObj[monthYear]?.count ?? 0) + 1 };
                });
                
                return chartObj;
            },
            getChartData(chartObj, seriesName) {
                var chartDataObj = {};
                
                Object.keys(chartObj).forEach(monthyear => {
                    var total = chartObj[monthyear].count;
                    chartDataObj[monthyear] = { value: total, tooltext: monthyear + ', ' + seriesName + ', ' + total + (total == 1 ? ' run' : ' runs') };
                });   

                return chartDataObj;
            }                      
        }
    }
</script>








