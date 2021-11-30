<template>
    <div v-if="!loading" id="divGridContainer" class="container-lg p-0">
        <div v-if="!isgame" class="row no-gutters pr-1 pt-1 pb-0">
            <!--<div class="col-sm-1 align-self-top pt-1 mr-2">
                <label class="tab-row-name">Game:</label>
            </div>-->
            <div class="col tab-list">
                <ul class="nav nav-pills">
                    <li class="nav-item py-1 pr-1" v-for="(game, gameIndex) in items" :key="game.id">
                        <a class="game nav-link p-2" :class="{ 'active' : gameID == game.id }" href="#/" :data-value="game.id" data-toggle="pill" @click="onGameClick">{{ game.name }}</a>
                    </li>
                </ul>
            </div>
        </div>
        <div v-for="(game, gameIndex) in items" :key="game.id">
            <div v-if="gameID == game.id">
                <div class="row no-gutters pr-1 pt-1 pb-0 pr-0">
                    <!--<div class="col-sm-1 align-self-top pt-1 mr-2">
                        <label class="tab-row-name">Category Type:</label>
                    </div>-->
                    <div class="col tab-list">
                        <ul class="nav nav-pills">
                            <li class="nav-item py-1 pr-1" v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id">
                                <a class="categoryType nav-link p-2" :class="{ 'active' : categoryTypeID == categoryType.id }" :data-value="categoryType.id" href="#/" data-toggle="pill" @click="onCategoryTypeClick">{{ categoryType.name }}</a>
                            </li>
                        </ul>
                    </div>
                </div>
                <div v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id">
                    <div v-if="categoryTypeID == categoryType.id">
                        <div class="row no-gutters pr-1 pt-1 pb-0 pr-0">
                            <!--<div class="col-sm-1 align-self-top pt-1 mr-2">
                                <label class="tab-row-name">Category:</label>
                            </div>-->
                            <div class="col tab-list">
                                <ul class="nav nav-pills">
                                    <li class="nav-item py-1 pr-1" v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id)" :key="category.id">
                                        <a class="category nav-link p-2" :class="{ 'active' : categoryID == category.id }" :data-value="category.id" href="#/" data-toggle="pill" @click="onCategoryClick">{{ category.name }}</a>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id)" :key="category.id">
                            <div v-if="categoryID == category.id">
                                <!--<worldrecord-grid :isgame="isgame" :id="id" :categorytypeid="categoryTypeID.toString()"></worldrecord-grid>-->
                                <worldrecord-grid :gameid="game.id.toString()" :categorytypeid="categoryType.id.toString()" :categoryid="category.id.toString()" :userid="userID"></worldrecord-grid>
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
                selected: [],
                gameID: '',
                categoryTypeID: '',
                categoryID: '',
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

                this.categoryID = this.categoryID || game.categories.find(category => category.categoryTypeID == that.categoryTypeID).id;
            },
            resetSelected: function () {
                var that = this;
                var game = this.items.find(game => game.id == that.gameID);

                var categoryTypeID = this.selected.find(item => item.gameID == that.gameID && item.type == 'categorytype')?.categoryTypeID;
                this.categoryTypeID = categoryTypeID || game.categoryTypes[0].id;

                var categoryID = this.selected.find(item => item.gameID == that.gameID && item.categoryTypeID == that.categoryTypeID && item.type == 'category')?.categoryID;
                this.categoryID = categoryID || game.categories.find(category => category.categoryTypeID == that.categoryTypeID).id;
            },
            onGameClick: function (event) {
                var value = event.target.getAttribute('data-value');
                this.gameID = value;

                this.resetSelected();
            },
            onCategoryTypeClick: function (event) {
                var value = event.target.getAttribute('data-value');
                this.categoryTypeID = value;

                var that = this;
                this.selected = this.selected.filter(item => !(item.gameID == that.gameID && item.type == 'categorytype'));
                this.selected.push({ gameID: this.gameID, categoryTypeID: this.categoryTypeID, type: 'categorytype' });

                this.resetSelected();
            },
            onCategoryClick: function (event) {
                var value = event.target.getAttribute('data-value');
                this.categoryID = value;

                var that = this;
                this.selected = this.selected.filter(item => !(item.gameID == that.gameID && item.categoryTypeID == that.categoryTypeID && item.type == 'category'));
                this.selected.push({ gameID: this.gameID, categoryTypeID: this.categoryTypeID, categoryID: this.categoryID, type: 'category' });

                this.resetSelected();
            }
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




