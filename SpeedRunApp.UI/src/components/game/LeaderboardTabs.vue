﻿<template>
    <div v-if="loading">
        <div class="d-flex">
            <div class="mx-auto">
                <i class="fas fa-spinner fa-spin fa-lg"></i>
            </div>
        </div>
    </div> 
    <div v-else id="divSpeedRunGridTabContainer">       
        <div class="row no-gutters pr-1">
            <div class="col tab-list">
                <ul class="nav nav-pills">
                    <li class="categoryType nav-item py-1 pr-1" v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id">
                        <a class="nav-link p-2" :class="{ 'active' : categoryTypeID == categoryType.id }" href="#/" data-type="categoryType" :data-value="categoryType.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ categoryType.name }}</a>
                    </li>
                    <button-dropdown v-show="false" class="more py-1 pr-1" :btnclasses="'btn-secondary'">
                        <template v-slot:text>
                            <span>More...</span>
                        </template>
                        <template v-slot:options>
                            <template v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id">
                                <a class="dropdown-item d-none" :class="{ 'active' : categoryTypeID == categoryType.id }" href="#/" data-type="categoryType" :data-value="categoryType.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ categoryType.name }}</a>
                            </template>
                        </template>
                    </button-dropdown>                       
                </ul>
            </div>                    
        </div>
        <div v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id">
            <div v-if="categoryTypeID == categoryType.id">
                <div class="row no-gutters pr-1">
                    <div class="col tab-list">
                        <ul class="nav nav-pills">
                            <li class="category nav-item py-1 pr-1" v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id && (!hideempty || ctg.hasData) && (showmisc || !ctg.isMisc))" :key="category.id">
                                <a class="nav-link p-2" :class="{ 'active' : categoryID == category.id }" href="#/" data-type="category" :data-value="category.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ category.name }}</a>
                            </li>
                            <button-dropdown v-show="false" class="more py-1 pr-1" :btnclasses="'btn-secondary'">
                                <template v-slot:text>
                                    <span>More...</span>
                                </template>
                                <template v-slot:options>
                                    <template v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id && (!hideempty || ctg.hasData) && (showmisc || !ctg.isMisc))" :key="category.id">
                                        <a class="dropdown-item d-none" :class="{ 'active' : categoryID == category.id }" href="#/" data-type="category" :data-value="category.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ category.name }}</a>
                                    </template>
                                </template>
                            </button-dropdown>    
                        </ul>
                    </div>                           
                </div>
                <div v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id)" :key="category.id">
                    <div v-if="categoryID == category.id">                                
                        <div v-if="categoryTypeID == 0">
                            <div v-if="game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == category.id && (variable.scopeTypeID == '0' || variable.scopeTypeID == '1')).length > 0">
                                <leaderboard-tabs-variable :items="game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == category.id && (variable.scopeTypeID == '0' || variable.scopeTypeID == '1'))" :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="category.id.toString()" :levelid="''" :subcategoryvariablevalueids="subCategoryVariableValueIDs" :speedrunid="speedRunID" :prevdata="''" :variableindex="variableIndex" :hideempty="hideempty" :showcharts="showCharts" :showalldata="showAllData" :showmilliseconds="game.showMilliseconds" :variables="game.variables" :exporttypes="exportTypes" :title="title" :istimerasc="category.isTimerAsc" @ontabclick="onTabClick" @onshowchartsclick2="onShowChartsClick" @update:showalldata="showAllData = $event"></leaderboard-tabs-variable>
                            </div>
                            <div v-else>                              
                                <leaderboard-grid :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="category.id.toString()" :levelid="''" :variablevalues="''" :speedrunid="speedRunID" :showcharts="showCharts" :showalldata="showAllData" :showmilliseconds="game.showMilliseconds" :variables="game.variables" :title="title" :istimerasc="category.isTimerAsc" :exporttypes="exportTypes" @onshowchartsclick1="onShowChartsClick" @update:showalldata="showAllData = $event"></leaderboard-grid>
                            </div>
                        </div>
                        <div v-else>
                            <div class="row no-gutters pr-1">
                                <div class="col tab-list">
                                    <ul class="nav nav-pills">
                                        <li class="level nav-item py-1 pr-1" v-for="(level, levelIndex) in game.levels.filter(lvl => lvl.categoryID == category.id && (!hideempty || lvl.hasData))" :key="level.id">
                                            <a class="nav-link p-2" :class="{ 'active' : levelID == level.id }" href="#/" data-type="level" :data-value="level.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ level.name }}</a>
                                        </li>
                                        <button-dropdown v-show="false" class="more py-1 pr-1" :btnclasses="'btn-secondary'">
                                            <template v-slot:text>
                                                <span>More...</span>
                                            </template>
                                            <template v-slot:options>
                                                <template v-for="(level, levelIndex) in game.levels.filter(lvl => lvl.categoryID == category.id && (!hideempty || lvl.hasData))" :key="level.id">
                                                    <a class="dropdown-item d-none" :class="{ 'active' : levelID == level.id }" href="#/" data-type="level" :data-value="level.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ level.name }}</a>
                                                </template>
                                            </template>
                                        </button-dropdown>   
                                    </ul>
                                </div>                                      
                            </div>
                            <div v-for="(level, levelIndex) in game.levels.filter(lvl => lvl.categoryID == category.id)" :key="level.id">
                                <div v-if="levelID == level.id">
                                    <div v-if="game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == category.id && variable.levelID == level.id && (variable.scopeTypeID == '0' || variable.scopeTypeID == '2' || variable.scopeTypeID == '3')).length > 0">
                                        <leaderboard-tabs-variable :items="game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == category.id && variable.levelID == level.id && (variable.scopeTypeID == '0' || variable.scopeTypeID == '2' || variable.scopeTypeID == '3'))" :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="category.id.toString()" :levelid="level.id.toString()" :subcategoryvariablevalueids="subCategoryVariableValueIDs" :speedrunid="speedRunID" :prevdata="''" :variableindex="variableIndex" :hideempty="hideempty" :showcharts="showCharts" :showalldata="showAllData" :showmilliseconds="game.showMilliseconds" :variables="game.variables" :exporttypes="exportTypes" :title="title" :istimerasc="category.isTimerAsc" @ontabclick="onTabClick" @onshowchartsclick2="onShowChartsClick" @update:showalldata="showAllData = $event"></leaderboard-tabs-variable>
                                    </div>
                                    <div v-else>
                                        <leaderboard-grid :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="category.id.toString()" :levelid="level.id.toString()" :variablevalues="''" :speedrunid="speedRunID" :showcharts="showCharts" :showalldata="showAllData" :showmilliseconds="game.showMilliseconds" :variables="game.variables" :title="title" :istimerasc="category.isTimerAsc" :exporttypes="exportTypes" @onshowchartsclick1="onShowChartsClick" @update:showalldata="showAllData = $event"></leaderboard-grid>
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

    export default {
        name: "LeaderboardTabs",
        props: {
            id: String,
            speedrunid: String,
            hideempty: Boolean,
            showmisc: Boolean
        },
        data() {
            return {
                game: {},
                categoryTypeID: '',
                categoryID: '',
                levelID: '',
                subCategoryVariableValueIDs: {},
                variableIndex: 0,
                speedRunID: this.speedrunid,
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
                var variableValueNames = Object.keys(this.subCategoryVariableValueIDs).map(i => this.subCategoryVariableValueIDs[i]).join(' - ');              
                
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
            window.addEventListener('resize', this.resizeSRTabs);
        },       
        updated: function () {
            this.resizeSRTabs();
        },
        methods: {
            loadData() {
                var that = this;
                this.loading = true;

                var url = '/Game/GetLeaderboardTabs?gameID=' + this.id + '&speedRunID=' + this.speedrunid;
                var prms = axios.get(url)
                                .then(res => {
                                    that.game = res.data.tabItems[0];
                                    that.exportTypes = res.data.exportTypes,
                                    that.categoryTypeID = res.data.categoryTypeID;
                                    that.categoryID = res.data.categoryID;
                                    that.levelID = res.data.levelID;
                                    that.subCategoryVariableValueIDs = res.data.subCategoryVariableValueIDs;
                                    that.showAllData = res.data.showAllData;   
                                    that.showMisc = res.data.showMisc;                                                                     
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

                if (!this.subCategoryVariableValueIDs) {
                    var eligibleVariables = [];
                    if (this.categoryTypeID == 0) {
                        eligibleVariables = game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == that.categoryID && (variable.scopeTypeID == '0' || variable.scopeTypeID == '1'));
                    } else {
                        eligibleVariables = game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == that.categoryID && variable.levelID == that.levelID && (variable.scopeTypeID == '0' || variable.scopeTypeID == '2' || variable.scopeTypeID == '3'));
                    }

                    this.subCategoryVariableValueIDs = {};
                    this.setSubCategoryVariableValueIDs(eligibleVariables, 0);
                }
            },
            setSubCategoryVariableValueIDs: function(variables, count) {
                var that = this;
                variables?.forEach(variable => {
                    if(!that.subCategoryVariableValueIDs.hasOwnProperty(variable.name + count)) {
                        if(variable.variableValues && variable.variableValues.length > 0) {
                            var va = variable.variableValues.filter(va => (!that.hideempty || va.hasData))[0];
                            that.subCategoryVariableValueIDs[variable.name + count] = va.name;
                        
                            if (va.subVariables && va.subVariables.length > 0) {
                                that.setSubCategoryVariableValueIDs(va.subVariables, count + 1);
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
                    eligibleVariables = game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == that.categoryID && (variable.scopeTypeID == '0' || variable.scopeTypeID == '1'));
                } else {
                    eligibleVariables = game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == that.categoryID && variable.levelID == that.levelID && (variable.scopeTypeID == '0' || variable.scopeTypeID == '2' || variable.scopeTypeID == '3'));
                }

                var newSubCategoryVariableValueIDs = {};
                this.resetSubCategoryVariableValueIDs(eligibleVariables, newSubCategoryVariableValueIDs, 0);
                this.subCategoryVariableValueIDs = newSubCategoryVariableValueIDs;
                this.speedRunID = '';
            },
            resetSubCategoryVariableValueIDs: function (variables, newSubCategoryVariableValueIDs, count) {
                var that = this;                                                                               
                variables?.forEach(variable => {
                    if (!newSubCategoryVariableValueIDs.hasOwnProperty(variable.name + count)) {
                        if(variable.variableValues && variable.variableValues.length > 0) {
                            var va = variable.variableValues.filter(va => Object.keys(that.subCategoryVariableValueIDs).map(key => that.subCategoryVariableValueIDs[key]).filter(x => x == va.name).length > 0 && (!that.hideempty || va.hasData))[0] ?? variable.variableValues.filter(va => (!that.hideempty || va.hasData))[0];
                            newSubCategoryVariableValueIDs[variable.name + count] = va.name;

                            if (va.subVariables && va.subVariables.length > 0) {
                                that.resetSubCategoryVariableValueIDs(va.subVariables, newSubCategoryVariableValueIDs, count + 1);
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
                        this.subCategoryVariableValueIDs[variableName] = value;
                        break;                                                                                                   
                }
                                
                this.resetSelected();                
            },    
            onShowAllDataClick: function (event) {
                this.showAllData = !this.showAllData;              
            },               
            onShowChartsClick: function (event) {
                this.showCharts = !this.showCharts;
            },                     
            resizeSRTabs: function () {
                var rows = document.querySelectorAll('#divSpeedRunGridTabContainer .tab-list');

                for (var g = 0; g < rows.length; g++) {
                    var totalWidth = 0;
                    var tabitems = rows[g].querySelectorAll('li:not(.dropdown-item)');
                    var morediv =  rows[g].querySelector('.more');
                    var morebtn =  morediv.querySelector('.btn');
                    var moreItems = morediv.querySelectorAll('a.dropdown-item');
                    var moredrp = morediv.querySelector('.dropdown-menu');

                    for (var i = 0; i < tabitems.length; i++) {
                        tabitems[i].style.left = "-10000px";
                        tabitems[i].classList.remove('d-none');
                        totalWidth += tabitems[i].offsetWidth;
                        if (totalWidth > ((rows[g].offsetWidth - tabitems[i].offsetWidth) - 30)) {
                            tabitems[i].classList.add('d-none');
                            moreItems[i].classList.remove('d-none');
                        } else {
                            tabitems[i].classList.remove('d-none');
                            moreItems[i].classList.add('d-none');                     
                        }
                    }

                    var items = Array.from(morediv.querySelectorAll('a:not(.d-none)'));
                    if (items.length > 0) {
                        var item = items.find(i=>i.classList.contains('active'));
                        if (item) {
                            morebtn.innerHTML = item.innerHTML;
                            morebtn.classList.add('active');
                        } else {
                            morebtn.innerHTML = 'More...';
                            morebtn.classList.remove('active');
                        }
                        
                        morediv.style.display = 'block';
                    } else {
                        morediv.style.display = 'none';                       
                    }

                    var ww = document.documentElement.clientWidth;
                    var pos = this.getPosition(morediv);
                    if (pos.x > (ww / 2)) {
                        moredrp.classList.remove('dropdown-menu-left');
                        moredrp.classList.add('dropdown-menu-right');
                    } else {
                        moredrp.classList.remove('dropdown-menu-right');
                        moredrp.classList.add('dropdown-menu-left');
                    }
               }
            },
            getPosition: function (element) {
                var xPosition = 0;
                var yPosition = 0;

                while (element) {
                    xPosition += (element.offsetLeft - element.scrollLeft + element.clientLeft);
                    yPosition += (element.offsetTop - element.scrollTop + element.clientTop);
                    element = element.offsetParent;
                }
                return {
                    x: xPosition,
                    y: yPosition
                };
            }
        }
    };
</script>









