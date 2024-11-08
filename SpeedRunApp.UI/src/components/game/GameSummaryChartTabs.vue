<template>
    <div v-if="loading">
        <div class="d-flex">
            <div class="mx-auto">
                <i class="fas fa-spinner fa-spin fa-lg"></i>
            </div>
        </div>
    </div> 
    <div v-else-if="game.categoryTypes" id="divGameChartTabContainer">
        <div class="row no-gutters pe-1 pt-1 pb-0">
            <div class="col tab-list">
                <ul class="nav nav-underline">
                    <li class="categoryType nav-item py-1 pe-1" v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id">
                        <a class="nav-link" :class="{ 'active' : categoryTypeID == categoryType.id }" href="#/" data-type="categoryType" :data-value="categoryType.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ categoryType.name }}</a>
                    </li>
                    <li class="nav-item dropdown more" v-show="false">
                        <a class="nav-link dropdown-toggle" data-bs-toggle="dropdown" href="#" role="button" aria-expanded="false">More...</a>                        
                        <ul class="dropdown-menu dropdown-menu-end dropdown-menu-lg-start">
                            <li v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id" class="d-none">
                                <a class="dropdown-item" :class="{ 'active' : categoryTypeID == categoryType.id }" href="#/" data-type="categoryType" :data-value="categoryType.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ categoryType.name }}</a>
                            </li>
                        </ul>
                    </li>                        
                </ul>
            </div>                    
        </div>
        <div v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id">
            <div v-if="categoryTypeID == categoryType.id">
                <div v-if="categoryTypeID == 0">
                    <game-summary-charts :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="''" :categories="game.categories" :levels="game.levels" :variables="game.variables" :subcategoryvariablevaluetabs="game.subCategoryVariablesTabs" :showmilliseconds="game.showMilliseconds" :subcaption="subCaption"></game-summary-charts>
                </div>
                <div v-else>
                    <div class="row no-gutters pe-1 pt-1 pb-0">
                        <div class="col tab-list">
                            <ul class="nav nav-underline">
                                <li class="category nav-item py-1 pe-1" v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id && ctg.hasData)" :key="category.id">
                                    <a class="nav-link" :class="{ 'active' : categoryID == category.id }" href="#/" data-type="category" :data-value="category.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ category.name }}</a>
                                </li>
                                <li class="nav-item dropdown more" v-show="false">
                                    <a class="nav-link dropdown-toggle" data-bs-toggle="dropdown" href="#" role="button" aria-expanded="false">More...</a>                        
                                    <ul class="dropdown-menu dropdown-menu-end dropdown-menu-lg-start">
                                        <li v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id && ctg.hasData)" :key="category.id" class="d-none">
                                            <a class="dropdown-item d-none" :class="{ 'active' : categoryID == category.id }" href="#/" data-type="category" :data-value="category.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ category.name }}</a>
                                        </li>
                                    </ul>
                                </li>                                  
                            </ul>
                        </div>
                    </div>                           
                    <div v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id && ctg.hasData)" :key="category.id">
                        <div v-if="categoryID == category.id">                                
                            <game-summary-charts :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="category.id.toString()" :categories="game.categories" :levels="game.levels.filter(lvl => lvl.categoryID == category.id)" :variables="game.variables" :subcategoryvariablevaluetabs="game.subCategoryVariablesTabs" :showmilliseconds="game.showMilliseconds" :subcaption="subCaption"></game-summary-charts>                        
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div> 
    <div v-else class="text-center mt-2">
        <span class="fw-bold text-secondary">No categories</span>
    </div>       
</template>
<script>
    import axios from 'axios';
    import { resizeTabs } from '../../js/common.js';

    export default {
        name: "GameSummaryChartTabs",
        props: {       
            id: String
        },
        data() {
            return {
                game: {},
                categoryTypeID: '',
                categoryID: '',
                loading: true
            }
        },
        computed: {
            subCaption: function () {
                var result = '';
                var game = this.game;
                var gameName = game.name;                
                var categoryTypeName = game.categoryTypes.filter(i => i.id == this.categoryTypeID)[0]?.name;
                var categoryName = game.categories.filter(i=>i.id == this.categoryID)[0]?.name;

                result = [gameName, categoryTypeName, categoryName].join(' - ');
                result = result.replace(/^[ -]+|[ -]+$/g, '');
                
                return result;
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

                var url = '/Game/GetGameChartTabs?gameID=' + this.id;
                var prms = axios.get(url)
                                .then(res => {
                                    that.game = res.data.tabItems[0];
                                    that.initSelected();
                                    that.loading = false;
                                    return res;
                                })
                                .catch(err => { console.error(err); return Promise.reject(err); });

                return prms;
            },            
            onTabClick: function (event) {                
                var type = event.target.getAttribute('data-type');
                var value = event.target.getAttribute('data-value');

                switch(type) {
                    case 'categoryType':
                        this.categoryTypeID = value;
                        break;
                    case 'category':
                        this.categoryID = value;
                        break;                                                                                                                                                                                            
                }

                this.resetSelected();               
            },
            initSelected: function () {
                var that = this;
                var game = this.game;

                if (game.categoryTypes) {
                    this.categoryTypeID = this.categoryTypeID || game.categoryTypes[0].id;
                    
                    if (this.categoryTypeID == 1) {
                        this.categoryID = this.categoryID || game.categories.find(category => category.categoryTypeID == that.categoryTypeID)?.id;
                    } else {
                        this.categoryID = '';
                    }   
                }                  
            },                      
            resetSelected: function () {
                var that = this;
                var game = this.game;

                if (game.categoryTypes) {
                    if (game.categoryTypes.filter(i => i.id == that.categoryTypeID).length == 0) {
                        this.categoryTypeID = game.categoryTypes[0].id;
                    }

                    if (this.categoryTypeID == 1) {
                        if (game.categories.filter(i => i.categoryTypeID == that.categoryTypeID && i.id == that.categoryID).length == 0) {
                            this.categoryID = game.categories.filter(ctg => ctg.categoryTypeID == that.categoryTypeID)[0]?.id;
                        }  
                    } else {
                        this.categoryID = '';
                    } 
                }      
            }       
        }       
    };
</script>












