<template>
    <div class="mt-3">
        <div v-if="gridID == 0 || gridID == 1" class="row mb-2">
            <div class="col">
                <div class="dropdown">
                    <button class="btn btn-secondary btn-sm dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                        <span>
                            <i class="fa fa-filter"></i>
                        </span>
                    </button>
                    <ul class="dropdown-menu">
                        <li v-if="gridID == 0 || gridID == 1">
                            <div class="dropdown-item">
                                <div class="form-check form-switch">
                                    <input id="chkShowMisc" class="form-check-input" type="checkbox" v-model="showMisc">
                                    <label class="form-check-label" for="chkShowMisc"><span>Show Misc</span></label>
                                </div>                                               
                            </div>
                        </li>
                        <li v-if="gridID == 0">
                            <div class="dropdown-item">
                                <div class="form-check form-switch">
                                    <input id="chkHideEmpty" class="form-check-input" type="checkbox" v-model="hideEmpty">
                                    <label class="form-check-label" for="chkHideEmpty"><span>Hide Empty</span></label>
                                </div>                                                
                            </div> 
                        </li>
                    </ul>
                </div>                
            </div>
        </div>                               
        <div id="divGameTabContainer" class="row mb-2">
            <div class="col tab-list">                
                <ul class="nav nav-underline">
                    <li class="nav-item">
                        <a class="nav-link" :class="{ 'active' : gridID == 0 }" href="#/" data-value="0"  @click="onTabClick">{{ "Leaderboards" }}</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" :class="{ 'active' : gridID == 1 }" href="#/" data-value="1"  @click="onTabClick">{{ "World Recs" }}</a>            
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" :class="{ 'active' : gridID == 2 }" href="#/" data-value="2"  @click="onTabClick">{{ "Summary Charts" }}</a>            
                    </li>
                    <li class="nav-item dropdown more" v-show="false">
                        <a class="nav-link dropdown-toggle" data-bs-toggle="dropdown" href="#" role="button" aria-expanded="false">More...</a>                        
                        <ul class="dropdown-menu dropdown-menu-end dropdown-menu-lg-start">
                            <li class="d-none">
                                <a class="dropdown-item" :class="{ 'active' : gridID == 0 }" href="#/" data-value="0"  @click="onTabClick">{{ "Leaderboards" }}</a>            
                            </li>
                            <li class="d-none">
                                <a class="dropdown-item" :class="{ 'active' : gridID == 1 }" href="#/" data-value="1"  @click="onTabClick">{{ "World Recs" }}</a>            
                            </li>
                            <li class="d-none">
                                <a class="dropdown-item" :class="{ 'active' : gridID == 2 }" href="#/" data-value="2"  @click="onTabClick">{{ "Summary Charts" }}</a>                          
                            </li>
                        </ul>
                    </li>                                                            
                </ul>
            </div>
        </div>
        <div v-if="gridID == 0">
            <leaderboard-tabs :id="id" :speedruncode="speedruncode" :hideempty="hideEmpty" :showmisc="showMisc" @update:showmisc="showMisc = $event"></leaderboard-tabs>
        </div>
        <div v-else-if="gridID == 1">
            <worldrecord-tabs :id="id" :showmisc="showMisc"></worldrecord-tabs>   
        </div>
        <div v-else>
            <game-summary-chart-tabs :id="id"></game-summary-chart-tabs>
        </div>        
    </div>
</template>
<script>
    import { resizeTabs } from '../../js/common.js';

    export default {
        name: "GameDetailsTabs",
        props: {
            id: String,
            speedruncode: String           
        },
        data() {
            return {
                gridID: '0',
                hideEmpty: true,
                showMisc: false
            }
        },      
        mounted: function () {
            window.addEventListener('resize', this.onResize);
        },
        destroyed() {
            window.removeEventListener('resize', this.onResize);     
        },                             
        methods: {
            onTabClick: function (event) {
                var value = event.target.getAttribute('data-value');
                this.gridID = value;              
            },
            onResize() {
                var that = this;
                if (that.width != document.documentElement.clientWidth) {  
                    resizeTabs();
                }                 
            }                                     
        }       
    };
</script>












