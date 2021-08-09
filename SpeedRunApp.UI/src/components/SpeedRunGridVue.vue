<template>
    <div style="overflow:auto;">
        <div id="divGridContainer" class="tab-grid-container pt-2">
            <div v-if="isGame" class="game-tabs row no-gutters px-1 pt-1 pb-0">
                <div class="tab-row-name-container col-auto align-self-end">
                    <label class="tab-row-name">Game:</label>
                </div>
                <div class="col pl-2 scroller-tab-wrapper">
                    <div class="row no-gutters">
                        <div class="scroller-tab scroller-tab-left-end btn btn-secondary"><i class="fa fa-step-backward"></i></div>
                        <div class="scroller-tab scroller-tab-left btn btn-secondary"><i class="fa fa-angle-left"></i></div>
                        <div class="col scroller-tab-list-container">
                            <ul class="nav nav-pills scroller-tab-list">
                                <li class="nav-item game" v-for="(game, gameindex) in items" :game="game" :gameindex="gameindex" :key="game.id">
                                    <a class="nav-link" :class="{ (gameindex == 0): active }" :href="'#divGame-' + game.id" data-toggle="pill">{{ game.name }}</a>
                                </li>
                            </ul>
                        </div>
                        <div class="scroller-tab scroller-tab-right btn btn-secondary"><i class="fa fa-angle-right"></i></div>
                        <div class="scroller-tab scroller-tab-right-end btn btn-secondary"><i class="fa fa-step-forward"></i></div>
                    </div>
                </div>
            </div>
            <!--<div v-for="(game, gameindex) in items" :item="game" :index="index" :key="game.id">
                <div :id="'divGame-' + gameindex" class="game-tab-pane p-0 <%= gameIndex == 0 ? ' show' : '' %>">
                    <div class="categoryType-tabs row no-gutters pl-1 pt-1 pb-0 pr-0">
                        <div class="tab-row-name-container col-auto align-self-end">
                            <label class="tab-row-name">Category Type:</label>
                        </div>
                        <div class="col pl-2 scroller-tab-wrapper">
                            <div class="row no-gutters">
                                <div class="scroller-tab scroller-tab-left-end btn btn-secondary"><i class="fa fa-step-backward"></i></div>
                                <div class="scroller-tab scroller-tab-left btn btn-secondary"><i class="fa fa-angle-left"></i></div>
                                <div class="col scroller-tab-list-container">
                                    <ul class="nav nav-pills scroller-tab-list">
                                        <% _.each(game.categoryTypes, function(categoryType, categoryTypeIndex) { %>
                                        <li class="nav-item categoryType" v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :item="categoryType" :index="index" :key="categoryType.id">
                                            <a class="nav-link" :class="(categoryTypeIndex == 0) : active" href="#'#divGame-' + game.id + '-CategoryType-' + '<%= categoryTypeIndex %>" data-toggle="pill"><%= categoryType.name %></a>
                                        </li>
                                        <% }) %>
                                    </ul>
                                </div>
                                <div class="scroller-tab scroller-tab-right btn btn-secondary"><i class="fa fa-angle-right"></i></div>
                                <div class="scroller-tab scroller-tab-right-end btn btn-secondary"><i class="fa fa-step-forward"></i></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>-->
        </div>
    </div>
</template>
<script>
    import axios from 'axios';

    export default {
        name: "SpeedRunGridVue",
        props: {
            isgame: Boolean,
            id: String
        },
        data() {
            return {
                items: [],
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

                var url = this.isgame ? '../Game/GetSpeedRunGrid' : '../User/GetSpeedRunGrid';

                axios.get(url, { params: { id: this.id } })
                    .then(res => {
                        that.items = res.data;
                        that.loading = false;
                        return res;
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            }
        }
    };
</script>


