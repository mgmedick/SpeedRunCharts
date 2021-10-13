<template>
    <div>
        <div v-if="loading">
            <div class="d-flex">
                <div class="mx-auto">
                    <i class="fas fa-spinner fa-spin fa-lg"></i>
                </div>
            </div>
        </div>
        <div v-else id="divSpeedRunEdit" class="container p-0">
            <div class="form-group row no-gutters mb-2">
                <div class="col-sm-10 m-auto">
                    <div class="p-0 embed-responsive embed-responsive-16by9">
                        <iframe class="embed-responsive-item"
                                :src="item.speedRunVM.videoLink"
                                loading="lazy"
                                frameborder="0"
                                autoplay="0"
                                scrolling="no"
                                allowfullscreen="true"></iframe>
                    </div>
                </div>
            </div>        
            <div class="form-group row no-gutters mb-2">
                <label class="col-sm-2 col-form-label">Game</label>
                <div class="col-sm-10">
                    <span class="icon-wrapper">
                        <input type="text" name="game" class="form-control mr-sm-2 game-search" style="width:300px;" placeholder="Search Games" aria-label="Search" v-model="item.speedRunVM.game.name">
                    </span>
                </div>
            </div>
            <div class="form-group row no-gutters mb-2">
                <label class="col-sm-2 col-form-label">Status</label>
                <div class="col-sm-10">
                    <select id="drpStatus" class="custom-select form-control" style="width:220px;" v-model="item.speedRunVM.statusType.id">
                        <option v-for="statusType in item.statusTypes" :value="statusType.id">{{ statusType.name }}</option>
                    </select>
                </div>
            </div>
            <div class="form-group row no-gutters mb-2" v-if="item.speedRunVM.statusType.id != 0">
                <label class="col-sm-2 col-form-label">Verified Date</label>
                <div class="col-sm-10">
                    <input type="date" class="form-control" value="item.speedRunVM.verifyDateString" style="width:170px;">
                </div>
            </div>
            <div class="form-group row no-gutters mb-2">
                <label class="col-sm-2 col-form-label">Players</label>
                <div class="col-sm-10">
                    <div style="width:300px;">
                        <vue-next-select v-model="playerids" :options="item.players" label-by="name" value-by="id" :disabled="readonly" multiple taggable></vue-next-select>
                    </div>
                </div>
            </div>
            <div class="form-group row no-gutters mb-2">
                <label class="col-sm-2 col-form-label">Emulated</label>
                <div class="col-sm-10">
                    <div class="custom-control custom-switch pt-2">
                        <input id="chkIsEmulated1" type="checkbox" class="custom-control-input" v-model="item.speedRunVM.isEmulated">
                        <label class="custom-control-label" for="chkIsEmulated1"></label>
                    </div>
                </div>
            </div>
            <div class="form-group row no-gutters mb-2">
                <label class="col-sm-2 col-form-label">Category Type</label>
                <div class="col-sm-10">
                    <select id="drpCategoryType" class="custom-select form-control" style="width:220px;" v-model="item.speedRunVM.categoryType.id">
                        <option v-for="categoryType in item.categoryTypes" :value="categoryType.id">{{ categoryType.name }}</option>
                    </select>
                </div>
            </div>
            <div class="form-group row no-gutters mb-2">
                <label class="col-sm-2 col-form-label">Platform</label>
                <div class="col-sm-10">
                    <select id="drpPlatform" class="custom-select form-control" style="width:220px;" v-model="item.speedRunVM.platform.id">
                        <option v-for="platform in item.platforms" :value="platform.id">{{ platform.name }}</option>
                    </select>
                </div>
            </div>
            <div class="form-group row no-gutters mb-2">
                <label class="col-sm-2 col-form-label">Category</label>
                <div class="col-sm-10">
                    <select id="drpCategory" class="custom-select form-control" style="width:220px;" v-model="item.speedRunVM.category.id">
                        <option v-for="category in item.categories" :value="category.id">{{ category.name }}</option>
                    </select>
                </div>
            </div>
            <div class="form-group row no-gutters mb-2" v-if="item.speedRunVM.level">
                <label class="col-sm-2 col-form-label">Level</label>
                <div class="col-sm-10">
                    <select id="drpLevel" class="custom-select form-control" style="width:220px;" v-model="item.speedRunVM.level.id">
                        <option v-for="level in item.levels" :value="level.id">{{ level.name }}</option>
                    </select>
                </div>
            </div>
            <div class="form-group row no-gutters mb-2">
                <label class="col-sm-2 col-form-label">Times</label>
                <div class="col-sm-auto pl-0">
                    <table class="table-responsive">
                        <thead>
                            <tr>
                                <th class="font-weight-normal p-1" style="font-size:12px;">Primary Time</th>
                                <th class="font-weight-normal p-1" style="font-size:12px;">Real Time</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    <input type="text" name="primaryTime" class="form-control time" style="width:125px" v-model="item.speedRunVM.primaryTimeString" />
                                </td>
                                <td>
                                    <input type="text" name="realTime" class="form-control time" style="width:125px" v-model="item.speedRunVM.realTimeString" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="col-sm-auto pl-0">
                    <table class="table-responsive">
                        <thead>
                            <tr>
                                <th class="font-weight-normal p-1" style="font-size:12px;">Real Time w/o Loads</th>
                                <th class="font-weight-normal p-1" style="font-size:12px;">Game Time</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    <input type="text" name="realTime" class="form-control time" style="width:125px" v-model="item.speedRunVM.realTimeWithoutLoadsString" />
                                </td>
                                <td>
                                    <input type="text" name="gameTime" class="form-control time" style="width:125px" v-model="item.speedRunVM.gameTimeString" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="row no-gutters pt-1" v-if="!readonly">
                <div class="form-group">
                    <input id="btnSave" type="button" class="btn btn-primary" value="Save" @click="save" />
                </div>
            </div>
        </div>
    </div>   
</template>
<script>
    import axios from 'axios'

    export default {
        name: 'SpeedRunEditVue',
        props: {
            gameid: String,
            speedrunid: String,
            readonly: Boolean
        },
        data: function () {
            return {
                item: {},
                loading: false
            }
        },
        computed: {
            playerids: function () {
                return this.item.speedRunVM.players.map(i => i.id);
            }
        },
        created: function () {
            this.loadData().then(i => { this.init(); });
        },
        methods: {
            loadData: function () {
                var that = this;
                this.loading = true;

                var prms = axios.get('../SpeedRun/GetEditSpeedRun', { params: { gameID: this.gameid, speedRunID: this.speedrunid } })
                    .then(res => {
                        that.item = res.data;
                        that.loading = false;

                        return res;
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });

                return prms;
            },
            init: function () {
                if (this.readonly) {
                    Array.from(this.$el.querySelectorAll('#divSpeedRunEdit input[type=text], input[type=date], input[type=checkbox], select')).forEach((el) => el.disabled = true);
                }
            },
            save: function () { }
        }
    };
</script>


