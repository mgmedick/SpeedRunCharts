<template>
    <div class="card" style="border: none; border-radius: 0px;">
        <div class="container">
            <div class="row">
                <div class="col-lg-4" style="min-height:300px;">
                    <div v-if="loading" class="d-flex" style="height:100%;">
                        <div class="m-auto">
                            <i class="fas fa-spinner fa-spin fa-lg"></i>
                        </div>
                    </div>
                    <div v-else @click="onChartClick($event, 1)" class="expandable" style="height:100%;">
                        <game-speedrun-count-line-chart chartconainerid="divGameChart1" :tabledata="tabledata" :categorytypeid="categorytypeid" :categories="categories" :levels="levels" :showmilliseconds="showmilliseconds"></game-speedrun-count-line-chart>                
                    </div>
                </div>
                <div class="col-lg-4" style="min-height:300px;">
                    <div v-if="loading" class="d-flex" style="height:100%;">
                        <div class="m-auto">
                            <i class="fas fa-spinner fa-spin fa-lg"></i>
                        </div>
                    </div>
                    <div v-else @click="onChartClick($event, 2)" class="expandable" style="height:100%;">
                        <!-- <leaderboard-percentile-chart chartconainerid="divChart2" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc"></leaderboard-percentile-chart>                 -->
                    </div>
                </div>
                <div class="col-lg-4" style="min-height:300px;">
                    <div v-if="loading" class="d-flex" style="height:100%;">
                        <div class="m-auto">
                            <i class="fas fa-spinner fa-spin fa-lg"></i>
                        </div>
                    </div>
                    <div v-else @click="onChartClick($event, 3)" class="expandable" style="height:100%;">
                        <!-- <leaderboard-top-chart chartconainerid="divChart3" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc"></leaderboard-top-chart>                 -->
                    </div>
                </div>
            </div>            
            <modal v-if="showChartModal && !loading" contentclass="cmv-modal-lg" bodyclass="p-0" @close="showChartModal = false">
                <template v-slot:title>
                    {{ chartModalTitle }}
                </template>
                <div v-if="selectedChartID == 1">            
                    <game-speedrun-count-line-chart chartconainerid="divChartModal" :tabledata="tabledata" :categorytypeid="categorytypeid" :categories="categories" :levels="levels" :showmilliseconds="showmilliseconds" :ismodal="true"></game-speedrun-count-line-chart>                
                </div>
                <div v-else-if="selectedChartID == 2">
                    <!-- <leaderboard-percentile-chart chartconainerid="divChartModal" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" :ismodal="true"></leaderboard-percentile-chart>                 -->
                </div>
                <div v-else-if="selectedChartID == 3">
                    <!-- <leaderboard-top-chart chartconainerid="divChartModal" :tabledata="tabledata" :title="title" :showmilliseconds="showmilliseconds" :istimerasc="istimerasc" :ismodal="true"></leaderboard-top-chart>                 -->
                </div>            
            </modal>
        </div>
    </div>
</template>
<script>
    import axios from 'axios';
    
    export default {
        name: "GameChartContainer",
        props: {
            gameid: String,
            categorytypeid: String,
            categoryid: String,
            categories: Array,
            levels: Array,
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
                        title = 'Number of Runs (Last 12 months)';
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

                axios.get('/SpeedRun/GetGameChartData', { params: { gameID: this.gameid, categoryTypeID: this.categorytypeid, categoryID: this.categoryid } })
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






