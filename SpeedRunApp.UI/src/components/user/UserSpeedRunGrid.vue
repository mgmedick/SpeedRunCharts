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
            <div class="table-responsive">
                <table class="table table-dark">
                    <thead>
                        <th style="width: 60px;"></th>
                        <th v-if="tabledata.filter(i=> i.subCategoryVariableValues).length > 0">Subcategory</th>
                        <th style="width: 60px;">#</th>
                        <th>Time</th>
                        <th>Platform</th>
                        <th>Submitted</th>
                        <th v-for="variable in variables?.filter(i => tabledata.filter(el => el[i.id]).length > 0)" :key="variable.id">
                            {{ variable.name }}
                        </th>
                    </thead>
                    <tbody>
                        <tr v-for="item in tabledata.filter(i => (showalldata || i.isPersonalBest))" :key="item.id" style="white-space: nowrap;">
                            <td>
                                <div class="d-table" style="border:none; border-collapse:collapse; border-spacing:0;">
                                    <div class="d-table-row">
                                        <div class="d-table-cell" style="border:none; padding:0px; vertical-align: middle;">
                                            <span><a href="#/" draggable="false"><i class="fas fa-play-circle fa-lg" :data-id="item.id" @click="showSpeedRunDetails"></i></a></span>
                                        </div>
                                        <div v-if="item.isPersonalBest && tabledata.filter(i => i.gameID == item.gameID && i.categoryID == item.categoryID && i.levelID == item.levelID && i.subCategoryVariableValueIDs == item.subCategoryVariableValueIDs).length > 1" class="d-table-cell pl-2" style="border:none; padding:0px; vertical-align: bottom;">
                                            <span><a href="#/" draggable="false"><img src="/dist/fonts/bar-chart.svg" class="img-fluid align-self-center" alt="Responsive image" style="min-width:18px;" :data-id="item.id" @click="showSpeedRunCharts"></a></span>                                
                                        </div>                                        
                                    </div>
                                </div>
                            </td>
                            <td v-if="item.subCategoryVariableValues">
                                <span>{{ item.subCategoryVariableValues }}</span>
                            </td>
                            <td>
                                <span>{{ item.rank ?? '-' }}</span>
                            </td>
                            <td>
                                <span>{{ showmilliseconds ? item.primaryTimeString : item.primaryTimeSecondsString }}</span>               
                            </td>
                            <td>
                                <span>{{ item.platform?.name }}</span>               
                            </td> 
                            <td>
                                <span v-tippy="item.relativeDateSubmittedString">{{ dateFormatter(item.dateSubmitted) }}</span>               
                            </td>                             
                            <td v-for="variable in variables?.filter(i => tabledata.filter(el => el[i.id]).length > 0)" :key="variable.id">
                                <div>{{ item[variable.id] }}</div>
                            </td>                                                                       
                        </tr>
                    </tbody>
                </table> 
            </div>
        </div>
        <modal v-if="showDetailModal" contentclass="cmv-modal-lg" @close="showDetailModal = false">
            <template v-slot:title>
                Details
            </template>
            <div class="container p-0">
                <speedrun-edit :gameid="selectedSpeedRun.gameID.toString()" :speedrunid="selectedSpeedRun.id.toString()" :readonly="true" />
            </div>
        </modal>   
        <modal v-if="showChartModal" contentclass="cmv-modal-xl" bodyclass="p-0" @close="showChartModal = false">
            <template v-slot:title>
                User Charts
            </template>
            <div class="container p-0">
                <user-speedrun-chart-container :showmilliseconds="showmilliseconds" :gameid="selectedSpeedRun.gameID.toString()" :categoryid="selectedSpeedRun.categoryID.toString()" :levelid="selectedSpeedRun.levelID?.toString()" :variablevalues="selectedSpeedRun.subCategoryVariableValueIDs" :userid="userid" :title="title" :istimerasc="istimerasc"></user-speedrun-chart-container>
            </div>
        </modal>            
    </div>   
</template>
<script>
    const dayjs = require('dayjs');
    import 'tabulator-tables/dist/css/bootstrap/tabulator_bootstrap.min.css'
    import { polyfill } from "mobile-drag-drop";
    import { scrollBehaviourDragImageTranslateOverride } from "mobile-drag-drop/scroll-behaviour";

    export default {
        name: "UserSpeedRunGrid",
        props: {
            userid: String,
            gamename: String,
            categoryname: String,
            levelname:String,
            tabledata: Array,
            showmilliseconds: Boolean,
            variables: Array,
            istimerasc: Boolean,
            showalldata: Boolean          
        },
        data() {
            return {   
                tableData: [],          
                loading: true,
                selectedSpeedRun: {},
                showDetailModal: false,
                showChartModal: false,
                pageSize: 100
            }
        },
        computed: {
            isMediaMedium: function () {
                return window.innerWidth > 768;
            },
            title: function () {
                var result = '';
                result = [this.gamename, this.categoryname, this.levelname, this.selectedSpeedRun.subCategoryVariableValues].join(' - ');
                result = result.replace(/^[ -]+|[ -]+$/g, '');
                
                return result;
            }
        },                  
        mounted: function() {
            polyfill({
                dragImageTranslateOverride: scrollBehaviourDragImageTranslateOverride
            });            
            this.loadData();
            window.speedRunGridVue = this;
            window.addEventListener( 'touchmove', function() {}, {passive: false});
        },
        methods: {
            loadData() {
                var that = this;
                this.loading = true;
                this.tableData = that.tabledata;

                if (this.istimerasc) {
                    this.tableData = this.tableData.sort((a, b) => { return b?.primaryTime.ticks - a?.primaryTime.ticks });
                }

                that.initGrid(this.tableData);
                that.loading = false;
            },     
            initGrid(tableData) {
                var that = this;
                
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
            },  
            dateFormatter(value) {
                var html = '';

                if (value){
                    html += dayjs(value).format("MM/DD/YYYY");
                }

                return html;
            },                        
            showSpeedRunDetails(event) {
                var id = event.target.getAttribute('data-id');             
                this.selectedSpeedRun = this.tabledata.find(i => i.id == id);
                this.showDetailModal = true;
            },
            showSpeedRunCharts(event) {
                var id = event.target.getAttribute('data-id');             
                this.selectedSpeedRun = this.tabledata.find(i => i.id == id);
                this.showChartModal = true;
            }                           
        }             
    };
</script>










