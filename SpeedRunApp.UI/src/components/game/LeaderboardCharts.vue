<template>
    <div class="card" style="border: none; border-radius: 0px;">
        <div class="card-header">
            <h5 class="mb-0">
                <div v-if="showcharts">
                    <a class="btn btn-link fw-bold text-decoration-none text-reset px-0" style="line-height: 15px;" href="#/" draggable="false" @click="$emit('onshowchartsclick', $event)"><i class="fa fa-chevron-down align-self-center"></i><i class="fa fa-chart-simple"></i><span class="ms-2">Hide Charts</span></a>
                </div>
                <div v-else>
                    <a class="btn btn-link fw-bold text-decoration-none text-reset px-0" style="line-height: 15px;" href="#/" draggable="false" @click="$emit('onshowchartsclick', $event)"><i class="fa fa-chevron-right align-self-center"></i><i class="fa fa-chart-simple"></i><span class="ms-2">Show Charts</span></a>
                </div>
            </h5>
        </div>
        <div v-if="showcharts">
            <div class="row no-gutters">
                <div class="col-lg-6">
                    <div class="ratio ratio-4x3">
                        <div v-if="loading" class="d-flex" style="height:100%;">
                            <div class="m-auto">
                                <i class="fas fa-spinner fa-spin fa-lg"></i>
                            </div>
                        </div>
                        <div v-else style="height:100%;">
                            <leaderboard-worldrecord-chart chartconainerid="divChart1" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" @onexpandchartclick="onExpandChartClick($event, 1)"></leaderboard-worldrecord-chart>                                 
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
                            <leaderboard-percentile-chart chartconainerid="divChart2" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" @onexpandchartclick="onExpandChartClick($event, 2)"></leaderboard-percentile-chart>                
                        </div>
                    </div>
                </div>
            </div>
            <div class="row no-gutters">
                <div class="col-lg-6">
                    <div class="ratio ratio-4x3">
                        <div v-if="loading" class="d-flex" style="height:100%;">
                            <div class="m-auto">
                                <i class="fas fa-spinner fa-spin fa-lg"></i>
                            </div>
                        </div>
                        <div v-else style="height:100%;">
                            <leaderboard-top-chart chartconainerid="divChart3" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" @onexpandchartclick="onExpandChartClick($event, 3)"></leaderboard-top-chart>                
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
                            <leaderboard-top-line-chart chartconainerid="divChart4" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" @onexpandchartclick="onExpandChartClick($event, 4)"></leaderboard-top-line-chart>                
                        </div>
                    </div>                         
                </div>                
            </div>          
        </div>
        <div ref="chartmodal" class="modal modal-xl" tabindex="-1">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">{{ chartModalTitle }}</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div v-if="selectedChartID == 1">
                            <div class="ratio ratio-4x3">   
                                <leaderboard-worldrecord-chart chartconainerid="divChartModal" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" :ismodal="true"></leaderboard-worldrecord-chart>                
                            </div>
                        </div>
                        <div v-else-if="selectedChartID == 2">
                            <div class="ratio ratio-4x3">  
                                <leaderboard-percentile-chart chartconainerid="divChartModal" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" :ismodal="true"></leaderboard-percentile-chart>                
                            </div>
                        </div>     
                        <div v-else-if="selectedChartID == 3">
                            <div class="ratio ratio-4x3">  
                                <leaderboard-top-chart chartconainerid="divChartModal" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" :ismodal="true"></leaderboard-top-chart>                
                            </div>                
                        </div>  
                        <div v-else-if="selectedChartID == 4">
                            <div class="ratio ratio-4x3">  
                                <leaderboard-top-line-chart chartconainerid="divChartModal" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" :ismodal="true"></leaderboard-top-line-chart>                
                            </div>                
                        </div>                          
                    </div>
                </div>
            </div>
        </div>         
    </div>
</template>
<script>
    import axios from 'axios';
    import { Modal } from 'bootstrap';
  
    export default {
        name: "LeaderboardChartContainer",
        emits: ["onshowchartsclick"],
        props: {
            gameid: String,
            categorytypeid: String,
            categoryid: String,
            levelid: String,
            variablevalues: String,            
            showcharts: Boolean,
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
                        title = 'World Record History Chart';
                        break;
                    case 2:
                        title = 'Time Percentiles Chart';
                        break;
                    case 3:
                        title = 'Top 10 Runs Chart';
                        break;
                    case 4:
                        title = 'Top 10 Runs Players History Chart';
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

                axios.get('/Game/GetLeaderboardChartData', { params: { gameID: this.gameid, categoryTypeID: this.categorytypeid, categoryID: this.categoryid, levelID: this.levelid, subCategoryVariableValueIDs: this.variablevalues } })
                    .then(res => {
                        that.tabledata = res.data;                                             
                        that.loading = false;  
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            },  
            onExpandChartClick(event, chartID) {
                this.selectedChartID = chartID;                 

                this.$nextTick(function() {
                    new Modal(this.$refs.chartmodal).show();
                });                 
            }                        
        }
    }
</script>






