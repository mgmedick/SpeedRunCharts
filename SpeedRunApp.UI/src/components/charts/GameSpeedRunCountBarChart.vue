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
    import StackedColumn2D from 'fusioncharts/viz/stackedcolumn2d';
    import CandyTheme from "fusioncharts/themes/es/fusioncharts.theme.candy";
    FusionCharts.addDep(StackedColumn2D, CandyTheme);

    export default {
        name: "GameSpeedRunCountBarChart",
        emits: ["onexpandchartclick"],
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
            isMediaLarge: function () {
                return this.$el.clientWidth > 992;
            },                
            caption: function () {
                return (this.categorytypeid == 0 ? 'Category' : 'Level') + ' Counts';
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
                return this.isMediaLarge ? 12 : 10;
            },                    
            outCnvBaseFontSize: function () {
                return this.isMediaLarge ? 13 : 11;
            },      
            legendItemFontSize: function () {
                return this.isMediaLarge ? 12 : 10;
            },
            valueFontSize: function () {
                return this.isMediaLarge ? 12 : 10;
            },                        
            legendIconScale: function () {
                return this.isMediaLarge ? .8 : .5;
            },                
            legendNumRows: function () {
                return this.isMediaLarge ? 5 : 2;
            },   
            legendNumColumns: function () {
                return this.isMediaLarge ? 5 : 2;
            },                                                                                                          
            bgColor: function () {
                return document.body.classList.contains('theme-dark') ? "#303030" : "#f8f9fa";
            },
            fontColor: function () {
                return document.body.classList.contains('theme-dark') ? "#fff" : "#212529";
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
                var dataset = [];
                var chartObj = {};
                var categories = [];

                if (this.tabledata?.length > 0) {
                    var _alldata = JSON.parse(JSON.stringify(this.tabledata));

                    if (this.categorytypeid == 0) {
                        this.categories.filter(i => i.categoryTypeID == that.categorytypeid).forEach(category => {
                            var categoryName = category.name.replace(/( \(empty\)$)/g, '');                            
                            var _data = _alldata.filter(i => i.categoryID == category.id);   

                            if (_data.length > 0) {
                                that.setChartObj(_data, chartObj, categoryName);
                            } 
                        });
                    } else {
                        this.levels.filter(i => i.categoryID == that.categoryid).forEach(level => {    
                            var levelName = level.name.replace(/( \(empty\)$)/g, ''); 
                            var _data = _alldata.filter(i => i.categoryID == that.categoryid && i.levelID == level.id);

                            if (_data.length > 0) {
                                that.setChartObj(_data, chartObj, levelName);
                            } 
                        });                      
                    }

                    if (Object.keys(chartObj).length > 0) {
                        var chartDataObj = that.getChartData(chartObj);

                        var categoryObj = {};
                        categoryObj["category"] = Object.keys(chartObj).filter(i =>i != "count").map(key => {
                            var labelObj = {};
                            labelObj["label"] = key;
                            return labelObj;
                        });                    
                        categories.push(categoryObj);

                        Object.keys(chartDataObj).forEach(key => {
                            dataset.push({seriesname: key, data: chartDataObj[key] })
                        });  
                    }                    
                }

                var chartConfig = {
                    type: "stackedcolumn2d",
                    renderAt: this.chartconainerid,
                    width: "100%",
                    height: "100%",
                    dataFormat: "json",
                    dataSource: {
                        chart: {
                            caption: this.caption,
                            captionFontSize: this.captionFontSize,   
                            captionAlignment:"center",
                            alignCaptionWithCanvas: 0,                                                     
                            subCaption: this.subCaption,
                            subCaptionFontSize: this.subCaptionFontSize,
                            subCaptionFontColor: "#888",
                            xAxis: '',
                            yAxis: 'Total Runs',  
                            showZeroPlaneValue: 0,
                            yAxisMinValue: 0,                                                       
                            labelFontSize: this.labelFontSize,
                            labelVAlign: 'middle',
                            //labelDisplay: 'ROTATE',
                            rotateLabels: dataset.length > 10 ? 1: 0,
                            slantLabels: dataset.length > 10 ? 1: 0,                        
                            exportEnabled: 1,
                            showValues: 1,
                            //plotTooltext: "$label, $seriesName, $value runs",
                            valueFontSize: this.valueFontSize,
                            outCnvBaseFontSize: this.outCnvBaseFontSize,                                                      
                            numberOfDecimals: 0,
                            useRoundEdges: 0,
                            useEllipsesWhenOverflow: 1,
                            showLegend: 0,
                            legendItemFontSize: this.legendItemFontSize,
                            legendIconScale: this.legendIconScale,
                            palettecolors: this.paletteColors.join(','),                                                   
                            theme: "candy",
                            bgColor: this.bgColor,
                            valueFontColor: "#212529",
                            baseFontColor: this.fontColor,
                            outCnvBaseFontColor: this.fontColor                            
                        },
                        categories: categories,
                        dataset: dataset
                    }
                };

                return chartConfig;
            },
            setChartObj(data, chartObj, seriesName) {
                var that = this;

                data.forEach(item => {
                    chartObj[seriesName] = chartObj[seriesName] || {};
                    chartObj[seriesName] = { count: (chartObj[seriesName]?.count ?? 0) + 1 };

                    // var variableValueNames = '';
                    // if (item.subCategoryVariableValueIDs) {
                    //     item.subCategoryVariableValueIDs.split(",").forEach(variableValueID => {
                    //         var variable = that.variables?.find(x => x.variableValues.filter(i => i.id == variableValueID).length > 0);
                    //         if (variable && variable.isSubCategory) {
                    //             var variableValue = variable.variableValues.find(i => i.id == variableValueID);
                    //             if (variableValue) {
                    //                 variableValueNames += ', ' + variableValue.name;
                    //             }
                    //         }
                    //     });
                    //     variableValueNames = variableValueNames.replace(/(^, )|(, $)/g, '');
                    //     chartObj[seriesName][variableValueNames] = { count: (chartObj[seriesName][variableValueNames]?.count ?? 0) + 1 };
                    // } else {
                    //     chartObj[seriesName] = { count: (chartObj[seriesName]?.count ?? 0) + 1 };
                    // }
                });
                
                return chartObj;
            },
            getChartData(chartObj) {
                var chartDataObj = {};

                var allVariableValueNames = []
                Object.keys(chartObj).filter(i => i != "count").forEach(seriesName => {
                    Object.keys(chartObj[seriesName]).forEach(variableValueName => {
                        if(allVariableValueNames.indexOf(variableValueName) == -1){
                            allVariableValueNames.push(variableValueName);
                        }
                    })
                });

                var fullCategoryKeys = [];
                Object.keys(chartObj).filter(i => i != "count").forEach(seriesName => {
                    if (Object.keys(chartObj[seriesName]).filter(i => i != "count").length > 0) {
                        Object.keys(chartObj[seriesName]).filter(i => i != "count").forEach(variableValueNames => {
                            var total = chartObj[seriesName][variableValueNames].count;
                            chartDataObj[variableValueNames] = chartDataObj[variableValueNames] || [];
                            chartDataObj[variableValueNames].push({ value: total, tooltext: variableValueNames + ', ' + total + (total == 1 ? ' run' : ' runs') });
                        });
                    } else {
                        fullCategoryKeys.push(seriesName);
                        var total = chartObj[seriesName].count;
                        chartDataObj[seriesName] = chartDataObj[seriesName] || [];
                        chartDataObj[seriesName].push({ value: total, tooltext: seriesName + ', ' + total + (total == 1 ? ' run' : ' runs') });
                    }

                    allVariableValueNames.forEach(variableValueName => {
                        if (Object.keys(chartObj[seriesName]).filter(i => i == variableValueName).length == 0) {
                            chartDataObj[variableValueName] = chartDataObj[variableValueName] || [];
                            chartDataObj[variableValueName].push({ value: null, tooltext: ' ' });
                        }
                    });                    
                });

                fullCategoryKeys.forEach(seriesName => {
                    Object.keys(chartObj).filter(i =>i != "count").forEach((seriesName1, index) => {
                        if (seriesName != seriesName1) {
                            chartDataObj[seriesName].splice(index, 0, { value: null, tooltext: ' ' }); 
                        }
                    })
                });
                
                return chartDataObj;
            }            
        }
    }
</script>








