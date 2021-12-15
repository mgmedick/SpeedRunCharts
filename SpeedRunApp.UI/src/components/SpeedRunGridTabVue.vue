<template>
    <div v-if="loading">
        <div class="d-flex">
            <div class="mx-auto">
                <i class="fas fa-spinner fa-spin fa-lg"></i>
            </div>
        </div>
    </div> 
    <div v-else id="divSpeedRunGridTabContainer" class="container-lg p-0">
        <div v-if="!isgame" class="row no-gutters pr-1 pt-1 pb-0">
            <div class="col tab-list">
                <ul class="nav nav-pills">
                    <li class="nav-item py-1 pr-1" v-for="(game, gameIndex) in items" :key="game.id">
                        <a class="nav-link p-2" :class="{ 'active' : gameID == game.id }" href="#/" data-type="game" :data-value="game.id" data-toggle="pill" @click="onTabClick">{{ game.name }}</a>
                    </li>
                    <button-dropdown v-show="false" class="more py-1 pr-1" :btnclasses="'btn-secondary'" :listclasses="'dropdown-menu-right'">
                        <template v-slot:text>
                            <span>More...</span>
                        </template>
                        <template v-slot:options>
                            <template v-for="(game, gameIndex) in items" :key="game.id">
                                <a class="dropdown-item d-none" :class="{ 'active' : gameID == game.id }" href="#/" data-type="game" :data-value="game.id" @click="onTabClick">{{ game.name }}</a>
                            </template>
                        </template>
                    </button-dropdown>    
                </ul>
            </div>
        </div>
        <div v-for="(game, gameIndex) in items" :key="game.id">
            <div v-if="gameID == game.id">
                <div class="row no-gutters pr-1 pt-1 pb-0 pr-0">
                    <div class="col tab-list">
                        <ul class="nav nav-pills">
                            <li class="categoryType nav-item py-1 pr-1" v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id">
                                <a class="nav-link p-2" :class="{ 'active' : categoryTypeID == categoryType.id }" href="#/" data-type="categoryType" :data-value="categoryType.id" data-toggle="pill" @click="onTabClick">{{ categoryType.name }}</a>
                            </li>
                            <button-dropdown v-show="false" class="more py-1 pr-1" :btnclasses="'btn-secondary'" :listclasses="'dropdown-menu-right'">
                                <template v-slot:text>
                                    <span>More...</span>
                                </template>
                                <template v-slot:options>
                                    <template v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id">
                                        <a class="dropdown-item d-none" :class="{ 'active' : categoryTypeID == categoryType.id }" href="#/" data-type="categoryType" :data-value="categoryType.id" data-toggle="pill" @click="onTabClick">{{ categoryType.name }}</a>
                                    </template>
                                </template>
                            </button-dropdown>                       
                        </ul>
                    </div>
                </div>
                <div v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id">
                    <div v-if="categoryTypeID == categoryType.id">
                        <div class="categoryrow row no-gutters pr-1 pt-1 pb-0 pr-0">
                            <div class="col tab-list">
                                <ul class="nav nav-pills">
                                    <li class="category nav-item py-1 pr-1" v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id && (!hideEmpty || ctg.hasData))" :key="category.id">
                                        <a class="nav-link p-2" :class="{ 'active' : categoryID == category.id }" href="#/" data-type="category" :data-value="category.id" data-toggle="pill" @click="onTabClick">{{ category.name }}</a>
                                    </li>
                                    <button-dropdown v-show="false" class="more py-1 pr-1" :btnclasses="'btn-secondary'" :listclasses="'dropdown-menu-right'">
                                        <template v-slot:text>
                                            <span>More...</span>
                                        </template>
                                        <template v-slot:options>
                                            <template v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id)" :key="category.id">
                                                <a class="dropdown-item d-none" :class="{ 'active' : categoryID == category.id }" href="#/" data-type="category" :data-value="category.id" data-toggle="pill" @click="onTabClick">{{ category.name }}</a>
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
                                        <speedrun-grid-tab-variable :items="game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == category.id && (variable.scopeTypeID == '0' || variable.scopeTypeID == '1'))" :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="category.id.toString()" :levelid="''" :subcategoryvariablevalueids="subCategoryVariableValueIDs" :userid="userID" :prevdata="''" :variableindex="variableIndex" :hideempty="hideEmpty" @ontabclick="onTabClick" @onhideemptyclick="onHideEmptyClick"></speedrun-grid-tab-variable>
                                    </div>
                                    <div v-else>
                                        <div v-if="isgame" class="row no-gutters pr-1 pt-1">
                                            <div class="col-auto">
                                                <label class="tab-row-name pr-2">Hide Empty:</label>
                                            </div>
                                            <div class="col align-self-center">
                                                <div class="custom-control custom-switch">
                                                    <input id="chkHideEmpty" type="checkbox" class="custom-control-input" data-toggle="toggle" v-model="hideEmpty" @click="onHideEmptyClick">
                                                    <label class="custom-control-label" for="chkHideEmpty"></label>
                                                </div>
                                            </div>
                                        </div> 
                                        <speedrun-grid :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="category.id.toString()" :levelid="''" :variablevalues="''" :userid="userID"></speedrun-grid>
                                    </div>
                                </div>
                                <div v-else>
                                    <div class="levelrow row no-gutters pr-1 pt-1 pb-0 pr-0">
                                        <div class="col tab-list">
                                            <ul class="nav nav-pills">
                                                <li class="level nav-item py-1 pr-1" v-for="(level, levelIndex) in game.levelTabs.filter(lvl => lvl.categoryID == category.id && (!hideEmpty || lvl.hasData))" :key="level.id">
                                                    <a class="nav-link p-2" :class="{ 'active' : levelID == level.id }" href="#/" data-type="level" :data-value="level.id" data-toggle="pill" @click="onTabClick">{{ level.name }}</a>
                                                </li>
                                                <button-dropdown v-show="false" class="more py-1 pr-1" :btnclasses="'btn-secondary'" :listclasses="'dropdown-menu-right'">
                                                    <template v-slot:text>
                                                        <span>More...</span>
                                                    </template>
                                                    <template v-slot:options>
                                                        <template v-for="(level, levelIndex) in game.levelTabs.filter(lvl => lvl.categoryID == category.id && (!hideEmpty || lvl.hasData))" :key="level.id">
                                                            <a class="dropdown-item d-none" :class="{ 'active' : levelID == level.id }" href="#/" data-type="level" :data-value="level.id" data-toggle="pill" @click="onTabClick">{{ level.name }}</a>
                                                        </template>
                                                    </template>
                                                </button-dropdown>   
                                            </ul>
                                        </div>
                                    </div>
                                    <div v-for="(level, levelIndex) in game.levelTabs.filter(lvl => lvl.categoryID == category.id)" :key="level.id">
                                        <div v-if="levelID == level.id">
                                            <div v-if="game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == category.id && variable.levelID == level.id && (variable.scopeTypeID == '0' || variable.scopeTypeID == '2' || variable.scopeTypeID == '3')).length > 0">
                                                <speedrun-grid-tab-variable :items="game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == category.id && variable.levelID == level.id && (variable.scopeTypeID == '0' || variable.scopeTypeID == '2' || variable.scopeTypeID == '3'))" :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="category.id.toString()" :levelid="level.id.toString()" :subcategoryvariablevalueids="subCategoryVariableValueIDs" :userid="userID" :prevdata="''" :variableindex="variableIndex" :hideempty="hideEmpty" @ontabclick="onTabClick" @onhideemptyclick="onHideEmptyClick"></speedrun-grid-tab-variable>
                                            </div>
                                            <div v-else>
                                                <div v-if="isgame" class="row no-gutters pr-1 pt-1">
                                                    <div class="col-auto">
                                                        <label class="tab-row-name pr-2">Hide Empty:</label>
                                                    </div>
                                                    <div class="col align-self-center">
                                                        <div class="custom-control custom-switch">
                                                            <input id="chkHideEmpty" type="checkbox" class="custom-control-input" data-toggle="toggle" v-model="hideEmpty" @click="onHideEmptyClick">
                                                            <label class="custom-control-label" for="chkHideEmpty"></label>
                                                        </div>
                                                    </div>
                                                </div>                                                 
                                                <speedrun-grid :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="category.id.toString()" :levelid="level.id.toString()" :variablevalues="''" :userid="userID"></speedrun-grid>
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
    </div>
