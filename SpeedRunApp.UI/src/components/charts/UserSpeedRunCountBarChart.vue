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
    import Column2D from 'fusioncharts/viz/column2d';
    import CandyTheme from "fusioncharts/themes/es/fusioncharts.theme.candy";
    FusionCharts.addDep(Column2D, CandyTheme);

    export default {
        name: "UserSpeedRunCountBarChart",
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
                return (this.categorytypeid == 0 ? 'Game' : 'Level') + ' Run Counts';
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
                var chartObj = {};

                if (this.tabledata?.length > 0) {
                    var _alldata = JSON.parse(JSON.stringify(this.tabledata));

                    if (this.categorytypeid == 0) {
                        this.games.filter(i => i.categories.filter(i=>i.categoryTypeID == that.categorytypeid).length > 0).forEach(game =>{
                            var categories = game.categories.filter(i=>i.categoryTypeID == that.categorytypeid).map(i => { return i.id });
                            var _data = _alldata.filter(i => i.gameID == game.id && categories.indexOf(i.categoryID) > -1);

                            if (_data.length > 0) {
                                that.setChartObj(_data, chartObj, game.name);
                            }
                        });
                    } else {
                        this.games.filter(i => i.categories.filter(i=>i.categoryTypeID == that.categorytypeid).length > 0).forEach(game =>{
                            game.levels.forEach(level => {    
                                var levelName = level.name.replace(/( \(empty\)$)/g, ''); 
                                var _data = _alldata.filter(i => i.gameID == game.id && i.levelID == level.id);

                                if (_data.length > 0) {
                                    that.setChartObj(_data, chartObj, game.name + ", " + levelName);
                                } 
                            });
                        });                      
                    }

                    if (Object.keys(chartObj).length > 0) {
                        dataset = Object.entries(chartObj).map(x => {
                            return { label: x[0], value: x[1].count }
                        });                        
                    }                    
                }

                var chartConfig = {
                    type: "column2d",
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
                            showLegend: 0,
                            palettecolors: this.paletteColors.join(','),                                                   
                            theme: "candy",
                            bgColor: this.bgColor,
                            valueFontColor: "#fff",
                            textOutline: 1,
                            baseFontColor: this.fontColor,
                            labelFontColor: this.fontColor,
                            outCnvBaseFontColor: this.fontColor                            
                        },
                        data: dataset
                    }
                };

                return chartConfig;
            },
            setChartObj(data, chartObj, seriesName) {
                var that = this;

                data.forEach(item => {
                    chartObj[seriesName] = chartObj[seriesName] || {};
                    chartObj[seriesName] = { count: (chartObj[seriesName]?.count ?? 0) + 1 };
                });
                
                return chartObj;
            }          
        }
    }
</script>








