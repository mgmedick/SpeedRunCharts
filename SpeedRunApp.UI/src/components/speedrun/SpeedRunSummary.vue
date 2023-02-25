<template>
    <div class="speedRunSummary">
        <div class="pt-2 px-2 d-flex">
            <div class="col-1 p-0 align-self-center">
                <div style="max-width:37px;">
                    <div class="img-round">
                        <img :src="item.gameCoverImageLink" class="img-fluid" alt="Responsive image">
                    </div>
                </div>
            </div>
            <div class="col-8 pl-2 pr-0 align-self-end">
                <div class="nowrap-elipsis align-self-start" style="font-size: 14px; font-weight: 500;">
                    <a :href="'/Game/GameDetails/' + item.game.abbr" class="text-primary">{{ item.game.name }}</a>
                </div>
                <div class="align-self-end" style="line-height: 12px;">
                    <small class="text-secondary">{{ item.relativeVerifyDateStringShort }}</small><span v-if="item.viewCountString">&nbsp;&middot;&nbsp;<small class="text-secondary">{{ item.viewCountString + " views" }}</small></span>
                </div>                    
            </div>
            <div class="col-auto ml-auto p-0 align-self-start">
                <button class="btn btn-secondary detail" @click="showModal = true" style="font-size:12px;">Details</button>
            </div>
        </div>
        <div class="px-2 pb-2 pt-1 d-flex">
            <div class="col-7 p-0 align-self-center" style="overflow:hidden;">
                <div>   
                    <div class="text-secondary nowrap-elipsis" style="font-size: 14px; font-weight: 500;">
                        <a :href="'/Game/GameDetails/' + item.game.abbr + '?speedRunID=' + item.speedRunComID" class="text-primary"><template v-if="item.rankString"><i v-if="getIconClass(item.rank)" class="fa fa-trophy pr-1" :class="getIconClass(item.rank)"></i><span>{{ item.rankString }}</span>&nbsp;-&nbsp;</template><small>{{ item.primaryTimeString }}</small></a>
                    </div>
                    <div class="text-secondary" style="font-size: 14px; font-weight: 600;">
                        <template v-for="(player, index) in item.players">                               
                            <span v-if="player.colorLight && player.colorDark" class='username-text username-color-light' :style="'background: linear-gradient(to right,' + player.colorLight + ',' + (player.colorToLight || player.colorLight) + ');'">
                                <span class='username-text username-color-dark' :style="'background: linear-gradient(to right,' + player.colorDark + ',' + (player.colorToDark || player.colorDark) + ');'">
                                    <a :href="'/User/UserDetails/' + player.abbr + '?speedRunID=' + item.speedRunComID" class="text-primary">{{ player.name }}</a>
                                </span>
                            </span>
                            <span v-else class="username-text">
                                <a :href="'/User/UserDetails/' + player.abbr + '?speedRunID=' + item.speedRunComID">{{ player.name }}</a>
                            </span>
                            {{ (item.players.length -1 != index) ? ', ' : '' }}
                        </template>
                    </div>
                </div>                            
                <div>
                    <span v-if="item.categoryType?.name" class="badge badge-secondary font-weight-normal mr-1 mt-1 text-secondary">{{ item.categoryType?.name }}</span>
                    <span v-if="item.category?.name" class="badge badge-secondary font-weight-normal mr-1 mt-1 text-secondary">{{ item.category?.name }}</span>
                    <span v-if="item.level?.name" class="badge badge-secondary font-weight-normal mr-1 mt-1 text-secondary">{{ item.level?.name }}</span>
                    <template v-for="(subCategoryVariableValueName, index) in item.subCategoryVariableValueNames">
                        <span class="badge badge-secondary font-weight-normal mr-1 mt-1 text-secondary">{{ subCategoryVariableValueName }}</span>
                    </template>
                </div>                      
            </div>
            <div class="col-5 align-self-end p-0 show-sm" @click="showVideo = !showVideo">
                <div :class="{ 'stretchy-wrapper' : item.isVideoThumbnailLowRes }">
                    <div class="embed-responsive embed-responsive-16by9">
                        <div class="embed-responsive-item">
                            <div style="position:relative;">
                                <img :src="item.videoThumbnailLink" style="width: 100%; height: auto; overflow:hidden;"/>
                                <i class="play-icon fa fa-play fa-lg"></i>
                            </div>
                        </div>
                    </div>
                </div>                  
            </div>                    
        </div>
        <div class="body p-0 show-md">
            <div v-if="!showVideo" @click="showVideo = !showVideo;">
                <div :class="{ 'stretchy-wrapper' : item.isVideoThumbnailLowRes }">
                    <div class="embed-responsive embed-responsive-16by9">
                        <div class="embed-responsive-item">
                            <div style="position:relative;">
                                <img :src="item.videoThumbnailLink" style="width: 100%; height: auto; overflow:hidden;"/>
                                <i class="play-icon fa fa-play fa-5x"></i> 
                            </div>
                        </div>
                    </div>
                </div>               
            </div>
            <div v-else class="embed-responsive embed-responsive-16by9">                    
                <iframe :src="item.videoLinkAutoplay"
                    frameborder="0"
                    scrolling="no"
                    width="100%"
                    height="auto"                  
                    allowfullscreen="true"></iframe>                 
            </div>                
        </div>            
        <div v-if="showVideo" class="body p-0 embed-responsive embed-responsive-16by9 show-sm">
            <iframe :src="item.videoLink"
                    frameborder="0"
                    scrolling="no"
                    width="100%"
                    height="auto"                        
                    allowfullscreen="true"></iframe> 
        </div>
        <input type="hidden" class="orderValue" :value="item.id" />
        <modal v-if="showModal" contentclass="cmv-modal-lg" @close="showModal = false">
            <template v-slot:title>
                Details
            </template>
            <div class="container">
                <speedrun-edit :gameid="item.game.id.toString()" :speedrunid="item.id.toString()" :readonly="true" />
            </div>
        </modal>
    </div>
</template>
<script>
    export default {
        name: "SpeedRunSummary",
        props: {
            item: Object,
            index: Number
        },
        data() {
            return {
                showModal: false,
                showVideo: false
            }
        },
        computed: {
            isMediaMedium: function () {
                return window.innerWidth > 768;
            }
        },        
        methods: {
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
            }
        }       
    };
</script>






