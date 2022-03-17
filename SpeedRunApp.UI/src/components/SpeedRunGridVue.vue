<template>
    <div>      
        <div v-if="loading">
            <div class="d-flex">
                <div class="mx-auto">
                    <i class="fas fa-spinner fa-spin fa-lg"></i>
                </div>
            </div>
        </div>        
        <div class="mt-2 mx-0 grid-container container-lg p-0" style="min-height:150px;">
            <speedrun-grid-chart v-if="!loading" :isgame="!userid" :showcharts="showcharts" :gameid="gameid" :categorytypeid="categorytypeid" :categoryid="categoryid" :levelid="levelid" :variablevalues="variablevalues"  @onshowchartsclick="$emit('onshowchartsclick1', $event)"></speedrun-grid-chart>
            <div id="tblGrid" ref="table"></div>
        </div>
        <custom-modal v-model="showDetailModal" v-if="showDetailModal" contentclass="modal-lg">
            <template v-slot:title>
                Details
            </template>
            <speedrun-edit :gameid="gameid" :speedrunid="speedRunID" :readonly="true" />
        </custom-modal>    
    </div>   
</template>
<script>
    const dayjs = require('dayjs');
    import axios from 'axios';    
    import { getIntOrdinalString, escapeHtml } from '../js/common.js';
    import Tabulator from 'tabulator-tables';
    import 'tabulator-tables/dist/css/bootstrap/tabulator_bootstrap.min.css'
    import tippy from 'tippy.js'
    import 'tippy.js/dist/tippy.css'

    export default {
        name: "SpeedRunGridVue",
        emits: ["onshowchartsclick1"],
        props: {
            gameid: String,
            categorytypeid: String,
            categoryid: String,
            levelid: String,
            variablevalues: String,
            userid: String,
            showalldata: Boolean,
            showcharts: Boolean,
            variables: Array
        },
        data() {
            return {
                table: {},
                tableData: [],
                loading: true,
                speedRunID: String,
                showDetailModal: false
            }
        },
        computed: {
            isMediaMedium: function () {
                return window.innerWidth > 768;
            }
        },
        watch: {
            showalldata: function (val, oldVal) {
                this.loadData(true);
            }            
        },
        mounted: function() {
            this.loadData();
            window.speedRunGridVue = this;
        },
        methods: {
            loadData(isReload) {
                var that = this;
                if (isReload) {
                    this.$refs.table.classList.add("d-none");
                }
                this.loading = true;

                axios.get('/SpeedRun/GetSpeedRunGridData', { params: { gameID: this.gameid, categoryID: this.categoryid, levelID: this.levelid, subCategoryVariableValueIDs: this.variablevalues, userID: this.userid, showAllData: this.showalldata } })
                    .then(res => {
                        that.tableData = res.data;
                        if (isReload) {
                            that.$refs.table.classList.remove("d-none"); 
                        }                                                
                        that.initGrid(res.data);  
                        that.loading = false;  
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            },                                                                              
            initGrid(tableData) {
                var that = this;
                var players = [...new Set(tableData.flatMap(el => el.players?.map(el1 => el1.name)))].sort((a, b) => { return a?.toLowerCase().localeCompare(b?.toLowerCase()) });
                
                var columns = [
                    { title: "", field: "id", formatter: that.optionsFormatter, hozAlign: "center", headerSort: false, width:50, widthShrink:2 }, //, minWidth:30, maxWidth:50
                    { title: "Rank", field: "rank", sorter: "number", formatter: that.rankFormatter, headerFilter: "select", headerFilterParams: { values: true, multiselect: true }, headerFilterFunc: that.rankHeaderFilter, width: 75 }, //minWidth:40, maxWidth:75
                    { title: "Players", field: "players", sorter:that.playerSorter, formatter: that.playerFormatter, headerFilter:"select", headerFilterParams:{ values:players, multiselect:true }, headerFilterFunc: that.playerHeaderFilter, minWidth:135, widthGrow:2 }, //minWidth:125
                    { title: "Time", field: "primaryTime.ticks", formatter: that.primaryTimeFormatter, sorter: "number", width: 125 }, //minWidth:100, maxWidth:125
                    { title: "Platform", field: "platformName", headerFilter:"select", headerFilterParams:{ values:true, multiselect:true }, headerFilterFunc:"in", minWidth:100, widthGrow:1 }, //minWidth:100
                    { title: "Submitted Date", field: "dateSubmitted", formatter: that.dateFormatter, formatterParams:{ outputFormat:"MM/DD/YYYY", tooltipFieldName:"relativeDateSubmittedString" }, headerFilter: that.dateEditor, headerFilterFunc: that.dateHeaderFilter, minWidth:150 }, //minWidth:140, maxWidth:170
                    { title: "Verified Date", field: "verifyDate", formatter:that.dateFormatter, formatterParams:{ outputFormat:"MM/DD/YYYY", tooltipFieldName:"relativeVerifyDateString" }, headerFilter: that.dateEditor, headerFilterFunc: that.dateHeaderFilter, minWidth:150 }, //minWidth:140, maxWidth:170
                    { title: "primaryTimeString", field: "primaryTimeString", visible: false },
                    { title: "relativeDateSubmittedString", field: "relativeDateSubmittedString", visible: false },
                    { title: "relativeVerifyDateString", field: "relativeVerifyDateString", visible: false }
                ];

                tableData.forEach(item => {
                    if (item.variableValues) {
                        Object.keys(item.variableValues).forEach(variableID => {
                            var variable = that.variables?.filter(x => x.id == variableID)[0];
                            if (variable && !variable.isSubCategory) {
                                var variableValue = variable.variableValues?.filter(i => i.id == item.variableValues[variableID])[0]
                                if (variableValue) {
                                    item[variableID] = variableValue.name;
                                }
                            }
                        })
                    }
                });
                                
                var variables = that.variables?.filter(i => tableData.filter(el => el[i.id]).length > 0);
                variables?.forEach(variable => { 
                    columns.push({ title: variable.name, field: variable.id.toString(), headerFilter:"select", headerFilterParams:{ values:true, multiselect:true }, headerFilterFunc:"in", minWidth:140, widthGrow:1 },)
                });

                columns.push({ title: "", field: "comment", formatter: that.commentFormatter, hozAlign: "center", headerSort: false, width: 50, widthShrink:2 });

                this.table = new Tabulator(that.$refs.table, {
                    data: tableData,
                    layout: "fitColumns",
                    reactiveData:true,
                    //responsiveLayout: false,
                    tooltips: false,
                    tooltipsHeader:false,
                    pagination: "local",
                    paginationSize: 100,
                    movableColumns: this.isMediaMedium,
                    resizableColumns: this.isMediaMedium ? "header" : false,
                    //resizableRows: false,
                    initialSort: [             //set the initial sort order of the data
                        { column: "primaryTime.ticks", dir: "asc" },
                    ],
                    columns: columns,
                    renderComplete:function() {
                        Array.from(that.$el.querySelectorAll('.tippy-tooltip')).forEach(el => {
                            var value = el.getAttribute('data-content');
                            var cellElement = el.closest('.tabulator-cell');

                            tippy(cellElement, {
                                content: escapeHtml(value),
                                allowHTML: true,
                                arrow:false,
                                placement:'bottom'//,
                                //maxWidth:"200px"
                            })
                        });
                    },
                });
            },
            optionsFormatter(cell, formatterParams, onRendered) {
                var value = cell.getValue();

                var html = "<div>"
                html += "<div class='d-table' style='border:none; border-collapse:collapse; border-spacing:0; margin:auto;'>";
                html += "<div class='d-table-row'>";
                html += "<div class='d-table-cell pl-1 ' style='border:none; padding:0px; width:30px;'>";
                html += "<a href=\"javascript:window.speedRunGridVue.showSpeedRunDetails('" + value + "');\"><i class='fas fa-play-circle fa-lg'></i></a>";
                html += "</div>";
                html += "</div>";
                html += "</div>";
                html += "</div>";

                return html;
            },
            rankFormatter(cell, formatterParams, onRendered) {
                var html = '';
                var value = cell.getValue();

                var num = parseInt(value);
                var html = (num) ? getIntOrdinalString(num) : '-';

                return html;
            },
            playerFormatter(cell, formatterParams, onRendered) {
                var value = cell.getValue();
                var valueString = value?.map(el => el.name).join();
                var html = valueString ? '<span class="tippy-tooltip" data-content="' + escapeHtml(valueString) + '">' : '<span>'

                value?.forEach(el => {
                    if (el.id > 0) {
                        html += "<a href='/User/UserDetails/" + el.abbr + "'>" + el.name + "</a><br/>";
                    } else {
                        html += el.name;
                    }          
                });

                html += '</span>'

                return html;
            },
            primaryTimeFormatter(cell, formatterParams, onRendered) {
                var html = '';
                var value = cell.getRow().getCell("primaryTimeString").getValue();

                if (value) {
                    html += value
                }

                return html;
            },
            dateFormatter(cell, formatterParams, onRendered) {
                var tooltip = formatterParams.tooltipFieldName ? cell.getRow().getCell(formatterParams.tooltipFieldName).getValue() : '';
                var html = tooltip ? '<span class="tippy-tooltip" data-content="' + escapeHtml(tooltip) + '">' : '<span>'
                var value = cell.getValue();
                var formatString = formatterParams.outputFormat;

                if(value){
                    html += dayjs(value).format(formatString);
                }

                html+='</span>'

                return html;
            },
            commentFormatter(cell, formatterParams, onRendered) {
                var html = '';
                var value = cell.getValue();

                if (value != null) {
                    html = '<i class="fas fa-comment tippy-tooltip" data-content="' + value + '"></i>'
                }

                return html;
            },                     
            dateSorter(a, b, aRow, bRow, column, dir, sorterParams){
                return new Date(a) - new Date(b);
            },            
            playerSorter(a, b, aRow, bRow, column, dir, sorterParams){
                return a[0]?.name.toLowerCase().localeCompare(b[0]?.name.toLowerCase());
            }, 
            dateEditor (cell, onRendered, success, cancel, editorParams){
                var editorDiv = document.createElement("div");   

                var editor = document.createElement("input");
                editor.setAttribute("type", "date");
                editor.style.width = "90%";
                editor.style.boxSizing = "border-box";
                editor.style.fontSize = "12px";

                editor.addEventListener("change", successFunc);
                editor.addEventListener("blur", successFunc);
                editor.addEventListener("keydown", onKeydown);

                var button = document.createElement("button");
                button.innerHTML = "x";
                button.classList.add("px-1");
                button.style.width = "10%;";
                button.style.border = "none";
                button.style.backgroundColor = "#303030";
                button.style.color = "#fff";
                                
                button.addEventListener("click", onClearClick);

                editorDiv.appendChild(editor);
                editorDiv.appendChild(button);

                function successFunc(el){
                    var dateString = '';
                    if(editor.value){
                        dateString = dayjs(editor.value).format("MM/DD/YYYY");
                    }

                    success(dateString);
                }

                function onKeydown (event) {
                    const key = event.key;
                    if (key === "Backspace" || key === "Delete") {
                        clearFunc(event.target);
                    }
                }

                function onClearClick(event){
                    clearFunc(event.target.previousSibling);
                }

                function clearFunc(el){
                    el.value='';
                    el.dispatchEvent(new Event('change'));
                }

                return editorDiv;
            },            
            rankHeaderFilter(headerValue, rowValue, rowData, filterParams){
                if(headerValue.length == 0){
                    return true;
                }

                return headerValue.indexOf(rowValue?.toString()) > -1 
            },                     
            playerHeaderFilter(headerValue, rowValue, rowData, filterParams){
                if(headerValue.length == 0){
                    return true;
                }

                return rowValue?.filter(el => { 
                    return headerValue.indexOf(el.name) > -1 
                    }).length > 0;
            },        
            dateHeaderFilter(headerValue, rowValue, rowData, filterParams){
                if(!headerValue){
                    return true;
                }
                
                var value = dayjs(rowValue).format("MM/DD/YYYY"); 

                return headerValue == value; 
            },                                 
            showSpeedRunDetails(id) {
                this.speedRunID = id;
                this.showDetailModal = true;
            }                              
        }
    };
</script>








