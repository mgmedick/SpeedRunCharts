<template>
    <div class="mt-4">
        <div class="row no-gutters pe-1 pt-1 pb-0">
            <div class="col tab-list">
                <ul class="nav nav-pills">
                    <li class="nav-item py-1 pe-1">
                        <a class="nav-link p-2" :class="{ 'active' : gridID == 0 }" href="#/" data-value="0" draggable="false" @click="onTabClick">{{ "Runs" }}</a>
                    </li>
                    <li class="nav-item py-1 pe-1">
                        <a class="nav-link p-2" :class="{ 'active' : gridID == 1 }" href="#/" data-value="1" draggable="false" @click="onTabClick">{{ "Summary Charts" }}</a>
                    </li>
                    <div class="dropdown more py-1 pe-1" v-show="false">
                        <button class="btn btn-secondary btn-sm dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                            <span>More...</span>
                        </button>
                        <ul class="dropdown-menu dropdown-menu-end dropdown-menu-lg-start">
                            <li class="d-none">
                                <a class="dropdown-item" :class="{ 'active' : gridID == 0 }" href="#/" data-value="0" draggable="false" @click="onTabClick">{{ "Runs" }}</a>            
                            </li>
                            <li class="d-none">
                                <a class="dropdown-item" :class="{ 'active' : gridID == 1 }" href="#/" data-value="1" draggable="false" @click="onTabClick">{{ "Summary Charts" }}</a>            
                            </li>
                        </ul>
                    </div>                                        
                </ul>
            </div>
        </div>
        <div v-if="gridID == 0">
            <player-speedrun-tabs :id="id" :speedrunid="speedrunid"></player-speedrun-tabs>
        </div>
        <div v-else>
            <player-summary-chart-tabs :id="id"></player-summary-chart-tabs>
        </div>           
    </div>
</template>
<script>
    import { resizeTabs } from '../../js/common.js';

    export default {
        name: "PlayerDetailsTabs",
        props: {
            id: String,
            speedrunid: String           
        },
        data() {
            return {
                gridID: '0'
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












