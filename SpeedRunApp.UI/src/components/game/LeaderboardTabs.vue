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
                    <li class="categoryType nav-item py-1 pe-1" v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id">
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
                <div class="row no-gutters pe-1">
                    <div class="col tab-list">
                        <ul class="nav nav-pills">
                            <li class="category nav-item py-1 pe-1" v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id && (!hideempty || ctg.hasData) && (showmisc || !ctg.isMisc))" :key="category.id">
                                <a class="nav-link p-2" :class="{ 'active' : categoryID == category.id }" href="#/" data-type="category" :data-value="category.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ category.name }}</a>
                            </li>
                            <div class="dropdown more py-1 pe-1" v-show="false">
                                <button class="btn btn-secondary btn-sm dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                    <span>More...</span>
                                </button>
                                <ul class="dropdown-menu dropdown-menu-end dropdown-menu-lg-start">
                                    <li v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id && (!hideempty || ctg.hasData) && (showmisc || !ctg.isMisc))" :key="category.id" class="d-none">
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
                            <div v-if="game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == category.id && (variable.variableScopeTypeID == '0' || variable.variableScopeTypeID == '1')).length > 0">
                                <leaderboard-tabs-variable :items="game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == category.id && (variable.variableScopeTypeID == '0' || variable.variableScopeTypeID == '1'))" :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="category.id.toString()" :levelid="''" :subcategoryvariablevalues="subCategoryVariableValues" :speedruncode="speedRunCode" :prevdata="''" :variableindex="variableIndex" :hideempty="hideempty" :showcharts="showCharts" :showalldata="showAllData" :showmilliseconds="game.showMilliseconds" :variables="game.variables" :exporttypes="exportTypes" :title="title" :istimerasc="category.isTimerAsc" @ontabclick="onTabClick" @onshowchartsclick2="onShowChartsClick" @update:showalldata="showAllData = $event"></leaderboard-tabs-variable>
                            </div>
                            <div v-else>                              
                                <leaderboard-grid :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="category.id.toString()" :levelid="''" :variablevalues="''" :speedruncode="speedRunCode" :showcharts="showCharts" :showalldata="showAllData" :showmilliseconds="game.showMilliseconds" :variables="game.variables" :title="title" :istimerasc="category.isTimerAsc" :exporttypes="exportTypes" @onshowchartsclick1="onShowChartsClick" @update:showalldata="showAllData = $event"></leaderboard-grid>
                            </div>
                        </div>
                        <div v-else>
                            <div class="row no-gutters pe-1">
                                <div class="col tab-list">
                                    <ul class="nav nav-pills">
                                        <li class="level nav-item py-1 pe-1" v-for="(level, levelIndex) in game.levels.filter(lvl => lvl.categoryID == category.id && (!hideempty || lvl.hasData))" :key="level.id">
                                            <a class="nav-link p-2" :class="{ 'active' : levelID == level.id }" href="#/" data-type="level" :data-value="level.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ level.name }}</a>
                                        </li>
                                        <div class="dropdown more py-1 pe-1" v-show="false">
                                            <button class="btn btn-secondary btn-sm dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                                <span>More...</span>
                                            </button>
                                            <ul class="dropdown-menu dropdown-menu-end dropdown-menu-lg-start">
                                                <li v-for="(level, levelIndex) in game.levels.filter(lvl => lvl.categoryID == category.id && (!hideempty || lvl.hasData))" :key="level.id" class="d-none">
                                                    <a class="dropdown-item" :class="{ 'active' : levelID == level.id }" href="#/" data-type="level" :data-value="level.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ level.name }}</a>
                                                </li>
                                            </ul>
                                        </div>                                         
                                    </ul>
                                </div>                                      
                            </div>
                            <div v-for="(level, levelIndex) in game.levels.filter(lvl => lvl.categoryID == category.id)" :key="level.id">
                                <div v-if="levelID == level.id">
                                    <div v-if="game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == category.id && variable.levelID == level.id && (variable.variableScopeTypeID == '0' || variable.variableScopeTypeID == '2' || variable.variableScopeTypeID == '3')).length > 0">
                                        <leaderboard-tabs-variable :items="game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == category.id && variable.levelID == level.id && (variable.variableScopeTypeID == '0' || variable.variableScopeTypeID == '2' || variable.variableScopeTypeID == '3'))" :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="category.id.toString()" :levelid="level.id.toString()" :subcategoryvariablevalues="subCategoryVariableValues" :speedruncode="speedRunCode" :prevdata="''" :variableindex="variableIndex" :hideempty="hideempty" :showcharts="showCharts" :showalldata="showAllData" :showmilliseconds="game.showMilliseconds" :variables="game.variables" :exporttypes="exportTypes" :title="title" :istimerasc="category.isTimerAsc" @ontabclick="onTabClick" @onshowchartsclick2="onShowChartsClick" @update:showalldata="showAllData = $event"></leaderboard-tabs-variable>
                                    </div>
                                    <div v-else>
                                        <leaderboard-grid :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="category.id.toString()" :levelid="level.id.toString()" :variablevalues="''" :speedruncode="speedRunCode" :showcharts="showCharts" :showalldata="showAllData" :showmilliseconds="game.showMilliseconds" :variables="game.variables" :title="title" :istimerasc="category.isTimerAsc" :exporttypes="exportTypes" @onshowchartsclick1="onShowChartsClick" @update:showalldata="showAllData = $event"></leaderboard-grid>
                                    </div>
                                </div>
                            </div>
                        </div>
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
        name: "LeaderboardTabs",
        props: {
            id: String,
            speedruncode: String,
            hideempty: Boolean,
            showmisc: Boolean
        },
        data() {
            return {
                game: {},
                categoryTypeID: '',
                categoryID: '',
                levelID: '',
                subCategoryVariableValues: {},
                variableIndex: 0,
                speedRunCode: this.speedruncode,
                showCharts: true,
                showMisc: this.showmisc,
                showAllData: false,               
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
                var variableValueNames = Object.keys(this.subCategoryVariableValues).map(i => this.subCategoryVariableValues[i]).join(' - ');              
                
                result = [gameName, categoryName, levelName, variableValueNames].join(' - ');
                result = result.replace(/^[ -]+|[ -]+$/g, '');
                
                return result;
            }
        }, 
        watch: {
            hideempty: function (val, oldVal) {
                this.resetSelected();
            },      
            showmisc: function (val, oldVal) {
                this.resetSelected();
            },                  
            showMisc: function (val, oldVal) {
                this.$emit('update:showmisc', val); 
            }               
        },           
        mounted: function () {
            if (window.innerWidth > 992) {
                this.showCharts = true;
            } else {
                this.showCharts = false;
            }

            this.loadData();
        },               
        updated: function () {
            resizeTabs();
        },
        methods: {
            loadData() {
                var that = this;
                this.loading = true;

                var url = '/Game/GetLeaderboardTabs?gameID=' + this.id + (this.speedruncode ? '&speedRunCode=' + this.speedruncode : '');
                var prms = axios.get(url)
                                .then(res => {
                                    that.game = res.data.tabItems[0];
                                    that.exportTypes = res.data.exportTypes;
                                    if (res.data.runVW) {
                                        var run = res.data.runVW;
                                        that.categoryTypeID = run.categoryTypeID;
                                        that.categoryID = run.categoryID;
                                        that.levelID = run.levelID;
                                        that.subCategoryVariableValues = run.subCategoryVariableValues;
                                        that.showAllData = !(!!run.rank);   
                                        that.showMisc = run.isMiscellaneous;    
                                    }                                                                 
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

                this.categoryTypeID = this.categoryTypeID || game.categoryTypes[0].id;

                this.categoryID = this.categoryID || game.categories.find(category => category.categoryTypeID == that.categoryTypeID && (!that.hideempty || category.hasData) && (that.showmisc || !category.isMisc))?.id;

                if (this.categoryTypeID == 1) {
                    this.levelID = this.levelID || (game.levels ? game.levels.filter(lvl => lvl.categoryID == that.categoryID && (!that.hideempty || lvl.hasData))[0]?.id : '');
                } else {
                    this.levelID = '';
                }

                if (Object.keys(this.subCategoryVariableValues).length == 0) {
                    var eligibleVariables = [];
                    if (this.categoryTypeID == 0) {
                        eligibleVariables = game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == that.categoryID && (variable.variableScopeTypeID == '0' || variable.variableScopeTypeID == '1'));
                    } else {
                        eligibleVariables = game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == that.categoryID && variable.levelID == that.levelID && (variable.variableScopeTypeID == '0' || variable.variableScopeTypeID == '2' || variable.variableScopeTypeID == '3'));
                    }

                    this.subCategoryVariableValues = {};
                    this.setSubCategoryVariableValues(eligibleVariables, 0);
                }
            },
            setSubCategoryVariableValues: function(variables, count) {
                var that = this;
                variables?.forEach(variable => {
                    if(!that.subCategoryVariableValues.hasOwnProperty(variable.name + count)) {
                        if(variable.variableValues && variable.variableValues.length > 0) {
                            var va = variable.variableValues.filter(va => (!that.hideempty || va.hasData))[0];
                            that.subCategoryVariableValues[variable.name + count] = va.name;
                        
                            if (va.subVariables && va.subVariables.length > 0) {
                                that.setSubCategoryVariableValues(va.subVariables, count + 1);
                            }
                        }
                    } 
                });
            },                                    
            resetSelected: function () {
                var that = this;
                var game = this.game;

                if (game.categoryTypes.filter(i => i.id == that.categoryTypeID).length == 0) {
                    this.categoryTypeID = game.categoryTypes[0]?.id;
                }

                if (game.categories.filter(i => i.categoryTypeID == that.categoryTypeID && i.id == that.categoryID && (!that.hideempty || i.hasData) && (that.showmisc || !i.isMisc)).length == 0) {
                    this.categoryID = game.categories.filter(ctg => ctg.categoryTypeID == that.categoryTypeID && (!that.hideempty || ctg.hasData) && (that.showmisc || !ctg.isMisc))[0]?.id;
                }

                if (this.categoryTypeID == 1) {
                    if(game.levels?.filter(i => i.categoryID == that.categoryID && i.id == that.levelID && (!that.hideempty || i.hasData)).length == 0) {
                        this.levelID = game.levels?.filter(lvl => lvl.categoryID == that.categoryID && (!that.hideempty || lvl.hasData))[0]?.id;
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

                var newSubCategoryVariableValues = {};
                this.resetSubCategoryVariableValues(eligibleVariables, newSubCategoryVariableValues, 0);
                this.subCategoryVariableValues = newSubCategoryVariableValues;
                this.speedRunCode = '';
            },
            resetSubCategoryVariableValues: function (variables, newSubCategoryVariableValues, count) {
                var that = this;                                                                               
                variables?.forEach(variable => {
                    if (!newSubCategoryVariableValues.hasOwnProperty(variable.name + count)) {
                        if(variable.variableValues && variable.variableValues.length > 0) {
                            var va = variable.variableValues.filter(va => Object.keys(that.subCategoryVariableValues).map(key => that.subCategoryVariableValues[key]).filter(x => x == va.name).length > 0 && (!that.hideempty || va.hasData))[0] ?? variable.variableValues.filter(va => (!that.hideempty || va.hasData))[0];
                            newSubCategoryVariableValues[variable.name + count] = va.name;

                            if (va.subVariables && va.subVariables.length > 0) {
                                that.resetSubCategoryVariableValues(va.subVariables, newSubCategoryVariableValues, count + 1);
                            }
                        }
                    }
                });
            },
            onTabClick: function (event) {                
                var type = event.target.getAttribute('data-type');
                var value = event.target.getAttribute('data-value');
                var variableName = event.target.getAttribute('data-variable');

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
                    case 'variableValue':
                        this.subCategoryVariableValues[variableName] = value;
                        break;                                                                                                   
                }
                                
                this.resetSelected();                
            },    
            onShowAllDataClick: function (event) {
                this.showAllData = !this.showAllData;              
            },               
            onShowChartsClick: function (event) {
                this.showCharts = !this.showCharts;
            }          
        }
    };
</script>









