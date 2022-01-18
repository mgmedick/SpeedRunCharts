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
            <div id="tblWorldRecordGrid" class="mb-0"></div>
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
    import moment from 'moment';
    import axios from 'axios';    
    import { getIntOrdinalString } from '../js/common.js';
    import Tabulator from 'tabulator-tables';
    import 'tabulator-tables/dist/css/bootstrap/tabulator_bootstrap.min.css'
    import { escape } from 'lodash';
    import tippy from 'tippy.js'
    import 'tippy.js/dist/tippy.css'

    export default {
        name: "WorldRecordGridVue",
        props: {
            gameid: String,
            categorytypeid: String,
            categoryid: String,
            levelid: String,
            variablevalues: String,
            userid: String
        },
        data() {
            return {
                table: {},
                tableData: [],
                loading: true,
                speedRunID: String,
                showDetailModal: false,
                showAllData: false
            }
        },
        computed: {
            isMediaMedium: function () {
                return window.innerWidth > 768;
            }
        },            
        created: function () {
            this.loadData();
        },
        mounted: function() {
            window.worldRecordGridVue = this;
        },
        methods: {
            loadData() {
                var that = this;
                this.loading = true;

                axios.get('../SpeedRun/GetWorldRecordGridData', { params: { gameID: this.gameid, categoryTypeID: this.categorytypeid, categoryID: this.categoryid, levelID: this.levelid, userID: this.userid } })
                    .then(res => {
                        that.tableData = res.data;
                        that.initGrid(res.data);
                        that.loading = false;                      
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            },                                                                 
            initGrid(tableData) {
                var that = this;
                var players = [...new Set(tableData.flatMap(el => el.players?.map(el1 => el1.name)))].sort((a, b) => { return a?.toLowerCase().localeCompare(b?.toLowerCase()) });

                var columns = [
                    { title: "", field: "id", formatter: that.optionsFormatter, hozAlign: "center", headerSort: false, width: 50, widthShrink: 2 }, //, minWidth:30, maxWidth:50
                    { title: "Rank", field: "rank", sorter: "number", formatter: that.rankFormatter, headerFilter: "select", headerFilterParams: { values: true, multiselect: true }, headerFilterFunc: that.rankHeaderFilter, width: 75 }, //minWidth:40, maxWidth:75
                    { title: "Players", field: "players", sorter: that.playerSorter, formatter: that.playerFormatter, headerFilter: "select", headerFilterParams: { values: players, multiselect: true }, headerFilterFunc: that.playerHeaderFilter, minWidth: 135, widthGrow: 1 }, //minWidth:125
                    { title: "Time", field: "primaryTime.ticks", formatter: that.primaryTimeFormatter, sorter: "number", width: 125 }, //minWidth:100, maxWidth:125
                    { title: "primaryTimeString", field: "primaryTimeString", visible: false },
                    { title: "relativeDateSubmittedString", field: "relativeDateSubmittedString", visible: false },
                    { title: "relativeVerifyDateString", field: "relativeVerifyDateString", visible: false }
                ];

                tableData.forEach(item => {
                    if (item.subCategoryVariableValueIDs && item.variableValues) {
                        Object.keys(item.variableValues).forEach(variableID => {
                            if (item.subCategoryVariableValueIDs.split(",").indexOf(item.variableValues[variableID].id.toString()) > -1) {
                                var variableName = item.variables.filter(x => x.id == variableID)[0].name;
                                item[variableName] = item.variableValues[variableID].name;
                                item[variableName + 'sort'] = item.variableValues[variableID].id;
                            }
                        })
                    }
                });

                var variables = tableData.filter(el => el.variables).flatMap(el => el.variables.filter(variable => el[variable.name]));
                var distinctVariables = [...new Set(variables.map(obj => obj.name))].map(name => { return variables.find(obj => obj.name === name) });

                distinctVariables?.forEach(variable => {
                    var variableValuesSorted = tableData.filter(el => el.subCategoryVariableValueIDs && el.variableValues)
                                                        .flatMap(el => Object.keys(el.variableValues).filter(variableID => variableID == variable.id.toString() && el.subCategoryVariableValueIDs.split(",").indexOf(el.variableValues[variableID].id.toString()) > -1)
                                                        .map(x => el.variableValues[x])).sort((a, b) => { return a?.id - b?.id });
                    var variableValueNames = [...new Set(variableValuesSorted.map(x => x.name))];
                    columns.push({ title: variable.name, field: variable.name, formatter: that.toolTipFormatter, headerFilter: "select", headerFilterParams: { values: variableValueNames, multiselect: true }, headerFilterFunc: "in", minWidth: 140, widthGrow: 1 },)
                    columns.push({ title: variable.name + 'sort', field: variable.name + 'sort', visible: false },)
                });

                columns.push({ title: "Submitted Date", field: "dateSubmitted", formatter: that.dateFormatter, formatterParams: { outputFormat: "MM/DD/YYYY", tooltipFieldName: "relativeDateSubmittedString" }, headerFilter: that.dateEditor, headerFilterFunc: that.dateHeaderFilter, minWidth: 150 });
                columns.push({ title: "", field: "comment", formatter: that.commentFormatter, hozAlign: "center", headerSort: false, width: 50, widthShrink: 2 });

                var sortList = [];
                distinctVariables?.slice().reverse().forEach(variable => {
                    sortList.push({ column: variable.id + 'sort', dir: "asc" })
                });

                this.table = new Tabulator("#tblWorldRecordGrid", {
                    data: tableData,
                    layout: "fitColumns",
                    //responsiveLayout: false,
                    tooltips: false,
                    tooltipsHeader:false,
                    pagination: "local",
                    paginationSize: 10,
                    movableColumns: this.isMediaMedium,
                    resizableColumns: this.isMediaMedium ? "header" : false,
                    //resizableRows: true,
                    initialSort: sortList,
                    columns: columns,
                    renderComplete:function() {
                        Array.from(that.$el.querySelectorAll('.tippy-tooltip')).forEach(el => {
                            var value = el.getAttribute('data-content');
                            var cellElement = el.closest('.tabulator-cell');

                            tippy(cellElement, {
                                content: escape(value),
                                allowHTML: true,
                                arrow:false,
                                placement: 'bottom',
                                width: "250px"
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
                html += "<a href=\"javascript:window.worldRecordGridVue.showSpeedRunDetails('" + value + "');\"><i class='fas fa-play-circle fa-lg'></i></a>";
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
                var html = valueString ? '<span class="tippy-tooltip" data-content="' + escape(valueString) + '">' : '<span>'

                value?.forEach(el => {
                    if (el.id > 0) {
                        html += "<a href='../User/UserDetails?userID=" + el.id + "'>" + el.name + "</a><br/>";
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
            toolTipFormatter(cell, formatterParams, onRendered) {
                var value = cell.getValue();
                var html = '';
                if (value) {
                    html += '<span class="tippy-tooltip" data-content="' + escape(value) + '">';
                    html += value;
                    html += '</span>';
                }

                return html;
            },
            dateFormatter(cell, formatterParams, onRendered) {
                var tooltip = formatterParams.tooltipFieldName ? cell.getRow().getCell(formatterParams.tooltipFieldName).getValue() : '';
                var html = tooltip ? '<span class="tippy-tooltip" data-content="' + escape(tooltip) + '">' : '<span>'
                var value = cell.getValue();
                var formatString = formatterParams.outputFormat;

                if(value){
                    html +=  new moment(value).format(formatString);
                }

                html+='</span>'

                return html;
            },
            commentFormatter(cell, formatterParams, onRendered) {
                var html = '';
                var value = cell.getValue();

                if (value != null) {
                    html = '<i class="far fa-comment tippy-tooltip" data-content="' + escape(value) + '"></i>'
                }

                return html;
            },
            subCategoryVisible() {
                return this.tableData.filter(x => x.subCategoryVariableValues).length > 0;
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

                var value = new moment.utc(rowValue).format("MM/DD/YYYY"); 

                return headerValue == value; 
            },                                 
            showSpeedRunDetails(id) {
                this.speedRunID = id;
                this.showDetailModal = true;
            }
        }
    };
</script>










