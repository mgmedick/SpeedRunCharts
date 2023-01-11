<template>
    <div style="height:100%;">
        <div v-if="loading" class="d-flex" style="height:100%;">
            <div class="m-auto">
                <i class="fas fa-spinner fa-spin fa-lg"></i>
            </div>
        </div>
        <div :id="chartconainerid"></div>
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
        name: "GameSpeedRunCountLineChart",
        props: {  
            tabledata: Array,
            categories: Array,
            categorytypeid: String,
            categoryid: String,           
            showmilliseconds: Boolean,
            ismodal: Boolean,
            chartconainerid: String
        },
        data() {
            return {
                loading: true,
                chart: {}
            }
        },        
        computed: {    
            chartType: function () {
                var that = this;
                var filteredCategories = this.categories.filter(i => i.categoryTypeID == that.categorytypeid);
                return filteredCategories.filter(i => i.isTimerAsc).length == filteredCategories.length ? 'inversemsline' : 'msline'
            },                     
            captionFontSize: function () {
                return this.ismodal ? 14 : 12;
            },
            subCaption: function () {
                return this.categorytypeid == 0 ? 'PerGame Categories' : 'PerLevel Categories'
            },              
            subCaptionFontSize: function () {
                return this.ismodal ? 13 : 11;
            },  
            labelFontSize: function () {
                return this.ismodal ? 13 : 11;
            },                    
            yAxisValueFontSize: function () {
                return this.ismodal ? 13 : 11;
            },      
            legendItemFontSize: function () {
                return this.ismodal ? 12 : 10;
            },                                                      
            bgColor: function () {
                return document.body.classList.contains('theme-dark') ? "#303030" : "#f8f9fa";
            },
            fontColor: function () {
                return document.body.classList.contains('theme-dark') ? "#fff" : "#212529";
            }                                   
        },              
        mounted: function () {
            this.loadChart();
        },
        methods: {   
            // loadData() {
            //     var that = this;
            //     this.loading = true;

            //     axios.get('/SpeedRun/GetGameSpeedRunCountLineChartData', { params: { gameID: this.gameid } })
            //         .then(res => {
            //             that.tabledata = res.data; 
            //             that.loadChart();                                          
            //         })
            //         .catch(err => { console.error(err); return Promise.reject(err); });
            // },                     
            loadChart() {
                var that = this;
                this.loading = true;
                FusionCharts.ready(function () {
                    that.chart = new FusionCharts(that.initChart());
                    that.chart.render(); 
                    that.loading = false;                                          
                });
            },
            initChart() {
                var that = this;
                var categoryObj = {};
                var timePeriods = [];
                var categories = [];
                var dataset = [];

                if (this.tabledata?.length > 0) {
                    var maxDate = new Date();
                    var minDate = dayjs(maxDate).add(-12, "months").toDate();
                    var timePeriods = getDateDiffList("month", minDate, maxDate).map(x => { return dayjs(x).format("MM/YYYY") });

                    categoryObj["category"] = timePeriods.map(item => {
                        var labelObj = {};
                        labelObj["label"] = item;
                        return labelObj;
                    });
                    categories.push(categoryObj);

                    var _data = JSON.parse(JSON.stringify(this.tabledata));

                    if (this.categorytypeid == 0) {
                        this.categories.filter(i => i.categoryTypeID == that.categorytypeid).forEach(category => {                                   
                            _data = _data.filter(i => i.categoryID == category.id).sort((a, b) => { 
                                return new Date(b.dateSubmitted) - new Date(a.dateSubmitted);
                            });   

                            var chartDataObj = {};
                            _data.forEach(item => {
                                var monthYear = dayjs(item.dateSubmitted).format("MM/YYYY")
                                var total = (chartDataObj[monthYear]?.value ?? 0) + 1;
                                chartDataObj[monthYear] = { value: total, tooltext: monthYear + " - " + total + " runs" }
                            });

                            timePeriods.forEach(timePeriod => {
                                if (!chartDataObj.hasOwnProperty(timePeriod)) {
                                    chartDataObj[timePeriod] = { value: 0, tooltext: timePeriod + ' - 0 runs' };
                                }
                            });

                            var chartData = Object.entries(chartDataObj)?.map(x => ({ label: x[0], value: x[1]?.value, tooltext: x[1]?.tooltext }));                        
                                        
                            if (chartData.length > 0) {
                                chartData = chartData.sort((a, b) => {
                                    var monthyeara = a.label.split("/");
                                    var monthyearb = b.label.split("/");

                                    return new Date(monthyeara[1], 1, monthyeara[0]) - new Date(monthyearb[1], 1, monthyearb[0])
                                });                                       
                            }

                            dataset.push({ seriesname: category.name, data: chartData });
                        });
                    } else {
                        this.levels.forEach(level => {                                   
                            _data = _data.filter(i => i.categoryID == that.categoryid && i.levelID == level.id).sort((a, b) => { 
                                return new Date(b.dateSubmitted) - new Date(a.dateSubmitted);
                            });   

                            var chartDataObj = {};
                            _data.forEach(item => {
                                var monthYear = dayjs(item.dateSubmitted).format("MM/YYYY")
                                var total = (chartDataObj[monthYear]?.value ?? 0) + 1;
                                chartDataObj[monthYear] = { value: total, tooltext: monthYear + " - " + total + " runs" }
                            });

                            timePeriods.forEach(timePeriod => {
                                if (!chartDataObj.hasOwnProperty(timePeriod)) {
                                    chartDataObj[timePeriod] = { value: 0, tooltext: timePeriod + ' - 0 runs' };
                                }
                            });

                            var chartData = Object.entries(chartDataObj)?.map(x => ({ label: x[0], value: x[1]?.value, tooltext: x[1]?.tooltext }));                        
                                        
                            if (chartData.length > 0) {
                                chartData = chartData.sort((a, b) => {
                                    var monthyeara = a.label.split("/");
                                    var monthyearb = b.label.split("/");

                                    return new Date(monthyeara[1], 1, monthyeara[0]) - new Date(monthyearb[1], 1, monthyearb[0])
                                });                                       
                            }

                            dataset.push({ seriesname: level.name, data: chartData });
                        });
                    }
                }

                var chartConfig = {
                    type: this.chartType,
                    renderAt: this.chartconainerid,
                    width: "100%",
                    height: this.ismodal ? "500" : "100%",
                    dataFormat: "json",
                    dataSource: {
                        chart: {
                            caption: 'Number of Runs (Last 12 months)',
                            captionFontSize: this.captionFontSize,                           
                            subCaption: this.subCaption,
                            subCaptionFontSize: this.subCaptionFontSize,
                            captionAlignment:"center",
                            xAxis: 'Date',
                            yAxis: 'Runs',
                            canvasTopPadding: 5,
                            canvasBottomPadding: 5,
                            canvasLeftPadding: 10,
                            canvasRightPadding: 10,
                            labelFontSize: this.labelFontSize,
                            showLabels: 1,
                            rotateLabels: 1,
                            slantLabels: 1,
                            showToolTip: 1,
                            seriesNameInToolTip: 0,
                            hoverOnEmpty: 0,
                            showLegend: 1,
                            legendItemFontSize: this.legendItemFontSize,
                            lineThickness: 2,
                            anchorRadius: 5,
                            anchorBgColor: this.bgColor,
                            anchorBorderThickness: 1,                                                   
                            exportEnabled: 1,
                            showValues: 0,
                            formatNumberScale: 1,
                            numberOfDecimals: 0,
                            // numberscalevalue: this.showmilliseconds ? "1000,60,60" : "60,60",                           
                            // numberscaleunit: this.showmilliseconds ? "s,m,h" : "m,h",
                            // defaultnumberscale: this.showmilliseconds ? "ms" : "s",
                            // scalerecursively: "1",
                            // maxscalerecursion: "-1",                            
                            scaleseparator: " ",
                            connectNullData: 1,
                            plotBinSize: 1.5,
                            //setAdaptiveYMin: 1,
                            yAxisValueFontSize: this.yAxisValueFontSize,                        
                            theme: "candy",
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






