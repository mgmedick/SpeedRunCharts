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
    import FusionCharts from 'fusioncharts/core';
    import StackedBar2D from 'fusioncharts/viz/stackedbar2d';
    import CandyTheme from "fusioncharts/themes/es/fusioncharts.theme.candy";
    FusionCharts.addDep(StackedBar2D, CandyTheme);  

    export default {
        name: "UserSpeedRunTopChart",
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
                loading: true,
                chart: {}
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
            valueFontSize: function () {
                return this.isMediaLarge ? 13 : 11;
            },       
            outCnvBaseFontSize: function () {
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
                    that.chart = new FusionCharts(that.initChart());
                    that.chart.render(); 
                    that.loading = false;                                          
                });
            },
            initChart() {
                var that = this;                
                var categories = [];
                var dataset = [];
                var ymax = 0;
                var ymin = 0;

                if (this.tabledata?.length > 0) {
                    var _data = JSON.parse(JSON.stringify(this.tabledata)); 

                    var sortedData = [];
                    if (this.istimerasc){
                        sortedData = _data.sort((a, b) => { return b?.primaryTimeMilliseconds - a?.primaryTimeMilliseconds; });
                    } else {
                        sortedData = _data.sort((a, b) => { return a?.primaryTimeMilliseconds - b?.primaryTimeMilliseconds; });
                    }

                    var data = sortedData.slice(0, 10);

                    var chartDataObj = {};
                    var categoryObj = {};
                    data.forEach(item => {
                        var playerNames = item.players?.map(user => user.name).join("{br}");

                        chartDataObj[playerNames] = this.showmilliseconds ? item.primaryTimeMilliseconds : item.primaryTimeSeconds;
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
                        return { value: this.showmilliseconds ? item.primaryTimeMilliseconds : item.primaryTimeSeconds };
                    });

                    if (dataValues && dataValues.length > 0) {
                        ymax = dataValues[dataValues.length - 1].value;
                        ymin = this.showmilliseconds ? dataValues[0].value - 1000 : dataValues[0].value - 60;
                        
                        if (ymin < 0) {
                            ymin = 0;
                        }
                    }

                    dataset.push({ seriesname: '', data: dataValues });
                }

                const chartConfig = {
                    type: "stackedBar2D",
                    renderAt: this.chartconainerid,
                    width: "100%",
                    height: "100%",
                    dataFormat: "json",
                    dataSource: {
                        chart: {
                            caption: 'Top 10',
                            captionFontSize: this.captionFontSize,
                            captionAlignment:"center",
                            captionFontColor: this.fontColor,
                            alignCaptionWithCanvas: 0, 
                            subCaption: that.title,
                            subCaptionFontSize: this.subCaptionFontSize,
                            subCaptionFontColor: "#888",
                            xAxis: '',
                            yAxis: 'Time (Minutes)',
                            yAxisMaxValue: ymax,
                            yAxisMinValue: ymin,                             
                            labelFontSize: this.labelFontSize,
                            labelVAlign: 'middle',                            
                            exportEnabled: 1,
                            showValues: 1,
                            valueFontSize: this.valueFontSize,
                            outCnvBaseFontSize: this.outCnvBaseFontSize,
                            formatNumberScale: 1,
                            numberOfDecimals: 0,
                            useRoundEdges: 0,
                            numberscalevalue: this.showmilliseconds ? "1000,60,60" : "60,60",                           
                            numberscaleunit: this.showmilliseconds ? "s,m,h" : "m,h",
                            defaultnumberscale: this.showmilliseconds ? "ms" : "s",
                            scalerecursively: "1",
                            maxscalerecursion: "-1",
                            scaleseparator: " ",                           
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






