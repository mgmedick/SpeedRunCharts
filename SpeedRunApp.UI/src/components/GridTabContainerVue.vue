﻿<template>
    <div class="container p-0 mt-4">
        <div class="row no-gutters pr-1 pt-1 pb-0 pr-0">
            <div class="col tab-list">
                <ul class="nav nav-pills">
                    <li class="nav-item py-1 pr-1">
                        <a class="nav-link p-2" :class="{ 'active' : gridID == 0 }" href="#/" data-value="0" @click="onTabClick">{{ isgame ? "Leaderboards" : "Runs" }}</a>
                    </li>
                    <li class="nav-item py-1 pr-1">
                        <a class="nav-link p-2" :class="{ 'active' : gridID == 1 }" href="#/" data-value="1" @click="onTabClick">{{ isgame ? "World Records" : "Personal Bests" }}</a>            
                    </li>
                </ul>
            </div>
        </div>
        <div v-if="gridID == 0">
            <speedrun-grid-tab :isgame="isgame" :id="id" :speedrunid="speedrunid"></speedrun-grid-tab>
        </div>
        <div v-else>
            <worldrecord-grid-tab :isgame="isgame" :id="id"></worldrecord-grid-tab>   
        </div>
    </div>
</template>
<script>
    export default {
        name: "GridTabContainerVue",
        props: {
            isgame: Boolean,
            id: String,
            speedrunid: String           
        },
        data() {
            return {
                gridID: '0'
            }
        },
        methods: {
            onTabClick: function (event) {
                var value = event.target.getAttribute('data-value');
                this.gridID = value;              
            },
        }       
    };
</script>












