<template>
    <div>   
        <div v-if="loading">
            <div class="d-flex">
                <div class="mx-auto">
                    <i class="fas fa-spinner fa-spin fa-lg"></i>
                </div>
            </div>
        </div>
        <div>
            <div class="row g-2 my-2">
                <div class="col-auto ms-auto">
                    <div class="dropdown">
                        <button class="btn btn-secondary btn-sm dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                            <span>
                                <i class="fa fa-filter"></i><span class="ps-2">...</span>
                            </span>
                        </button>
                        <ul class="dropdown-menu dropdown-menu-end dropdown-menu-lg-start">
                            <li>
                                <div class="dropdown-item">
                                    <div class="form-check form-switch">
                                        <input id="chkShowAllData" class="form-check-input" type="checkbox" v-model="showAllData">
                                        <label class="form-check-label" for="chkShowAllData"><span>Show Obsolete</span></label>
                                    </div>                                                     
                                </div>
                            </li> 
                        </ul>
                    </div>   
                </div>
                <div class="col-auto">
                    <div class="dropdown">
                        <button class="btn btn-secondary btn-sm dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                            <span>Export</span>
                        </button>
                        <ul class="dropdown-menu dropdown-menu-end dropdown-menu-lg-start">
                            <li v-for="(exporttype, i) in exporttypes" :key="i">
                                <a class="dropdown-item" href="#/" :data-value="exporttype.id" data-toggle="pill"  @click="onExportClick">{{ exporttype.name }}</a>
                            </li>
                        </ul>
                    </div>                     
                </div>                                                                
            </div>    
            <div class="mt-1 grid-container" style="min-height:150px;">             
                <div class="card" :style="[ loading ? { display:'none' } : null ]" style="border-radius: 0px; border-style: dashed;">
                    <div class="card-header border-0 bg-body"  @drop.prevent="onGroupAdd" @dragenter.prevent @dragover.prevent>
                        <div v-if="groups.length == 0" class="text-muted fw-500 text-center"><small>Drag column headers here to group</small></div>
                        <span v-if="groups.length > 0" class="fw-bold me-2"><small>Group By:</small></span>
                        <span v-for="(group, i) in groups" :key="i" class="fs-5"><span class="badge text-bg-secondary me-1 fw-normal">{{ group.title }}&nbsp;&nbsp;<span class="fas fa-times fa-sm" @click.stop="onGroupRemove(group.field)" style="cursor:pointer"></span></span></span>                 
                    </div>
                </div>
                <div class="grid" :class="tableClass" :style="[ loading ? { display:'none' } : null ]"></div>
            </div>
        </div>
        <div ref="detailmodal" class="modal modal-lg" tabindex="-1">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Details</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">    
                        <speedrun-details ref="speedrundetails" v-if="selectedSpeedRunID" :speedrunid="selectedSpeedRunID" />                     
                    </div>
                </div>
            </div>
        </div>            
    </div>   
