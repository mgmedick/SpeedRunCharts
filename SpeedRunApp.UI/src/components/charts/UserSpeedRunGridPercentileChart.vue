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
    import { formatTime } from '../../js/common.js';
    import FusionCharts from 'fusioncharts/core';
    import Pie2D from 'fusioncharts/viz/pie2d';  
    import CandyTheme from "fusioncharts/themes/es/fusioncharts.theme.candy";
    FusionCharts.addDep(Pie2D, CandyTheme);  

    export default {
        name: "UserSpeedRunGridPercentileChart",
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
                loading: true,
                chart: {}
            }
        },        
        computed: {       
            captionFontSize: function () {
                return this.ismodal ? 14 : 12;
            },     
            subCaptionFontSize: function () {
                return this.ismodal ? 13 : 11;
            },      
            legendItemFontSize: function () {
                return this.ismodal ? 13 : 11;
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
                var dataset = [];

                if (this.tabledata?.length > 0) {
                    var _data = JSON.parse(JSON.stringify(this.tabledata)); 
                    var allSpeedRunTimes = [];
                    if (this.istimerasc) {
                        allSpeedRunTimes = _data.sort((a, b) => { return b?.primaryTimeMilliseconds - a?.primaryTimeMilliseconds; });
                    } else {
                        allSpeedRunTimes = _data.sort((a, b) => { return a?.primaryTimeMilliseconds - b?.primaryTimeMilliseconds; });
                    }

                    var chartDataObj = {};
                    var percIncrement = 5;
                    var maxPerc = 25;
                    var showEvery = 2;
                    var maxNumCategories = Math.round((100 / percIncrement) / showEvery) + 1;

                    var prevPercNum = null;
                    var prevIndex = null;
                    var prevTime = null;
                    var timeFormat = this.showmilliseconds ? "milliseconds" : "seconds";

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
                            percent = Math.ceil((values.length / allSpeedRunTimes.length) * 100) || 0;
                            key = (this.istimerasc ? '<= ' : '>= ') + formatTime(timeFormat, prevTime) + " (" + percent + "%)";
                            chartDataObj[key] = values;
                            break;
                        } else {
                            time = this.showmilliseconds ? allSpeedRunTimes[index].primaryTimeMilliseconds : allSpeedRunTimes[index].primaryTimeSeconds;
                            percent = Math.trunc((values.length / allSpeedRunTimes.length) * 100) || 0;
                            key = (this.istimerasc ? '> ' : '< ') + formatTime(timeFormat, time) + " (" + percent + "%)";

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
                    renderAt: this.chartconainerid,
                    width: "100%",
                    height: this.ismodal ? "500" : "100%",
                    dataFormat: "json",
                    dataSource: {
                        chart: {
                            caption: 'Time Percentiles',
                            captionFontSize: this.captionFontSize,                            
                            subCaption: this.title,
                            subCaptionFontSize: this.subCaptionFontSize,
                            showValues: 1,
                            legendItemFontSize: this.legendItemFontSize,
                            formatNumberScale: 0,
                            numberOfDecimals: 0,
                            showPercentValues: 0,
                            showPercentInTooltip: 0,
                            exportEnabled: 1,
                            showLegend: 1,
                            showLabels: 0,
                            theme: "candy",
                            bgColor: this.bgColor,
                            baseFontColor: this.fontColor,
                            outCnvBaseFontColor: this.fontColor
                        },
                        data: dataset
                    }
                };

                return chartConfig;
            }            
        }
    }
</script>






