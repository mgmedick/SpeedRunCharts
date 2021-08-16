<template>
<div>
    <div v-if="loading">
        <div class="d-flex">
            <div class="mx-auto">
                <i class="fas fa-spinner fa-spin fa-lg"></i>
            </div>
        </div>
    </div>
    <div class="mt-2 grid-container" :style="[ loading ? { display: 'none'} : null ]">
        <div id="tblGrid" class="mt-1"></div>
    </div>
 </div>   
</template>
<script>
    window.moment = require('moment');   
    import axios from 'axios';
    import { getIntOrdinalString } from '../js/common.js';
    import Tabulator from 'tabulator-tables';
    import 'tabulator-tables/dist/css/bootstrap/tabulator_bootstrap.min.css'
    import Datepicker from 'vanillajs-datepicker/Datepicker';

    export default {
        name: "SpeedRunGridVue",
        props: {
            gameid: String,
            categorytypeid: String,
            categoryid: String,
            levelid: String,
            variablevalues: String
        },
        data() {
            return {
                table: {},
                loading: true
            }
        },
        computed: {
        },
        created: function () {
            this.loadData();
        },
        methods: {
            loadData() {
                var that = this;
                this.loading = true;

                axios.get('../SpeedRun/GetSpeedRunGridData', { params: { gameID: this.gameid, categoryTypeID: this.categorytypeid, categoryID: this.categoryid, levelID: this.levelid, variableValueIDs: this.variablevalues } })
                    .then(res => {
                        that.initGrid(res.data);
                        that.loading = false;
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            },
            initGrid: function (tableData) {
                var that = this;
                var players = [...new Set(tableData.flatMap(el => el.players.map(el1 => el1.name)))].sort((a, b) => { return a?.toLowerCase().localeCompare(b?.toLowerCase()) });
                
                var columns = [
                    { title: "", field: "id", width: 100, formatter: that.optionsFormatter, hozAlign: "center", headerSort:false },
                    { title: "Rank", field: "rank", width: 75, sorter: "number", formatter: that.rankFormatter },
                    { title: "Players", field: "players", width: 160, sorter:that.playerSorter, formatter: that.playerFormatter, headerFilter:"select", headerFilterParams:{ values:players, multiselect:true }, headerFilterFunc: that.playerHeaderFilter },
                    { title: "Platform", field: "platformName", width: 160, headerFilter:"select", headerFilterParams:{ values:true, multiselect:true }, headerFilterFunc:"in" },
                    { title: "Emulated", field: "isEmulatedString", width: 125 },
                    { title: "Time", field: "primaryTime.ticks", formatter: that.primaryTimeFormatter, sorter: "number", width: 160 },
                    { title: "Submitted Date", field: "dateSubmitted", width: 160, tooltip: that.dateSubmittedToolTip, formatter: "datetime", formatterParams:{ outputFormat:"MM/DD/YYYY HH:MM" }, headerFilter: that.dateEditor, headerFilterFunc: that.dateHeaderFilter },
                    { title: "Verified Date", field: "verifyDate", width: 160, tooltip: that.verifyDateToolTip, formatter: "datetime", formatterParams:{ outputFormat:"MM/DD/YYYY HH:MM" }, headerFilter: that.dateEditor, headerFilterFunc: that.dateHeaderFilter  },
                    { title: "primaryTimeString", field: "primaryTimeString", width: 160, visible: false },
                    { title: "relativeDateSubmittedString", field: "relativeDateSubmittedString", visible: false },
                    { title: "relativeVerifyDateString", field: "relativeVerifyDateString", visible: false }
                ];

                tableData.forEach(item => {
                    if(item.variableValues){
                        Object.keys(item.variableValues).forEach(variableID => {
                            item[variableID] = item.variableValues[variableID].name;
                        })
                    }
                });

                var variables = tableData.flatMap(el => el.variables.map(el => el));
                var distinctVariables = [ ...new Set( variables.map( obj => obj.id) ) ].map( id => { return variables.find(obj => obj.id === id) } )                

                distinctVariables?.forEach(variable => { 
                    columns.push({ title: variable.name, field: variable.id.toString(), width: 120 },)
                });

                this.table = new Tabulator("#tblGrid", {
                    data: tableData,           //load row data from array
                    layout: "fitColumns",      //fit columns to width of table
                    responsiveLayout: "collapse",  //hide columns that dont fit on the table
                    tooltips: true,            //show tool tips on cells
                    pagination: "local",       //paginate the data
                    paginationSize: 50,        //allow 7 rows per page of data
                    movableColumns: true,      //allow column order to be changed
                    resizableRows: true,       //allow row order to be changed
                    initialSort: [             //set the initial sort order of the data
                        { column: "rank", dir: "asc" },
                    ],
                    columns: columns
                });
            },
            optionsFormatter(cell, formatterParams, onRendered) {
                var value = cell.getValue();

                var html = "<div>"
                html += "<div class='d-table' style='border:none; border-collapse:collapse; border-spacing:0; margin:auto;'>";
                html += "<div class='d-table-row'>";
                html += "<div class='d-table-cell' style='border:none; padding:0px; width:30px;'>";
                html += "<a href=\"javascript:showSpeedRunSummary('" + value + "');\"><i class='fas fa-play-circle'></i></a>";
                html += "</div>";
                html += "<div class='d-table-cell pl-1 ' style='border:none; padding:0px; width:30px;'>";
                html += "<a href=\"javascript:showSpeedRunDetails('" + value + "');\"><i class='fas fa-edit'></i></a>";
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
                var html = '';
                var value = cell.getValue();

                value.forEach(el => {
                    if (el.id > 0) {
                        html += "<a href='../User/UserDetails?userID=" + el.id + "'>" + el.name + "</a><br/>";
                    } else {
                        html += el.name;
                    }          
                });

                return html;
            },
            // dateFormatter(cell, formatterParams, onRendered) {
            //     var html = '';
            //     var value = cell.getValue();

            //     if (value) {
            //         var date = new Date(value);
            //         html += ("0" + (date.getMonth() + 1).toString()).substr(-2) + "/" + ("0" + date.getDate().toString()).substr(-2)  + "/" + (date.getFullYear().toString());
            //     }

            //     return html;
            // },
            primaryTimeFormatter(cell, formatterParams, onRendered) {
                var html = '';
                var value = cell.getRow().getCell("primaryTimeString").getValue();

                if (value) {
                    html += value
                }

                return html;
            },
            dateSorter(a, b, aRow, bRow, column, dir, sorterParams){
                return new Date(a) - new Date(b);
            },            
            playerSorter(a, b, aRow, bRow, column, dir, sorterParams){
                return a?.toLowerCase().localeCompare(b?.toLowerCase());
            },
            dateSubmittedToolTip(cell) {
                var relativeDateSubmittedString = cell.getRow().getCell("relativeDateSubmittedString").getValue();

                return relativeDateSubmittedString;
            },
            verifyDateToolTip(cell) {
                var relativeVerifyDateString = cell.getRow().getCell("relativeVerifyDateString").getValue();

                return relativeVerifyDateString;
            },
            dateEditor (cell, onRendered, success, cancel, editorParams){
                var editor = document.createElement("input");
                editor.setAttribute("type", "date");

                editor.style.width = "100%";
                editor.style.boxSizing = "border-box";

                function successFunc(el){
                    var dateString = '';
                    if(editor.value){
                        dateString = new moment(editor.value).format("MM/DD/YYYY");
                    }

                    success(dateString);
                }

                function onKeydown (event) {
                    const key = event.key;
                    if (key === "Backspace" || key === "Delete") {
                        clearFunc(event.target);
                    }
                }

                function clearFunc(el){
                    el.value='';
                    el.dispatchEvent(new Event('change'));
                }

                editor.addEventListener("change", successFunc);
                editor.addEventListener("blur", successFunc);
                editor.addEventListener("keydown", onKeydown);

                return editor;
            },                   
            playerHeaderFilter(headerValue, rowValue, rowData, filterParams){
                if(headerValue.length == 0){
                    return true;
                }

                return rowValue.filter(el => { 
                    return headerValue.indexOf(el.name) > -1 
                    }).length > 0;
            },        
            dateHeaderFilter(headerValue, rowValue, rowData, filterParams){
                if(!headerValue){
                    return true;
                }

                var value = new moment(rowValue).format("MM/DD/YYYY"); 

                return headerValue == value; 
            },                 
            clearFilter: function() {
                this.filterField = '';
                this.filterType = '';
                this.filterValue = '';
                this.filterPlayerIDs= [];

                this.table.clearFilter();
            }                                  
        }
    };
</script>







