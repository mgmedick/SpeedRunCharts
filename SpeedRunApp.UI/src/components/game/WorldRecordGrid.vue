﻿<template>
    <div>        
        <div v-if="loading">
            <div class="d-flex">
                <div class="mx-auto">
                    <i class="fas fa-spinner fa-spin fa-lg"></i>
                </div>
            </div>
        </div>        
        <div class="mt-2 mx-0 grid-container container-lg p-0" style="min-height:150px;">
            <div class="grid-group" :style="[ loading ? { display:'none' } : null ]">
                <ul @drop.prevent="onGroupAdd" @dragenter.prevent @dragover.prevent>                    
                    <li v-if="groups.length == 0" class="group-placeholder">Drag columns here to group</li>
                    <li v-if="groups.length > 0" class="group-label">Group By:</li>
                    <li v-for="(group, i) in groups" :key="i" class="group-tag">
                        <span>{{ group.title }}</span>&nbsp;
                        <span class="fas fa-times fa-sm" @click.stop="onGroupRemove(group.field)" style="cursor:pointer"></span>
                    </li>                    
                </ul>
            </div>
            <div class="grid" style="[ loading ? { display:'none' } : null ]"></div>
        </div>
        <modal v-if="showDetailModal" contentclass="cmv-modal-lg" @close="showDetailModal = false">
            <template v-slot:title>
                Details
            </template>
            <speedrun-edit :gameid="gameid" :speedrunid="selectedSpeedRunID" :readonly="true" />
        </modal>    
    </div>   
