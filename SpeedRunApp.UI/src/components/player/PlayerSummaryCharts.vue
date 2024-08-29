<template>
    <div class="card mt-2" style="border: none; border-radius: 0px;">
        <div class="row no-gutters">
            <div class="col-lg-6">
                <div class="ratio ratio-4x3">
                    <div style="height:100%;">
                        <player-summary-bar-chart chartconainerid="divUserChart1" :categorytypeid="categorytypeid" :games="items" :tabledata="tabledata" @onexpandchartclick="onExpandChartClick($event, 1)" :ismodal="false"></player-summary-bar-chart>                                                 
                    </div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="ratio ratio-4x3">
                    <div style="height:100%;">
                        <player-summary-line-chart chartconainerid="divUserChart2" :categorytypeid="categorytypeid" :games="items" :tabledata="tabledata" @onexpandchartclick="onExpandChartClick($event, 2)" :ismodal="false"></player-summary-line-chart>                                                 
                    </div>
                </div>            
            </div>
        </div>
        <div class="row no-gutters">
            <div class="col-lg-12">
                <div class="ratio ratio-4x3">
                    <div style="height:100%;">
                        <player-summary-donut-chart chartconainerid="divUserChart3" :categorytypeid="categorytypeid" :games="items" :tabledata="tabledata" @onexpandchartclick="onExpandChartClick($event, 3)" :ismodal="false"></player-summary-donut-chart>                                                 
                    </div>
                </div>                        
            </div>               
        </div>           
        <div class="row no-gutters">
            <div class="col-lg-12">
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
                                <player-speedrun-count-bar-chart chartconainerid="divChartModal" :categorytypeid="categorytypeid" :games="items" :tabledata="tabledata" :ismodal="true"></player-speedrun-count-bar-chart>                                                 
                            </div>
                        </div>
                        <div v-else-if="selectedChartID == 2">
                            <div class="ratio ratio-4x3">  
                                <player-speedrun-count-line-chart chartconainerid="divChartModal" :categorytypeid="categorytypeid" :games="items" :tabledata="tabledata" :ismodal="true"></player-speedrun-count-line-chart>                                                 
                            </div>
                        </div>     
                        <div v-else-if="selectedChartID == 3">
                            <div class="ratio ratio-4x3">  
                                <player-speedrun-count-donut-chart chartconainerid="divChartModal" :categorytypeid="categorytypeid" :games="items" :tabledata="tabledata" :ismodal="true"></player-speedrun-count-donut-chart>                                                 
                            </div>                
                        </div>                           
                    </div>
                </div>
            </div>
        </div>                              
    </div>
</template>
<script>    
    export default {
        name: "PlayerSummaryCharts",
        props: {
            categorytypeid: String,
            items: Array,
            tabledata: Array
        },
        data() {
            return {
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
                        break;
                    case 3:
                        break;                        
                }

                return title;
            }                                                  
        },                                        
        methods: {                            
            onExpandChartClick(event, chartID) {
                this.selectedChartID = chartID;                 
                new Modal(this.$refs.chartmodal).show();
            }                          
        }
    }
</script>






