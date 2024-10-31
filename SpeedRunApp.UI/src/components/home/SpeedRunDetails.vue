<template>
    <div class="container p-0">
        <div v-if="loading">
            <div class="d-flex">
                <div class="mx-auto">
                    <i class="fas fa-spinner fa-spin fa-lg"></i>
                </div>
            </div>
        </div>
        <div v-else>
            <div class="mx-sm-0 mb-3">
                <div class="ratio ratio-16x9 mb-2">                    
                    <iframe v-if="item.embeddedVideoLinkAutoplay" :src="item.embeddedVideoLinkAutoplay"
                        frameborder="0"
                        scrolling="no"
                        width="100%"
                        height="auto"                  
                        allowfullscreen="true"></iframe>
                    <div v-else class="d-flex align-items-center justify-content-center text-muted">
                        <i class="fas fa-exclamation-circle pe-2"></i><span>No Embedded Video Available</span>
                    </div>                                           
                </div>
                <div class="d-flex g-2 py-2 px-sm-0 px-2">
                    <div class="align-self-start" style="width: 70px; flex: none;">
                        <div class="img-round">
                            <img :src="item.gameCoverImageLink" class="img-fluid" alt="Responsive image">
                        </div>
                    </div>
                    <div class="px-2" style="overflow: hidden;">       
                        <div class="mb-1">
                            <div class="nowrap-elipsis align-self-start">
                                <a :href="'/Game/GameDetails/' + encodeURIComponent(item.gameAbbr)" class="text-decoration-none text-reset" style="font-weight: 500;">{{ item.gameName }}</a>
                            </div>                 
                        </div>
                        <div class="nowrap-elipsis mb-1">  
                            <span class="text-body me-1">
                                <a :href="'/Game/GameDetails/' + encodeURIComponent(item.gameAbbr) + '?speedRunCode=' + item.code" class="text-body text-decoration-none"><template v-if="item.rankString"><i v-if="getIconClass(item.rank)" class="fa fa-trophy pe-1" :class="getIconClass(item.rank)"></i><span>{{ item.rankString }}</span>&nbsp;-&nbsp;</template><span style="font-size: 13px;">{{ item.primaryTimeString }}</span></a>
                            </span>&nbsp;-&nbsp;
                            <span class="text-body fw-bold">
                                <template v-for="(player, index) in item.players">                               
                                    <span v-if="player.colorLight && player.colorDark" class='playername-text playername-color-light' :style="'background: linear-gradient(to right,' + player.colorLight + ',' + (player.colorToLight || player.colorLight) + ');'">
                                        <span class='playername-text playername-color-dark' :style="'background: linear-gradient(to right,' + player.colorDark + ',' + (player.colorToDark || player.colorDark) + ');'">
                                            <a :href="'/Player/PlayerDetails/' + encodeURIComponent(player.abbr) + '?speedRunCode=' + item.code" class="text-primary text-decoration-none">{{ player.name }}</a>
                                        </span>
                                    </span>
                                    <span v-else class="playername-text">
                                        <a :href="'/Player/PlayerDetails/' + encodeURIComponent(player.abbr) + '?speedRunCode=' + item.code">{{ player.name }}</a>
                                    </span>
                                    <span class="text-body text-decoration-none">{{ (item.players.length -1 == index) ? '' : ', ' }}</span>
                                </template>
                            </span>
                        </div>
                        <div class="mb-1">                    
                            <span v-if="item.categoryTypeName" class="badge rounded-pill text-bg-secondary me-1 fw-500 nowrap-elipsis">{{ item.categoryTypeName }}</span>
                            <span v-if="item.categoryName" class="badge rounded-pill text-bg-secondary me-1 fw-500 nowrap-elipsis">{{ item.categoryName }}</span>
                            <span v-if="item.levelName" class="badge rounded-pill text-bg-secondary me-1 fw-500 nowrap-elipsis">{{ item.levelName }}</span>
                            <span v-for="(subCategoryVariableValue, index) in item.subCategoryVariableValueNames" class="badge rounded-pill text-bg-secondary me-1 fw-500 nowrap-elipsis">{{ subCategoryVariableValue }}</span>        
                        </div>
                    </div>
                </div>                    
                <!-- <div v-if="item.videoLinks" v-for="(video, index) in item.videoLinks">
                    <a class="link-offset-2 link-underline link-underline-opacity-0" :href="video.videoLink">{{ video.videoLink }}</a>
                </div>                    -->
            </div>
            <div class="px-3 px-sm-0">  
                <div class="mb-4">
                    <h6 class="text-body lead">Leaderboard</h6>    
                    <div class="row row-cols-lg-5 row-cols-sm-2 row-cols-1 g-3">
                        <div class="col">
                            <div class="card text-bg-secondary">
                                <div class="card-body p-2">
                                    <label class="fw-bold">Game</label>
                                    <div>
                                        <span>{{ item.gameName }}</span>
                                    </div>
                                </div>
                            </div>
                        </div>       
                        <div class="col">
                            <div class="card text-bg-secondary">
                                <div class="card-body p-2">
                                    <label class="fw-bold">Category</label>
                                    <div>
                                        <span>{{ item.categoryName }}</span>
                                    </div>
                                </div>
                            </div>
                        </div>     
                        <div v-if="item.levelName" class="col">
                            <div class="card text-bg-secondary">
                                <div class="card-body p-2">
                                    <label class="fw-bold">Level</label>
                                    <div>
                                        <span>{{ item.levelName }}</span>
                                    </div>
                                </div>
                            </div>
                        </div> 
                        <div v-if="item.variableValues" class="col" v-for="(variableValue, index) in item.variableValues">
                            <div class="card text-bg-secondary">
                                <div class="card-body p-2">
                                    <label class="fw-bold">{{ variableValue.variableName }}</label>
                                    <div>
                                        <span>{{ variableValue.name }}</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>               
                </div>
                <div>
                    <h6 class="text-body lead">Run</h6>     
                    <div class="row row-cols-lg-6 row-cols-sm-2 row-cols-1  g-3">
                        <div class="col">
                            <div class="card text-bg-secondary">
                                <div class="card-body p-2">
                                    <label class="fw-bold">Rank</label>
                                    <div>
                                        <i v-if="getIconClass(item.rank)" class="fa fa-trophy pe-2" :class="getIconClass(item.rank)"></i><span>{{ item.rankString ?? ' - ' }}</span>
                                    </div>
                                </div>
                            </div>
                        </div>                    
                        <div class="col">
                            <div class="card text-bg-secondary">
                                <div class="card-body p-2">
                                    <label class="fw-bold">Time</label>
                                    <div>
                                        <span>{{ item.primaryTimeString }}</span>
                                    </div>
                                </div>
                            </div>
                        </div>                   
                        <div class="col">
                            <div class="card text-bg-secondary">
                                <div class="card-body p-2">
                                    <label class="fw-bold">Players</label>
                                    <div>
                                        <template v-for="(player, index) in item.players">                               
                                            <span v-if="player.colorLight && player.colorDark" class='playername-text playername-color-light' :style="'background: linear-gradient(to right,' + player.colorLight + ',' + (player.colorToLight || player.colorLight) + ');'">
                                                <span class='playername-text playername-color-dark' :style="'background: linear-gradient(to right,' + player.colorDark + ',' + (player.colorToDark || player.colorDark) + ');'">
                                                    <a :href="'/Player/PlayerDetails/' + encodeURIComponent(player.abbr) + '?speedRunCode=' + item.code" class="text-primary">{{ player.name }}</a>
                                                </span>
                                            </span>
                                            <span v-else class="playername-text">
                                                <a :href="'/Player/PlayerDetails/' + encodeURIComponent(player.abbr) + '?speedRunCode=' + item.code">{{ player.name }}</a>
                                            </span>
                                            <span class="text-primary">{{ (item.players.length -1 == index) ? '' : ', ' }}</span>
                                        </template>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div v-if="item.platformName" class="col">
                            <div class="card text-bg-secondary">
                                <div class="card-body p-2">
                                    <label class="fw-bold">Platform</label>
                                    <div>
                                        <span>{{ item.platformName }}</span>
                                    </div>
                                </div>
                            </div>
                        </div>                     
                        <div class="col">
                            <div class="card text-bg-secondary">
                                <div class="card-body p-2">
                                    <label class="fw-bold">Submitted</label>
                                    <div>
                                        <span data-bs-toggle="tooltip" :data-bs-title="getFormattedDateString(item.dateSubmitted)">{{ item.relativeDateSubmittedString }}</span>
                                    </div>
                                </div>
                            </div>
                        </div>                    
                        <div class="col">
                            <div class="card text-bg-secondary">
                                <div class="card-body p-2">
                                    <label class="fw-bold">Verified</label>
                                    <div>
                                        <span data-bs-toggle="tooltip" :data-bs-title="getFormattedDateString(item.dateSubmitted)">{{ item.relativeVerifyDateString }}</span>
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
    import axios from 'axios'
    const dayjs = require('dayjs');
    import { Tooltip } from 'bootstrap';

    export default {
        name: 'SpeedRunDetails',
        props: {
            speedrunid: Number,
            speedrundetailsvm: Object            
        },
        data: function () {
            return {
                item: {},
                loading: false
            }
        },
        computed: {                                            
        },
        created: function () {
            if (this.speedrundetailsvm) {
                this.item = this.speedrundetailsvm;
            } else {
                this.loadData();
            }
        },
        methods: {
            loadData: function () {
                var that = this;
                this.loading = true;

                var prms = axios.get('/SpeedRun/GetSpeedRunDetails', { params: { speedRunID: this.speedrunid } })
                    .then(res => {
                        that.item = res.data;
                        that.loading = false;

                        that.$el.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(el => {
                            new Tooltip(el);                        
                        });

                        return res;
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });

                return prms;
            },     
            getIconClass: function (rank) {
                var iconClass = '';

                switch (rank) {
                    case 1:
                        iconClass = 'gold';
                        break;
                    case 2:
                        iconClass = 'silver';
                        break;
                    case 3:
                        iconClass = 'bronze';
                        break;
                }

                return iconClass;
            },
            getFormattedDateString: function (value) {
                return dayjs(value).format("MM/DD/YYYY");
            },                      
            save: function () { }
        }
    };
</script>


