<template>
    <div class="card" style="border: none; border-radius: 0px;">
        <div class="card-header" id="headingOne">
            <h5 class="mb-0">
                <div v-if="showcharts">
                    <a class="btn btn-link font-weight-bold d-flex align-items-end" style="line-height: 15px;" href="#/" draggable="false" @click="$emit('onshowchartsclick', $event)"><i class="fa fa-chevron-down align-self-center"></i><img src="/dist/fonts/bar-chart.svg" class="img-fluid brand-logo align-self-center mx-2" alt="Responsive image">Hide Charts</a>
                </div>
                <div v-else>
                    <a class="btn btn-link font-weight-bold d-flex align-items-end" style="line-height: 15px;" href="#/" draggable="false" @click="$emit('onshowchartsclick', $event)"><i class="fa fa-chevron-right align-self-center"></i><img src="/dist/fonts/bar-chart.svg" class="img-fluid brand-logo align-self-center mx-2" alt="Responsive image">Show Charts</a>
                </div>
            </h5>
        </div>
        <div class="container" :style="[ showcharts ? null : { display:'none' } ]">
            <div class="row">
                <div class="col-lg-4" style="min-height:300px;">
                    <div v-if="loading" class="d-flex" style="height:100%;">
                        <div class="m-auto">
                            <i class="fas fa-spinner fa-spin fa-lg"></i>
                        </div>
                    </div>
                    <div v-else @click="onChartClick($event, 1)" class="expandable" style="height:100%;">
                        <leaderboard-worldrecord-chart chartconainerid="divChart1" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc"></leaderboard-worldrecord-chart>                
                    </div>
                </div>
                <div class="col-lg-4" style="min-height:300px;">
                    <div v-if="loading" class="d-flex" style="height:100%;">
                        <div class="m-auto">
                            <i class="fas fa-spinner fa-spin fa-lg"></i>
                        </div>
                    </div>
                    <div v-else @click="onChartClick($event, 2)" class="expandable" style="height:100%;">
                        <leaderboard-percentile-chart chartconainerid="divChart2" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc"></leaderboard-percentile-chart>                
                    </div>
                </div>
                <div class="col-lg-4" style="min-height:300px;">
                    <div v-if="loading" class="d-flex" style="height:100%;">
                        <div class="m-auto">
                            <i class="fas fa-spinner fa-spin fa-lg"></i>
                        </div>
                    </div>
                    <div v-else @click="onChartClick($event, 3)" class="expandable" style="height:100%;">
                        <leaderboard-top-chart chartconainerid="divChart3" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc"></leaderboard-top-chart>                
                    </div>
                </div>
            </div>            
        </div>
        <modal v-if="showChartModal && !loading" contentclass="cmv-modal-lg" bodyclass="p-0" @close="showChartModal = false">
            <template v-slot:title>
                {{ chartModalTitle }}
            </template>
            <div v-if="selectedChartID == 1">            
                <leaderboard-worldrecord-chart chartconainerid="divChartModal" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" :ismodal="true"></leaderboard-worldrecord-chart>                
            </div>
            <div v-else-if="selectedChartID == 2">
                <leaderboard-percentile-chart chartconainerid="divChartModal" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" :ismodal="true"></leaderboard-percentile-chart>                
            </div>
            <div v-else-if="selectedChartID == 3">
                <leaderboard-top-chart chartconainerid="divChartModal" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" :ismodal="true"></leaderboard-top-chart>                
            </div>            
        </modal>              
    </div>
</template>
<script>
    import axios from 'axios';
    
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
                showChartModal: false,
                selectedChartID: 0
            }
        },
        computed: {                 
            chartModalTitle: function () {
                var title = '';

                switch(this.selectedChartID){
                    case 1:
                        title = 'World Records Chart';
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

                axios.get('/SpeedRun/GetLeaderboardGridData', { params: { gameID: this.gameid, categoryID: this.categoryid, levelID: this.levelid, subCategoryVariableValueIDs: this.variablevalues, showAllData: true } })
                    .then(res => {
                        that.tabledata = res.data;                                             
                        that.loading = false;  
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            },  
            onChartClick(event, chartID) {
                if (!event.target.innerHTML || event.target.innerHTML.indexOf("Export") == -1){
                    this.showChartModal = true;
                    this.selectedChartID = chartID;
                }
            }               
        }
    }
</script>






