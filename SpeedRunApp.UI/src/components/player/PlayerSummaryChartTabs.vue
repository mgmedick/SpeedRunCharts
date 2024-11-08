<template>
    <div v-if="loading">
        <div class="d-flex">
            <div class="mx-auto">
                <i class="fas fa-spinner fa-spin fa-lg"></i>
            </div>
        </div>
    </div> 
    <div v-else id="divSpeedRunGridTabContainer">
        <div class="row mb-2">
            <div class="col tab-list">
                <ul class="nav nav-underline">
                    <li class="categoryType nav-item" v-for="(categoryType, categoryTypeIndex) in categoryTypes" :key="categoryType.id">
                        <a class="nav-link" :class="{ 'active' : categoryTypeID == categoryType.id }" href="#/" data-type="categoryType" :data-value="categoryType.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ categoryType.name }}</a>
                    </li>
                    <li class="nav-item dropdown more" v-show="false">
                        <a class="nav-link dropdown-toggle" data-bs-toggle="dropdown" href="#" role="button" aria-expanded="false">More...</a>                        
                        <ul class="dropdown-menu dropdown-menu-end dropdown-menu-lg-start">
                            <li v-for="(categoryType, categoryTypeIndex) in categoryTypes" :key="categoryType.id" class="d-none">
                                <a class="dropdown-item" :class="{ 'active' : categoryTypeID == categoryType.id }" href="#/" data-type="categoryType" :data-value="categoryType.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ categoryType.name }}</a>
                            </li>
                        </ul>
                    </li>                      
                </ul>
            </div>                    
        </div>            
        <div v-for="(categoryType, categoryTypeIndex) in categoryTypes" :key="categoryType.id">
            <div v-if="categoryTypeID == categoryType.id">
                <player-summary-charts :playerid="id" :categorytypeid="categoryType.id.toString()" :items="items" :tabledata="tableData.filter(item => (categoryType.id == 0 && !item.levelID) || (categoryType.id == 1 && item.levelID))"></player-summary-charts>
            </div>
        </div>
    </div>
</template>
<script>
    import axios from 'axios';
    import { resizeTabs } from '../../js/common.js';

    export default {
        name: "PlayerSummaryChartsTabs",
        props: {
            id: String
        },
        data() {
            return {
                items: [],
                categoryTypes: [],
                tableData: [],
                categoryTypeID: '',
                loading: true
            }
        },   
        mounted: function () {
            this.loadData();
        },                
        updated: function () {
            resizeTabs();
        },                 
        methods: {
            loadData() {
                var that = this;
                this.loading = true;

                var url = '/Player/GetPlayerSpeedRunTabsAndData?playerID=' + this.id;
                var prms = axios.get(url)
                                .then(res => {
                                    that.items = res.data.tabItems;
                                    that.categoryTypes = res.data.categoryTypes,
                                    that.tableData = res.data.tableData,
                                    that.initSelected();
                                    that.loading = false;
                                    return res;
                                })
                                .catch(err => { console.error(err); return Promise.reject(err); });

                return prms;
            },
            initSelected: function () {
                this.categoryTypeID = this.categoryTypeID || this.categoryTypes[0].id;
            },            
            onTabClick: function (event) {
                var type = event.target.getAttribute('data-type');
                var value = event.target.getAttribute('data-value');

                switch(type) {
                    case 'categoryType':
                        this.categoryTypeID = value;
                        break;                                                                                                                                                                                            
                }
            }          
        }
    };
</script>









