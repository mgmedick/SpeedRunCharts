<template>
    <div style="overflow:auto;">
        <div id="divGridContainer" class="pt-2" style="max-width:1200px">
            <div v-if="isgame" class="row no-gutters px-1 pt-1 pb-0">
                <div class="col-sm-1 align-self-top">
                    <label>Game:</label>
                </div>
                <div class="col pl-2">
                    <ul class="nav nav-pills">
                        <li class="nav-item" v-for="(game, gameIndex) in items" :key="game.id">
                            <a class="nav-link" :class="{ 'active' : gameIndex == 0 }" :href="'#divGame-' + game.id" data-toggle="pill">{{ game.name }}</a>
                        </li>
                    </ul>
                </div>
            </div>
            <div v-for="(game, gameIndex) in items" :key="game.id">
                <div v-if="gameIndex == 0" :id="'divGame-' + gameIndex" class="p-0">
                    <div class="row no-gutters pl-1 pt-1 pb-0 pr-0">
                        <div class="col-sm-1 align-self-top">
                            <label>Category Type:</label>
                        </div>
                        <div class="col pl-2">
                            <ul class="nav nav-pills">
                                <li class="nav-item" v-for="(categoryType, categoryTypeIndex) in game.categoryTypes" :key="categoryType.id">
                                    <a class="nav-link" :class="{ 'active' : categoryTypeIndex == 0 }" href="'#divGame-' + gameIndex + '-CategoryType-' + categoryTypeIndex" data-toggle="pill">{{ categoryType.name }}</a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div :id="'divGame-' + gameIndex + 'CategoryType-' + categoryTypeIndex" v-for="(categoryType, categoryTypeIndex) in items" :key="categoryType.id">
                    <div class="row no-gutters pl-1 pt-1 pb-0 pr-0">
                        <div class="col-sm-1 align-self-top">
                            <label>Category:</label>
                        </div>
                        <div class="col pl-2">
                            <ul class="nav nav-pills">
                                <li class="nav-item" v-for="(category, categoryIndex) in game.categories" :key="category.id">
                                    <a class="nav-link" :class="{ 'active' : categoryIndex == 0 }" href="'#divGame-' + gameIndex + '-CategoryType-' + categoryTypeIndex + '-Category-' + categoryIndex" data-toggle="pill">{{ category.name }}</a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>                     
            </div>
        </div>
    </div>

