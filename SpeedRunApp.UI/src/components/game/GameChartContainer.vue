<template>
    <div class="card mt-2" style="border: none; border-radius: 0px;">
        <div class="container p-0">
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
                                <game-speedrun-count-bar-chart chartconainerid="divGameChart1" :tabledata="tabledata" :categorytypeid="categorytypeid" :categoryid="categoryid" :categories="categories" :levels="levels" :variables="variables" :showmilliseconds="showmilliseconds" :subcaption="subcaption" @onexpandchartclick="onExpandChartClick($event, 1)"></game-speedrun-count-bar-chart>                                                 
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
                            <div v-else style="width:100%; height:100%;"> 
                                <game-speedrun-count-line-chart chartconainerid="divGameChart2" :tabledata="tabledata" :categorytypeid="categorytypeid" :categoryid="categoryid" :categories="categories" :levels="levels" :variables="variables" :showmilliseconds="showmilliseconds" :subcaption="subcaption" @onexpandchartclick="onExpandChartClick($event, 2)"></game-speedrun-count-line-chart>                                      
                            </div>
                        </div>
                    </div>
                </div>
            </div>   
            <div class="row no-gutters">
                <div class="col-lg-12">
                    <div class="embed-responsive embed-responsive-4by3">
                        <div class="embed-responsive-item">
                            <div v-if="loading" class="d-flex" style="height:100%;">
                                <div class="m-auto">
                                    <i class="fas fa-spinner fa-spin fa-lg"></i>
                                </div>
                            </div>
                            <div v-else style="height:100%;">
                                <game-speedrun-count-doughnut-chart chartconainerid="divGameChart3" :tabledata="tabledata" :categorytypeid="categorytypeid" :categoryid="categoryid" :categories="categories" :levels="levels" :subcategoryvariablevaluetabs="subcategoryvariablevaluetabs" :showmilliseconds="showmilliseconds" :subcaption="subcaption" @onexpandchartclick="onExpandChartClick($event, 3)"></game-speedrun-count-doughnut-chart>                                        
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
                            <game-speedrun-count-bar-chart chartconainerid="divChartModal" :tabledata="tabledata" :categorytypeid="categorytypeid" :categoryid="categoryid" :categories="categories" :levels="levels" :variables="variables" :showmilliseconds="showmilliseconds" :subcaption="subcaption" :ismodal="true"></game-speedrun-count-bar-chart>  
                        </div>
                    </div>                                 
                </div>
                <div v-else-if="selectedChartID == 2">
                    <div class="embed-responsive embed-responsive-4by3">
                        <div class="embed-responsive-item">  
                            <game-speedrun-count-line-chart chartconainerid="divChartModal" :tabledata="tabledata" :categorytypeid="categorytypeid" :categoryid="categoryid" :categories="categories" :levels="levels" :variables="variables" :showmilliseconds="showmilliseconds" :subcaption="subcaption" :ismodal="true"></game-speedrun-count-line-chart>                
                        </div>
                    </div> 
                </div>     
                <div v-else-if="selectedChartID == 3">
                    <div class="embed-responsive embed-responsive-4by3">
                        <div class="embed-responsive-item">  
                            <game-speedrun-count-doughnut-chart chartconainerid="divChartModal" :tabledata="tabledata" :categorytypeid="categorytypeid" :categoryid="categoryid" :categories="categories" :levels="levels" :subcategoryvariablevaluetabs="subcategoryvariablevaluetabs" :showmilliseconds="showmilliseconds" :subcaption="subcaption" :ismodal="true"></game-speedrun-count-doughnut-chart>                
                        </div>
                    </div>                 
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
            variables: Array,
            subcategoryvariablevaluetabs: Array,
            subcaption: String,
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

                switch(this.selectedChartID) {
                    case 1:
                        title = (this.categorytypeid == 0 ? 'Category' : 'Level') + ' Counts Chart';
                        break;
                    case 2:
                        title = (this.categorytypeid == 0 ? 'Category' : 'Level') + ' Counts (Last 12 Months) Chart';
                        break;
                    case 3:
                        title = (this.categorytypeid == 0 ? 'Category' : 'Level') + ' Distribution Chart';
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
            onExpandChartClick(event, chartID) {
                this.selectedChartID = chartID;                 
                this.showChartModal = true;
            }                          
        }
    }
</script>






