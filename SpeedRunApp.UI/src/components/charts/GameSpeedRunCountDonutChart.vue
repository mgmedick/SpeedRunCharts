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
    import MultiLevelPie from 'fusioncharts/viz/multilevelpie';    
    import CandyTheme from "fusioncharts/themes/es/fusioncharts.theme.candy";
    FusionCharts.addDep(MultiLevelPie, CandyTheme);

    export default {
        name: "GameSpeedRunCountDonutChart",
        emits: ["onexpandchartclick"],
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
                width: document.documentElement.clientWidth,
                height: document.documentElement.clientHeight
            }
        },        
        computed: {   
            isMediaLarge: function () {
                return this.$el.clientWidth > 992;
            },    
            caption: function () {
                return (this.categorytypeid == 0 ? 'Category' : 'Level') + ' Category Runs Distribution';
            },                            
            captionFontSize: function () {
                return this.isMediaLarge ? 14 : 12;
            },              
            subCaption: function () {
                return this.subcaption;
            },                          
            subCaptionFontSize: function () {
                return this.isMediaLarge ? 12 : 10;
            },  
            labelFontSize: function () {
                return this.isMediaLarge ? 13 : 11;
            },                                
            yAxisValueFontSize: function () {
                return this.isMediaLarge ? 13 : 11;
            },      
            legendItemFontSize: function () {
                return this.isMediaLarge ? 12 : 10;
            },              
            valueFontSize: function () {
                return this.isMediaLarge ? 10 : 8;
            },                        
            legendIconScale: function () {
                return this.isMediaLarge ? .8 : .5;
            },                                                                    
            bgColor: function () {
                return document.body.classList.contains('theme-dark') ? "#303030" : "#f8f9fa";
            },
            fontColor: function () {
                return document.body.classList.contains('theme-dark') ? "#fff" : "#000";
            },
            autoRotateLabels: function () {
                return this.isMediaLarge ? 0 : 1;
            },
            paletteColors: function() {
                var colors = ['36b5d8','f0dc46','f066ac','6ec85a','6e80ca','e09653','e1d7ad','61c8c8','ebe4f4','e60049','0bb4ff','50e991','ffee00','9b19f5','ffa300','dc0ab4','b3d4ff','00bfa0','fd7f6f','7eb0d5','b2e061','bd7ebe','ffb55a','fff6b3','beb9db','fdcce5','8bd3c7','3366cc','dc3912','ff9900','109618','990099','0099c6','dd4477','b9d2d5','efd39e','efa7a7','bbf2d5','7db8b9','ffc197'];
                return colors;
            }                                           
        },            
        mounted: function () {
            this.loadChart();
            window.addEventListener('resize', this.resizeChart); 
        },        
        methods: {                      
            loadChart() {
                var that = this;
                this.loading = true;
                FusionCharts.ready(function () {
                    new FusionCharts(that.initChart()).render(); 
                    // that.width = document.documentElement.clientWidth;
                    // that.height = document.documentElement.clientHeight;                   
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
                    var allDataLabel = '';
                
                    if (this.categorytypeid == 0) {
                        this.categories.filter(i => i.categoryTypeID == that.categorytypeid).forEach(category => {
                            var categoryName = category.name.replace(/( \(empty\)$)/g, '');
                            var data = _alldata.filter(i => i.categoryID == category.id);
                            var count = data.length;

                            if (count > 0) {
                                chartObj[categoryName] = { count: count };

                                if (that.subcategoryvariablevaluetabs && that.subcategoryvariablevaluetabs.filter(i => i.categoryID == category.id).length > 0) {
                                    that.setVariableChartData(that.subcategoryvariablevaluetabs.filter(i => i.categoryID == category.id), data, chartObj[categoryName]);                                 
                                }  
                            }
                        });
                    } else {
                        this.levels.filter(i => i.categoryID == that.categoryid).forEach(level => { 
                            var levelName = level.name.replace(/( \(empty\)$)/g, '');
                            var data = _alldata.filter(i => i.categoryID == that.categoryid && i.levelID == level.id);
                            var count = data.length;
                            
                            if (count > 0) {
                                chartObj[levelName] = { count: count };

                                if (that.subcategoryvariablevaluetabs && that.subcategoryvariablevaluetabs.filter(i => i.categoryID == that.categoryid && i.levelID == level.id).length > 0) {
                                    that.setVariableChartData(that.subcategoryvariablevaluetabs.filter(i => i.categoryID == that.categoryid && i.levelID == level.id), data, chartObj[levelName]);                                 
                                } 
                            }                           
                        });                        
                    }

                    if (Object.keys(chartObj).length > 0) {
                        var allLabel = this.categorytypeid == 0 ? 'All full game' : 'All level';
                        var chartDataObj = { label: 'All', value: _alldata.length, tooltext: allLabel + ' ' + _alldata.length + ' runs (100%)', color: "#fff", alpha: 0 };
                        this.setChartData(chartObj, chartDataObj, _alldata.length);
                        dataset.push(chartDataObj);
                    }
                }

                var chartConfig = {
                    type: "multilevelpie",
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
                            subCaption: this.subCaption,
                            subCaptionFontSize: this.subCaptionFontSize,
                            subCaptionFontColor: "#888",
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
                            legendItemFontColor: this.fontColor,
                            showLabels: 1,
                            skipOverlapLabels: 1,
                            useEllipsesWhenOverflow: 1,
                            autoRotateLabels: 0,
                            theme: "candy",                          
                            palettecolors: this.paletteColors.join(','),                            
                            bgColor: this.bgColor,
                            baseFontColor: this.fontColor,
                            outCnvBaseFontColor: this.fontColor,
                            hoverFillColor: "#fff",
                            valueFontColor: "#fff",
                            valueFontSize: this.valueFontSize,
                            //valueFontBold: 1,
                            textOutline: 1
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
            setChartData(chartObj, chartDataObj, prevcount, tooltext, colorIndex, index) {
                var that = this;

                if (!tooltext) {
                    tooltext = '';
                }

                if (!colorIndex){
                    colorIndex = 0
                }

                if (!index){
                    index = 0
                }

                Object.keys(chartObj).filter(i => i != "count").forEach(key => {
                    chartDataObj["category"] = chartDataObj["category"] || [];
                    var count = chartObj[key].count;
                    var percent = prevcount > 0 ? Math.round((count / prevcount) * 100) : 0;
                    var currtooltext = tooltext + ', ' + key;
                    var currtooltextwithcount = (currtooltext  + ', ' + count + ' runs (' + percent + '%)').replace(/(^, )|(, $)/g, '')
                    var color = that.paletteColors[colorIndex];
                    var chartDataItem = { label: key, value: chartObj[key].count, tooltext: currtooltextwithcount, color: color };
                    if (chartDataItem.value > 0){
                        chartDataObj["category"].push(chartDataItem);
                    }
                    
                    if (Object.keys(chartObj[key]).filter(i => i != "count").length > 0) {
                        that.setChartData(chartObj[key], chartDataItem, count, currtooltext, colorIndex, index + 1);
                    }

                    if (index == 0) {
                        colorIndex = (colorIndex == (that.paletteColors.length -1)) ? 0 : colorIndex + 1;
                    }                    
                });
            },
            resizeChart() {
                var that = this;
                if (that.width != document.documentElement.clientWidth || that.height != document.documentElement.clientHeight) {     
                    that.width = document.documentElement.clientWidth;
                    that.height = document.documentElement.clientHeight;             
                    that.loadChart();
                }                
            }           
        }
    }
</script>