<!-- <div style="overflow:auto;">
    <div id="divGridContainer" class="tab-grid-container pt-2">
        <div class="game-tabs row no-gutters px-1 pt-1 pb-0" style="<%= (item.sender == 'Game') ? 'display:none' : '' %>">
            <div class="tab-row-name-container col-auto align-self-end">
                <label class="tab-row-name">Game:</label>
            </div>
            <div class="col pl-2 scroller-tab-wrapper">
                <div class="row no-gutters">
                    <div class="scroller-tab scroller-tab-left-end btn btn-secondary"><i class="fa fa-step-backward"></i></div>
                    <div class="scroller-tab scroller-tab-left btn btn-secondary"><i class="fa fa-angle-left"></i></div>
                    <div class="col scroller-tab-list-container">
                        <ul class="nav nav-pills scroller-tab-list">
                            <% _.each(item.tabItems, function(game, gameIndex) { %>
                            <li class="nav-item game">
                                <a class="nav-link <%= (gameIndex == 0) ? ' active' : '' %>" href="#divGame-<%= gameIndex %>" data-toggle="pill"><%= game.name %></a>
                            </li>
                            <% }) %>
                        </ul>
                    </div>
                    <div class="scroller-tab scroller-tab-right btn btn-secondary"><i class="fa fa-angle-right"></i></div>
                    <div class="scroller-tab scroller-tab-right-end btn btn-secondary"><i class="fa fa-step-forward"></i></div>
                </div>
            </div>
        </div>
        <% _.each(item.tabItems, function(game, gameIndex) { %>
        <div>
            <div id="divGame-<%= gameIndex %>" class="game-tab-pane p-0 <%= gameIndex == 0 ? ' show' : '' %>">
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
                                    <li class="nav-item categoryType">
                                        <a class="nav-link <%= (categoryTypeIndex == 0) ? ' active' : '' %>" href="#divGame-<%= gameIndex %>-CategoryType-<%= categoryTypeIndex %>" data-toggle="pill"><%= categoryType.name %></a>
                                    </li>
                                    <% }) %>
                                </ul>
                            </div>
                            <div class="scroller-tab scroller-tab-right btn btn-secondary"><i class="fa fa-angle-right"></i></div>
                            <div class="scroller-tab scroller-tab-right-end btn btn-secondary"><i class="fa fa-step-forward"></i></div>
                        </div>
                    </div>
                </div>
                <% _.each(game.categoryTypes, function(categoryType, categoryTypeIndex) { %>
                <div id="divGame-<%= gameIndex %>-CategoryType-<%= categoryTypeIndex %>" class="categoryType-tab-pane <%= (categoryTypeIndex == 0) ? ' show' : '' %>">
                    <div class="category-tabs row no-gutters pl-1 pt-1 pb-0 pr-0">
                        <div class="tab-row-name-container col-auto align-self-end">
                            <label class="tab-row-name">Category:</label>
                        </div>
                        <div class="col pl-2 scroller-tab-wrapper">
                            <div class="row no-gutters">
                                <div class="scroller-tab scroller-tab-left-end btn btn-secondary"><i class="fa fa-step-backward"></i></div>
                                <div class="scroller-tab scroller-tab-left btn btn-secondary"><i class="fa fa-angle-left"></i></div>
                                <div class="col scroller-tab-list-container">
                                    <ul class="nav nav-pills scroller-tab-list">
                                        <% _.each(_.filter(game.categories, function(ft) { return ft.categoryTypeID == categoryType.id }), function(category, categoryIndex) { %>
                                        <li class="nav-item category">
                                            <a class="nav-link <%= (categoryIndex == 0) ? ' active' : '' %>" href="#divGame-<%= gameIndex %>-CategoryType-<%= categoryTypeIndex %>-Category-<%= categoryIndex %>" data-toggle="pill" data-categorytype="<%= category.categoryTypeID %>"><%= category.name %><% if(!category.hasData) { %><i class="pl-1">(empty)</i><% } %></a>
                                        </li>
                                        <% }) %>
                                    </ul>
                                </div>
                                <div class="scroller-tab scroller-tab-right btn btn-secondary"><i class="fa fa-angle-right"></i></div>
                                <div class="scroller-tab scroller-tab-right-end btn btn-secondary"><i class="fa fa-step-forward"></i></div>
                            </div>
                        </div>
                    </div>
                    <% _.each(_.filter(game.categories, function(ft) { return ft.categoryTypeID == categoryType.id }), function(category, categoryIndex) { %>
                    <div id="divGame-<%= gameIndex %>-CategoryType-<%= categoryTypeIndex %>-Category-<%= categoryIndex %>" class="category-tab-pane <%= (categoryIndex == 0) ? ' show' : '' %>">
                        <div class="category-results">
                            <% if(_.filter(game.subCategoryVariablesTabs, function(ft) { return ft.categoryID == category.id && (ft.scopeTypeID == '0' || ft.scopeTypeID == '1') }).length > 0) { %>
                            <%= fn.renderSpeedRunGridVariableTemplate({ item: new speedRunGridVariableModel(_.filter(game.subCategoryVariablesTabs, function(ft) { return ft.categoryID == category.id && (ft.scopeTypeID == '0' || ft.scopeTypeID == '1'); }), 'category', game.id, categoryType.id, category.id, '', gameIndex, categoryTypeIndex, categoryIndex, '', '', '', 0), fn: { renderSpeedRunGridVariableTemplate: fn.renderSpeedRunGridVariableTemplate }}) %>
                            <% } else { %>
                            <div class="grid-container" data-gameid="<%= game.id %>" data-categorytype="<%= categoryType.id %>" data-categoryid="<%= category.id %>" data-levelid="">
                                <table id="tblGame-<%= gameIndex %>-CategoryType-<%= categoryTypeIndex %>-Category-<%= categoryIndex %>-Level-" class="grid"></table>
                                <div id="divPager-<%= gameIndex %>-CategoryType-<%= categoryTypeIndex %>-Category-<%= categoryIndex %>-Level-" class="pager"></div>
                                <div id="divLoading-<%= gameIndex %>-CategoryType-<%= categoryTypeIndex %>-Category-<%= categoryIndex %>-Level-" class="loading">
                                    <div class="container d-flex ml-0" style="height:465px;">
                                        <div class="mx-auto align-self-center">
                                            <i class="fa fa-spinner fa-spin fa-2x"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <% } %>
                        </div>
                        <div class="level-results">
                            <div class="level-tabs row no-gutters pl-1 pt-1 pb-0 pr-0">
                                <div class="tab-row-name-container col-auto align-self-end">
                                    <label class="tab-row-name">Levels:</label>
                                </div>
                                <div class="col pl-2 scroller-tab-wrapper">
                                    <div class="row no-gutters">
                                        <div class="scroller-tab scroller-tab-left-end btn btn-secondary"><i class="fa fa-step-backward"></i></div>
                                        <div class="scroller-tab scroller-tab-left btn btn-secondary"><i class="fa fa-angle-left"></i></div>
                                        <div class="col scroller-tab-list-container">
                                            <ul class="nav nav-pills scroller-tab-list">
                                                <% _.each(_.filter(game.levelTabs, function(ft) { return ft.categoryID == category.id }), function(level, levelIndex) { %>
                                                <li class="nav-item level">
                                                    <a class="nav-link <%= (levelIndex == 0) ? ' active' : '' %>" href="#divGame-<%= gameIndex %>-CategoryType-<%= categoryTypeIndex %>-Category-<%= categoryIndex %>-Level-<%= levelIndex %>" data-toggle="pill"><%= level.name %><% if(!level.hasData) { %><i class="pl-1">(empty)</i><% } %></a>
                                                </li>
                                                <% }) %>
                                            </ul>
                                        </div>
                                        <div class="scroller-tab scroller-tab-right btn btn-secondary"><i class="fa fa-angle-right"></i></div>
                                        <div class="scroller-tab scroller-tab-right-end btn btn-secondary"><i class="fa fa-step-forward"></i></div>
                                    </div>
                                </div>
                            </div>
                            <% _.each(_.filter(game.levelTabs, function(ft) { return ft.categoryID == category.id }), function(level, levelIndex) { %>
                            <div id="divGame-<%= gameIndex %>-CategoryType-<%= categoryTypeIndex %>-Category-<%= categoryIndex %>-Level-<%= levelIndex %>" class="level-tab-pane">
                                <% if(_.filter(game.subCategoryVariablesTabs, function(ft) { return ft.categoryID == category.id && ft.levelID == level.id && (ft.scopeTypeID == '0' || ft.scopeTypeID == '2' || ft.scopeTypeID == '3') }).length > 0) { %>
                                <%= fn.renderSpeedRunGridVariableTemplate({ item: new speedRunGridVariableModel(_.filter(game.subCategoryVariablesTabs, function(ft) { return ft.categoryID == category.id && ft.levelID == level.id && (ft.scopeTypeID == '0' || ft.scopeTypeID == '2' || ft.scopeTypeID == '3') }), 'level', game.id, categoryType.id, category.id, level.id, gameIndex, categoryTypeIndex, categoryIndex, levelIndex, '', '', 0), fn: { renderSpeedRunGridVariableTemplate: fn.renderSpeedRunGridVariableTemplate }}) %>
                                <% } else { %>
                                <div class="grid-container" data-gameid="<%= game.id %>" data-categorytype="<%= categoryType.id %>" data-categoryid="<%= category.id %>" data-levelid="<%= level.id %>">
                                    <table id="tblGame-<%= gameIndex %>-CategoryType-<%= categoryTypeIndex %>-Category-<%= categoryIndex %>-Level-<%= levelIndex %>-VariableValue-NA" class="grid"></table>
                                    <div id="divPager-<%= gameIndex %>-CategoryType-<%= categoryTypeIndex %>-Category-<%= categoryIndex %>-Level-<%= levelIndex %>-VariableValue-NA" class="pager"></div>
                                    <div id="divLoading-<%= gameIndex %>-CategoryType-<%= categoryTypeIndex %>-Category-<%= categoryIndex %>-Level-<%= levelIndex %>-VariableValue-NA" class="loading">
                                        <div class="container d-flex ml-0" style="height:465px;">
                                            <div class="mx-auto align-self-center">
                                                <i class="fa fa-spinner fa-spin fa-2x"></i>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <% } %>
                            </div>
                            <% }) %>
                        </div>
                    </div>
                    <% }) %>
                </div>
                <% }) %>
            </div>
        </div>
        <% }) %>
    </div>
</div> -->


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
                        that.loading = false;
                        return res;
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            }
        }
    };
</script>


