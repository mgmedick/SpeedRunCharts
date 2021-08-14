<template>
<div>
    <div v-if="loading">
        <div class="d-flex">
            <div class="mx-auto">
                <i class="fas fa-spinner fa-spin fa-lg"></i>
            </div>
        </div>
    </div>
    <div class="mt-2 grid-container" style="width: 1100px;" :style="[ loading ? { display: 'none'} : null ]">
        <div class="form-inline">
            <div class="form-group">
                <label class="pl-2 pr-1">Field</label>
                <select class="custom-select form-control" style="width:220px;" v-model="filterField" @change="updateFilter">
                    <option v-for="column in columns" :value="column.value" :key="column.value">{{ column.text }}</option>
                </select>    
            </div>       
            <div class="form-group pr-2">
                <label class="pl-2 pr-1">Type</label>
                <select class="custom-select form-control" style="width:220px;" v-model="filterType" @change="updateFilter">
                    <option v-for="type in filterTypes" :value="type" :key="type">{{ type }}</option>
                </select>    
            </div>
            <div class="form-group pr-2">
                <label class="pl-2 pr-1">Value</label>
                <input type="text" class="form-control" placeholder="Filter Value" v-model.lazy="filterValue" @change="updateFilter">
            </div>        
            <input id="btnSave" type="button" class="btn btn-primary" value="Clear" @click="clearFilter" />
        </div>
        <div id="tblGrid" class="mt-1"></div>
    </div>
 </div>   
</template>
<script>
    import axios from 'axios';
    import { getIntOrdinalString } from '../js/common.js';
    import Tabulator from 'tabulator-tables';
    import 'tabulator-tables/dist/css/bootstrap/tabulator_bootstrap.min.css'

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
                filterValue: '',
                filterField: '',
                filterType: '',
                columns: [{value:'rank', text:'Rank'},
                          {value:'players', text:'Players'},
                          {value:'platformName', text:'Platform'},
                          {value:'isEmulatedString', text:'Emulated'},
                          {value:'primaryTimeString', text:'Time'},
                          {value:'dateSubmittedString', text:'Submitted Date'},
                          {value:'verifyDateString', text:'Verified Date'}],
                filterTypes: ['=','<','<=','>','>=','!=','like'],
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
                this.table = new Tabulator("#tblGrid", {
                    data: tableData,           //load row data from array
                    layout: "fitColumns",      //fit columns to width of table
                    responsiveLayout: "hide",  //hide columns that dont fit on the table
                    tooltips: true,            //show tool tips on cells
                    pagination: "local",       //paginate the data
                    paginationSize: 50,        //allow 7 rows per page of data
                    movableColumns: true,      //allow column order to be changed
                    resizableRows: true,       //allow row order to be changed
                    initialSort: [             //set the initial sort order of the data
                        { column: "rank", dir: "asc" },
                    ],
                    columns: [
                        { title: "", field: "id", width: 100, formatter: that.optionsFormatter, hozAlign: "center" },
                        { title: "Rank", field: "rank", width: 75, sorter: "number", formatter: that.rankFormatter },
                        { title: "Players", field: "players", width: 160, formatter: that.playerFormatter },
                        { title: "Platform", field: "platformName", width: 160 },
                        { title: "Emulated", field: "isEmulatedString", width: 125 },
                        { title: "Time", field: "primaryTimeString", width: 160 },
                        { title: "Submitted Date", field: "dateSubmitted", width: 160, formatter: that.dateFormatter, tooltip: that.dateSubmittedToolTip },
                        { title: "Verified Date", field: "verifyDate", width: 160, formatter: that.dateFormatter, tooltip: that.verifyDateToolTip },
                        { title: "relativeDateSubmittedString", field: "relativeDateSubmittedString", visible: false },
                        { title: "relativeVerifyDateString", field: "relativeVerifyDateString", visible: false }
                    ],
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

                $(value).each(function () {
                    if (this.id > 0) {
                        html += "<a href='../User/UserDetails?userID=" + this.id + "'>" + this.name + "</a><br/>";
                    } else {
                        html += this.name;
                    }
                });

                return html;
            },
            dateFormatter(cell, formatterParams, onRendered) {
                var html = '';
                var value = cell.getValue();

                if (value) {
                    var date = new Date(value);
                    html += (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
                }

                return html;
            },
            dateSubmittedToolTip(cell) {
                var relativeDateSubmittedString = cell.getRow().getCell("relativeDateSubmittedString").getValue();

                return relativeDateSubmittedString;
            },
            verifyDateToolTip(cell) {
                var relativeVerifyDateString = cell.getRow().getCell("relativeVerifyDateString").getValue();

                return relativeVerifyDateString;
            },
            updateFilter: function() {
                if (this.filterValue){
                    this.table.setFilter(this.filterField, this.filterType, this.filterValue);
                }
           },
           clearFilter: function() {
               this.filterField = '';
               this.filterType = '';
               this.filterValue = '';

               this.table.clearFilter();
           }                                  
        }
    };
</script>







