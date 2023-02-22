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
    import { formatTime } from '../../js/common.js';
    import FusionCharts from 'fusioncharts/core';
    import Pie2D from 'fusioncharts/viz/pie2d';  
    import CandyTheme from "fusioncharts/themes/es/fusioncharts.theme.candy";
    FusionCharts.addDep(Pie2D, CandyTheme);  

    export default {
        name: "LeaderboardPercentileChart",
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
            legendItemFontSize: function () {
                return this.isMediaLarge ? 13 : 11;
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
                    height: "100%",
                    dataFormat: "json",
                    dataSource: {
                        chart: {
                            caption: 'Time Percentiles',
                            captionFontSize: this.captionFontSize, 
                            captionAlignment:"center",
                            captionFontColor: this.fontColor,
                            alignCaptionWithCanvas: 0,                             
                            subCaption: this.title,
                            subCaptionFontSize: this.subCaptionFontSize,
                            subCaptionFontColor: "#888",
                            showValues: 1,
                            legendItemFontSize: this.legendItemFontSize,
                            legendItemFontColor: this.fontColor,
                            formatNumberScale: 0,
                            numberOfDecimals: 0,
                            showPercentValues: 0,
                            showPercentInTooltip: 0,
                            exportEnabled: 1,
                            showLegend: 1,
                            showLabels: 0,
                            theme: "candy",
                            palettecolors: this.paletteColors.join(','),                            
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






