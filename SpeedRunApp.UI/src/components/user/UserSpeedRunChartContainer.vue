<template>
    <div class="card" style="border: none; border-radius: 0px;">
        <div class="card-header">
            <h5 class="mb-0">
                <div v-if="showcharts">
                    <a class="btn btn-link font-weight-bold d-flex align-items-end" style="line-height: 15px;" href="#/" draggable="false" @click="$emit('onshowchartsclick', $event)"><i class="fa fa-chevron-down align-self-center"></i><img src="/dist/fonts/bar-chart.svg" class="img-fluid brand-logo align-self-center mx-2" alt="Responsive image">Hide Leaderboard Charts</a>
                </div>
                <div v-else>
                    <a class="btn btn-link font-weight-bold d-flex align-items-end" style="line-height: 15px;" href="#/" draggable="false" @click="$emit('onshowchartsclick', $event)"><i class="fa fa-chevron-right align-self-center"></i><img src="/dist/fonts/bar-chart.svg" class="img-fluid brand-logo align-self-center mx-2" alt="Responsive image">Show Leaderboard Charts</a>
                </div>
            </h5>
        </div>
        <div v-if="showcharts" class="container p-0">
            <div class="row no-gutters">
                <div class="col-lg-4">
                    <div class="embed-responsive embed-responsive-4by3">
                        <div class="embed-responsive-item">
                            <div v-if="loading" class="d-flex" style="height:100%;">
                                <div class="m-auto">
                                    <i class="fas fa-spinner fa-spin fa-lg"></i>
                                </div>
                            </div>
                            <div v-else style="height:100%;">
                                <user-speedrun-personalbest-chart chartconainerid="divChart1" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" @onexpandchartclick="onExpandChartClick($event, 1)"></user-speedrun-personalbest-chart>                
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4">
                    <div class="embed-responsive embed-responsive-4by3">
                        <div class="embed-responsive-item">                    
                            <div v-if="loading" class="d-flex" style="height:100%;">
                                <div class="m-auto">
                                    <i class="fas fa-spinner fa-spin fa-lg"></i>
                                </div>
                            </div>
                            <div v-else style="height:100%;">
                                <user-speedrun-percentile-chart chartconainerid="divChart2" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" @onexpandchartclick="onExpandChartClick($event, 2)"></user-speedrun-percentile-chart>                
                            </div>
                        </div>                          
                    </div>
                </div>
                <div class="col-lg-4">
                    <div class="embed-responsive embed-responsive-4by3">
                        <div class="embed-responsive-item">                      
                            <div v-if="loading" class="d-flex" style="height:100%;">
                                <div class="m-auto">
                                    <i class="fas fa-spinner fa-spin fa-lg"></i>
                                </div>
                            </div>
                            <div v-else style="height:100%;">
                                <user-speedrun-top-chart chartconainerid="divChart3" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" @onexpandchartclick="onExpandChartClick($event, 3)"></user-speedrun-top-chart>                   
                            </div>
                        </div>
                    </div>
                </div>
            </div>            
        </div>
        <modal v-if="showChartModal && !loading" contentclass="cmv-modal-xl" bodyclass="p-0" @close="showChartModal = false">
            <template v-slot:title>
                {{ chartModalTitle }}
            </template>
            <div v-if="selectedChartID == 1">   
                <div class="embed-responsive embed-responsive-4by3">
                    <div class="embed-responsive-item">         
                        <user-speedrun-personalbest-chart chartconainerid="divChartModal" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" :ismodal="true"></user-speedrun-personalbest-chart>                
                    </div>
                </div>
            </div>
            <div v-else-if="selectedChartID == 2">
                <div class="embed-responsive embed-responsive-4by3">
                    <div class="embed-responsive-item">                  
                        <user-speedrun-percentile-chart chartconainerid="divChartModal" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" :ismodal="true"></user-speedrun-percentile-chart>                
                    </div>
                </div>            
            </div>
            <div v-else-if="selectedChartID == 3">
                <div class="embed-responsive embed-responsive-4by3">
                    <div class="embed-responsive-item">   
                        <user-speedrun-top-chart chartconainerid="divChartModal" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" :ismodal="true"></user-speedrun-top-chart>                
                    </div>
                </div> 
            </div>            
        </modal>              
    </div>
</template>
<script>
    import axios from 'axios';
    
    export default {
        name: "UserSpeedRunChartContainer",
        emits: ["onshowchartsclick"],
        props: {
            gameid: String,
            categorytypeid: String,
            categoryid: String,
            levelid: String,
            variablevalues: String,            
            userid: String,            
            showcharts: Boolean,
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
                        title = 'Personal Bests Chart';
                        break;
                    case 2:
                        title = 'Time Percentiles Chart';
                        break;
                    case 3:
                        title = 'Top 10 Chart';
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

                axios.get('/SpeedRun/GetUserSpeedRunGridData', { params: { gameID: this.gameid, categoryID: this.categoryid, levelID: this.levelid, subCategoryVariableValueIDs: this.variablevalues, userID: this.userid } })
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






