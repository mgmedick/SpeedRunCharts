<template>
    <div>
        <div class="row no-gutters">
            <div class="col-lg-6">
                <div class="embed-responsive embed-responsive-4by3">
                    <div class="embed-responsive-item">                      
                        <div v-if="loading" class="d-flex" style="height:100%;">
                            <div class="m-auto">
                                <i class="fas fa-spinner fa-spin fa-lg"></i>
                            </div>
                        </div>
                        <div v-else style="height:100%;">
                            <player-speedrun-top-chart chartconainerid="divChart3" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" @onexpandchartclick="onExpandChartClick($event, 3)"></player-speedrun-top-chart>                   
                        </div>
                    </div>
                </div>                
            </div>
            <div class="col-lg-6">
                <div class="embed-responsive embed-responsive-4by3">
                    <div class="embed-responsive-item">                    
                        <div v-if="loading" class="d-flex" style="height:100%;">
                            <div class="m-auto">
                                <i class="fas fa-spinner fa-spin fa-lg"></i>
                            </div>
                        </div>
                        <div v-else style="height:100%;">
                            <player-speedrun-percentile-chart chartconainerid="divChart2" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" @onexpandchartclick="onExpandChartClick($event, 2)"></player-speedrun-percentile-chart>                
                        </div>
                    </div>                          
                </div>
            </div>
            <div class="col-lg-12">
                <div class="embed-responsive embed-responsive-4by3">
                    <div class="embed-responsive-item">
                        <div v-if="loading" class="d-flex" style="height:100%;">
                            <div class="m-auto">
                                <i class="fas fa-spinner fa-spin fa-lg"></i>
                            </div>
                        </div>
                        <div v-else style="height:100%;">
                            <player-speedrun-personalbest-chart chartconainerid="divChart1" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" @onexpandchartclick="onExpandChartClick($event, 1)"></player-speedrun-personalbest-chart>                
                        </div>
                    </div>
                </div>
            </div>
        </div>            
    </div>           
</template>
<script>
    import axios from 'axios';
    
    export default {
        name: "PlayerSpeedRunChartContainer",
        props: {
            gameid: String,
            categorytypeid: String,
            categoryid: String,
            levelid: String,
            variablevalues: String,            
            userid: String,            
            title: String,
            istimerasc: Boolean,
            showmilliseconds: Boolean
        },
        data() {
            return {
                tabledata: [],
                loading: true,
                showChartModal: false,
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

                axios.get('/SpeedRun/GetPlayerSpeedRunChartData', { params: { gameID: this.gameid, categoryTypeID: this.categorytypeid, categoryID: this.categoryid, levelID: this.levelid, subCategoryVariableValueIDs: this.variablevalues, userID: this.userid } })
                    .then(res => {
                        that.tabledata = res.data;                                             
                        that.loading = false;  
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            },  
            onExpandChartClick(event, chartID) {
                this.selectedChartID = chartID;                 
                this.showChartModal = true;
            }                                       
        }
    }
</script>






