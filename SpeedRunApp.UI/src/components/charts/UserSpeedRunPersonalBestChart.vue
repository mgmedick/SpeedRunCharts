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
        name: "UserSpeedRunPersonalBestChart",
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
            caption: function () {
                return 'Personal Bests';
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
            bgColor: function () {
                return document.body.classList.contains('theme-dark') ? "#303030" : "#f8f9fa";
            },
            fontColor: function () {
                return document.body.classList.contains('theme-dark') ? "#fff" : "#212529";
            },
            height: function () {
                return Math.floor(this.$el.clientWidth / 1.33);
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
                var labelStep = 0;

                if (this.tabledata?.length > 0) {
                    var _data = JSON.parse(JSON.stringify(this.tabledata)); 
                    if (this.istimerasc) {
                        _data = _data.sort((a, b) => { 
                            return b?.primaryTimeMilliseconds - a?.primaryTimeMilliseconds
                        });
                    } else {
                        _data = _data.sort((a, b) => { 
                            return a?.primaryTimeMilliseconds - b?.primaryTimeMilliseconds
                        });
                    }
                                        
                    var filteredData = [];
                    while (_data.length > 0) {
                        var item = _data[0];
                        filteredData.push(item);

                        if (this.istimerasc) {
                            _data = _data.filter(x => x.dateSubmitted < item.dateSubmitted)
                                        .sort((a, b) => { return b?.primaryTimeMilliseconds - a?.primaryTimeMilliseconds });
                        } else {
                            _data = _data.filter(x => x.dateSubmitted < item.dateSubmitted)
                                        .sort((a, b) => { return a?.primaryTimeMilliseconds - b?.primaryTimeMilliseconds });
                        }                       
                    }

                    var dates = filteredData.map(item => { return new Date(item.dateSubmitted) });
                    var maxDate = dayjs(Math.max.apply(null, dates)).startOf('day').toDate();//.add(1, "days").toDate();
                    var minDate = dayjs(Math.min.apply(null, dates)).startOf('day').toDate();
                    //var minDate = dayjs(maxDate).add(-24, "months");
                    //filteredData = filteredData.filter(x => { return new Date(x.dateSubmitted) >= minDate; });

                    var _timePeriods = getDateDiffList("day", minDate, maxDate).map(x => { return dayjs(x).format("MM/DD/YYYY") });

                    var groupedObj = {};
                    var chartDataObj = {};
                    var categoryObj = {};

                    filteredData.forEach(item => {
                        var monthDayYear = dayjs(item.dateSubmitted).format("MM/DD/YYYY")
                        var playerNames = item.players?.map(user => user.name).join("{br}");

                        groupedObj[monthDayYear] = groupedObj[monthDayYear] || [];
                        if(that.showmilliseconds){
                            groupedObj[monthDayYear].push({ primaryTime: item.primaryTimeMilliseconds, primaryTimeString: item.primaryTimeString, playerNames: playerNames });                        
                        } else {
                            groupedObj[monthDayYear].push({ primaryTime: item.primaryTimeSeconds, primaryTimeString: item.primaryTimeSecondsString, playerNames: playerNames });                        
                        }
                    });

                    if (Object.keys(groupedObj).length > 0) {
                        for (var key in groupedObj) {
                            chartDataObj[key] = chartDataObj[key] || [];

                            var minItem = {};
                            if (this.istimerasc) {
                                minItem = groupedObj[key].sort((a, b) => { return b?.primaryTime - a?.primaryTime })[0];
                            } else {
                                minItem = groupedObj[key].sort((a, b) => { return a?.primaryTime - b?.primaryTime })[0];
                            }

                            chartDataObj[key] = { value: minItem.primaryTime, tooltext: key + "{br}" + minItem.playerNames + "{br}" + minItem.primaryTimeString };                                                  
                        }

                        _timePeriods.forEach(timePeriod => {
                            if (!chartDataObj.hasOwnProperty(timePeriod)) {
                                chartDataObj[timePeriod] = { value: null, tooltext: ' ' };
                            }
                        });
                    }
                        
                    categoryObj["category"] = _timePeriods.map(item => {
                        var labelObj = {};
                        labelObj["label"] = item;
                        return labelObj;
                    });
                    categories.push(categoryObj);

                    var chartData = Object.entries(chartDataObj)?.map(x => ({ label: x[0], value: x[1]?.value, tooltext: x[1]?.tooltext }));                        
                                   
                    if (chartData.length > 0) {
                        chartData = chartData.sort((a, b) => {
                            var monthdayyeara = a.label.split("/");
                            var monthdayyearb = b.label.split("/");

                            return new Date(monthdayyeara[2], monthdayyeara[0] - 1, monthdayyeara[1]) - new Date(monthdayyearb[2], monthdayyearb[0] - 1, monthdayyearb[1])
                        });

                        ymax = chartData[0].value;
                        ymin = chartData[chartData.length - 1].value;    
                        var maxLabelCount = this.ismodal ? 20 : 10;
                        labelStep = Math.floor(chartData.length / maxLabelCount);                                         
                    }

                    dataset.push({ seriesname: '', data: chartData });
                }

                var chartConfig = {
                    type: this.istimerasc ? "inversemsline" : "msline",
                    renderAt: this.chartconainerid,
                    width: "100%",
                    height: "100%",
                    dataFormat: "json",
                    dataSource: {
                        chart: {
                            caption: this.caption,
                            captionFontSize: this.captionFontSize,                           
                            subCaption: this.title,
                            subCaptionFontSize: this.subCaptionFontSize,
                            captionAlignment:"center",
                            xAxis: 'Date',
                            yAxis: 'Time (Minutes)',
                            canvasTopPadding: 5,
                            canvasBottomPadding: 5,
                            canvasLeftPadding: 10,
                            canvasRightPadding: 10,
                            labelFontSize: this.labelFontSize,
                            showLabels: 1,
                            labelStep: labelStep,
                            rotateLabels: 1,
                            slantLabels: 1,
                            showToolTip: 1,
                            hoverOnEmpty: 0,
                            showLegend: 0,
                            lineThickness: 2,
                            anchorRadius: 5,
                            anchorBgColor: this.bgColor,
                            anchorBorderThickness: 1,                                                   
                            exportEnabled: 1,
                            showValues: 0,
                            formatNumberScale: 1,
                            numberOfDecimals: 0,
                            numberscalevalue: this.showmilliseconds ? "1000,60,60" : "60,60",                           
                            numberscaleunit: this.showmilliseconds ? "s,m,h" : "m,h",
                            defaultnumberscale: this.showmilliseconds ? "ms" : "s",
                            scalerecursively: "1",
                            maxscalerecursion: "-1",                            
                            scaleseparator: " ",
                            connectNullData: 1,
                            plotBinSize: 1.5,
                            //setAdaptiveYMin: 1,
                            yAxisMaxValue: ymax,
                            yAxisMinValue: ymin,
                            yAxisValueFontSize: this.yAxisValueFontSize,                        
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






