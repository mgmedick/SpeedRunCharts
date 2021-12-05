<template>
    <div v-if="loading">
        <div class="d-flex">
            <div class="mx-auto">
                <i class="fas fa-spinner fa-spin fa-lg"></i>
            </div>
        </div>
    </div>
    <div v-else id="divWorldRecorGridTabContainer" class="container-lg p-0">
        <div v-if="!isgame" class="row no-gutters pr-1 pt-1 pb-0">
            <div class="col tab-list">
                <ul class="nav nav-pills">
                    <li class="nav-item py-1 pr-1" v-for="(game, gameIndex) in items" :key="game.id">
                        <a class="nav-link p-2" :class="{ 'active' : gameID == game.id }" href="#/" data-type="game" :data-value="game.id" data-toggle="pill" @click="onTabClick">{{ game.name }}</a>
                    </li>
                    <button-dropdown v-show="false" class="more py-1 pr-1" :class="{ 'active' : moreActive['game'] }">
                        <template v-slot:text>
                            <span>More...</span>
                        </template>
                        <template v-slot:options>
                            <li class="dropdown-item" v-for="(game, gameIndex) in items" :key="game.id">
                                <a class="nav-link p-2" :class="{ 'active' : gameID == game.id }" href="#/" data-type="game" :data-value="game.id" data-toggle="pill" @click="onTabClick">{{ game.name }}</a>
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
                            <li class="nav-item py-1 pr-1" v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id">
                                <a class="nav-link p-2" :class="{ 'active' : categoryTypeID == categoryType.id }" href="#/" data-type="categoryType" :data-value="categoryType.id" data-toggle="pill" @click="onTabClick">{{ categoryType.name }}</a>
                            </li>
                            <button-dropdown v-show="false" class="more py-1 pr-1" :class="{ 'active' : moreActive['categoryType'] }">
                                <template v-slot:text>
                                    <span>More...</span>
                                </template>
                                <template v-slot:options>
                                    <li class="dropdown-item" v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id">
                                        <a class="nav-link p-2" :class="{ 'active' : categoryTypeID == categoryType.id }" href="#/" data-type="categoryType" :data-value="categoryType.id" data-toggle="pill" @click="onTabClick">{{ categoryType.name }}</a>
                                    </li>
                                </template>
                            </button-dropdown>                                
                        </ul>
                    </div>
                </div>
                <div v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id">
                    <div v-if="categoryTypeID == categoryType.id">
                        <div class="row no-gutters pr-1 pt-1 pb-0 pr-0">
                            <div class="col tab-list">
                                <ul class="nav nav-pills">
                                    <li class="nav-item py-1 pr-1" v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id)" :key="category.id">
                                        <a class="nav-link p-2" :class="{ 'active' : categoryID == category.id }" href="#/" data-type="category" :data-value="category.id" data-toggle="pill" @click="onTabClick">{{ category.name }}</a>
                                    </li>
                                    <button-dropdown v-show="false" class="more py-1 pr-1" :class="{ 'active' : moreActive['category'] }">
                                        <template v-slot:text>
                                            <span>More...</span>
                                        </template>
                                        <template v-slot:options>
                                            <li class="dropdown-item" v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id)" :key="category.id">
                                                <a class="nav-link p-2" :class="{ 'active' : categoryID == category.id }" href="#/" data-type="category" :data-value="category.id" data-toggle="pill" @click="onTabClick">{{ category.name }}</a>
                                            </li>
                                        </template>
                                    </button-dropdown>                                     
                                </ul>
                            </div>
                        </div>                        
                        <div v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id)" :key="category.id">
                            <div v-if="categoryID == category.id">
                                <div v-if="categoryTypeID == 0">
                                    <worldrecord-grid :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="category.id.toString()" :levelid="''" :userid="userID"></worldrecord-grid>
                                </div>
                                <div v-else>
                                    <div class="levelrow row no-gutters pr-1 pt-1 pb-0 pr-0">
                                        <div class="col tab-list">
                                            <ul class="nav nav-pills">
                                                <li class="level nav-item py-1 pr-1" v-for="(level, levelIndex) in game.levelTabs.filter(lvl => lvl.categoryID == category.id && (!hideEmpty || lvl.hasData))" :key="level.id">
                                                    <a class="nav-link p-2" :class="{ 'active' : levelID == level.id }" href="#/" data-type="level" :data-value="level.id" data-toggle="pill" @click="onTabClick">{{ level.name }}</a>
                                                </li>
                                                <button-dropdown v-show="false" class="more py-1 pr-1" :class="{ 'active' : moreActive['level'] }">
                                                    <template v-slot:text>
                                                        <span>More...</span>
                                                    </template>
                                                    <template v-slot:options>
                                                        <li class="dropdown-item" v-for="(level, levelIndex) in game.levelTabs.filter(lvl => lvl.categoryID == category.id && (!hideEmpty || lvl.hasData))" :key="level.id">
                                                            <a class="nav-link p-2" :class="{ 'active' : levelID == level.id }" href="#/" data-type="level" :data-value="level.id" data-toggle="pill" @click="onTabClick">{{ level.name }}</a>
                                                        </li>
                                                    </template>
                                                </button-dropdown>   
                                            </ul>
                                        </div>
                                    </div>
                                    <div v-for="(level, levelIndex) in game.levelTabs.filter(lvl => lvl.categoryID == category.id)" :key="level.id">
                                        <div v-if="levelID == level.id">                                            
                                            <worldrecord-grid :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="category.id.toString()" :levelid="level.id.toString()" :userid="userID"></worldrecord-grid>
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
        name: "WorldRecordGridTabVue",
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
                moreActive: {},
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
            window.addEventListener('resize', this.resizeWRTabs);
        },
        updated: function () {
            this.resizeWRTabs();
        },        
        methods: {
            loadData() {
                var that = this;
                this.loading = true;

                var prms = axios.get('../Game/GetSpeedRunGridTabs', { params: { ID: this.id, isGame: this.isgame } })
                                .then(res => {
                                    that.items = res.data.tabItems;
                                    //that.gameID = res.data.gameID;
                                    //that.categoryTypeID = res.data.categoryTypeID;
                                    //that.categoryID = res.data.categoryID;
                                    //that.levelID = res.data.levelID;
                                    //that.subCategoryVariableValueIDs = res.data.subCategoryVariableValueIDs;
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

                if (this.categoryTypeID == 1) {
                    this.levelID = this.levelID || (game.levelTabs ? game.levelTabs.filter(lvl => lvl.categoryID == that.categoryID)[0]?.id : '');
                } else {
                    this.levelID = '';
                }                
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

                if (this.categoryTypeID == 1) {
                    if(game.levelTabs.filter(i => i.categoryID == that.categoryID && i.id == that.levelID).length == 0) {
                        this.levelID = game.levelTabs.filter(lvl => lvl.categoryID == that.categoryID)[0]?.id;
                    }
                } else {
                    this.levelID = '';
                }                
            },
            onTabClick: function (event) {
                var type = event.target.getAttribute('data-type');
                var value = event.target.getAttribute('data-value');
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
                }

                this.moreActive[type] = isMore;

                this.resetSelected();                
            },
            resizeWRTabs: function () {
                var rows = document.querySelectorAll('#divWorldRecorGridTabContainer .tab-list');

                for (var g = 0; g < rows.length; g++) {
                    var totalWidth = 0;
                    var tabitems = rows[g].querySelectorAll('li:not(.dropdown-item)');
                    var moreItems = rows[g].querySelectorAll('li.dropdown-item');
                    var morebtn =  rows[g].querySelector('.more');

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

                    morebtn.style.display = morebtn.querySelectorAll('li:not(.d-none)').length > 0 ? 'block' : 'none';
               }
            }                        
            // onGameClick: function (event) {
            //     var value = event.target.getAttribute('data-value');
            //     this.gameID = value;

            //     this.resetSelected();
            // },
            // onCategoryTypeClick: function (event) {
            //     var value = event.target.getAttribute('data-value');
            //     this.categoryTypeID = value;

            //     var that = this;
            //     this.selected = this.selected.filter(item => !(item.gameID == that.gameID && item.type == 'categorytype'));
            //     this.selected.push({ gameID: this.gameID, categoryTypeID: this.categoryTypeID, type: 'categorytype' });

            //     this.resetSelected();
            // },
            // onCategoryClick: function (event) {
            //     var value = event.target.getAttribute('data-value');
            //     this.categoryID = value;

            //     var that = this;
            //     this.selected = this.selected.filter(item => !(item.gameID == that.gameID && item.categoryTypeID == that.categoryTypeID && item.type == 'category'));
            //     this.selected.push({ gameID: this.gameID, categoryTypeID: this.categoryTypeID, categoryID: this.categoryID, type: 'category' });

            //     this.resetSelected();
            // }
        }
    };
</script>
<style>
    .tab-list .nav-link {
        background-color: #313131;
        font-size: 13px;
        font-weight: bold;
    }

/*    .tab-list .nav-link:hover {
        background-color: #2b2a2a;
    }
*/
   .tab-row-name {
        font-size: 14px !important;
        line-height: 18px;
        font-weight: bold;
    } 
</style>




