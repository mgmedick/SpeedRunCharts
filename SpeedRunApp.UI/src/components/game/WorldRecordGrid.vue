<template>
    <div>        
        <div v-if="loading">
            <div class="d-flex">
                <div class="mx-auto">
                    <i class="fas fa-spinner fa-spin fa-lg"></i>
                </div>
            </div>
        </div>  
        <div class="mt-4">  
            <div class="row g-2 mb-2">
                <div class="col-auto ms-auto">
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
            <div class="mt-2 grid-container" style="min-height:150px;">
                <div class="card" :style="[ loading ? { display:'none' } : null ]" style="border-radius: 0px; border-style: dashed;">
                    <div class="card-header"  @drop.prevent="onGroupAdd" @dragenter.prevent @dragover.prevent>
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
    // import 'tabulator-tables/dist/css/tabulator_bootstrap5.min.css'
    // import tippy from 'tippy.js'
    // import 'tippy.js/dist/tippy.css'
    // import { polyfill } from "mobile-drag-drop";
    // import { scrollBehaviourDragImageTranslateOverride } from "mobile-drag-drop/scroll-behaviour";
    import { Tooltip, Modal } from 'bootstrap';

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
            subcategoryvariablevaluetabs: Array,
            showmisc: Boolean,
            title: String,
            exporttypes: Array          
        },
        data() {
            return {
                table: {},
                tableData: [],
                groups: [],
                loading: true,
                selectedSpeedRunID: null,
                pageSize: 100,
                theme: document.documentElement.dataset.bsTheme
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
 
            // this.$refs.detailmodal.addEventListener('show.bs.modal', event => {
            //     this.$refs.speedrundetails.loadData();
            // }); 

            this.$refs.detailmodal.addEventListener('hidden.bs.modal', event => {
                this.selectedSpeedRunID = null;
            });               

            this.loadData();
            window.gameWorldRecordGridVue = this;
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

                axios.get('/Game/GetWorldRecordGridData', { params: { gameID: this.gameid, categoryTypeID: this.categorytypeid, categoryID: this.categoryid, levelID: this.levelid } })
                    .then(res => {
                        that.tableData = res.data;
                        that.initGrid(res.data);                                              
                        that.loading = false;                      
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
                
                if (that.showcategories) {
                    tableData = tableData.filter(i => (that.showmisc || !i.isMiscellaneous));
                }

                var players = [...new Set(tableData.flatMap(el => el.players?.map(el1 => el1.name)))].sort((a, b) => { return a?.toLowerCase().localeCompare(b?.toLowerCase()) });

                var columns = [
                    { title: "", field: "id", visible: false }, //, minWidth:30, maxWidth:50
                    { title: "#", field: "rank", formatter: that.rankFormatter, headerSort: false, width: 20 }, //minWidth:40, maxWidth:75                    
                    { title: "Category", field: "categoryName", headerFilter: "select", headerFilterParams: { values: true, multiselect: true }, minWidth: 150, widthGrow: 2, visible: that.showcategories }, //, minWidth: 100, widthGrow: 1                    
                    { title: "Level", field: "levelName", headerFilter: "select", headerFilterParams: { values: true, multiselect: true }, minWidth: 150, widthGrow: 2, visible: that.showlevels }, //, minWidth: 100, widthGrow: 1                   
                    { title: "primaryTimeMillisecondsString", field: "primaryTimeMillisecondsString", visible: false },
                    { title: "relativeDateSubmittedString", field: "relativeDateSubmittedString", visible: false },
                    { title: "relativeVerifyDateString", field: "relativeVerifyDateString", visible: false },
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

                columns.push({ title: "Players", field: "playerNames", formatter: that.playerFormatter, headerFilter: "select", headerFilterParams:{ values:players, multiselect:true }, headerFilterFunc: that.playerHeaderFilter, minWidth:135, widthGrow:1 });
                columns.push({ title: "primaryTimeMillisecondsString", field: "primaryTimeMillisecondsString", visible: false, download: true, titleDownload: "Time" });                   
                columns.push({ title: "Time", field: "primaryTimeMilliseconds", formatter: that.primaryTimeFormatter, sorter: "number", width: 135, titleDownload: "Time (ms)" });
                columns.push({ title: "Submitted", field: "dateSubmitted", sorter: "date", formatter: that.dateFormatter, formatterParams: { outputFormat: "MM/DD/YYYY", tooltipFieldName: "relativeDateSubmittedString" }, accessorDownload: that.dateDownloadAccessor, accessorDownloadParams: { outputFormat:"MM/DD/YYYY" }, headerFilter: that.dateEditor, headerFilterFunc: that.dateHeaderFilter, minWidth: 120 });
                columns.push({ title: "Verified", field: "verifyDate", sorter: "date", formatter:that.dateFormatter, formatterParams:{ outputFormat:"MM/DD/YYYY", tooltipFieldName:"relativeVerifyDateString" }, accessorDownload: that.dateDownloadAccessor, accessorDownloadParams: { outputFormat:"MM/DD/YYYY" }, headerFilter: that.dateEditor, headerFilterFunc: that.dateHeaderFilter, minWidth: 120 });                                                                        
                columns.push({ title: "VideoLinks", field: "videoLinks", accessorDownload: that.videoLinksDownloadAccessor, visible: false, download: true, titleDownload: "Videos" });                

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
                    }
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
            formatColumnField(fieldName) {
                var field = fieldName.replace(/./g,'_');
                return field;
            },
            getVariableGroupByList(subCategoryVariableValueNames, tableData, variableValueIDs, index) {
                var that = this;

                if (!variableValueIDs) {
                    variableValueIDs = '';
                }
                
                if (!index){
                    index = 0;
                }

                if (index < 3) {
                    subCategoryVariableValueNames?.forEach(variable => {
                        var variableData = [];
                        variable.variableValues.forEach(variableValue => {
                            var currVariableValueIDs = (variableValueIDs + "," + variableValue.id).replace(/(^,)|(,$)/g, '');
                            var data = tableData.filter(i => variable.categoryID == i.categoryID && variable.levelID == i.levelID && i.subCategoryVariableValueIDs && i.subCategoryVariableValueIDs.startsWith(currVariableValueIDs));
                            variableData = variableData.concat(data);
                            var uniqueVariableData = [...new Set(variableData?.map(obj => obj.subCategoryVariableValueIDs))];
                            var uniqueData = [...new Set(data?.map(obj => obj.subCategoryVariableValueIDs))];

                            if (that.showlevels) {
                                if (variable.levelID && variable.variableValues.length > 1){
                                    if (that.groups.filter(i => i.field == variable.id.toString()).length == 0) {
                                        that.groups.push({ field: variable.id.toString(), title: variable.name });
                                    }
                                }
                            } else if (that.showcategories) {
                                if (uniqueVariableData.length > 1) {
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
                html += "<div class='d-table-cell ps-1 ' style='border:none; padding:0px; width:30px;'>";
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
                        if (el.colorLight && el.colorDark) {
                            html += "<span class='playername-text playername-color-light' style='background: linear-gradient(to right," + el.colorLight + "," + (el.colorToLight || el.colorLight) + ");'>"
                            html += "<span class='playername-text playername-color-dark' style='background: linear-gradient(to right," + el.colorDark + "," + (el.colorToDark || el.colorDark) + ");'>";
                            html += "<a href='/Player/PlayerDetails/" + el.abbr + "'>" + el.name + "</a>"
                            html += "</span></span><br/>";                           
                        } else {
                            html += "<a href='/Player/PlayerDetails/" + el.abbr + "' class='playername-text'>" + el.name + "</a>"
                        }
                    } else {
                        html += el.name;
                    }          
                });

                html += '</span>'

                return html;
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
            toolTipFormatter(cell, formatterParams, onRendered) {
                var value = cell.getValue();
                var html = '';
                if (value) {
                    html += '<span data-bs-toggle="tooltip" data-bs-title="' + escapeHtml(value) + '">';
                    html += value;
                    html += '</span>';
                }

                return html;
            },
            dateFormatter(cell, formatterParams, onRendered) {
                var tooltip = formatterParams.tooltipFieldName ? cell.getRow().getCell(formatterParams.tooltipFieldName).getValue() : '';
                var html = tooltip ? '<span data-bs-toggle="tooltip" data-bs-title="' + escapeHtml(tooltip) + '">' : '<span>';
                var value = cell.getValue();
                var formatString = formatterParams.outputFormat;

                if(value){
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










