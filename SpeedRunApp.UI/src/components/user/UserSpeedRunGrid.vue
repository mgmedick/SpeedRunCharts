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
                <table class="table table-sm table-dark">
                    <tbody>
                        <tr v-for="item in tabledata.filter(i => (showalldata || i.isPersonalBest))" :key="item.id">
                            <td style="width: 5%; vertical-align: middle;">
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
                            <td style="width: 33%; vertical-align: middle;">
                                <div><span style="font-weight: bold;">{{ item.categoryName }}</span></div>
                                <div v-if="item.levelName"><span style="font-weight: 600;">{{ item.levelName }}</span></div>
                                <div v-if="item.subCategoryVariableValues">
                                    <span>{{ item.subCategoryVariableValues }}</span>
                                </div>                                
                            </td>
                            <td style="width: 32%; vertical-align: middle;">
                                <div><a :href="'/Game/GameDetails/' + gameabbr + '?speedRunID=' + item.speedRunComID" class="text-primary"><i v-if="getIconClass(item.rank)" class="fa fa-trophy pr-1" :class="getIconClass(item.rank)"></i><span>{{ item.rankString ?? '-' }}</span></a></div>                                
                                <div><span>{{ showmilliseconds ? item.primaryTimeString : item.primaryTimeSecondsString }}</span></div>               
                            </td>
                            <td style="width: 30%; vertical-align: middle;">
                                <div><span>{{ item.platform?.name }}</span></div>  
                                <div><span>{{ item.relativeDateSubmittedStringShort }}</span></div>               
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
                <user-speedrun-chart-container :gameid="selectedSpeedRun.gameID.toString()" :categoryid="selectedSpeedRun.categoryID.toString()" :levelid="selectedSpeedRun.levelID?.toString()" :variablevalues="selectedSpeedRun.subCategoryVariableValueIDs" :userid="userid" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="selectedSpeedRun.isTimerAscending"></user-speedrun-chart-container>
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
            gameabbr: String,
            tabledata: Array,
            showmilliseconds: Boolean,
            variables: Array,
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
                result = [this.selectedSpeedRun.gameName, this.selectedSpeedRun.categoryName, this.selectedSpeedRun.levelName, this.selectedSpeedRun.subCategoryVariableValues].join(' - ');
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
            getIconClass: function (rank) {
                var iconClass = '';

                switch (rank) {
                    case 1:
                        iconClass = 'gold';
                        break;
                    case 2:
                        iconClass = 'silver';
                        break;
                    case 3:
                        iconClass = 'bronze';
                        break;
                }

                return iconClass;
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










