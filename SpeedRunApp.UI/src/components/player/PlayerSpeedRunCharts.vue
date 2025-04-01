<template>
    <div>
        <div class="row g-3 mb-3">
            <div class="col-lg-6">
                <div class="ratio ratio-4x3">                      
                    <div v-if="loading" class="d-flex" style="height:100%;">
                        <div class="m-auto">
                            <i class="fas fa-spinner fa-spin fa-lg"></i>
                        </div>
                    </div>
                    <div v-else style="height:100%;">
                        <player-speedrun-top-chart chartconainerid="divChart3" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" @onexpandchartclick="onExpandChartClick($event, 3)" :ismodal="true"></player-speedrun-top-chart>                   
                    </div>
                </div>               
            </div>
            <div class="col-lg-6">
                <div class="ratio ratio-4x3">                    
                    <div v-if="loading" class="d-flex" style="height:100%;">
                        <div class="m-auto">
                            <i class="fas fa-spinner fa-spin fa-lg"></i>
                        </div>
                    </div>
                    <div v-else style="height:100%;">
                        <player-speedrun-percentile-chart chartconainerid="divChart2" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" @onexpandchartclick="onExpandChartClick($event, 2)" :ismodal="true"></player-speedrun-percentile-chart>                
                    </div>
                </div>                          
            </div>
            <div class="col-lg-12">
                <div class="ratio ratio-4x3">
                    <div v-if="loading" class="d-flex" style="height:100%;">
                        <div class="m-auto">
                            <i class="fas fa-spinner fa-spin fa-lg"></i>
                        </div>
                    </div>
                    <div v-else style="height:100%;">
                        <player-speedrun-personalbest-chart chartconainerid="divChart1" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" @onexpandchartclick="onExpandChartClick($event, 1)" :ismodal="true"></player-speedrun-personalbest-chart>                
                    </div>
                </div>
            </div>
        </div>                    
    </div>           
</template>
<script>
    import axios from 'axios';
    
    export default {
        name: "PlayerSpeedRunCharts",
        props: {
            gameid: String,
            categorytypeid: String,
            categoryid: String,
            levelid: String,
            variablevalues: String,            
            playerid: String,            
            title: String,
            istimerasc: Boolean,
            showmilliseconds: Boolean
        },
        data() {
            return {
                tabledata: [],
                loading: true,
                selectedChartID: 0
            }
        },
        computed: {                 
            chartModalTitle: function () {
                var title = '';

                switch(this.selectedChartID){
                    case 1:
                        title = 'Personal Best History Chart';
                        break;
                    case 2:
                        title = 'Time Percentiles Chart';
                        break;
                    case 3:
                        title = 'Top 10 Runs Chart';
                        break;
                }

                return title;
            }                                   
        },                                     
        mounted: function () {
            this.loadData();
        },      
        methods: {                        
            loadData() {
                var that = this;
                this.loading = true;

                axios.get('/Player/GetPlayerSpeedRunChartData', { params: { gameID: this.gameid, categoryTypeID: this.categorytypeid, categoryID: this.categoryid, levelID: this.levelid, subCategoryVariableValueIDs: this.variablevalues, playerID: this.playerid } })
                    .then(res => {
                        that.tabledata = res.data;                                             
                        that.loading = false;  
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            },  
            onExpandChartClick(event, chartID) {
                this.selectedChartID = chartID;              
            }                                   
        }
    }
</script>