</template>
<script>
    import axios from 'axios';

    export default {
        name: "SpeedRunGridTabVue",
        props: {
            isgame: Boolean,
            id: String
        },
        data() {
            return {
                items: [],
                gameID: '',
                categoryTypeID: '',
                categoryID: '',
                levelID: '',
                subCategoryVariableValueIDs: {},
                variableIndex: 0,
                hideEmpty: true,
                loading: true
            }
        },
        computed: {
            userID: function () {
                return this.isgame ? '' : this.id;
            }
        },
        created: function () {
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

                var prms = axios.get('../Game/GetSpeedRunGridTabs', { params: { ID: this.id, isGame: this.isgame } })
                                .then(res => {
                                    that.items = res.data.tabItems;
                                    that.gameID = res.data.gameID;
                                    that.categoryTypeID = res.data.categoryTypeID;
                                    that.categoryID = res.data.categoryID;
                                    that.levelID = res.data.levelID;
                                    that.subCategoryVariableValueIDs = res.data.subCategoryVariableValueIDs;
                                    that.initSelected();
                                    that.loading = false;
                                    return res;
                                })
                                .catch(err => { console.error(err); return Promise.reject(err); });

                return prms;
            },
            initSelected: function () {
                var that = this;
                this.gameID = this.gameID || this.items[0].id;

                var game = this.items.find(game => game.id == that.gameID);

                this.categoryTypeID = this.categoryTypeID || game.categoryTypes[0].id;

                this.categoryID = this.categoryID || game.categories.find(category => category.categoryTypeID == that.categoryTypeID && (!that.hideEmpty || category.hasData))?.id;

                if (this.categoryTypeID == 1) {
                    this.levelID = this.levelID || (game.levelTabs ? game.levelTabs.filter(lvl => lvl.categoryID == that.categoryID && (!that.hideEmpty || lvl.hasData))[0]?.id : '');
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
                    this.setSubCategoryVariableValueIDs(eligibleVariables);
                    // this.subCategoryVariableValueIDs = {};
                    // eligibleVariables.forEach(variable => {    
                    //     that.subCategoryVariableValueIDs[variable.name] = variable.variableValues.filter(va => (!that.hideEmpty || va.hasData))[0]?.name            
                    // });

                    // if (this.categoryTypeID == 0) {
                    //     game.subCategoryVariables?.filter(variable => variable.categoryID == that.categoryID && (variable.scopeTypeID == '0' || variable.scopeTypeID == '1'))
                    //         .forEach(variable => { that.subCategoryVariableValueIDs[variable.name] = variable.variableValues.filter(va => (!that.hideEmpty || va.hasData))[0]?.name })

                    // } else if (this.categoryTypeID == 1) {
                    //     game.subCategoryVariables?.filter(variable => variable.categoryID == that.categoryID && (variable.scopeTypeID == '0' || variable.scopeTypeID == '2' || variable.scopeTypeID == '3'))
                    //         .forEach(variable => { that.subCategoryVariableValueIDs[variable.name] = variable.variableValues.filter(va => (!that.hideEmpty || va.hasData))[0]?.name })
                    // }
                }
            },
            setSubCategoryVariableValueIDs: function(variables) {
                var that = this;
                variables?.forEach(variable => {
                    if(!that.subCategoryVariableValueIDs.hasOwnProperty(variable.name)) {
                        that.subCategoryVariableValueIDs[variable.name] = variable.variableValues.filter(va => (!that.hideEmpty || va.hasData))[0]?.name
                    }
                    variable.variableValues.forEach(variableValue => {
                        if (variableValue.subVariables && variableValue.subVariables.length > 0) {
                            that.setSubCategoryVariableValueIDs(variableValue.subVariables);
                        }
                    });    
                });
            },            
            resetSelected: function () {
                var that = this;
                var game = this.items.find(game => game.id == that.gameID);

                if (game.categoryTypes.filter(i => i.id == that.categoryTypeID).length == 0) {
                    this.categoryTypeID = game.categoryTypes[0]?.id;
                }

                if (game.categories.filter(i => i.categoryTypeID == that.categoryTypeID && i.id == that.categoryID && (!that.hideEmpty || i.hasData)).length == 0) {
                    this.categoryID = game.categories.filter(ctg => ctg.categoryTypeID == that.categoryTypeID && (!that.hideEmpty || ctg.hasData))[0]?.id;
                }

                if (this.categoryTypeID == 1) {
                    if(game.levelTabs?.filter(i => i.categoryID == that.categoryID && i.id == that.levelID && (!that.hideEmpty || i.hasData)).length == 0) {
                        this.levelID = game.levelTabs?.filter(lvl => lvl.categoryID == that.categoryID && (!that.hideEmpty || lvl.hasData))[0]?.id;
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
                this.resetSubCategoryVariableValueIDs(eligibleVariables, newSubCategoryVariableValueIDs);
                // eligibleVariables.forEach(variable => {    
                //     var variableValue = variable.variableValues.filter(variableValue => Object.keys(that.subCategoryVariableValueIDs).map(key => that.subCategoryVariableValueIDs[key]).filter(x => x == variableValue.name).length > 0 && (!that.hideEmpty || variableValue.hasData))[0] ?? variable.variableValues.filter(va => (!that.hideEmpty || va.hasData))[0];
                //     newSubCategoryVariableValueIDs[variable.name] = variableValue?.name;              
                // });

                this.subCategoryVariableValueIDs = newSubCategoryVariableValueIDs;
            },
            resetSubCategoryVariableValueIDs: function(variables, newSubCategoryVariableValueIDs) {
                var that = this;
                variables?.forEach(variable => {
                    if(!newSubCategoryVariableValueIDs.hasOwnProperty(variable.name)) {
                        var va = variable.variableValues.filter(va => Object.keys(that.subCategoryVariableValueIDs).map(key => that.subCategoryVariableValueIDs[key]).filter(x => x == va.name).length > 0 && (!that.hideEmpty || va.hasData))[0] ?? variable.variableValues.filter(va => (!that.hideEmpty || va.hasData))[0];
                        newSubCategoryVariableValueIDs[variable.name] = va.name;
                    }

                    variable.variableValues.forEach(variableValue => {
                        if (variableValue.subVariables && variableValue.subVariables.length > 0) {
                            that.resetSubCategoryVariableValueIDs(variableValue.subVariables, newSubCategoryVariableValueIDs);
                        }
                    });    
                });
            },
            onTabClick: function (event) {
                var type = event.target.getAttribute('data-type');
                var value = event.target.getAttribute('data-value');
                var variableName = event.target.getAttribute('data-variable');
                var isMore = event.target.parentElement.classList.contains('dropdown-item');

                switch(type) {
                    case 'game':
                        this.gameID = value;
                        break;
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
            onHideEmptyClick: function (event) {
                this.hideEmpty = !this.hideEmpty;
                this.resetSelected();
            },            
            resizeSRTabs: function () {
                var rows = document.querySelectorAll('#divSpeedRunGridTabContainer .tab-list');

                for (var g = 0; g < rows.length; g++) {
                    var totalWidth = 0;
                    var tabitems = rows[g].querySelectorAll('li:not(.dropdown-item)');
                    var morebtn =  rows[g].querySelector('.more');
                    var moreItems = morebtn.querySelectorAll('a.dropdown-item');

                    for (var i = 0; i < tabitems.length; i++) {
                        tabitems[i].style.left = "-10000px";
                        tabitems[i].classList.remove('d-none');
                        totalWidth += tabitems[i].offsetWidth;
                        if (totalWidth > (rows[g].offsetWidth - 100)) {
                            tabitems[i].classList.add('d-none');
                            moreItems[i].classList.remove('d-none');
                        } else {
                            tabitems[i].classList.remove('d-none');
                            moreItems[i].classList.add('d-none');                     
                        }
                    }

                    morebtn.style.display = morebtn.querySelectorAll('a:not(.d-none)').length > 0 ? 'block' : 'none';
               }
            }                                   
        }
    };
</script>
<style>
    .tab-list .nav {
        /*overflow: hidden; */
        flex-wrap: nowrap !important;
        white-space: nowrap !important;
    }

    .tab-list .nav-link {
        background-color: #313131;
        font-size: 13px;
        font-weight: bold;
    }

    .tab-list .dropdown .btn.dropdown-toggle {
        background-color: #313131;
        font-size: 13px;
        font-weight: bold;
        border: none !important;
        padding: 0.5rem !important;
    }

    .tab-list .dropdown .btn.dropdown-toggle.active {
        background-color: var(--primary) !important;
    }

    .tab-row-name {
        font-size: 14px !important;
        line-height: 18px;
        font-weight: bold;
    } 
</style>




