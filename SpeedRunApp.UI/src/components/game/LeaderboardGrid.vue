﻿<template>
    <div>   
        <div v-if="loading">
            <div class="d-flex">
                <div class="mx-auto">
                    <i class="fas fa-spinner fa-spin fa-lg"></i>
                </div>
            </div>
        </div>
        <div class="mt-2">
            <div class="row no-gutters pr-1">
                <div class="col-auto ml-auto pr-1">
                    <button-dropdown :btnclasses="'btn-secondary btn-sm'" :listclasses="'dropdown-menu-right'">
                        <template v-slot:text>
                            <span>
                                <i class="fa fa-filter"></i><span class="pl-2">...</span>
                            </span>
                        </template>
                        <template v-slot:options>
                            <div class="dropdown-item">
                                <div class="custom-control custom-switch">
                                    <input id="chkShowAllData" type="checkbox" class="custom-control-input" data-toggle="toggle" v-model="showAllData">
                                    <label class="custom-control-label pl-1" for="chkShowAllData"><span class="pl-2">Show Obsolete</span></label>
                                </div>                    
                            </div>                      
                        </template>
                    </button-dropdown>
                </div>
                <div class="col-auto">
                    <button-dropdown :btnclasses="'btn-secondary btn-sm'" :listclasses="'dropdown-menu-right'">
                        <template v-slot:text>
                            <span>Export</span>
                        </template>
                        <template v-slot:options>
                            <template v-for="(exporttype, i) in exporttypes" :key="i">
                                <a class="dropdown-item" href="#/" :data-value="exporttype.id" data-toggle="pill" draggable="false" @click="onExportClick">{{ exporttype.name }}</a>
                            </template>
                        </template>
                    </button-dropdown>
                </div>                                                                
            </div>    
            <div class="mt-1 grid-container" style="min-height:150px;">             
                <leaderboard-chart-container v-if="!loading" :showcharts="showcharts" :showmilliseconds="showmilliseconds" :gameid="gameid" :categorytypeid="categorytypeid" :categoryid="categoryid" :levelid="levelid" :variablevalues="variablevalues" :userid="userid" :title="title" :istimerasc="istimerasc" @onshowchartsclick="$emit('onshowchartsclick1', $event)"></leaderboard-chart-container>
                <div class="grid-group" :style="[ loading ? { display:'none' } : null ]">
                    <ul @drop.prevent="onGroupAdd" @dragenter.prevent @dragover.prevent>                    
                        <li v-if="groups.length == 0" class="group-placeholder">Drag column headers here to group</li>
                        <li v-if="groups.length > 0" class="group-label">Group By:</li>
                        <li v-for="(group, i) in groups" :key="i" class="group-tag">
                            <span>{{ group.title }}</span>&nbsp;
                            <span class="fas fa-times fa-sm" @click.stop="onGroupRemove(group.field)" style="cursor:pointer"></span>
                        </li>                    
                    </ul>
                </div>
                <div class="grid" :style="[ loading ? { display:'none' } : null ]"></div>
            </div>
        </div>
        <modal v-if="showDetailModal" contentclass="cmv-modal-lg" @close="showDetailModal = false">
            <template v-slot:title>
                Details
            </template>
            <div class="container p-0">
                <speedrun-edit :gameid="gameid" :speedrunid="selectedSpeedRunID" :readonly="true" />
            </div>
        </modal>    
    </div>   
