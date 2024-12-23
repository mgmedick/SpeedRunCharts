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
                        <a class="nav-link" :class="{ 'active' : categoryTypeID == categoryType.id }" href="#/" data-type="categoryType" :data-value="categoryType.id" data-toggle="pill"  @click="onTabClick">{{ categoryType.name }}</a>
                    </li>
                    <li class="nav-item dropdown more" v-show="false">
                        <a class="nav-link dropdown-toggle" data-bs-toggle="dropdown" href="#" role="button" aria-expanded="false">More...</a>                        
                        <ul class="dropdown-menu dropdown-menu-end dropdown-menu-lg-start">
                            <li v-for="(categoryType, categoryTypeIndex) in categoryTypes" :key="categoryType.id" class="d-none">
                                <a class="dropdown-item" :class="{ 'active' : categoryTypeID == categoryType.id }" href="#/" data-type="categoryType" :data-value="categoryType.id" data-toggle="pill"  @click="onTabClick">{{ categoryType.name }}</a>
                            </li>
                        </ul>
                    </li>                       
                </ul>
            </div>                    
        </div>
        <div class="row mb-2">
            <div class="col-auto ms-auto">
                <div class="dropdown">
                    <button class="btn btn-secondary btn-sm dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                        <span>
                            <i class="fa fa-filter"></i>
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
                        <div class="d-flex align-items-end g-2 py-2 px-sm-0 px-2">
                            <div style="width: 50px; flex: none;">
                                <div class="img-round">
                                    <img :src="game.coverImageUri" class="img-fluid" alt="Responsive image">
                                </div>
                            </div>                       
                            <h6 class="fw-bold px-2 nowrap-elipsis"><a :href="'/Game/GameDetails/' + encodeURIComponent(game.abbr)" class="text-decoration-none text-reset">{{ game.name }}</a></h6>
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
        },              
        updated: function () {
            resizeTabs();
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
            }                                
        }
    };
</script>









