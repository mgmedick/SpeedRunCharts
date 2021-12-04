<template>
    <div v-if="!loading" id="divGridContainer" class="container-lg p-0">
        <div v-if="!isgame" class="gamerow row no-gutters pr-1 pt-1 pb-0">
            <div class="col tab-list">
                <ul class="nav nav-pills">
                    <li class="game nav-item py-1 pr-1" v-for="(game, gameIndex) in items" :key="game.id">
                        <a class="nav-link p-2" :class="{ 'active' : gameID == game.id }" href="#/" :data-value="game.id" data-toggle="pill" @click="onGameClick">{{ game.name }}</a>
                    </li>
                    <button-dropdown v-if="additionalItems.length > 0" class="py-1 pr-1" :class="{ 'active' : additionalItems.filter(i => i.id == gameID).length > 0 }">
                        <template v-slot:text>
                            <span>More...</span>
                        </template>
                        <template v-slot:options>
                            <li class="game dropdown-item" v-for="(game, gameIndex) in additionalItems" :key="game.id">
                                <a class="nav-link p-2" :class="{ 'active' : gameID == game.id }" href="#/" :data-value="game.id" data-toggle="pill" @click="onGameClick">{{ game.name }}</a>
                            </li>
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
                                <a class="nav-link p-2" :class="{ 'active' : categoryTypeID == categoryType.id }" :data-value="categoryType.id" href="#/" data-toggle="pill" @click="onCategoryTypeClick">{{ categoryType.name }}</a>
                            </li>
                        </ul>
                    </div>
                </div>
                <div v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id">
                    <div v-if="categoryTypeID == categoryType.id">
                        <div class="categoryrow row no-gutters pr-1 pt-1 pb-0 pr-0">
                            <div class="col tab-list">
                                <ul class="nav nav-pills">
                                    <li class="category nav-item py-1 pr-1" v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id)" :key="category.id">
                                        <a class="nav-link p-2" :class="{ 'active' : categoryID == category.id }" :data-value="category.id" :data-name="category.name" href="#/" data-toggle="pill" @click="onCategoryClick">{{ category.name }}</a>
                                    </li>
                                    <button-dropdown v-if="additionalCategories.length > 0" class="py-1 pr-1" :class="{ 'active' : additionalCategories.filter(i => i.id == categoryID).length > 0 }">
                                        <template v-slot:text>
                                            <span>More...</span>
                                        </template>
                                        <template v-slot:options>
                                            <li class="category dropdown-item" v-for="(category, categoryIndex) in additionalCategories" :key="category.id">
                                                <a class="nav-link p-2" :class="{ 'active' : categoryID == category.id }" href="#/" :data-value="category.id" data-toggle="pill" @click="onCategoryClick">{{ category.name }}</a>
                                            </li>
                                        </template>
                                    </button-dropdown>
                                </ul>
                            </div>
                        </div>
                        <div v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id)" :key="category.id">
                            <div v-if="categoryID == category.id">                                
                                <div v-if="categoryTypeID == 0">
                                    <div v-if="game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == category.id && (variable.scopeTypeID == '0' || variable.scopeTypeID == '1')).length > 0">
                                        <speedrun-grid-tab-variable :items="game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == category.id && (variable.scopeTypeID == '0' || variable.scopeTypeID == '1'))" :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="category.id.toString()" :levelid="''" :subcategoryvariablevalueids="subCategoryVariableValueIDs" :userid="userID" :prevdata="''" :additionalvariablevalues="additionalVariableValues" :variableindex="variableIndex" @variablevalueclick="onVariableValueClick"></speedrun-grid-tab-variable>
                                    </div>
                                    <div v-else>
                                        <speedrun-grid :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="category.id.toString()" :levelid="''" :variablevalues="''" :userid="userID"></speedrun-grid>
                                    </div>
                                </div>
                                <div v-else>
                                    <div class="levelrow row no-gutters pr-1 pt-1 pb-0 pr-0">
                                        <div class="col tab-list">
                                            <ul class="nav nav-pills">
                                                <li class="level nav-item py-1 pr-1" v-for="(level, levelIndex) in game.levels" :key="level.id">
                                                    <a class="nav-link p-2" :class="{ 'active' : levelID == level.id }" href="#/" :data-value="level.id" data-toggle="pill" @click="onLevelClick">{{ level.name }}</a>
                                                </li>
                                                <button-dropdown v-if="additionalLevels.length > 0" class="py-1 pr-1" :class="{ 'active' : additionalLevels.filter(i => i.id == levelID).length > 0 }">
                                                    <template v-slot:text>
                                                        <span>More...</span>
                                                    </template>
                                                    <template v-slot:options>
                                                        <li class="level dropdown-item" v-for="(level, levelIndex) in additionalLevels" :key="level.id">
                                                            <a class="nav-link p-2" :class="{ 'active' : level == level.id }" href="#/" :data-value="level.id" data-toggle="pill" @click="onLevelClick">{{ level.name }}</a>
                                                        </li>
                                                    </template>
                                                </button-dropdown>
                                            </ul>
                                        </div>
                                    </div>
                                    <div v-for="(level, levelIndex) in game.levels" :key="level.id">
                                        <div v-if="levelID == level.id">
                                            <div v-if="game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == category.id && variable.levelID == level.id && (variable.scopeTypeID == '0' || variable.scopeTypeID == '2' || variable.scopeTypeID == '3')).length > 0">
                                                <speedrun-grid-tab-variable :items="game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == category.id && variable.levelID == level.id && (variable.scopeTypeID == '0' || variable.scopeTypeID == '2' || variable.scopeTypeID == '3'))" :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="category.id.toString()" :levelid="level.id.toString()" :subcategoryvariablevalueids="subCategoryVariableValueIDs" :userid="userID" :prevdata="''" :additionalvariablevalues="additionalVariableValues" :variableindex="variableIndex" @variablevalueclick="onVariableValueClick"></speedrun-grid-tab-variable>
                                            </div>
                                            <div v-else>
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
                loading: false,
                additionalItems: [],
                additionalCategories: [],
                additionalLevels: [],
                additionalVariableValues: []
            }
        },
        computed: {
            userID: function () {
                return this.isgame ? '' : this.id;
            }
        },
        created: function () {
            this.loadData();
            window.addEventListener('resize', this.onResizeTabs);
        },
        mounted: function() {
            var a = "test";
        },
        updated: function () {
            this.setAdditionals();
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

                this.categoryID = this.categoryID || game.categories.find(category => category.categoryTypeID == that.categoryTypeID).id;

                if (this.categoryTypeID == 1) {
                    this.levelID = this.levelID || (game.levels ? game.levels[0].id : '');
                } else {
                    this.levelID = '';
                }

                if (!this.subCategoryVariableValueIDs) {
                    this.subCategoryVariableValueIDs = {};
                    if (this.categoryTypeID == 0) {
                        game.subCategoryVariables?.filter(variable => variable.categoryID == that.categoryID && (variable.scopeTypeID == '0' || variable.scopeTypeID == '1'))
                            .forEach(variable => { that.subCategoryVariableValueIDs[variable.name] = variable.variableValues[0].name })

                    } else if (this.categoryTypeID == 1) {
                        game.subCategoryVariables?.filter(variable => variable.categoryID == that.categoryID && (variable.scopeTypeID == '0' || variable.scopeTypeID == '2' || variable.scopeTypeID == '3'))
                            .forEach(variable => { that.subCategoryVariableValueIDs[variable.name] = variable.variableValues[0].name })
                    }
                }
            },
            resetSelected: function () {
                var that = this;
                var game = this.items.find(game => game.id == that.gameID);

                if (game.categoryTypes.filter(i => i.id == that.categoryTypeID).length == 0) {
                    this.categoryTypeID = game.categoryTypes[0].id;
                }

                if (game.categories.filter(i => i.id == that.categoryID).length == 0) {
                    this.categoryID = game.categories[0].id;
                }

                if (this.categoryTypeID == 1 && game.levels.filter(i => i.id == that.levelID).length == 0) {
                    this.levelID = game.levels[0].id;
                } else {
                    this.levelID = '';
                }

                var eligibleVariables = [];
                if (this.categoryTypeID == 0) {
                    eligibleVariables = game.subCategoryVariables?.filter(variable => variable.categoryID == that.categoryID && (variable.scopeTypeID == '0' || variable.scopeTypeID == '1'));
                } else {
                    eligibleVariables = game.subCategoryVariables?.filter(variable => variable.categoryID == that.categoryID && variable.levelID == that.levelID && (variable.scopeTypeID == '0' || variable.scopeTypeID == '2' || variable.scopeTypeID == '3'));
                }

                var newSubCategoryVariableValueIDs = {};
                eligibleVariables.forEach(variable => {    
                    var variableValue = variable.variableValues.filter(variableValue => Object.keys(that.subCategoryVariableValueIDs).map(key => that.subCategoryVariableValueIDs[key]).filter(x => x == variableValue.name).length > 0)[0] ?? variable.variableValues[0];
                    newSubCategoryVariableValueIDs[variable.name] = variableValue.name;              
                });

                this.subCategoryVariableValueIDs = newSubCategoryVariableValueIDs;
            },
            onGameClick: function (event) {
                var value = event.target.getAttribute('data-value');
                this.gameID = value;

                this.resetSelected();
                this.additionalCategories = [];
                this.additionalLevels = [];
                this.additionalVariableValues = [];                    
            },
            onCategoryTypeClick: function (event) {
                var value = event.target.getAttribute('data-value');
                this.categoryTypeID = value;

                this.resetSelected();
                this.additionalCategories = [];
                this.additionalLevels = [];
                this.additionalVariableValues = [];                
            },
            onCategoryClick: function (event) {
                var value = event.target.getAttribute('data-value');
                this.categoryID = value;

                this.resetSelected();
                this.additionalLevels = [];
                this.additionalVariableValues = [];                
            },
            onLevelClick: function (event) {
                var value = event.target.getAttribute('data-value');
                this.levelID = value;

                this.resetSelected();
                this.additionalVariableValues = [];                 
            },
            onVariableValueClick: function (event) {
                var variableName = event.target.getAttribute('data-variable');
                var variableIndex = event.target.getAttribute('data-variableindex');
                var value = event.target.getAttribute('data-name');                
                this.subCategoryVariableValueIDs[variableName] = value;

                this.resetSelected();
                this.additionalVariableValues = this.additionalVariableValues.filter(x => x.variableIndex <= variableIndex);
            },
            onResizeTabs: function(){
                this.additionalItems = [];
                this.additionalCategories = [];
                this.additionalLevels = [];
                this.additionalVariableValues = [];
                this.setAdditionals();
            },
            setAdditionals: function() {
                this.setAdditionalItems();
                this.setAdditionalCategories();
                this.setAdditionalLevels();
                this.setAdditionalVariableValues();                
            },
            setAdditionalItems: function () {
                var totalWidth = 0;
                var items = document.querySelectorAll('.tab-list li.game:not(.dropdown-item)');

                for (var i = 0; i < items.length; i++) {
                    totalWidth += items[i].offsetWidth;
                    if (totalWidth > (document.querySelector('.gamerow').offsetWidth - 100)) {
                        var achorItem = items[i].querySelector('a');
                        var id = achorItem.getAttribute('data-value')
                        var name = achorItem.innerHTML;
                        this.additionalItems.push({ id: id, name: name });
                        items[i].remove();
                    }
                }
            },
            setAdditionalCategories: function () {
                var totalWidth = 0;
                var items = document.querySelectorAll('.tab-list li.category:not(.dropdown-item)');

                for (var i = 0; i < items.length; i++) {
                    totalWidth += items[i].offsetWidth;
                    if (totalWidth > (document.querySelector('.categoryrow').offsetWidth - 100)) {
                        var achorItem = items[i].querySelector('a');
                        var id = achorItem.getAttribute('data-value')
                        var name = achorItem.innerHTML;
                        this.additionalCategories.push({ id: id, name: name });
                        items[i].remove();
                    }
                }
            },
            setAdditionalLevels: function () {
                var totalWidth = 0;
                var items = document.querySelectorAll('.tab-list li.level:not(.dropdown-item)');

                for (var i = 0; i < items.length; i++) {
                    totalWidth += items[i].offsetWidth;
                    if (totalWidth > (document.querySelector('.levelrow').offsetWidth - 100)) {
                        var achorItem = items[i].querySelector('a');
                        var id = achorItem.getAttribute('data-value')
                        var name = achorItem.innerHTML;
                        this.additionalLevels.push({ id: id, name: name });
                        items[i].remove();
                    }
                }
            },
            setAdditionalVariableValues: function () {
                var variableItems = document.querySelectorAll('.variablerow');
                for (var g = 0; g < variableItems.length; g++) {
                    var totalWidth = 0;
                    var items = variableItems[g].querySelectorAll('.tab-list li.variableValue:not(.dropdown-item)');

                    for (var i = 0; i < items.length; i++) {
                        totalWidth += items[i].offsetWidth;
                        if (totalWidth > (variableItems[g].offsetWidth - 100)) {
                            var achorItem = items[i].querySelector('a');
                            var variableName = achorItem.getAttribute('data-variable');
                            var id = achorItem.getAttribute('data-value');
                            var name = achorItem.getAttribute('data-name');
                            this.additionalVariableValues.push({ variableName: variableName, variableIndex: g, id: id, name: name });
                            items[i].remove();
                        }
                    }
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

    .tab-list .dropdown.active .btn.dropdown-toggle {
        background-color: var(--primary);
    }

    .tab-row-name {
        font-size: 14px !important;
        line-height: 18px;
        font-weight: bold;
    } 
</style>




