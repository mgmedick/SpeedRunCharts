<template>
    <div class="card mt-2" style="border: none; border-radius: 0px;">
        <div class="container">
            <div class="row">
                <div class="col-lg-6" style="min-height:400px;">
                    <div v-if="loading" class="d-flex" style="height:100%;">
                        <div class="m-auto">
                            <i class="fas fa-spinner fa-spin fa-lg"></i>
                        </div>
                    </div>
                    <div v-else style="height:100%;">
                        <game-speedrun-count-doughnut-chart chartconainerid="divGameChart1" :tabledata="tabledata" :categorytypeid="categorytypeid" :categoryid="categoryid" :categories="categories" :levels="levels" :subcategoryvariablevaluetabs="subcategoryvariablevaluetabs" :showmilliseconds="showmilliseconds" :subcaption="subcaption"></game-speedrun-count-doughnut-chart>                
                        <div style="float:right; cursor:pointer !important;" @click="onChartClick($event, 1)"><i class="fas fa-expand" style="position:absolute; bottom:10px; right:20px;"></i></div>
                        <div class="clearfix"></div>
                    </div>
                </div>
                <div class="col-lg-6" style="min-height:400px;">
                    <div v-if="loading" class="d-flex" style="height:100%;">
                        <div class="m-auto">
                            <i class="fas fa-spinner fa-spin fa-lg"></i>
                        </div>
                    </div>
                    <div v-else style="height:100%;">    
                        <game-speedrun-count-line-chart chartconainerid="divGameChart2" :tabledata="tabledata" :categorytypeid="categorytypeid" :categoryid="categoryid" :categories="categories" :levels="levels" :variables="variables" :showmilliseconds="showmilliseconds" :subcaption="subcaption"></game-speedrun-count-line-chart> 
                        <div style="float:right; cursor:pointer !important;" @click="onChartClick($event, 2)"><i class="fas fa-expand" style="position:absolute; bottom:10px; right:20px;"></i></div>
                        <div class="clearfix"></div>                                       
                    </div>
                </div>
            </div>            
            <modal v-if="showChartModal && !loading" contentclass="cmv-modal-lg" bodyclass="p-0" @close="showChartModal = false">
                <template v-slot:title>
                    {{ chartModalTitle }}
                </template>
                <div v-if="selectedChartID == 1">         
                    <game-speedrun-count-doughnut-chart chartconainerid="divChartModal" :tabledata="tabledata" :categorytypeid="categorytypeid" :categoryid="categoryid" :categories="categories" :levels="levels" :subcategoryvariablevaluetabs="subcategoryvariablevaluetabs" :showmilliseconds="showmilliseconds" :subcaption="subcaption" :ismodal="true"></game-speedrun-count-doughnut-chart>                
                </div>
                <div v-else-if="selectedChartID == 2">
                    <game-speedrun-count-line-chart chartconainerid="divChartModal" :tabledata="tabledata" :categorytypeid="categorytypeid" :categoryid="categoryid" :categories="categories" :levels="levels" :variables="variables" :showmilliseconds="showmilliseconds" :subcaption="subcaption" :ismodal="true"></game-speedrun-count-line-chart>                
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

                switch(this.selectedChartID){
                    case 1:
                        title = (this.categorytypeid == 0 ? 'Category' : 'Level') + ' Trends (Last 12 months)';
                        break;
                    case 2:
                        title = (this.categorytypeid == 0 ? 'Category' : 'Level') + ' Distribution';
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