</template>
<script>
    const dayjs = require('dayjs');
    import axios from 'axios';    
    import { escapeHtml, isValidDate } from '../../js/common.js';
    import Tabulator from 'tabulator-tables';
    import 'tabulator-tables/dist/css/bootstrap/tabulator_bootstrap.min.css'
    import tippy from 'tippy.js'
    import 'tippy.js/dist/tippy.css'
    import { polyfill } from "mobile-drag-drop";
    import { scrollBehaviourDragImageTranslateOverride } from "mobile-drag-drop/scroll-behaviour";

    export default {
        name: "WorldRecordGrid",
        props: {
            gameid: String,
            categorytypeid: String,
            categoryid: String,
            levelid: String,
            variablevalues: String,
            showmilliseconds: Boolean,
            showcategories: Boolean,
            showlevels: Boolean,
            variables: Array,
            subcategoryvariablevaluetabs: Array
        },
        data() {
            return {
                table: {},
                tableData: [],
                groups: [],
                loading: true,
                showDetailModal: false,
                pageSize: 100
            }
        },
        computed: {
            isMediaMedium: function () {
                return window.innerWidth > 768;
            }
        },
        mounted: function() {
            polyfill({
                dragImageTranslateOverride: scrollBehaviourDragImageTranslateOverride
            });            
            this.loadData();
            window.gameWorldRecordGridVue = this;
            window.addEventListener( 'touchmove', function() {}, { passive: false });
        },
        methods: {
            loadData() {
                var that = this;
                this.loading = true;

                axios.get('/SpeedRun/GetWorldRecordGridData', { params: { gameID: this.gameid, categoryTypeID: this.categorytypeid, categoryID: this.categoryid, levelID: this.levelid } })
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
                    { title: "", field: "id", formatter: that.optionsFormatter, hozAlign: "center", headerSort: false, width: 20 }, //, width: 50, widthShrink: 2
                    { title: "#", field: "rank", formatter: that.rankFormatter, headerSort: false, width: 20 }, //minWidth:40, maxWidth:75                    
                    { title: "Category", field: "categoryName", headerFilter: "select", headerFilterParams: { values: true, multiselect: true }, minWidth: 150, widthGrow: 1, visible: that.showcategories }, //, minWidth: 100, widthGrow: 1                    
                    { title: "Level", field: "levelName", headerFilter: "select", headerFilterParams: { values: true, multiselect: true }, minWidth: 150, widthGrow: 2, visible: that.showlevels }, //, minWidth: 100, widthGrow: 1                   
                    { title: "primaryTimeString", field: "primaryTimeString", visible: false },
                    { title: "relativeDateSubmittedString", field: "relativeDateSubmittedString", visible: false },
                    { title: "primaryTimeSecondsString", field: "primaryTimeSecondsString", visible: false },
                    { title: "playersObj", field: "players", visible: false }
                ];

                tableData.forEach(item => {
                    if (item.subCategoryVariableValueIDs) {
                        item.subCategoryVariableValueIDs.split(",").forEach(variableValueID => {
                            var variable = that.variables?.filter(x => x.variableValues.filter(i => i.id == variableValueID).length > 0)[0];
                            if (variable && variable.isSubCategory) {
                                var variableValue = variable.variableValues.filter(i => i.id == variableValueID)[0]
                                if (variableValue) {
                                    item[variable.id.toString()] = variableValue.name;
                                    item[variable.id + 'sort'] = variableValue.id;
                                }
                            }
                        })
                    }
                });                

                var variables = that.variables?.filter(i => tableData.filter(el => el[i.id.toString()]).length > 0);
                var distinctVariables = [...new Set(variables?.map(obj => obj.id))].map(id => { return variables.find(obj => obj.id === id) });
                distinctVariables?.forEach(variable => {                             
                    var variableValuesSorted = variable.variableValues.filter(i => tableData.filter(el => el[variable.id + 'sort'] == i.id).length > 0).sort((a, b) => { return a?.id - b?.id });                                                                
                    var variableValueNames = [...new Set(variableValuesSorted.map(x => x.name))];
                    columns.push({ title: variable.name, field: variable.id.toString(), headerFilter: "select", headerFilterParams: { values: variableValueNames, multiselect: true }, headerFilterFunc: "in", minWidth: 150, widthGrow: 1 },)
                    columns.push({ title: variable.name + 'sort', field: variable.id + 'sort', visible: false },)
                });

                columns.push({ title: "Players", field: "playerNames", formatter: that.playerFormatter, headerFilter: "select", headerFilterParams:{ values:players, multiselect:true }, headerFilterFunc: that.playerHeaderFilter, minWidth:135, widthGrow:2 });
                columns.push({ title: "Time", field: "primaryTime.ticks", formatter: that.primaryTimeFormatter, sorter: "number", width: 135 });
                columns.push({ title: "Submitted", field: "dateSubmitted", sorter: "date", formatter: that.dateFormatter, formatterParams: { outputFormat: "MM/DD/YYYY", tooltipFieldName: "relativeDateSubmittedString" }, headerFilter: that.dateEditor, headerFilterFunc: that.dateHeaderFilter, minWidth: 120 });
                columns.push({ title: "", field: "comment", formatter: that.commentFormatter, hozAlign: "center", headerSort: false, width: 50 });

                if (that.subcategoryvariablevaluetabs && that.subcategoryvariablevaluetabs.length > 0) {
                    that.getVariableGroupByList(that.subcategoryvariablevaluetabs, tableData);
                }
                
                var sortList = [];
                distinctVariables?.slice().reverse().forEach((variable, variableindex) => {
                    sortList.push({ column: variable.id + 'sort', dir: "asc" });
                });

                var el = this.$el.querySelector('.grid');          
                this.table = new Tabulator(el, {
                    data: tableData,
                    layout: "fitColumns",
                    //responsiveLayout: false,
                    tooltips: false,
                    tooltipsHeader:false,
                    pagination: "local",
                    paginationSize: that.pageSize,
                    movableColumns: false,
                    resizableColumns: "header",
                    //resizableRows: true,
                    groupBy: that.groups.map(i => i.field),
                    initialSort: sortList,
                    columns: columns,
                    groupHeader: function(value, count, data, group) {
                        var html = that.getGroupText(group._group, count);
                        return html;
                    },
                    renderComplete:function() {
                        that.$el.querySelectorAll('.tabulator-header .tabulator-col').forEach(el => {
                            el.setAttribute('draggable', true);
                            el.addEventListener("dragstart", function(event) {
                                event.dataTransfer.setData("field", event.target.getAttribute('tabulator-field'));
                                event.dataTransfer.setData("title", event.target.querySelector('.tabulator-col-title').innerHTML);
                            });
                        });

                        Array.from(that.$el.querySelectorAll('.tippy-tooltip')).forEach(el => {
                            var value = el.getAttribute('data-content');
                            var cellElement = el.closest('.tabulator-cell');

                            tippy(cellElement, {
                                content: escapeHtml(value),
                                allowHTML: true,
                                arrow:false,
                                placement: 'bottom'
                            })
                        });

                        that.$el.querySelectorAll('.tabulator-header-filter input[type=search]').forEach(el => { el.addEventListener("keydown", that.onSearchKeyDown); });
                    },
                });                
            },
            onGroupAdd(event) {
                event.preventDefault();
                var field = event.dataTransfer.getData("field");  
                var title = event.dataTransfer.getData("title");                
                if (field) {
                    if (this.groups.filter(i => i.field == field).length == 0) {
                        this.groups.push({ field: field, title: title });
                        this.table.setGroupBy(this.groups.map(i => i.field));
                    }                    
                }                        
            },
            onGroupRemove(field) {
                if (this.groups.filter(i => i.field == field).length > 0) {
                    this.groups = this.groups.filter(i => i.field != field);
                    this.table.setGroupBy(this.groups.map(i => i.field));
                }
            },            
            formatColumnField(fieldName) {
                var field = fieldName.replace(/./g,'_');
                return field;
            },
            getVariableGroupByList(subCategoryVariableValues, tableData, variableValueIDs, index) {
                var that = this;

                if (!variableValueIDs) {
                    variableValueIDs = '';
                }
                
                if (!index){
                    index = 0;
                }

                if (index < 3) {
                    subCategoryVariableValues?.forEach(variable => {
                        variable.variableValues.forEach(variableValue => {
                            var currVariableValueIDs = (variableValueIDs + "," + variableValue.id).replace(/(^,)|(,$)/g, '');
                            var data = tableData.filter(i => variable.categoryID == i.categoryID && variable.levelID == i.levelID && i.subCategoryVariableValueIDs && i.subCategoryVariableValueIDs.startsWith(currVariableValueIDs));
                            var uniqueData = [...new Set(data?.map(obj => obj.subCategoryVariableValueIDs))];
    
                            if (that.showlevels) {
                                if (variable.levelID && variable.variableValues.length > 1){
                                    if (that.groups.filter(i => i.field == variable.id.toString()).length == 0) {
                                        that.groups.push({ field: variable.id.toString(), title: variable.name });
                                    }
                                }
                            } else if (uniqueData.length > 1) {
                                if (that.groups.filter(i => i.field == variable.id.toString()).length == 0) {
                                    that.groups.push({ field: variable.id.toString(), title: variable.name });
                                }
                            }

                            if (variableValue.subVariables && variableValue.subVariables.length > 0) {
                                that.getVariableGroupByList(variableValue.subVariables, tableData, currVariableValueIDs, index + 1);
                            }
                        });
                    });
                }
            },            
            getGroupText(group, count) {
                var html = '';
                if (group.key) {                
                    if (isValidDate(group.key, "MM/DD/YYYY")) {
                        html += dayjs(group.key).format("MM/DD/YYYY");            
                    } else {
                        html += group.key;
                    }
                }

                html += "<span>(" + count + " item)</span>";

                return html;
            },                                      
            optionsFormatter(cell, formatterParams, onRendered) {
                var value = cell.getValue();

                var html = "<div>"
                html += "<div class='d-table' style='border:none; border-collapse:collapse; border-spacing:0; margin:auto;'>";
                html += "<div class='d-table-row'>";
                html += "<div class='d-table-cell pl-1 ' style='border:none; padding:0px; width:30px;'>";
                html += "<a href=\"javascript:window.gameWorldRecordGridVue.showSpeedRunDetails('" + value + "');\" draggable='false'><i class='fas fa-play-circle fa-lg'></i></a>";
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
                var html = (num) ? num : '-';

                return html;
            },
            playerFormatter(cell, formatterParams, onRendered) {
                var value = cell.getRow().getCell("players").getValue();

                var html = '<span>'

                value?.forEach(el => {
                    if (el.id > 0) {
                        html += "<a href='/User/UserDetails/" + el.abbr + "' class='text-primary' draggable='false'>" + el.name + "</a><br/>";
                    } else {
                        html += el.name;
                    }          
                });

                html += '</span>'

                return html;
            },  
            primaryTimeFormatter(cell, formatterParams, onRendered) {
                var html = '';
                var primaryTimeColumn = this.showmilliseconds ? "primaryTimeString" : "primaryTimeSecondsString";
                var value = cell.getRow().getCell(primaryTimeColumn).getValue();

                if (value) {
                    html += value
                }

                return html;
            },
            toolTipFormatter(cell, formatterParams, onRendered) {
                var value = cell.getValue();
                var html = '';
                if (value) {
                    html += '<span class="tippy-tooltip" data-content="' + escapeHtml(value) + '">';
                    html += value;
                    html += '</span>';
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
                editor.style.width = "100%";
                editor.style.padding = "4px";
                editor.style.boxSizing = "border-box";
                editor.style.cursor= "default";

                editor.addEventListener("change", successFunc);
                editor.addEventListener("blur", successFunc);
                editor.addEventListener("keydown", onKeydown);

                editorDiv.appendChild(editor);

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
                        el.value='';
                        el.dispatchEvent(new Event('change'));
                    }
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

                var value = rowData.players;

                return value?.filter(el => { 
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
            onSearchKeyDown(event) {
                var el = event.target;
                var key = event.key;
                if (el.value && (key === "Backspace" || key === "Delete")) {
                    var columnEl = el.closest('.tabulator-col');
                    this.table.setHeaderFilterValue(columnEl, "");
                    document.querySelector('.tabulator-edit-select-list')?.remove();
                }
            },                                              
            showSpeedRunDetails(id) {
                this.selectedSpeedRunID = id;
                this.showDetailModal = true;
            }
        }
    };
</script>









