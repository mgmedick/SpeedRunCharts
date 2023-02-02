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
            captionFontSize: function () {
                return this.ismodal ? 14 : 12;
            },     
            subCaptionFontSize: function () {
                return this.ismodal ? 13 : 11;
            },      
            labelFontSize: function () {
                return this.ismodal ? 13 : 11;
            },     
            valueFontSize: function () {
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
                            subCaption: that.title,
                            subCaptionFontSize: this.subCaptionFontSize,
                            xAxis: '',
                            yAxis: 'Time (Minutes)',
                            yAxisMaxValue: ymax,
                            yAxisMinValue: ymin,                             
                            labelFontSize: this.labelFontSize,
                            labelVAlign: 'middle',                            
                            exportEnabled: 1,
                            showValues: 1,
                            valueFontSize: this.valueFontSize,
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






