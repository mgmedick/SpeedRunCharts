<template>
    <div style="height:100%;">
        <div v-if="loading" class="d-flex" style="height:100%;">
            <div class="m-auto">
                <i class="fas fa-spinner fa-spin fa-lg"></i>
            </div>
        </div>
        <div :id="chartconainerid" @mouseover="onHover"></div>
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
            caption: function () {
                return (this.categorytypeid == 0 ? 'Category' : 'Level') + ' Trends (Last 12 months)';
            },                               
            captionFontSize: function () {
                return this.ismodal ? 14 : 12;
            },  
            subCaption: function () {
                return this.subcaption + '{br}(empties excluded)'
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
                return this.ismodal ? 11 : 11;
            },          
            legendIconScale: function () {
                return this.ismodal ? .8 : .5;
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
                var categoryObj = {};
                var timePeriods = [];
                var categories = [];
                var dataset = [];

                if (this.tabledata?.length > 0) {
                    var maxDate = new Date();
                    var minDate = dayjs(maxDate).add(-12, "months").toDate();
                    var timePeriods = getDateDiffList("month", minDate, maxDate).map(x => { return dayjs(x).format("MM/YYYY") });

                    categoryObj["category"] = timePeriods.map(item => {
                        var labelObj = {};
                        labelObj["label"] = item;
                        return labelObj;
                    });
                    categories.push(categoryObj);

                    var _alldata = JSON.parse(JSON.stringify(this.tabledata));

                    if (this.categorytypeid == 0) {
                        this.categories.filter(i => i.categoryTypeID == that.categorytypeid).forEach(category => {                                   
                            var categoryName = category.name.replace(/( \(empty\)$)/g, '');                            
                            var _data = _alldata.filter(i => i.categoryID == category.id).sort((a, b) => { 
                                return new Date(b.dateSubmitted) - new Date(a.dateSubmitted);
                            });   

                            if (_data.length > 0) {
                                var chartObj = {};
                                _data.forEach(item => {
                                    var monthYear = dayjs(item.dateSubmitted).format("MM/YYYY");

                                    var variableValueNames = '';
                                    if (item.subCategoryVariableValueIDs) {
                                        chartObj[monthYear] = chartObj[monthYear] || {};
                                        item.subCategoryVariableValueIDs.split(",").forEach(variableValueID => {
                                            var variable = that.variables?.find(x => x.variableValues.filter(i => i.id == variableValueID).length > 0);
                                            if (variable && variable.isSubCategory) {
                                                var variableValue = variable.variableValues.find(i => i.id == variableValueID);
                                                if (variableValue) {
                                                    variableValueNames += ',' + variableValue.name;
                                                }
                                            }
                                        });
                                        variableValueNames = variableValueNames.replace(/(^,)|(,$)/g, '');
                                        chartObj[monthYear][variableValueNames] = (chartObj[monthYear][variableValueNames] ?? 0) + 1;
                                    } else {
                                        chartObj[monthYear] = (chartObj[monthYear] ?? 0) + 1;
                                    }
                                });

                                var chartDataObj = {};
                                Object.keys(chartObj).forEach(monthyear => {
                                    if (Object.keys(chartObj[monthyear]).length > 0) {
                                        var total = 0;
                                        var tooltiptext = '';

                                        Object.keys(chartObj[monthyear]).forEach(variableValueNames => {
                                            var count = chartObj[monthyear][variableValueNames];
                                            total += count;
                                            // tooltiptext += variableValueNames + ' (' + count + (count == 1 ? " run" : " runs") + ') + ';
                                            tooltiptext += count + ' (' + variableValueNames + ') + ';
                                        });
                                        tooltiptext = tooltiptext.replace(/(^ \+ )|( \+ $)/g, '');
                                        chartDataObj[monthyear] = { value: total, tooltext: categoryName + ', ' + monthyear + ', ' + total + (total == 1 ? ' run' : ' runs') + ' = ' + tooltiptext }                            
                                    } else {
                                        var total = chartObj[monthyear];
                                        chartDataObj[monthyear] = { value: total, tooltext: categoryName + ', ' + monthyear + ', ' + total + (total == 1 ? ' run' : ' runs') };
                                    }
                                });   

                                timePeriods.forEach(timePeriod => {
                                    if (!chartDataObj.hasOwnProperty(timePeriod)) {
                                        chartDataObj[timePeriod] = { value: 0, tooltext: categoryName + ', ' + timePeriod + ', 0 runs' };
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
                        this.levels.forEach(level => {    
                            var levelName = level.name.replace(/( \(empty\)$)/g, '');                            
                            var _data = _alldata.filter(i => i.categoryID == that.categoryid && i.levelID == level.id).sort((a, b) => { 
                                return new Date(b.dateSubmitted) - new Date(a.dateSubmitted);
                            });   

                            if (_data.length > 0) {
                                var chartObj = {};
                                _data.forEach(item => {
                                    var monthYear = dayjs(item.dateSubmitted).format("MM/YYYY")

                                    var variableValueNames = '';
                                    if (item.subCategoryVariableValueIDs) {
                                        chartObj[monthYear] = chartObj[monthYear] || {};
                                        item.subCategoryVariableValueIDs.split(",").forEach(variableValueID => {
                                            var variable = that.variables?.find(x => x.variableValues.filter(i => i.id == variableValueID).length > 0);
                                            if (variable && variable.isSubCategory) {
                                                var variableValue = variable.variableValues.find(i => i.id == variableValueID);
                                                if (variableValue) {
                                                    variableValueNames += ',' + variableValue.name;
                                                }
                                            }
                                        });
                                        variableValueNames = variableValueNames.replace(/(^,)|(,$)/g, '');
                                        chartObj[monthYear][variableValueNames] = (chartObj[monthYear][variableValueNames] ?? 0) + 1;
                                    } else {
                                        chartObj[monthYear] = (chartObj[monthYear] ?? 0) + 1;
                                    }
                                });

                                var chartDataObj = {};
                                Object.keys(chartObj).forEach(monthyear => {
                                    if (Object.keys(chartObj[monthyear]).length > 0) {
                                        var total = 0;
                                        var tooltiptext = '';

                                        Object.keys(chartObj[monthyear]).forEach(variableValueNames => {
                                            var count = chartObj[monthyear][variableValueNames];
                                            total += count;
                                            // tooltiptext += variableValueNames + ' (' + count + (count == 1 ? " run" : " runs") + ') + ';
                                            tooltiptext += count + ' (' + variableValueNames + ') + ';
                                        });
                                        tooltiptext = tooltiptext.replace(/(^ \+ )|( \+ $)/g, '');
                                        chartDataObj[monthyear] = { value: total, tooltext: levelName + ', ' + monthyear + ', ' + total + (total == 1 ? ' run' : ' runs') + ' = ' + tooltiptext }  
                                    } else {
                                        var total = chartObj[monthyear];
                                        chartDataObj[monthyear] = { value: total, tooltext: levelName + ', ' + monthyear + ', ' + total + (total == 1 ? ' run' : ' runs') };
                                    }
                                });   

                                timePeriods.forEach(timePeriod => {
                                    if (!chartDataObj.hasOwnProperty(timePeriod)) {
                                        chartDataObj[timePeriod] = { value: 0, tooltext: levelName + ', ' + timePeriod + ', 0 runs' };
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
                    height: this.ismodal ? "500" : "100%",
                    dataFormat: "json",
                    dataSource: {
                        chart: {
                            caption: this.caption,
                            captionFontSize: this.captionFontSize,                           
                            subCaption: this.subCaption,
                            subCaptionFontSize: this.subCaptionFontSize,
                            captionAlignment:"center",
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
                            tooltipPosition: "top",
                            hoverOnEmpty: 0,
                            showLegend: 1,
                            interactiveLegend: 1,
                            plotHighlightEffect: 'fadeout|anchorBgColor=7f7f7f, alpha=5',
                            legendItemFontSize: this.legendItemFontSize,
                            legendIconScale: this.legendIconScale,
                            // legendNumRows: 3,
                            // legendNumColumns: 3,
                            // legendWidth: 400,
                            lineThickness: 2,
                            anchorRadius: 5,
                            anchorBgColor: this.bgColor,
                            anchorBorderThickness: 1,                                                   
                            exportEnabled: 1,
                            showValues: 0,
                            formatNumber: 0,
                            // enableChartMouseMoveEvent: 1,
                            // formatNumberScale: 1,
                            // numberOfDecimals: 0,
                            // numberscalevalue: this.showmilliseconds ? "1000,60,60" : "60,60",                           
                            // numberscaleunit: this.showmilliseconds ? "s,m,h" : "m,h",
                            // defaultnumberscale: this.showmilliseconds ? "ms" : "s",
                            // scalerecursively: "1",
                            // maxscalerecursion: "-1",                            
                            scaleseparator: " ",
                            connectNullData: 1,
                            //plotBinSize: 1.5,
                            //setAdaptiveYMin: 1,
                            yAxisValueFontSize: this.yAxisValueFontSize,                        
                            theme: "candy",
                            palettecolors: "36b5d8,f0dc46,f066ac,6ec85a,6e80ca,e09653,e1d7ad,61c8c8,ebe4f4,e64141,f2003e,00abfe,00e886,c7f600,9500f2,ff9a04,e200aa,a4cdfe,01b596,ecd86f",
                            bgColor: this.bgColor,
                            baseFontColor: this.fontColor,
                            outCnvBaseFontColor: this.fontColor
                        },
                        // tooltip: {
                        //     enabled: 1,
                        //     style: {
                        //         text: {
                        //             fontsize: 8
                        //         }
                        //     },
                        // },
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
            }
        }
    }
</script>






