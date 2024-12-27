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
            <table class="table" :class="tableClass">
                <tbody>
                    <tr v-for="item in tabledata.filter(i => (showalldata || i.isPersonalBest) && (showmisc || !i.isMiscellaneous) && (!showwr || i.rank == 1))" :key="item.id" @click="showSpeedRunDetails(item.id)" style="cursor: pointer;">                   
                        <td style="vertical-align: middle;">
                            <div class="nowrap-elipsis"><span class="fw-bold">{{ item.categoryName }}</span></div>
                            <div v-if="item.levelName" class="nowrap-elipsis"><span class="fw-bold" style="font-style: italic;">{{ item.levelName }}</span></div>
                            <div v-if="item.subCategoryVariableValueNames" class="nowrap-elipsis">
                                <span>{{ item.subCategoryVariableValueNames }}</span>
                            </div>                                
                        </td>
                        <td style="vertical-align: middle;">
                            <div class="nowrap-elipsis"><a :href="'/Game/GameDetails/' + encodeURIComponent(gameabbr) + '?speedRunCode=' + item.code" class="text-decoration-none text-reset"><i v-if="getIconClass(item.rank)" class="fa fa-trophy pe-1" :class="getIconClass(item.rank)"></i><span>{{ item.rankString ?? '-' }}</span></a></div>                                
                            <div class="nowrap-elipsis"><span>{{ showmilliseconds ? item.primaryTimeMillisecondsString : item.primaryTimeSecondsString }}</span></div>               
                        </td>
                        <td style="width: auto; vertical-align: middle;">
                            <div class="nowrap-elipsis"><span>{{ item.platformName }}</span></div>  
                            <div class="nowrap-elipsis"><span>{{ item.relativeDateSubmittedStringShort }}</span></div>               
                        </td>
                        <td style="width: 5%; vertical-align: bottom;">
                            <div v-if="item.isPersonalBest" class="d-table-cell ps-2" style="padding-bottom: 5px;">
                                <span><a href="#/" class="text-decoration-none text-reset"><img src="/dist/fonts/bar-chart.svg" class="img-fluid align-self-center w-100" style="min-width: 25px;" alt="Responsive image" @click="showSpeedRunCharts($event, item.id)"></a></span>                                
                            </div>                                        
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
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button> 
                    </div>
                    <div class="modal-body">    
                        <speedrun-details ref="speedrundetails" v-if="selectedDetailSpeedRun" :speedrunid="selectedDetailSpeedRun.id" />                     
                    </div>
                </div>
            </div>
        </div> 
        <div ref="chartmodal" class="modal modal-xl" tabindex="-1">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">User Charts</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button> 
                    </div>
                    <div class="modal-body">
                        <player-speedrun-charts ref="playerspeedruncharts" v-if="selectedChartSpeedRun" :gameid="selectedChartSpeedRun.gameID.toString()" :categorytypeid="selectedChartSpeedRun.categoryTypeID.toString()" :categoryid="selectedChartSpeedRun.categoryID.toString()" :levelid="selectedChartSpeedRun.levelID?.toString()" :variablevalues="selectedChartSpeedRun.subCategoryVariableValueIDs" :playerid="playerid" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="selectedChartSpeedRun.isTimerAscending"></player-speedrun-charts>                         
                    </div>
                </div>
            </div>
        </div>           
    </div>   
</template>
<script>
    const dayjs = require('dayjs');
    // import 'tabulator-tables/dist/css/tabulator_bootstrap5.min.css'
    // import { polyfill } from "mobile-drag-drop";
    // import { scrollBehaviourDragImageTranslateOverride } from "mobile-drag-drop/scroll-behaviour";
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
                selectedDetailSpeedRun: null,
                selectedChartSpeedRun: null,
                pageSize: 100,
                theme: document.documentElement.dataset.bsTheme
            }
        },
        computed: {
            title: function () {
                var result = '';
                if (this.selectedChartSpeedRun) {
                    result = [this.selectedChartSpeedRun.gameName, this.selectedChartSpeedRun.categoryName, this.selectedChartSpeedRun.levelName, this.selectedChartSpeedRun.subCategoryVariableValueNames].join(' - ');                
                    result = result.replace(/^[ -]+|[ -]+$/g, '');
                }  
                return result;
            },            
            tableClass: function() {
                return this.theme == 'dark' ? "table-dark" : ""; 
            }              
        },              
        mounted: function() {
            var that = this;
            
            // polyfill({
            //     dragImageTranslateOverride: scrollBehaviourDragImageTranslateOverride
            // });            

            // that.$refs.detailmodal.addEventListener('show.bs.modal', event => {
            //     that.$refs.speedrundetails.loadData();
            // }); 

            that.$refs.detailmodal.addEventListener('hidden.bs.modal', event => {
                that.selectedDetailSpeedRun = null;
            });                    
            
            // that.$refs.chartmodal.addEventListener('show.bs.modal', event => {
            //     that.$refs.playerspeedruncharts.loadData();
            // }); 

            that.$refs.chartmodal.addEventListener('hidden.bs.modal', event => {
                that.selectedChartSpeedRun = null;
            });                        

            this.loadData();

            window.speedRunGridVue = this;
            window.addEventListener('touchmove', function() {}, {passive: false});
            window.addEventListener('themeUpdate', this.onThemeUpdate);
        },
        destroyed() {
            window.removeEventListener('themeUpdate', this.onThemeUpdate);
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
            showSpeedRunDetails(id) {
                this.selectedDetailSpeedRun = this.tabledata.find(i => i.id == id);
                
                this.$nextTick(function() {
                    new Modal(this.$refs.detailmodal).show();
                });
            },
            showSpeedRunCharts(event, id) {
                event.stopPropagation();
                event.target.blur();
                this.selectedChartSpeedRun = this.tabledata.find(i => i.id == id);

                this.$nextTick(function() {
                    new Modal(this.$refs.chartmodal).show();
                });
            },
            onThemeUpdate() {
                this.theme = document.documentElement.dataset.bsTheme;
                this.loadData();
            }               
        }             
    };
</script>










