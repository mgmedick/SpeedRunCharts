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
    const dayjs = require('dayjs');
    import { getDateDiffList } from '../../js/common.js';
    import FusionCharts from 'fusioncharts/core';
    import MSLine from 'fusioncharts/viz/msline';    
    import InverseMSLine from 'fusioncharts/viz/inversemsline';    
    import CandyTheme from "fusioncharts/themes/es/fusioncharts.theme.candy";
    FusionCharts.addDep(MSLine, InverseMSLine, CandyTheme);

    export default {
        name: "GameSpeedRunCountLineChart",
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
                loading: true
            }
        },        
        computed: {    
            isMediaLarge: function () {
                return this.$el.clientWidth > 992;
            },              
            chartType: function () {
                var that = this;
                var filteredCategories = this.categories.filter(i => i.categoryTypeID == that.categorytypeid);
                return filteredCategories.filter(i => i.isTimerAsc).length == filteredCategories.length ? 'inversemsline' : 'msline'
            },   
            caption: function () {
                return (this.categorytypeid == 0 ? 'Category' : 'Level') + ' Counts (Last 12 months)';
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
            legendIconScale: function () {
                return this.isMediaLarge ? .8 : .5;
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
                var categoryObj = {};
                var timePeriods = [];
                var categories = [];
                var dataset = [];

                if (this.tabledata?.length > 0) {
                    var _alldates = JSON.parse(JSON.stringify(this.tabledata)).sort((a, b) => { 
                                return new Date(b.dateSubmitted) - new Date(a.dateSubmitted);
                            }).map(i => new Date(i.dateSubmitted));

                    //var maxDate = dayjs(new Date()).add(-1, "months").toDate();
                    var maxDate = new Date();                    
                    var minDataDate = new Date(Math.min.apply(null, _alldates));
                    var minDate = dayjs(maxDate).add(-12, "months").toDate();
                    minDate = minDataDate > minDate ? minDataDate : minDate;
                    var timePeriods = getDateDiffList("month", minDate, maxDate).map(x => { return dayjs(x).format("MM/YYYY") });

                    categoryObj["category"] = timePeriods.map(item => {
                        var labelObj = {};
                        labelObj["label"] = item;
                        return labelObj;
                    });
                    categories.push(categoryObj);

                    var _alldata = JSON.parse(JSON.stringify(this.tabledata)).filter(i => new Date(i.dateSubmitted)>= minDate)

                    if (this.categorytypeid == 0) {
                        this.categories.filter(i => i.categoryTypeID == that.categorytypeid).forEach(category => {                                   
                            var categoryName = category.name.replace(/( \(empty\)$)/g, '');                            
                            var _data = _alldata.filter(i => i.categoryID == category.id).sort((a, b) => { 
                                return new Date(b.dateSubmitted) - new Date(a.dateSubmitted);
                            });   

                            if (_data.length > 0) {
                                var chartObj = that.getChartObj(_data);
                                var chartDataObj = that.getChartData(chartObj, categoryName);

                                timePeriods.forEach(timePeriod => {
                                    if (!chartDataObj.hasOwnProperty(timePeriod)) {
                                        chartDataObj[timePeriod] = { value: 0, tooltext: timePeriod + ', ' + categoryName + ', ' + ', 0 runs' };
                                    }
                                });

                                var chartData = Object.entries(chartDataObj)?.map(x => ({ label: x[0], value: x[1]?.value, tooltext: x[1]?.tooltext }));                        
                                            
                                if (chartData.length > 0) {
                                    chartData = chartData.sort((a, b) => {
                                        var monthyeara = a.label.split("/");
                                        var monthyearb = b.label.split("/");

                                        return new Date(monthyeara[1], 1, monthyeara[0]) - new Date(monthyearb[1], 1, monthyearb[0])
                                    });                                       
                                }

                                dataset.push({ seriesname: categoryName, data: chartData });
                            }
                        });
                    } else {
                        this.levels.filter(i => i.categoryID == that.categoryid).forEach(level => {    
                            var levelName = level.name.replace(/( \(empty\)$)/g, '');                            
                            var _data = _alldata.filter(i => i.categoryID == that.categoryid && i.levelID == level.id).sort((a, b) => { 
                                return new Date(b.dateSubmitted) - new Date(a.dateSubmitted);
                            });   

                            if (_data.length > 0) {
                                var chartObj = that.getChartObj(_data);
                                var chartDataObj = that.getChartData(chartObj, levelName);
                                
                                timePeriods.forEach(timePeriod => {
                                    if (!chartDataObj.hasOwnProperty(timePeriod)) {
                                        chartDataObj[timePeriod] = { value: 0, tooltext: timePeriod + ', ' + levelName + ', ' + ', 0 runs' };
                                    }
                                });

                                var chartData = Object.entries(chartDataObj)?.map(x => ({ label: x[0], value: x[1]?.value, tooltext: x[1]?.tooltext }));                        
                                            
                                if (chartData.length > 0) {
                                    chartData = chartData.sort((a, b) => {
                                        var monthyeara = a.label.split("/");
                                        var monthyearb = b.label.split("/");

                                        return new Date(monthyeara[1], 1, monthyeara[0]) - new Date(monthyearb[1], 1, monthyearb[0])
                                    });                                       
                                }

                                dataset.push({ seriesname: levelName, data: chartData });
                            }
                        });
                    }
                }

                var chartConfig = {
                    type: this.chartType,
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
                            xAxis: 'Date',
                            yAxis: 'Runs',
                            canvasTopPadding: 5,
                            canvasBottomPadding: 5,
                            canvasLeftPadding: 10,
                            canvasRightPadding: 10,
                            labelFontSize: this.labelFontSize,
                            showLabels: 1,
                            rotateLabels: 1,
                            slantLabels: 1,
                            showToolTip: 1,
                            hoverOnEmpty: 0,
                            showLegend: 1,
                            plotHighlightEffect: 'fadeout|anchorBgColor=7f7f7f, alpha=5',
                            legendItemFontSize: this.legendItemFontSize,
                            legendIconScale: this.legendIconScale,
                            legendItemFontColor: this.fontColor,
                            legendNumRows: 3,
                            legendNumColumns: 4,
                            alignLegendWithCanvas: 1,
                            lineThickness: 2,
                            anchorRadius: 5,
                            anchorBgColor: this.bgColor,
                            anchorBgAlpha: 0,
                            anchorBorderThickness: 1,                                                   
                            exportEnabled: 1,
                            showValues: 0,
                            formatNumber: 0,                        
                            scaleseparator: " ",
                            connectNullData: 1,
                            yAxisValueFontSize: this.yAxisValueFontSize,                        
                            theme: "candy",
                            palettecolors: this.paletteColors.join(','),                                                       
                            bgColor: this.bgColor,
                            baseFontColor: this.fontColor,
                            outCnvBaseFontColor: this.fontColor
                        },
                        categories: categories,
                        dataset: dataset
                    },
                    events: {
                        legendItemClicked: function (eventObj, dataObj) {
                            console.log('clicked')
                        }
                    }
                };

                return chartConfig;
            },
            getChartObj(data) {
                var that = this;
                var chartObj = {};

                data.forEach(item => {
                    var monthYear = dayjs(item.dateSubmitted).format("MM/YYYY");
                    chartObj[monthYear] = chartObj[monthYear] || {};

                    var variableValueNames = '';
                    if (item.subCategoryVariableValueIDs) {
                        item.subCategoryVariableValueIDs.split(",").forEach(variableValueID => {
                            var variable = that.variables?.find(x => x.variableValues.filter(i => i.id == variableValueID).length > 0);
                            if (variable && variable.isSubCategory) {
                                var variableValue = variable.variableValues.find(i => i.id == variableValueID);
                                if (variableValue) {
                                    variableValueNames += ', ' + variableValue.name;
                                }
                            }
                        });
                        variableValueNames = variableValueNames.replace(/(^, )|(, $)/g, '');
                        chartObj[monthYear][variableValueNames] = { count: (chartObj[monthYear][variableValueNames]?.count ?? 0) + 1 };
                    } else {
                        chartObj[monthYear] = { count: (chartObj[monthYear]?.count ?? 0) + 1 };
                    }
                });
                
                return chartObj;
            },
            getChartData(chartObj, seriesName) {
                var chartDataObj = {};
                
                Object.keys(chartObj).forEach(monthyear => {
                    if (Object.keys(chartObj[monthyear]).filter(i => i != "count").length > 0) {
                        var total = 0;
                        var tooltiptext = '';

                        Object.keys(chartObj[monthyear]).filter(i => i != "count").forEach(variableValueNames => {
                            var count = chartObj[monthyear][variableValueNames].count;
                            total += count;
                            tooltiptext += count + ' (' + variableValueNames + ') + ';
                        });
                        tooltiptext = tooltiptext.replace(/(^ \+ )|( \+ $)/g, '');
                        chartDataObj[monthyear] = { value: total, tooltext: monthyear + ', ' + seriesName + ', ' + total + (total == 1 ? ' run' : ' runs') + ' = ' + tooltiptext }                            
                    } else {
                        var total = chartObj[monthyear].count;
                        chartDataObj[monthyear] = { value: total, tooltext: monthyear + ', ' + seriesName + ', ' + total + (total == 1 ? ' run' : ' runs') };
                    }
                });   

                return chartDataObj;
            }             
        }
    }
</script>






