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
                            <a class="game nav-link p-2" :class="{ 'active' : gameIndex == 0 }" href="#" :data-value="game.id" data-toggle="pill" @click="onTabClick">{{ game.name }}</a>
                        </li>
                    </ul>
                </div>
            </div>
            <div v-for="(game, gameIndex) in items" :key="game.id">
                <div v-for="(game, gameIndex) in items" :key="game.id" class="p-0" :style="[ gameID == game.id ? null : { display:'none' } ]">
                    <div class="row no-gutters pl-1 pt-1 pb-0 pr-0">
                        <div class="col-sm-1 align-self-top pt-1">
                            <label>Category Type:</label>
                        </div>
                        <div class="col pl-2">
                            <ul class="nav nav-pills">
                                <li class="nav-item p-1" v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id">
                                    <a class="categoryType nav-link p-2" :class="{ 'active' : categoryTypeIndex == 0 }" :data-value="categoryType.id" href="#" data-toggle="pill" @click="onTabClick">{{ categoryType.name }}</a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id">
                    <div :style="[ categoryTypeID == categoryType.id ?  null  : { display:'none' } ]">
                        <div class="row no-gutters pl-1 pt-1 pb-0 pr-0">
                            <div class="col-sm-1 align-self-top pt-1">
                                <label>Category:</label>
                            </div>
                            <div class="col pl-2">
                                <ul class="nav nav-pills">
                                    <li class="nav-item p-1" v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id)" :key="category.id">
                                        <a class="category nav-link p-2" :class="{ 'active' : categoryIndex == 0 }" :data-value="category.id" href="#" data-toggle="pill" @click="onTabClick">{{ category.name }}</a>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div v-for="(category, categoryIndex) in game.categories.filter(ctg => ctg.categoryTypeID == categoryType.id)" :key="category.id">
                            <div :style="[ categoryID == category.id ?  null  : { display:'none' } ]">
                                <div v-if="categoryType.id == 1" class="row no-gutters pl-1 pt-1 pb-0 pr-0">
                                    <div class="col-sm-1 align-self-top pt-1">
                                        <label>Level:</label>
                                    </div>
                                    <div class="col pl-2">
                                        <ul class="nav nav-pills">
                                            <li class="nav-item p-1" v-for="(level, levelIndex) in game.levels" :key="level.id">
                                                <a class="level nav-link p-2" :class="{ 'active' : levelIndex == 0 }" href="#" :data-value="level.id" data-toggle="pill" @click="onTabClick">{{ level.name }}</a>
                                            </li>
                                        </ul>
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
            onTabClick: function (event) {
                var classname = event.target.classList[0];
                var value = event.target.getAttribute('data-value');
 
                switch(classname){
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

                Array.from(document.querySelectorAll('.' + classname)).forEach((el) => el.classList.remove('active'));
                event.target.classList.add('active');
            }
        }
    };
</script>
<style>
    .nav-link {
        background-color: #313131;
    }

    .nav-link:hover {
        background-color: #2b2a2a;
    }
</style>




