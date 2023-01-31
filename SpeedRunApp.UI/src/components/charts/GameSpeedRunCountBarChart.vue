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
    import StackedColumn2D from 'fusioncharts/viz/stackedcolumn2d';
    import CandyTheme from "fusioncharts/themes/es/fusioncharts.theme.candy";
    FusionCharts.addDep(StackedColumn2D, CandyTheme);

    export default {
        name: "GameSpeedRunCountBarChart",
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
                return this.ismodal ? 10 : 9;
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
                var categories = [];
                var allVariableValueNames = [];
                var ymax;

                if (this.tabledata?.length > 0) {
                    var _alldata = JSON.parse(JSON.stringify(this.tabledata));

                    var counts = [];
                    if (this.categorytypeid == 0) {
                        this.categories.filter(i => i.categoryTypeID == that.categorytypeid).forEach(category => {
                            var categoryName = category.name.replace(/( \(empty\)$)/g, '');
                            var data = _alldata.filter(i => i.categoryID == category.id);
                            counts.push(data.length);

                            if (data.length > 0) {
                                chartObj[categoryName] = { count: data.length };

                                if (that.subcategoryvariablevaluetabs && that.subcategoryvariablevaluetabs.filter(i => i.categoryID == category.id).length > 0) {
                                    that.setVariableChartData(that.subcategoryvariablevaluetabs.filter(i => i.categoryID == category.id), data, chartObj[categoryName], allVariableValueNames);                                 
                                }
                            }
                        });                   
                    } 

                    ymax = Math.max.apply(null, counts);

                    var categoryObj = {};
                    categoryObj["category"] = Object.keys(chartObj).filter(i => i != "count").map(key => {
                        var labelObj = {};
                        labelObj["label"] = key;
                        labelObj["showLabel"] = chartObj[key].count > 0 ? 1 : 0;
                        return labelObj;
                    });                    
                    categories.push(categoryObj);

                    var chartDataObj = {};
                    var fullCategoryKeys = [];
                    Object.keys(chartObj).filter(i => i != "count").forEach(key => {
                        if (Object.keys(chartObj[key]).filter(i => i != "count").length > 0) {
                            this.setChartData(chartObj[key], chartDataObj);
                        } else {
                            chartDataObj[key] = chartDataObj[key] || [];
                            var count = chartObj[key].count > 0 ? chartObj[key].count : null;
                            chartDataObj[key].push({ value: count, tooltext: key + ', ' + count });
                            fullCategoryKeys.push(key);
                        }

                        allVariableValueNames.forEach(variableValueName => {
                            if (!this.containsKey(chartObj[key], variableValueName)) {
                                chartDataObj[variableValueName] = chartDataObj[variableValueName] || [];
                                chartDataObj[variableValueName].push({ value: null });
                            }
                        });                       
                    });

                    fullCategoryKeys.forEach(key => {
                        Object.keys(chartObj).filter(i => i != "count").forEach((key1, index) => {
                            if (key != key1) {
                                chartDataObj[key].splice(index, 0, { value: null }); 
                            }
                        })
                    });

                    
                    Object.keys(chartDataObj).forEach(key => {
                        dataset.push({seriesname: key, data: chartDataObj[key] })
                    });                   
                }

                var chartConfig = {
                    type: "stackedcolumn2d",
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
                            xAxis: '',
                            yAxis: 'Total Runs',  
                            // yAxisMaxValue: ymax,
                            yAxisMinValue: 0,                                                       
                            labelFontSize: this.labelFontSize,
                            labelVAlign: 'middle',                            
                            exportEnabled: 1,
                            showValues: 1,
                            valueFontSize: this.valueFontSize,
                            numberOfDecimals: 0,
                            useRoundEdges: 0,
                            legendItemFontSize: this.labelFontSize,                        
                            theme: "candy",
                            bgColor: this.bgColor,
                            valueFontColor: "#000",
                            baseFontColor: this.fontColor,
                            outCnvBaseFontColor: this.fontColor                            
                        },
                        categories: categories,
                        dataset: dataset
                    }
                };

                return chartConfig;
            },
            setVariableChartData(variables, tableData, chartObj, allVariableValueNames, variableValueIDs) {
                var that = this;
                
                if (!variableValueIDs) {
                    variableValueIDs = '';
                }

                variables.forEach(variable => {
                    variable.variableValues.forEach(variableValue => {
                        var currVariableValueIDs = (variableValueIDs + ',' + variableValue.id).replace(/(^,)|(,$)/g, '');
                        var count = tableData.filter(i => variable.categoryID == i.categoryID && variable.levelID == i.levelID && i.subCategoryVariableValueIDs && i.subCategoryVariableValueIDs.startsWith(currVariableValueIDs)).length;
                        chartObj[variableValue.name] = { count: count };

                        if (allVariableValueNames.indexOf(variableValue.name) == -1){
                            allVariableValueNames.push(variableValue.name);
                        }

                        if (variableValue.subVariables && variableValue.subVariables.length > 0) {
                            that.setVariableChartData(variableValue.subVariables, tableData, chartObj[variableValue.name], allVariableValueNames, currVariableValueIDs);
                        }
                    });
                }); 
            },  
            containsKey(chartObj, targetKey) {
                var that = this;
                var result = false;

                for (var key in chartObj) {
                    if (key == targetKey) {
                        result = true;
                        break;
                    }

                    if (Object.keys(chartObj[key]).filter(i => i != "count").length > 0) {
                        result = that.containsKey(chartObj[key]);
                    }
                }

                return result;
            },                     
            setChartData(chartObj, chartDataObj) {
                var that = this;

                Object.keys(chartObj).filter(i => i != "count").forEach(key => {
                    chartDataObj[key] = chartDataObj[key] || [];
                    var chartDataItem = { value: chartObj[key].count };
                    chartDataObj[key].push(chartDataItem);

                    if (Object.keys(chartObj[key]).filter(i => i != "count").length > 0) {
                        that.setChartData(chartObj[key], chartDataObj);
                    }
                });
            }
        }
    }
</script>








