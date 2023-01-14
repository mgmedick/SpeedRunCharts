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
        name: "GameSpeedRunCountDonutChart",
        props: {  
            tabledata: Array,
            categories: Array,
            levels: Array,
            subcategoryvariablevaluetabs: Array,
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
                var chartObj = {};

                if (this.tabledata?.length > 0) {
                    var _alldata = JSON.parse(JSON.stringify(this.tabledata));

                    if (this.categorytypeid == 0) {
                        this.categories.filter(i => i.categoryTypeID == that.categorytypeid).forEach(category => { 
                            var data = _alldata.filter(i => i.categoryID == category.id);
                            chartObj[category.name] = { count: data.length };

                            if (that.subcategoryvariablevaluetabs && that.subcategoryvariablevaluetabs.filter(i => i.categoryID == category.id).length > 0) {
                                that.setVariableChartData(that.subcategoryvariablevaluetabs.filter(i => i.categoryID == category.id), data, chartObj[category.name]);                                 
                            }                            
                        });
                    }

                    var chartDataObj = { label: 'All', value: _alldata.length };
                    this.setChartData(chartObj, chartDataObj);
                    // Object.keys(chartObj).forEach(monthyear => {
                    //     if (Object.keys(chartObj[monthyear]).length > 0) {
                    //         var total = 0;
                    //         var tooltiptext = '';

                    //         Object.keys(chartObj[monthyear]).forEach(variableValueNames => {
                    //             var count = chartObj[monthyear][variableValueNames];
                    //             total += count;
                    //             // tooltiptext += variableValueNames + ' (' + count + (count == 1 ? " run" : " runs") + ') + ';
                    //             tooltiptext += count + ' (' + variableValueNames + ') + ';
                    //         });
                    //         tooltiptext = tooltiptext.replace(/(^ \+ )|( \+ $)/g, '');
                    //         chartDataObj[monthyear] = { value: total, tooltext: category.name + ': ' + total + (total == 1 ? ' run' : ' runs') + ' = ' + tooltiptext }                            
                    //     } else {
                    //         var total = chartObj[monthyear];
                    //         chartDataObj[monthyear] = { value: total, tooltext: category.name + ': ' + total + (total == 1 ? ' run' : ' runs') };
                    //     }
                    // });   

                    // Object.keys(chartDataObj).sort((a, b) => { return chartDataObj[b] - chartDataObj[a] }).forEach(key => {
                    //     dataset.push(chartDataObj);
                    // });
                    dataset.push(chartDataObj);
                }

                var chartConfig = {
                    type: "multilevelpie",
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
                        category: dataset
                    }
                };

                return chartConfig;
            },
            setVariableChartData(variables, tableData, chartObj, variableValueIDs) {
                var that = this;
                
                if (!variableValueIDs) {
                    variableValueIDs = '';
                }

                variables.forEach(variable => {
                    variable.variableValues.forEach(variableValue => {
                        var currVariableValueIDs = (variableValueIDs + ',' + variableValue.id).replace(/(^,)|(,$)/g, '');
                        var count = tableData.filter(i => variable.categoryID == i.categoryID && variable.levelID == i.levelID && i.subCategoryVariableValueIDs && i.subCategoryVariableValueIDs.startsWith(currVariableValueIDs)).length;
                        chartObj[variableValue.name] = { count: count };

                        if (variableValue.subVariables && variableValue.subVariables.length > 0) {
                            that.setVariableChartData(variableValue.subVariables, tableData, chartObj[variableValue.name], currVariableValueIDs);
                        }
                    });
                }); 
            },
            setChartData(chartObj, chartDataObj) {
                var that = this;

                Object.keys(chartObj).filter(i => i != "count").forEach(key => {
                    chartDataObj["category"] = chartDataObj["category"] || [];
                    var chartDataItem = { label: key, value: chartObj[key].count };
                    chartDataObj["category"].push(chartDataItem);
                    
                    if (Object.keys(chartObj[key]).filter(i => i != "count").length > 0) {
                        that.setChartData(chartObj[key], chartDataItem);
                    }
                });
            }           
        }
    }
</script>