</template>
<script>
    const dayjs = require('dayjs');
    import axios from 'axios';    
    import { escapeHtml, formatFileName, isValidDate } from '../../js/common.js';
    import {TabulatorFull as Tabulator} from 'tabulator-tables';
    // import 'tabulator-tables/dist/css/tabulator_bootstrap5.css'
    // import tippy from 'tippy.js'
    // import 'tippy.js/dist/tippy.css'
    // import { polyfill } from "mobile-drag-drop";
    // import { scrollBehaviourDragImageTranslateOverride } from "mobile-drag-drop/scroll-behaviour";
    import { Tooltip, Modal } from 'bootstrap';

    export default {
        name: "LeaderboardGrid",
        emits: ["onshowchartsclick1"],
        props: {
            gameid: String,
            categorytypeid: String,
            categoryid: String,
            levelid: String,
            variablevalues: String,
            speedruncode: String,
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
                speedRunCode: this.speedruncode,
                selectedSpeedRunID: '',
                showAllData: this.showalldata,
                pageSize: 100,
                theme: document.documentElement.dataset.bsTheme
            }
        },  
        watch: {
            showAllData: function (val, oldVal) {
                this.loadData();
            }              
        },  
        computed: {                                                           
            tableClass: function() {
                return this.theme == 'dark' ? "table-dark" : ""; 
            }                                                                              
        },                                       
        mounted: function() {
            // polyfill({
            //     dragImageTranslateOverride: scrollBehaviourDragImageTranslateOverride
            // });

            this.$refs.detailmodal.addEventListener('show.bs.modal', event => {
                this.$refs.speedrundetails.loadData();
            }); 

            this.loadData();
            window.speedRunGridVue = this;
            //window.addEventListener( 'touchmove', function() {}, { passive: false });
            window.addEventListener('themeUpdate', this.onThemeUpdate);
        },
        destroyed() {
            window.removeEventListener('themeUpdate', this.onThemeUpdate);
        },         
        methods: {
            loadData() {
                var that = this;
                this.loading = true;

                axios.get('/Game/GetLeaderboardGridData', { params: { gameID: this.gameid, categoryTypeID: this.categorytypeid, categoryID: this.categoryid, levelID: this.levelid, subCategoryVariableValueIDs: this.variablevalues, showAllData: this.showAllData } })
                    .then(res => {
                        that.tableData = res.data;
                        if (that.istimerasc) {
                            that.tableData = that.tableData.sort((a, b) => { return b?.primaryTimeMilliseconds - a?.primaryTimeMilliseconds });
                        }
                                                                        
                        that.initGrid(res.data); 
                        that.loading = false;
                        if (that.speedRunCode) {
                            var index = that.tableData.findIndex(i => i.code == that.speedRunCode);
                            if (index > -1) {
                                that.table.selectRow(that.tableData[index].id);
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
                    { title: "", field: "id", visible: false }, //, minWidth:30, maxWidth:50
                    { title: "#", field: "rank", sorter: "number", formatter: that.rankFormatter, hozAlign: "center", headerFilter: "select", headerFilterParams: { values: true, multiselect: true }, headerFilterFunc: that.rankHeaderFilter, width: 60 }, //minWidth:40, maxWidth:75
                    { title: "Players", field: "playerNames", formatter: that.playerFormatter, headerFilter: "select", headerFilterParams:{ values:players, multiselect:true }, headerFilterFunc: that.playerHeaderFilter, minWidth: 155, widthGrow:2 }, //minWidth:125
                    { title: "primaryTimeMillisecondsString", field: "primaryTimeMillisecondsString", visible: false, download: true, titleDownload: "Time" },                    
                    { title: "Time", field: "primaryTimeMilliseconds", formatter: that.primaryTimeFormatter, sorter: "number", width: 165, titleDownload: "Time (ms)" }, //minWidth:100, maxWidth:125                    
                    { title: "Platform", field: "platformName", headerFilter:"select", headerFilterParams:{ values:true, multiselect:true }, headerFilterFunc:"in", minWidth:100, widthGrow:1 }, //minWidth:100                    
                    { title: "relativeDateSubmittedString", field: "relativeDateSubmittedString", visible: false },
                    { title: "relativeVerifyDateString", field: "relativeVerifyDateString", visible: false },
                    { title: "primaryTimeSecondsString", field: "primaryTimeSecondsString", visible: false },
                    { title: "playersObj", field: "players", visible: false },
                    { title: "code", field: "code", visible: false },
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

                columns.push({ title: "Submitted", field: "dateSubmitted", sorter: that.dateSorter, formatter: that.dateFormatter, formatterParams:{ outputFormat:"MM/DD/YYYY", tooltipFieldName:"relativeDateSubmittedString" }, accessorDownload: that.dateDownloadAccessor, accessorDownloadParams: { outputFormat:"MM/DD/YYYY" }, headerFilter: that.dateEditor, headerFilterFunc: that.dateHeaderFilter, minWidth:150 });
                columns.push({ title: "Verified", field: "verifyDate", sorter: that.dateSorter, formatter:that.dateFormatter, formatterParams:{ outputFormat:"MM/DD/YYYY", tooltipFieldName:"relativeVerifyDateString" }, accessorDownload: that.dateDownloadAccessor, accessorDownloadParams: { outputFormat:"MM/DD/YYYY" }, headerFilter: that.dateEditor, headerFilterFunc: that.dateHeaderFilter, minWidth:150 });                                                        
                columns.push({ title: "VideoLinks", field: "videoLinks", accessorDownload: that.videoLinksDownloadAccessor, visible: false, download: true, titleDownload: "Videos" });

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
                        { column: "primaryTimeMilliseconds", dir: that.istimerasc ? "desc" : "asc" },
                    ],
                    columns: columns
                });
                this.table.on("renderComplete", that.onRenderComplete);
                this.table.on("rowClick", that.onRowClick);
            },
            onRenderComplete() {
                var that = this;

                that.$el.querySelectorAll('.tabulator-header .tabulator-col').forEach(el => {
                    el.setAttribute('draggable', true);
                    el.addEventListener("dragstart", function(event) {
                        event.dataTransfer.setData("field", event.target.getAttribute('tabulator-field'));
                        event.dataTransfer.setData("title", event.target.querySelector('.tabulator-col-title').innerHTML);
                    });
                });

                that.$el.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(el => {
                    new Tooltip(el);                        
                });

                that.$el.querySelectorAll('.tabulator-header-filter input[type=search]').forEach(el => { el.addEventListener("keydown", that.onSearchKeyDown); });
            },
            onRowClick(e, row) {
                var id = row.getCell("id").getValue();
                this.showSpeedRunDetails(id);
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
                html += "<div class='d-table-cell ps-1 ' style='border:none; padding:0px; width:30px;'>";
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
                            html += "<span class='playername-text playername-color-light' style='background: linear-gradient(to right," + el.colorLight + "," + (el.colorToLight || el.colorLight) + ");'>"
                            html += "<span class='playername-text playername-color-dark' style='background: linear-gradient(to right," + el.colorDark + "," + (el.colorToDark || el.colorDark) + ");'>";
                            html += "<a href='/Player/PlayerDetails/" + encodeURIComponent(el.abbr) + "' draggable='false' onclick='event.stopPropagation()'>" + el.name + "</a>"
                            html += "</span></span><br/>";                           
                        } else {
                            html += "<a href='/Player/PlayerDetails/" + encodeURIComponent(el.abbr) + "' class='playername-text' draggable='false'>" + el.name + "</a><br/>"
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
                var primaryTimeColumn = this.showmilliseconds ? "primaryTimeMillisecondsString" : "primaryTimeSecondsString";
                var value = cell.getRow().getCell(primaryTimeColumn).getValue();

                if (value) {
                    html += value
                }
                
                return html;
            },         
            dateFormatter(cell, formatterParams, onRendered) {
                var tooltip = formatterParams.tooltipFieldName ? cell.getRow().getCell(formatterParams.tooltipFieldName).getValue() : '';
                var html = tooltip ? '<span data-bs-toggle="tooltip" data-bs-title="' + escapeHtml(tooltip) + '">' : '<span>'
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

                this.$nextTick(function() {
                    new Modal(this.$refs.detailmodal).show();
                });                
            },
            onThemeUpdate() {
                this.theme = document.documentElement.dataset.bsTheme;
                this.loadData();
            }                             
        }             
    };
</script>










