<template>
    <div style="height:100%">
        <div v-if="loading" class="d-flex" style="height:100%;">
            <div class="m-auto">
                <i class="fas fa-spinner fa-spin fa-lg"></i>
            </div>
        </div>
        <div :id="chartconainerid"></div>
    </div>
</template>
<script>
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
            caption: function () {
                return (this.categorytypeid == 0 ? 'Category' : 'Level') + ' Distribution';
            },                            
            captionFontSize: function () {
                return this.ismodal ? 14 : 12;
            },
            subCaption: function () {
                return this.subcaption + '{br}(Empties Excluded)'
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
            valueFontSize: function () {
                return this.ismodal ? 12 : 10;
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
                var chartDataObj = {};

                if (this.tabledata?.length > 0) {
                    var _alldata = JSON.parse(JSON.stringify(this.tabledata));

                    if (this.categorytypeid == 0) {
                        this.categories.filter(i => i.categoryTypeID == that.categorytypeid).forEach(category => {
                            var categoryName = category.name.replace(/( \(empty\)$)/g, '');
                            var data = _alldata.filter(i => i.categoryID == category.id);
                            var count = data.length;

                            if (count > 0) {
                                var percent = Math.round((count / _alldata.length) * 100);
                                chartObj[categoryName] = { count: count, tooltext: categoryName + ', ' + count + ' runs (' + percent + '%)' };

                                if (that.subcategoryvariablevaluetabs && that.subcategoryvariablevaluetabs.filter(i => i.categoryID == category.id).length > 0) {
                                    that.setVariableChartData(that.subcategoryvariablevaluetabs.filter(i => i.categoryID == category.id), data, chartObj[categoryName], categoryName, count);                                 
                                }  
                            }
                        });
                    } else {
                        this.levels.filter(i => i.categoryID == that.categoryid).forEach(level => { 
                            var levelName = level.name.replace(/( \(empty\)$)/g, '');
                            var data = _alldata.filter(i => i.categoryID == that.categoryid && i.levelID == level.id);
                            var count = data.length;
                            
                            if (count > 0) {
                                var percent = Math.round((count / _alldata.length) * 100);
                                chartObj[levelName] = { count: count, tooltext: levelName + ', ' + count + ' runs (' + percent + '%)' };

                                if (that.subcategoryvariablevaluetabs && that.subcategoryvariablevaluetabs.filter(i => i.categoryID == that.categoryid && i.levelID == level.id).length > 0) {
                                    that.setVariableChartData(that.subcategoryvariablevaluetabs.filter(i => i.categoryID == that.categoryid && i.levelID == level.id), data, chartObj[levelName], levelName, count);                                 
                                } 
                            }                           
                        });                        
                    }

                    if (Object.keys(chartObj).length > 0) {
                        var chartDataObj = { label: 'All', value: _alldata.length, tooltext: 'All, ' + _alldata.length + ' runs (100%)', color: "#fff" };
                        this.setChartData(chartObj, chartDataObj);
                        dataset.push(chartDataObj);
                    }
                }

                var chartConfig = {
                    type: "multilevelpie",
                    renderAt: this.chartconainerid,
                    width: "100%",
                    height: this.ismodal ? "500" : "100%",
                    dataFormat: "json",
                    dataSource: {
                        chart: {
                            caption: this.caption,
                            captionFontSize: this.captionFontSize,                            
                            subCaption: this.subCaption,
                            subCaptionFontSize: this.subCaptionFontSize,
                            showValues: 0,
                            formatNumberScale: 0,
                            numberOfDecimals: 0,
                            showValuesInTooltip: 0,
                            showPercentValues: 1,
                            showPercentInTooltip: 0,
                            showToolTip: 1,
                            //plotTooltext: "$label, $value runs, $percentValue",
                            exportEnabled: 1,
                            legendItemFontSize: this.legendItemFontSize,
                            legendIconScale: this.legendIconScale,
                            showLabels: 1,
                            skipOverlapLabels: 1,
                            //autoRotateLabels: 1,
                            theme: "candy",
                            palettecolors: "36b5d8,f0dc46,f066ac,6ec85a,6e80ca,e09653,e1d7ad,61c8c8,ebe4f4,e64141,f2003e,00abfe,00e886,c7f600,9500f2,ff9a04,e200aa,a4cdfe,01b596,ecd86f",
                            bgColor: this.bgColor,
                            baseFontColor: this.fontColor,
                            outCnvBaseFontColor: this.fontColor,
                            hoverFillColor: "#fff",
                            valueFontColor: "#000",
                            valueFontSize: this.valueFontSize,
                            valueFontBold: 1
                        },
                        category: dataset
                    }
                };

                return chartConfig;
            },
            setVariableChartData(variables, tableData, chartObj, tooltext, prevcount, variableValueIDs) {
                var that = this;
                
                if (!variableValueIDs) {
                    variableValueIDs = '';
                }

                variables.forEach(variable => {
                    variable.variableValues.forEach(variableValue => {
                        var currVariableValueIDs = (variableValueIDs + ',' + variableValue.id).replace(/(^,)|(,$)/g, '');
                        var count = tableData.filter(i => variable.categoryID == i.categoryID && variable.levelID == i.levelID && i.subCategoryVariableValueIDs && i.subCategoryVariableValueIDs.startsWith(currVariableValueIDs)).length;
                        var percent = prevcount > 0 ? Math.round((count / prevcount) * 100) : 0;
                        var currtooltext = tooltext + ', ' + variableValue.name + ', ' + count + ' runs (' + percent + '%)';
                        chartObj[variableValue.name] = { count: count, tooltext: currtooltext };

                        if (variableValue.subVariables && variableValue.subVariables.length > 0) {
                            that.setVariableChartData(variableValue.subVariables, tableData, chartObj[variableValue.name], currtooltext, count, currVariableValueIDs);
                        }
                    });
                }); 
            },
            setChartData(chartObj, chartDataObj) {
                var that = this;
                var exclude = ["count", "tooltext"];
                              
                Object.keys(chartObj).filter(i => exclude.indexOf(i) == -1).forEach(key => {
                    chartDataObj["category"] = chartDataObj["category"] || [];
                    var chartDataItem = { label: key, value: chartObj[key].count, tooltext: chartObj[key].tooltext };
                    if (chartDataItem.value > 0){
                        chartDataObj["category"].push(chartDataItem);
                    }
                    
                    if (Object.keys(chartObj[key]).filter(i => exclude.indexOf(i) == -1).length > 0) {
                        that.setChartData(chartObj[key], chartDataItem);
                    }
                });
            }           
        }
    }
</script>








