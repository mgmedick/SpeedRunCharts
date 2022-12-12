<template>
    <div v-if="loading">
        <div class="d-flex">
            <div class="mx-auto">
                <i class="fas fa-spinner fa-spin fa-lg"></i>
            </div>
        </div>
    </div>
    <div v-else id="divWorldRecorGridTabContainer">
        <div v-for="(game, gameIndex) in items" :key="game.id">
            <div v-if="gameID == game.id">
                <div class="row no-gutters pr-1 pt-1 pb-0 pr-0">
                    <div class="col tab-list">
                        <ul class="nav nav-pills">
                            <li class="nav-item py-1 pr-1" v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id">
                                <a class="nav-link p-2" :class="{ 'active' : categoryTypeID == categoryType.id }" href="#/" data-type="categoryType" :data-value="categoryType.id" data-toggle="pill" @click="onTabClick">{{ categoryType.name }}</a>
                            </li>
                            <button-dropdown v-show="false" class="more py-1 pr-1" :btnclasses="'btn-secondary'">
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
                        <div v-if="categoryTypeID == 0">
                            <div v-if="categoryTypeID == 0">
                                {{ setCombinedAndSingleCategoryIDs(game, categoryTypeID) }}
                                <div v-if="combinedCategoryIDs.length > 0">
                                    <game-worldrecord-grid :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryids="combinedCategoryIDs.join(',')" :levelids="''" :showmilliseconds="game.showMilliseconds" :variables="game.variables"></game-worldrecord-grid>                              
                                </div>
                                <div v-for="(categoryID, categoryIndex) in singleCategoryIDs" :key="categoryID">
                                    <game-worldrecord-grid :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryids="categoryID.toString()" :levelids="''" :showmilliseconds="game.showMilliseconds" :variables="game.variables"></game-worldrecord-grid>
                                </div>
                            </div>
                        </div>
                        <div v-else>
                            <div class="row no-gutters pr-1 pt-1 pb-0 pr-0">
                                <div class="col tab-list">
                                    <ul class="nav nav-pills">
                                        <li class="category nav-item py-1 pr-1" v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id)" :key="category.id">
                                            <a class="nav-link p-2" :class="{ 'active' : categoryID == category.id }" href="#/" data-type="category" :data-value="category.id" data-toggle="pill" @click="onTabClick">{{ category.name }}</a>
                                        </li>
                                        <button-dropdown v-show="false" class="more py-1 pr-1" :btnclasses="'btn-secondary'">
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
                                    {{ setCombinedAndSingleLevelIDs(game, category.id) }}
                                    <div v-if="combinedLevelIDs.length > 0">
                                        <div v-if="game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == category.id && combinedLevelIDs.indexOf(variable.levelID) > -1 && (variable.scopeTypeID == '0' || variable.scopeTypeID == '2')).length > 0">
                                            {{ 'mulit var' }}
                                            <!-- <game-worldrecord-variable-grid-tab :items="[...new Set(game.subCategoryVariablesTabs?.map(obj => obj.id))].map(id => { return game.subCategoryVariablesTabs.find(obj => obj.id === id) }).filter(variable => variable.categoryID == category.id && combinedLevelIDs.indexOf(variable.levelID) > -1 && (variable.scopeTypeID == '0' || variable.scopeTypeID == '2'))" :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryids="category.id.toString()" :levelids="combinedLevelIDs.join(',')" :prevdata="''" :variableindex="variableIndex" :showmilliseconds="game.showMilliseconds" :variables="game.variables"></game-worldrecord-variable-grid-tab>                                                                         -->
                                            <game-worldrecord-variable-grid-tab :items="[game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == category.id && combinedLevelIDs.indexOf(variable.levelID) > -1 && (variable.scopeTypeID == '0' || variable.scopeTypeID == '2'))[0]]" :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryids="category.id.toString()" :levelids="combinedLevelIDs.join(',')" :prevdata="''" :variableindex="variableIndex" :showmilliseconds="game.showMilliseconds" :variables="game.variables"></game-worldrecord-variable-grid-tab>                                                                        
                                        </div>
                                        <div v-else>
                                            {{ 'mulit' }}
                                            <game-worldrecord-grid :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryids="category.id.toString()" :levelids="combinedLevelIDs.join(',')" :showmilliseconds="game.showMilliseconds" :variables="game.variables"></game-worldrecord-grid>
                                        </div>
                                    </div>
                                    <div v-for="(levelID, levelIndex) in singleLevelIDs" :key="levelID">
                                        {{ 'single' }}
                                        <!-- <game-worldrecord-grid :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryids="category.id.toString()" :levelids="levelID.toString()" :showmilliseconds="game.showMilliseconds" :variables="game.variables"></game-worldrecord-grid>                                             -->
                                        <game-worldrecord-variable-grid-tab :items="game.subCategoryVariablesTabs?.filter(variable => variable.categoryID == category.id && variable.levelID == levelID)" :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryids="category.id.toString()" :levelids="levelID.toString()" :prevdata="''" :variableindex="variableIndex" :showmilliseconds="game.showMilliseconds" :variables="game.variables"></game-worldrecord-variable-grid-tab>                                                                        
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
        name: "GameWorldRecordGridTab",
        props: {
            id: String
        },
        data() {
            return {
                items: [],
                gameID: '',
                categoryTypeID: '',
                categoryID: '',
                levelID: '',
                combinedCategoryIDs: [],
                singleCategoryIDs: [],                    
                combinedLevelIDs: [],
                singleLevelIDs: [],  
                variableIndex: 0,    
                loading: true
            }
        },
        computed: {
        },
        mounted: function () {
            this.loadData();            
            window.addEventListener('resize', this.resizeWRTabs);
        },
        updated: function () {
            this.resizeWRTabs();
        },        
        methods: {
            loadData() {
                var that = this;
                this.loading = true;

                var url = '/Game/GetWorldRecordGridTabs?gameID=' + this.id;
                var prms = axios.get(url)
                                .then(res => {
                                    that.items = res.data.tabItems;
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
                
                this.categoryID = this.categoryID || game.categories.find(category => category.categoryTypeID == that.categoryTypeID)?.id;                
            },
            resetSelected: function () {
                var that = this;
                var game = this.items.find(game => game.id == that.gameID);

                if (game.categoryTypes.filter(i => i.id == that.categoryTypeID).length == 0) {
                    this.categoryTypeID = game.categoryTypes[0].id;
                }

                if (game.categories.filter(i => i.categoryTypeID == that.categoryTypeID && i.id == that.categoryID).length == 0) {
                    this.categoryID = game.categories.filter(ctg => ctg.categoryTypeID == that.categoryTypeID)[0]?.id;
                }      
            },
            onTabClick: function (event) {
                var type = event.target.getAttribute('data-type');
                var value = event.target.getAttribute('data-value');

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
                }

                this.resetSelected();                
            },
            setCombinedAndSingleCategoryIDs: function(game, categoryTypeID) {
                var that = this;
                this.singleCategoryIDs = game.categories?.filter(ctg => ctg.categoryTypeID == categoryTypeID && game.subCategoryVariables && game.subCategoryVariables.filter(variable => variable.categoryID == ctg.id && (variable.scopeTypeID == '0' || variable.scopeTypeID == '1')).length > 0).map(ctg => ctg.id);
                this.combinedCategoryIDs = game.categories?.filter(ctg => ctg.categoryTypeID == categoryTypeID && that.singleCategoryIDs.indexOf(ctg.id) == -1).map(ctg => ctg.id);
            },            
            setCombinedAndSingleLevelIDs: function(game, categoryID) {
                var that = this;
                this.singleLevelIDs = game.levels?.filter(lvl=> lvl.categoryID == categoryID && game.subCategoryVariables && game.subCategoryVariables.filter(variable => variable.categoryID == categoryID && variable.levelID == lvl.id && variable.scopeTypeID == '3').length > 0).map(lvl => lvl.id);
                this.combinedLevelIDs = game.levels?.filter(lvl=> lvl.categoryID == categoryID && that.singleLevelIDs.indexOf(lvl.id) == -1).map(lvl => lvl.id);
            },
            resizeWRTabs: function () {
                var rows = document.querySelectorAll('#divWorldRecorGridTabContainer .tab-list');

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










