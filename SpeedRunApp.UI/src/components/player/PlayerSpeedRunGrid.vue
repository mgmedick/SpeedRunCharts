<template>
    <div>      
        <div v-if="loading">
            <div class="d-flex">
                <div class="mx-auto">
                    <i class="fas fa-spinner fa-spin fa-lg"></i>
                </div>
            </div>
        </div>        
        <div class="table-responsive mt-2">
            <table class="table table-sm table-dark">
                <tbody>
                    <tr v-for="item in tabledata.filter(i => (showalldata || i.isPersonalBest) && (showmisc || !i.isMiscellaneous) && (!showwr || i.rank == 1))" :key="item.id">
                        <td style="width: 5%; vertical-align: middle;">
                            <div class="d-table" style="border:none; border-collapse:collapse; border-spacing:0;">
                                <div class="d-table-row">
                                    <div class="d-table-cell" style="border:none; padding:0px; vertical-align: middle;">
                                        <span><a href="#/" draggable="false"><i class="fas fa-play-circle fa-lg" :data-id="item.id" @click="showSpeedRunDetails"></i></a></span>
                                    </div>
                                    <div v-if="item.isPersonalBest && tabledata.filter(i => i.gameID == item.gameID && i.categoryID == item.categoryID && i.levelID == item.levelID && i.subCategoryVariableValueIDs == item.subCategoryVariableValueIDs).length > 1" class="d-table-cell ps-2" style="border:none; padding:0px; vertical-align: bottom;">
                                        <span><a href="#/" draggable="false"><img src="/dist/fonts/bar-chart.svg" class="img-fluid align-self-center" alt="Responsive image" style="min-width:18px;" :data-id="item.id" @click="showSpeedRunCharts"></a></span>                                
                                    </div>                                        
                                </div>
                            </div>
                        </td>                        
                        <td style="width: 50%; vertical-align: middle;">
                            <div><span class="fw-bold">{{ item.categoryName }}</span></div>
                            <div v-if="item.levelName"><span class="fw-bold" style="font-style: italic;">{{ item.levelName }}</span></div>
                            <div v-if="item.subCategoryVariableValueNames">
                                <span style="font-size: 13px;">{{ item.subCategoryVariableValueNames }}</span>
                            </div>                                
                        </td>
                        <td style="width: 25%; vertical-align: middle;">
                            <div><a :href="'/Game/GameDetails/' + encodeURIComponent(gameabbr) + '?speedRunCode=' + item.code" class="text-primary"><i v-if="getIconClass(item.rank)" class="fa fa-trophy pe-1" :class="getIconClass(item.rank)"></i><span>{{ item.rankString ?? '-' }}</span></a></div>                                
                            <div><span style="font-size: 13px;">{{ showmilliseconds ? item.primaryTimeMillisecondsString : item.primaryTimeSecondsString }}</span></div>               
                        </td>
                        <td class="show-md" style="width: auto; vertical-align: middle;">
                            <div><span>{{ item.platformName }}</span></div>  
                            <div><span>{{ item.relativeDateSubmittedStringShort }}</span></div>               
                        </td>
                    </tr>
                </tbody>
            </table> 
        </div>
        <div ref="detailmodal" class="modal modal-lg" tabindex="-1">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Details</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>  
                    </div>
                    <div class="modal-body">                         
                    </div>
                </div>
            </div>
        </div>  
        <div ref="chartmodal" class="modal modal-xl" tabindex="-1">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">User Charts</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>  
                    </div>
                    <div class="modal-body">
                        <player-speedrun-charts ref="playerspeedruncharts" v-if="selectedSpeedRun" :gameid="selectedSpeedRun.gameID.toString()" :categorytypeid="selectedSpeedRun.categoryTypeID.toString()" :categoryid="selectedSpeedRun.categoryID.toString()" :levelid="selectedSpeedRun.levelID?.toString()" :variablevalues="selectedSpeedRun.subCategoryVariableValueIDs" :playerid="playerid" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="selectedSpeedRun.isTimerAscending"></player-speedrun-charts>                         
                    </div>
                </div>
            </div>
        </div>           
    </div>   
</template>
<script>
    const dayjs = require('dayjs');
    import 'tabulator-tables/dist/css/bootstrap/tabulator_bootstrap.min.css'
    import { polyfill } from "mobile-drag-drop";
    import { scrollBehaviourDragImageTranslateOverride } from "mobile-drag-drop/scroll-behaviour";
    import { Modal } from 'bootstrap';

    export default {
        name: "PlayerSpeedRunGrid",
        props: {
            playerid: String,
            gameabbr: String,
            tabledata: Array,
            showmilliseconds: Boolean,
            showalldata: Boolean,
            showmisc: Boolean,
            showwr: Boolean,
            variables: Array    
        },
        data() {
            return {   
                tableData: [],          
                loading: true,
                selectedSpeedRun: null,
                pageSize: 100
            }
        },
        computed: {
            title: function () {
                var result = '';
                if (this.selectedSpeedRun) {
                    result = [this.selectedSpeedRun.gameName, this.selectedSpeedRun.categoryName, this.selectedSpeedRun.levelName, this.selectedSpeedRun.subCategoryVariableValueNames].join(' - ');                
                    result = result.replace(/^[ -]+|[ -]+$/g, '');
                }  
                return result;
            }
        },                  
        mounted: function() {
            var that = this;
            
            polyfill({
                dragImageTranslateOverride: scrollBehaviourDragImageTranslateOverride
            });            
            this.loadData();

            that.$refs.chartmodal.addEventListener('show.bs.modal', event => {
                that.$refs.playerspeedruncharts.loadData();
            }); 

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
                new Modal(this.$refs.detailmodal).show();
            },
            showSpeedRunCharts(event) {
                var id = event.target.getAttribute('data-id');             
                this.selectedSpeedRun = this.tabledata.find(i => i.id == id);

                this.$nextTick(function() {
                    new Modal(this.$refs.chartmodal).show();
                });
            }
        }             
    };
</script>










