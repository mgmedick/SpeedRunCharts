<template>
    <div class="mt-4">
        <div class="row mb-2">
            <div class="col tab-list">
                <ul class="nav nav-underline">
                    <li class="nav-item">
                        <a class="nav-link" :class="{ 'active' : gridID == 0 }" href="#/" data-value="0" draggable="false" @click="onTabClick">{{ "Runs" }}</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" :class="{ 'active' : gridID == 1 }" href="#/" data-value="1" draggable="false" @click="onTabClick">{{ "Summary Charts" }}</a>
                    </li>
                    <li class="nav-item dropdown more" v-show="false">
                        <a class="nav-link dropdown-toggle" data-bs-toggle="dropdown" href="#" role="button" aria-expanded="false">More...</a>                        
                        <ul class="dropdown-menu dropdown-menu-end dropdown-menu-lg-start">
                            <li class="d-none">
                                <a class="dropdown-item" :class="{ 'active' : gridID == 0 }" href="#/" data-value="0" draggable="false" @click="onTabClick">{{ "Runs" }}</a>            
                            </li>
                            <li class="d-none">
                                <a class="dropdown-item" :class="{ 'active' : gridID == 1 }" href="#/" data-value="1" draggable="false" @click="onTabClick">{{ "Summary Charts" }}</a>            
                            </li>
                        </ul>
                    </li>                        
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












