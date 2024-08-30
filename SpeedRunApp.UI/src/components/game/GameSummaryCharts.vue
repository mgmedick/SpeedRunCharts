<template>
    <div class="card mt-2" style="border: none; border-radius: 0px;">
        <div class="row no-gutters">
            <div class="col-lg-6">
                <div class="ratio ratio-4x3">
                    <div v-if="loading" class="d-flex" style="height:100%;">
                        <div class="m-auto">
                            <i class="fas fa-spinner fa-spin fa-lg"></i>
                        </div>
                    </div>
                    <div v-else style="height:100%;">
                        <game-summary-bar-chart chartconainerid="divGameChart1" :tabledata="tabledata" :categorytypeid="categorytypeid" :categoryid="categoryid" :categories="categories" :levels="levels" :variables="variables" :showmilliseconds="showmilliseconds" :subcaption="subcaption" @onexpandchartclick="onExpandChartClick($event, 1)"></game-summary-bar-chart>                                                 
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
                        <game-summary-line-chart chartconainerid="divGameChart2" :tabledata="tabledata" :categorytypeid="categorytypeid" :categoryid="categoryid" :categories="categories" :levels="levels" :variables="variables" :showmilliseconds="showmilliseconds" :subcaption="subcaption" @onexpandchartclick="onExpandChartClick($event, 2)"></game-summary-line-chart>                                      
                    </div>
                </div>
            </div>
        </div>   
        <div class="row no-gutters">
            <div class="col-lg-12">
                <div class="ratio ratio-4x3">
                    <div v-if="loading" class="d-flex" style="height:100%;">
                        <div class="m-auto">
                            <i class="fas fa-spinner fa-spin fa-lg"></i>
                        </div>
                    </div>
                    <div v-else style="height:100%;">
                        <game-summary-doughnut-chart chartconainerid="divGameChart3" :tabledata="tabledata" :categorytypeid="categorytypeid" :categoryid="categoryid" :categories="categories" :levels="levels" :subcategoryvariablevaluetabs="subcategoryvariablevaluetabs" :showmilliseconds="showmilliseconds" :subcaption="subcaption" @onexpandchartclick="onExpandChartClick($event, 3)"></game-summary-doughnut-chart>                                        
                    </div>
                </div>                        
            </div>               
        </div>
        <div ref="chartmodal" class="modal modal-xl" tabindex="-1">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">{{ chartModalTitle }}</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>  
                    </div>
                    <div class="modal-body">
                        <div v-if="selectedChartID == 1">
                            <div class="ratio ratio-4x3">   
                                <game-summary-bar-chart chartconainerid="divGameChartModal" :tabledata="tabledata" :categorytypeid="categorytypeid" :categoryid="categoryid" :categories="categories" :levels="levels" :variables="variables" :showmilliseconds="showmilliseconds" :subcaption="subcaption" :ismodal="true"></game-summary-bar-chart>  
                            </div>
                        </div>
                        <div v-else-if="selectedChartID == 2">
                            <div class="ratio ratio-4x3">  
                                <game-summary-line-chart chartconainerid="divGameChartModal" :tabledata="tabledata" :categorytypeid="categorytypeid" :categoryid="categoryid" :categories="categories" :levels="levels" :variables="variables" :showmilliseconds="showmilliseconds" :subcaption="subcaption" :ismodal="true"></game-summary-line-chart>                
                            </div>
                        </div>     
                        <div v-else-if="selectedChartID == 3">
                            <div class="ratio ratio-4x3">  
                                <game-summary-doughnut-chart chartconainerid="divGameChartModal" :tabledata="tabledata" :categorytypeid="categorytypeid" :categoryid="categoryid" :categories="categories" :levels="levels" :subcategoryvariablevaluetabs="subcategoryvariablevaluetabs" :showmilliseconds="showmilliseconds" :subcaption="subcaption" :ismodal="true"></game-summary-doughnut-chart>                
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
        name: "GameSummaryCharts",
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
                selectedChartID: 0
            }
        },
        computed: {                 
            chartModalTitle: function () {
                var title = '';

                switch(this.selectedChartID) {
                    case 1:
                        title = (this.categorytypeid == 0 ? 'Category' : 'Level') + ' Run Counts Chart';
                        break;
                    case 2:
                        title = (this.categorytypeid == 0 ? 'Category' : 'Level') + ' Run Counts (Last 12 Months) Chart';
                        break;
                    case 3:
                        title = (this.categorytypeid == 0 ? 'Category' : 'Level') + ' Run Distribution Chart';
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

                axios.get('/Game/GetGameSummaryChartData', { params: { gameID: this.gameid, categoryTypeID: this.categorytypeid, categoryID: this.categoryid } })
                    .then(res => {
                        that.tabledata = res.data;                                             
                        that.loading = false;  
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            },       
            onExpandChartClick(event, chartID) {
                this.selectedChartID = chartID;                 
                new Modal(this.$refs.chartmodal).show();
            }                          
        }
    }
</script>






