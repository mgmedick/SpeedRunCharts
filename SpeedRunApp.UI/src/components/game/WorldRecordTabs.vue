<template>
    <div v-if="loading">
        <div class="d-flex">
            <div class="mx-auto">
                <i class="fas fa-spinner fa-spin fa-lg"></i>
            </div>
        </div>
    </div>
    <div v-else-if="game.categoryTypes" id="divWorldRecorGridTabContainer">
        <div class="row no-gutters pe-1">
            <div class="col tab-list">
                <ul class="nav nav-pills">
                    <li class="nav-item py-1 pe-1" v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id">
                        <a class="nav-link p-2" :class="{ 'active' : categoryTypeID == categoryType.id }" href="#/" data-type="categoryType" :data-value="categoryType.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ categoryType.name }}</a>
                    </li>
                    <div class="dropdown more py-1 pe-1" v-show="false">
                        <button class="btn btn-secondary btn-sm dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                            <span>More...</span>
                        </button>
                        <ul class="dropdown-menu dropdown-menu-end dropdown-menu-lg-start">
                            <li v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id" class="d-none">
                                <a class="dropdown-item" :class="{ 'active' : categoryTypeID == categoryType.id }" href="#/" data-type="categoryType" :data-value="categoryType.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ categoryType.name }}</a>
                            </li>
                        </ul>
                    </div>                                                      
                </ul>
            </div>
        </div>
        <div v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id">
            <div v-if="categoryTypeID == categoryType.id">
                <div v-if="categoryTypeID == 0 && (!game.subCategoryVariables || game.subCategoryVariables.filter(variable => variable.categoryID && variable.isSingleCategory).length == 0)">
                    <worldrecord-grid :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="''" :levelid="''" :showmilliseconds="game.showMilliseconds" :variables="game.variables" :subcategoryvariablevaluetabs="game.subCategoryVariablesTabs?.filter(variable => variable.categoryID && !variable.levelID)" :showcategories="true" :showlevels="false" :showmisc="showmisc" :title="title" :exporttypes="exportTypes"></worldrecord-grid>                              
                </div>                    
                <div v-else>
                    <div class="row no-gutters pe-1">
                        <div class="col tab-list">
                            <ul class="nav nav-pills">
                                <li class="category nav-item py-1 pe-1" v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id && (showmisc || !ctg.isMisc))" :key="category.id">
                                    <a class="nav-link p-2" :class="{ 'active' : categoryID == category.id }" href="#/" data-type="category" :data-value="category.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ category.name }}</a>
                                </li>
                                <div class="dropdown more py-1 pe-1" v-show="false">
                                    <button class="btn btn-secondary btn-sm dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                        <span>More...</span>
                                    </button>
                                    <ul class="dropdown-menu dropdown-menu-end dropdown-menu-lg-start">
                                        <li v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id)" :key="category.id" class="d-none">
                                            <a class="dropdown-item" :class="{ 'active' : categoryID == category.id }" href="#/" data-type="category" :data-value="category.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ category.name }}</a>
                                        </li>
                                    </ul>
                                </div>                                     
                            </ul>
                        </div>                           
                    </div>
                    <div v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id)" :key="category.id">
                        <div v-if="categoryID == category.id">
                            <div v-if="categoryTypeID == 0">
                                <worldrecord-grid :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="category.id.toString()" :levelid="''" :showmilliseconds="game.showMilliseconds" :variables="game.variables" :subcategoryvariablevaluetabs="game.subCategoryVariablesTabs.filter(variable => variable.categoryID == category.id && !variable.levelID)" :showcategories="false" :showlevels="false" :showmisc="showmisc" :title="title" :exporttypes="exportTypes"></worldrecord-grid>
                            </div>
                            <div v-else>
                                <div v-if="!game.subCategoryVariables || game.subCategoryVariables.filter(variable => variable.categoryID == categoryID && variable.levelID && variable.variableScopeTypeID == '3').length == 0">
                                    <worldrecord-grid :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="category.id.toString()" :levelid="''" :showmilliseconds="game.showMilliseconds" :variables="game.variables" :subcategoryvariablevaluetabs="game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == category.id && variable.levelID)" :showcategories="false" :showlevels="true" :showmisc="showmisc" :title="title" :exporttypes="exportTypes"></worldrecord-grid>                              
                                </div>
                                <div v-else>
                                    <div class="row no-gutters pe-1">
                                        <div class="col tab-list">
                                            <ul class="nav nav-pills">
                                                <li class="level nav-item py-1 pe-1" v-for="(level, levelIndex) in game.levels.filter(lvl => lvl.categoryID == category.id)" :key="level.id">
                                                    <a class="nav-link p-2" :class="{ 'active' : levelID == level.id }" href="#/" data-type="level" :data-value="level.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ level.name }}</a>
                                                </li>
                                                <div class="dropdown more py-1 pe-1" v-show="false">
                                                    <button class="btn btn-secondary btn-sm dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                                        <span>More...</span>
                                                    </button>
                                                    <ul class="dropdown-menu dropdown-menu-end dropdown-menu-lg-start">
                                                        <li v-for="(level, levelIndex) in game.levels.filter(lvl => lvl.categoryID == category.id)" :key="level.id" class="d-none">
                                                            <a class="dropdown-item" :class="{ 'active' : levelID == level.id }" href="#/" data-type="level" :data-value="level.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ level.name }}</a>
                                                        </li>
                                                    </ul>
                                                </div>                                                     
                                            </ul>
                                        </div>                                      
                                    </div>
                                    <div v-for="(level, levelIndex) in game.levels.filter(lvl => lvl.categoryID == category.id)" :key="level.id">
                                        <div v-if="levelID == level.id">
                                            <worldrecord-grid :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="category.id.toString()" :levelid="level.id.toString()" :showmilliseconds="game.showMilliseconds" :variables="game.variables" :subcategoryvariablevaluetabs="game.subCategoryVariablesTabs.filter(variable => variable.categoryID == category.id && variable.levelID == level.id)" :showcategories="false" :showlevels="false" :showmisc="showmisc" :title="title" :exporttypes="exportTypes"></worldrecord-grid>                              
                                        </div>
                                    </div>
                                </div>                                        
                            </div>
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
        name: "WorldRecordTabs",
        props: {
            id: String,
            showmisc: Boolean
        },
        data() {
            return {
                game: {},
                categoryTypeID: '',
                categoryID: '',
                levelID: '',
                variableIndex: 0,  
                exportTypes: [],
                loading: true
            }
        },
        computed: {
            title: function () {
                var result = '';
                var game = this.game;
                var gameName = game.name;
                var categoryName = game.categories.filter(i=>i.id == this.categoryID)[0]?.name;
                var levelName = game.levels?.filter(i=>i.id == this.levelID)[0]?.name;
                
                result = [gameName, categoryName, levelName].join(' - ');
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

                var url = '/Game/GetWorldRecordTabs?gameID=' + this.id;
                var prms = axios.get(url)
                                .then(res => {
                                    that.game = res.data.tabItems[0];
                                    that.exportTypes = res.data.exportTypes,
                                    that.initSelected();
                                    that.loading = false;
                                    return res;
                                })
                                .catch(err => { console.error(err); return Promise.reject(err); });

                return prms;
            },
            initSelected: function () {
                var that = this;
                var game = this.game;

                if (game.categoryTypes) {
                    this.categoryTypeID = this.categoryTypeID || game.categoryTypes[0].id;
                    
                    this.categoryID = this.categoryID || game.categories.find(category => category.categoryTypeID == that.categoryTypeID)?.id;

                    if (this.categoryTypeID == 1) {
                        this.levelID = this.levelID || (game.levels ? game.levels.filter(lvl => lvl.categoryID == that.categoryID)[0]?.id : '');
                    } else {
                        this.levelID = '';
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

                    if (game.categories.filter(i => i.categoryTypeID == that.categoryTypeID && i.id == that.categoryID).length == 0) {
                        this.categoryID = game.categories.filter(ctg => ctg.categoryTypeID == that.categoryTypeID)[0]?.id;
                    }    

                    if (this.categoryTypeID == 1) {
                        if(game.levels?.filter(i => i.categoryID == that.categoryID && i.id == that.levelID).length == 0) {
                            this.levelID = game.levels?.filter(lvl => lvl.categoryID == that.categoryID)[0]?.id;
                        }
                    } else {
                        this.levelID = '';
                    }

                    var eligibleVariables = [];
                    if (this.categoryTypeID == 0) {
                        eligibleVariables = game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == that.categoryID && (variable.variableScopeTypeID == '0' || variable.variableScopeTypeID == '1'));
                    } else {
                        eligibleVariables = game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == that.categoryID && variable.levelID == that.levelID && (variable.variableScopeTypeID == '0' || variable.variableScopeTypeID == '2' || variable.variableScopeTypeID == '3'));
                    }
                }         
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
                    case 'level':
                        this.levelID = value;
                        break;                                                                                                                                                                                                
                }

                this.resetSelected();                
            }              
        }
    };
</script>










