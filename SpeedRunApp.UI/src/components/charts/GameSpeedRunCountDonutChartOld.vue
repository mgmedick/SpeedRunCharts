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
    import MultiLevelPie from 'fusioncharts/viz/multilevelpie';    
    import CandyTheme from "fusioncharts/themes/es/fusioncharts.theme.candy";
    FusionCharts.addDep(MultiLevelPie, CandyTheme);

    export default {
        name: "GameSpeedRunCountDonutChartOld",
        props: {  
            tabledata: Array,
            categories: Array,
            levels: Array,
            variables: Array,            
            categorytypeid: String,
            categoryid: String,           
            showmilliseconds: Boolean,
            subcaption: String,
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
                return this.ismodal ? 13 : 11;
            },          
            legendIconScale: function () {
                return this.ismodal ? 1 : .5;
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
                var chartDataObj = {};

                if (this.tabledata?.length > 0) {
                    var _alldata = JSON.parse(JSON.stringify(this.tabledata));

                    if (this.categorytypeid == 0) {
                        this.categories.filter(i => i.categoryTypeID == that.categorytypeid).forEach(category => { 
                            chartDataObj[category.name] = {};
                            if(that.variables?.filter(i => i.categoryid == category.id && variable.isSubCategory).length > 0){
                                that.variables?.filter(i => i.categoryid == category.id && variable.isSubCategory).forEach(variable => {
                                    variable.variableValues?.forEach(variableValue => {
                                        
                                    });
                                });                                  
                            } else {
                                chartDataObj[category.name] = {};
                            }

                            var _data = _alldata.filter(i => i.categoryID == category.id);
                            chartDataObj[category.name] = _data.length;
                        });
                    } else {
                        this.levels.forEach(level => {                                   
                            var _data = _alldata.filter(i => i.categoryID == that.categoryid && i.levelID == level.id);
                            chartDataObj[level.name] = _data.length;
                        });
                    }

                    Object.keys(chartDataObj).sort((a, b) => { return chartDataObj[b] - chartDataObj[a] }).forEach(key => {
                        dataset.push({ label: key, value: chartDataObj[key] });
                    });
                }

                var chartConfig = {
                    type: "doughnut2d",
                    renderAt: this.chartconainerid,
                    width: "100%",
                    height: this.ismodal ? "500" : "100%",
                    dataFormat: "json",
                    dataSource: {
                        chart: {
                            caption: 'Total Runs',
                            captionFontSize: this.captionFontSize,                            
                            subCaption: this.subcaption,
                            subCaptionFontSize: this.subCaptionFontSize,
                            showValues: 1,
                            formatNumberScale: 0,
                            numberOfDecimals: 0,
                            showPercentValues: 0,
                            showPercentInTooltip: 0,
                            exportEnabled: 1,
                            showLegend: 1,
                            legendItemFontSize: this.legendItemFontSize,
                            legendIconScale: this.legendIconScale,
                            showLabels: 0,
                            theme: "candy",
                            palettecolors: "36b5d8,f0dc46,f066ac,6ec85a,6e80ca,e09653,e1d7ad,61c8c8,ebe4f4,e64141,f2003e,00abfe,00e886,c7f600,9500f2,ff9a04,e200aa,a4cdfe,01b596,ecd86f",
                            bgColor: this.bgColor,
                            baseFontColor: this.fontColor,
                            outCnvBaseFontColor: this.fontColor,
                        },
                        data: dataset
                    }
                };

                return chartConfig;
            }           
        }
    }
</script>






