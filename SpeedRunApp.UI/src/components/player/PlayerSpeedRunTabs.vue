<template>
    <div v-if="loading">
        <div class="d-flex">
            <div class="mx-auto">
                <i class="fas fa-spinner fa-spin fa-lg"></i>
            </div>
        </div>
    </div> 
    <div v-else id="divSpeedRunGridTabContainer">
        <div class="row no-gutters pe-1">
            <div class="col tab-list">
                <ul class="nav nav-pills">
                    <li class="categoryType nav-item py-1 pe-1" v-for="(categoryType, categoryTypeIndex) in categoryTypes" :key="categoryType.id">
                        <a class="nav-link p-2" :class="{ 'active' : categoryTypeID == categoryType.id }" href="#/" data-type="categoryType" :data-value="categoryType.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ categoryType.name }}</a>
                    </li>
                    <div class="dropdown more py-1 pe-1" v-show="false">
                        <button class="btn btn-secondary btn-sm dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                            <span>More...</span>
                        </button>
                        <ul class="dropdown-menu dropdown-menu-end dropdown-menu-lg-start">
                            <li v-for="(categoryType, categoryTypeIndex) in categoryTypes" :key="categoryType.id" class="d-none">
                                <a class="dropdown-item" :class="{ 'active' : categoryTypeID == categoryType.id }" href="#/" data-type="categoryType" :data-value="categoryType.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ categoryType.name }}</a>
                            </li>
                        </ul>
                    </div>                                            
                </ul>
            </div>                    
        </div>
        <div class="row no-gutters">
            <div class="col-auto ms-auto">
                <div class="dropdown">
                    <button class="btn btn-secondary btn-sm dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                        <span>
                            <i class="fa fa-filter"></i><span class="ps-2">...</span>
                        </span>
                    </button>
                    <ul class="dropdown-menu">
                        <li>
                            <div class="dropdown-item">
                                <div class="form-check form-switch">
                                    <input id="chkShowAllData" class="form-check-input" type="checkbox" v-model="showAllData">
                                    <label class="form-check-label" for="chkShowAllData"><span>Show Obsolete</span></label>
                                </div>                                                     
                            </div>
                        </li> 
                        <li>
                            <div class="dropdown-item">
                                <div class="form-check form-switch">
                                    <input id="chkShowWR" class="form-check-input" type="checkbox" v-model="showWR">
                                    <label class="form-check-label" for="chkShowWR"><span>Show WRs Only</span></label>
                                </div>                                                   
                            </div>
                        </li>
                        <li>
                            <div class="dropdown-item">
                                <div class="form-check form-switch">
                                    <input id="chkShowMisc" class="form-check-input" type="checkbox" v-model="showMisc">
                                    <label class="form-check-label" for="chkShowMisc"><span>Show Misc</span></label>
                                </div>                                                    
                            </div>
                        </li>                                                           
                    </ul>
                </div>                 
            </div>
        </div>             
        <div v-for="(categoryType, categoryTypeIndex) in categoryTypes" :key="categoryType.id">
            <div v-if="categoryTypeID == categoryType.id">
                <div v-for="(game, gameIndex) in items.filter(item => item.categoryTypes.filter(i => i.id == categoryType.id).length > 0)" :key="game.id" class="mt-4">
                    <div v-if="tableData.filter(item => item.gameID == game.id && ((categoryType.id == 0 && !item.levelID) || (categoryType.id == 1 && item.levelID)) && (showMisc || !item.isMiscellaneous) && (!showWR || item.rank == 1)).length > 0">
                        <div class="row g-2">
                            <div class="col-1 p-0" style="max-width:37px;">
                                <div class="img-round">
                                    <img :src="game.coverImageUri" class="img-fluid" alt="Responsive image">
                                </div>
                            </div>                            
                            <div class="col-11 align-self-end">
                                <h6 class="fw-bold mb-0"><a :href="'/Game/GameDetails/' + encodeURIComponent(game.abbr)" class="text-primary">{{ game.name }}</a></h6>
                            </div>
                        </div>
                        <player-speedrun-grid :playerid="id" :gameabbr="game.abbr" :tabledata="tableData.filter(item => item.gameID == game.id && ((categoryType.id == 0 && !item.levelID) || (categoryType.id == 1 && item.levelID)))" :showmilliseconds="game.showMilliseconds" :variables="game.variables" :showalldata="showAllData" :showmisc="showMisc" :showwr="showWR"></player-speedrun-grid>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import axios from 'axios';
    import { resizeTabs } from '../../js/common.js';

    export default {
        name: "PlayerSpeedRunTabs",
        props: {
            id: String,
            speedruncode: String
        },
        data() {
            return {
                items: [],
                categoryTypes: [],
                tableData: [],
                categoryTypeID: '',
                showAllData: false,
                showMisc: true,
                showWR: false,
                loading: true
            }
        },   
        mounted: function () {
            this.loadData();
            window.addEventListener('resize', this.onResize);
        },
        destroyed() {
            window.removeEventListener('resize', this.onResize);     
        },                  
        updated: function () {
            this.onResize();
        },                  
        methods: {
            loadData() {
                var that = this;
                this.loading = true;

                var url = '/Player/GetPlayerSpeedRunTabsAndData?playerID=' + this.id + (this.speedruncode ? '&speedRunCode=' + this.speedruncode : '');
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
            },
            onResize() {
                var that = this;
                if (that.width != document.documentElement.clientWidth) {  
                    resizeTabs(document.getElementById('divSpeedRunGridTabContainer'));
                }                 
            }                                   
        }
    };
</script>