</template>
<script>
    const dayjs = require('dayjs');
    import axios from 'axios';    
    import { escapeHtml, formatFileName, isValidDate } from '../../js/common.js';
    import Tabulator from 'tabulator-tables';
    import 'tabulator-tables/dist/css/bootstrap/tabulator_bootstrap.min.css'
    import tippy from 'tippy.js'
    import 'tippy.js/dist/tippy.css'
    import { polyfill } from "mobile-drag-drop";
    import { scrollBehaviourDragImageTranslateOverride } from "mobile-drag-drop/scroll-behaviour";

    export default {
        name: "LeaderboardGrid",
        emits: ["onshowchartsclick1"],
        props: {
            gameid: String,
            categorytypeid: String,
            categoryid: String,
            levelid: String,
            variablevalues: String,
            speedrunid: String,
            userid: String,
            showcharts: Boolean,          
            showalldata: Boolean,
            showmilliseconds: Boolean,
            variables: Array,
            title: String,
            istimerasc: Boolean,
            exporttypes: Array          
        },
        data() {
            return {
                table: {},
                tableData: [],
                groups: [],
                loading: true,
                speedRunID: this.speedrunid,
                selectedSpeedRunID: '',
                showAllData: this.showalldata,
                showDetailModal: false,
                pageSize: 100
            }
        },  
        watch: {
            showalldata: function (val, oldVal) {
                this.loadData();
            },
            showAllData: function (val, oldVal) {
                this.$emit('update:showalldata', val); 
            }              
        },                     
        mounted: function() {
            polyfill({
                dragImageTranslateOverride: scrollBehaviourDragImageTranslateOverride
            });
            this.loadData();
            window.speedRunGridVue = this;
            //window.addEventListener( 'touchmove', function() {}, { passive: false });
        },
        methods: {
            loadData() {
                var that = this;
                this.loading = true;

                axios.get('/SpeedRun/GetLeaderboardGridData', { params: { gameID: this.gameid, categoryID: this.categoryid, levelID: this.levelid, subCategoryVariableValueIDs: this.variablevalues, showAllData: this.showalldata } })
                    .then(res => {
                        that.tableData = res.data;
                        if (that.istimerasc) {
                            that.tableData = that.tableData.sort((a, b) => { return b?.primaryTimeTicks - a?.primaryTimeTicks });
                        }
                                                                        
                        that.initGrid(res.data); 
                        that.loading = false;
                        if (that.speedRunID) {
                            var index = that.tableData.findIndex(i => i.id == that.speedRunID);
                            if (index > -1) {
                                that.table.selectRow(that.speedRunID);
                                var page = Math.ceil(index / that.pageSize);
                                if(page > 1) {
                                    that.table.setPage(page);
                                }
                            }
                        }
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            },  
            export(exportTypeID) {
                var title = formatFileName(this.title);
                switch(exportTypeID){
                    case "0":
                        this.table.download('csv', title + ".csv");
                        break;
                    case "1":
                        this.table.download('json', title + ".json");
                        break;              
                }                
            },                                                                                        
            initGrid(tableData) {
                var that = this;
                var players = [...new Set(tableData.flatMap(el => el.players?.map(el1 => el1.name)))].sort((a, b) => { return a?.toLowerCase().localeCompare(b?.toLowerCase()) });
                
                var columns = [
                    { title: "", field: "id", formatter: that.optionsFormatter, hozAlign: "center", headerSort: false, width:50, widthShrink:2, download:false }, //, minWidth:30, maxWidth:50
                    { title: "#", field: "rank", sorter: "number", formatter: that.rankFormatter, headerFilter: "select", headerFilterParams: { values: true, multiselect: true }, headerFilterFunc: that.rankHeaderFilter, width: 60 }, //minWidth:40, maxWidth:75
                    { title: "Players", field: "playerNames", formatter: that.playerFormatter, headerFilter: "select", headerFilterParams:{ values:players, multiselect:true }, headerFilterFunc: that.playerHeaderFilter, minWidth:135, widthGrow:2 }, //minWidth:125
                    { title: "primaryTimeString", field: "primaryTimeString", visible: false, download: true, titleDownload: "Time" },                    
                    { title: "Time", field: "primaryTimeTicks", formatter: that.primaryTimeFormatter, sorter: "number", width: 135, titleDownload: "Time (ticks)" }, //minWidth:100, maxWidth:125                    
                    { title: "Platform", field: "platformName", headerFilter:"select", headerFilterParams:{ values:true, multiselect:true }, headerFilterFunc:"in", minWidth:100, widthGrow:1 }, //minWidth:100                    
                    { title: "relativeDateSubmittedString", field: "relativeDateSubmittedString", visible: false },
                    { title: "relativeVerifyDateString", field: "relativeVerifyDateString", visible: false },
                    { title: "primaryTimeSecondsString", field: "primaryTimeSecondsString", visible: false },
                    { title: "playersObj", field: "players", visible: false }
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

                columns.push({ title: "Submitted Date", field: "dateSubmitted", sorter: "date", formatter: that.dateFormatter, formatterParams:{ outputFormat:"MM/DD/YYYY", tooltipFieldName:"relativeDateSubmittedString" }, accessorDownload: that.dateDownloadAccessor, accessorDownloadParams: { outputFormat:"MM/DD/YYYY" }, headerFilter: that.dateEditor, headerFilterFunc: that.dateHeaderFilter, minWidth:150 });
                columns.push({ title: "Verified Date", field: "verifyDate", sorter: "date", formatter:that.dateFormatter, formatterParams:{ outputFormat:"MM/DD/YYYY", tooltipFieldName:"relativeVerifyDateString" }, accessorDownload: that.dateDownloadAccessor, accessorDownloadParams: { outputFormat:"MM/DD/YYYY" }, headerFilter: that.dateEditor, headerFilterFunc: that.dateHeaderFilter, minWidth:150 });                                                        
                columns.push({ title: "VideoLinks", field: "videoLinks", accessorDownload: that.videoLinksDownloadAccessor, visible: false, download: true, titleDownload: "Videos" });
                columns.push({ title: "", field: "comment", formatter: that.commentFormatter, accessorDownload: that.commentDownloadAccessor, hozAlign: "center", headerSort: false, width: 50, widthShrink:2, download:false });

                var el = this.$el.querySelector('.grid');          
                this.table = new Tabulator(el, {
                    data: tableData,
                    layout: "fitColumns",
                    reactiveData:true,
                    //responsiveLayout: false,
                    selectable: false,
                    tooltips: false,
                    tooltipsHeader:false,
                    pagination: "local",
                    paginationSize: that.pageSize,
                    movableColumns: false,
                    resizableColumns: "header",
                    //resizableRows: false,
                    groupBy: that.groups.map(i => i.field),
                    groupHeader: function(value, count, data, group) {
                        var html = that.getGroupText(group._group, count);
                        return html;
                    },                    
                    initialSort: [
                        { column: "primaryTimeTicks", dir: that.istimerasc ? "desc" : "asc" },
                    ],
                    columns: columns,
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
                                placement:'bottom'
                            })
                        });

                        that.$el.querySelectorAll('.tabulator-header-filter input[type=search]').forEach(el => { el.addEventListener("keydown", that.onSearchKeyDown); });
                    }
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
                html += "<a href=\"javascript:window.speedRunGridVue.showSpeedRunDetails('" + value + "');\" draggable='false'><i class='fas fa-play-circle fa-lg'></i></a>";
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
                        if (el.colorLight && el.colorDark) {
                            html += "<span class='username-text username-color-light' style='background: linear-gradient(to right," + el.colorLight + "," + (el.colorToLight || el.colorLight) + ");'>"
                            html += "<span class='username-text username-color-dark' style='background: linear-gradient(to right," + el.colorDark + "," + (el.colorToDark || el.colorDark) + ");'>";
                            html += "<a href='/User/UserDetails/" + el.abbr + "' draggable='false'>" + el.name + "</a>"
                            html += "</span></span><br/>";                           
                        } else {
                            html += "<a href='/User/UserDetails/" + el.abbr + "' class='username-text' draggable='false'>" + el.name + "</a>"
                        }
                    } else {
                        html += el.name;
                    }          
                });

                html += '</span>'

                return html;
            },   
            playerDownloadAccessor(value, data, type, params, column) {
                return value?.map(el => el.name).join('\r\n');
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
            dateFormatter(cell, formatterParams, onRendered) {
                var tooltip = formatterParams.tooltipFieldName ? cell.getRow().getCell(formatterParams.tooltipFieldName).getValue() : '';
                var html = tooltip ? '<span class="tippy-tooltip" data-content="' + escapeHtml(tooltip) + '">' : '<span>'
                var value = cell.getValue();
                var formatString = formatterParams.outputFormat;

                if(value) {
                    html += dayjs(value).format(formatString);
                }

                html+='</span>'

                return html;
            },
            dateDownloadAccessor(value, data, type, params, column) {
                var html = '';
                var formatString = params.outputFormat;

                if (value) {
                    html += dayjs(value).format(formatString);
                }
                
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
            commentDownloadAccessor(value, data, type, params, column) {
                return value ?? '';
            },  
            videoLinksDownloadAccessor(value, data, type, params, column) {
                return value?.join('\r\n');
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
            onExportClick: function (event) {
                var value = event.target.getAttribute('data-value');
                this.export(value);
            },                                    
            showSpeedRunDetails(id) {
                this.selectedSpeedRunID = id;
                this.showDetailModal = true;
            }                              
        }             
    };
</script>










