<template>
    <div style="overflow:auto;">
        <div id="divGridContainer" class="pt-2" style="max-width:1200px">
            <div v-if="isgame" class="row no-gutters px-1 pt-1 pb-0">
                <div class="col-sm-1 align-self-top pt-1">
                    <label>Game:</label>
                </div>
                <div class="col pl-2">
                    <ul class="nav nav-pills">
                        <li class="nav-item p-1" v-for="(game, gameIndex) in items" :key="game.id">
                            <a class="game nav-link p-2" :class="{ 'active' : gameID == game.id }" href="#" :data-value="game.id" data-toggle="pill" @click="onGameClick">{{ game.name }}</a>
                        </li>
                    </ul>
                </div>
            </div>
            <div v-for="(game, gameIndex) in items" :key="game.id">
                <div v-if="gameID == game.id" v-for="(game, gameIndex) in items" :key="game.id" class="p-0">
                    <div class="row no-gutters pl-1 pt-1 pb-0 pr-0">
                        <div class="col-sm-1 align-self-top pt-1">
                            <label>Category Type:</label>
                        </div>
                        <div class="col pl-2">
                            <ul class="nav nav-pills">
                                <li class="nav-item p-1" v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id">
                                    <a class="categoryType nav-link p-2" :class="{ 'active' : categoryTypeID == categoryType.id }" :data-value="categoryType.id" href="#" data-toggle="pill" @click="onCategoryTypeClick">{{ categoryType.name }}</a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id">
                    <div v-if="categoryTypeID == categoryType.id">
                        <div class="row no-gutters pl-1 pt-1 pb-0 pr-0">
                            <div class="col-sm-1 align-self-top pt-1">
                                <label>Category:</label>
                            </div>
                            <div class="col pl-2">
                                <ul class="nav nav-pills">
                                    <li class="nav-item p-1" v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id)" :key="category.id">
                                        <a class="category nav-link p-2" :class="{ 'active' : categoryID == category.id }" :data-value="category.id" href="#" data-toggle="pill" @click="onCategoryClick">{{ category.name }}</a>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id)" :key="category.id">
                            <div v-if="categoryID == category.id">
                                <div v-if="categoryTypeID == 1" class="row no-gutters pl-1 pt-1 pb-0 pr-0">
                                    <div class="col-sm-1 align-self-top pt-1">
                                        <label>Level:</label>
                                    </div>
                                    <div class="col pl-2">
                                        <ul class="nav nav-pills">
                                            <li class="nav-item p-1" v-for="(level, levelIndex) in game.levels" :key="level.id">
                                                <a class="level nav-link p-2" :class="{ 'active' : levelID == level.id }" href="#" :data-value="level.id" data-toggle="pill" @click="onLevelClick">{{ level.name }}</a>
                                            </li>
                                        </ul>
                                        <!--<div class="btn-group btn-group-toggle pr-2">
                                            <label class="btn btn-primary btn-sm category active">
                                                <input type="radio" id="btnLevel0" name="btnLevel" autocomplete="off" value="0" v-model="levelID" @click="onTabClick"><i class="fa fa-certificate fa-lg"></i>&nbsp;New
                                            </label>
                                        </div>-->
                                    </div>
                                        <!--<div class="btn-group btn-group-toggle pr-2">
                                            <label class="btn btn-primary btn-sm category active">
                                                <input type="radio" id="btnCategory0" name="btnCategory" autocomplete="off" value="0" v-model="categoryid" @click="onCategoryChange"><i class="fa fa-certificate fa-lg"></i>&nbsp;New
                                            </label>
                                            <label class="btn btn-primary btn-sm category">
                                                <input type="radio" id="btnCategory1" name="btnCategory" autocomplete="off" value="1" v-model="categoryid" @click="onCategoryChange"><i class="fa fa-percentage fa-lg"></i>&nbsp;First
                                            </label>
                                            <label class="btn btn-primary btn-sm category">
                                                <input type="radio" id="btnCategory2" name="btnCategory" autocomplete="off" value="2" v-model="categoryid" @click="onCategoryChange"><i class="fa fa-award fa-lg"></i>&nbsp;Top 5%
                                            </label>
                                            <label class="btn btn-primary btn-sm category">
                                                <input type="radio" id="btnCategory3" name="btnCategory" autocomplete="off" value="3" v-model="categoryid" @click="onCategoryChange"><i class="fa fa-fire fa-lg"></i>&nbsp;Top 3
                                            </label>
                                            <label class="btn btn-primary btn-sm category">
                                                <input type="radio" id="btnCategory4" name="btnCategory" autocomplete="off" value="4" v-model="categoryid" @click="onCategoryChange"><i class="fa fa-star fa-lg"></i>&nbsp;PBs
                                            </label>
                                            <i class="fa fa-info-circle pl-1" data-toggle="tooltip" data-placement="bottom" data-html="true" title="<b>New:</b> Newly verified runs.<br/><br/><b>Top 5%:</b> Runs with rank in top 5% of category.<br/><br/><b>First:</b> Runs with 1st place in category.<br/><br/><b>Top 3:</b> Runs in top 3 with 20 or more runs.<br/><br/><b>PBs:</b> Personal Bests for that category."></i>
                                        </div>-->
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
                selected: {},
                gameID: '',
                categoryTypeID: '',
                categoryIDs: '',
                levelID: '',
                loading: true
            }
        },
        computed: {
        },
        created: function () {
            this.loadData();
        },
        methods: {
            loadData() {
                var that = this;
                this.loading = true;

                var url = this.isgame ? '../Game/GetSpeedRunGridTabs' : '../User/GetSpeedRunGridTabs';

                axios.get(url, { params: { ID: this.id } })
                    .then(res => {
                        that.items = res.data.tabItems;
                        that.gameID = res.data.gameID;
                        that.categoryTypeID = res.data.categoryTypeID;
                        that.categoryID = res.data.categoryID;
                        that.levelID = res.data.levelID;
                        that.loading = false;
                        return res;
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            },
            onGameClick: function (event) {
                var value = event.target.getAttribute('data-value');
                this.gameID = value;
            },
            onCategoryTypeClick: function (event) {
                var value = event.target.getAttribute('data-value');
                this.categoryTypeID = value;
            },
            onCategoryClick: function (event) {
                var value = event.target.getAttribute('data-value');
                categoryIDs[this.categoryTypeID] = value;
                this.categoryID = value;
            },
            onLevelClick: function (event) {
                var value = event.target.getAttribute('data-value');
                this.levelID = value;
            }
        }
    };

    function getFormattedData(data) {
        var gridData = {};

        $(data).each(function () {
            var categoryTypeID = this.categoryType.id;
            var gameID = this.game.id;
            var categoryID = this.category.id;
            var levelID = this.level ? this.level.id : '';
            var variableValues = $(this.subCategoryVariableValues).filter(function () { return !levelID && this.variable.gameID == gameID && this.variable.categoryID == categoryID }).map(function () {
                return this.variable.id + "|" + this.id;
            }).get().join(",");

            gridData[categoryTypeID] = gridData[categoryTypeID] || [];
            gridData[categoryTypeID][gameID] = gridData[categoryTypeID][gameID] || [];
            gridData[categoryTypeID][gameID][categoryID] = gridData[categoryTypeID][gameID][categoryID] || [];
            gridData[categoryTypeID][gameID][categoryID][levelID] = gridData[categoryTypeID][gameID][categoryID][levelID] || [];
            gridData[categoryTypeID][gameID][categoryID][levelID][variableValues] = gridData[categoryTypeID][gameID][categoryID][levelID][variableValues] || [];

            gridData[categoryTypeID][gameID][categoryID][levelID][variableValues].push(this);
        });

        return gridData;
    }

</script>
<style>
    .nav-link {
        background-color: #313131;
    }

    .nav-link:hover {
        background-color: #2b2a2a;
    }
</style>




