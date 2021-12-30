<template>
    <div class="container mx-auto p-0" style="max-width:598px; margin-bottom:20px;">
        <div class="speedRunSummary bg-dark">
            <template v-if="isMediaMedium">
                <div class="container pt-2 px-2 pb-0 d-flex">
                    <div class="p-0 col-1">
                        <div style="max-width:37px;">
                            <div class="img-round">
                                <img :src="item.gameCoverImageLink" class="img-fluid" alt="Responsive image">
                            </div>
                        </div>
                    </div>
                    <div class="col-auto nowrap-elipsis p-0 align-self-end" style="max-width:70%;">
                        <a :href="'../Game/GameDetails?gameID=' + item.game.id" class="text-primary">{{ item.game.name }}</a>
                    </div>
                    <div class="col-auto pl-2 align-self-end">
                        &middot;
                        <small class="text-secondary pl-1">{{ item.relativeVerifyDateStringShort }}</small>
                    </div>
                    <div class="col-auto ml-auto p-0 align-self-start">
                        <button class="btn btn-secondary detail" @click="showModal = true" style="font-size:12px;">Details</button>
                    </div>
                </div>
                <div class="container px-2 pb-2 pt-1 d-flex">
                    <div class="col-auto p-0 align-self-center">
                        <div class="text-secondary nowrap-elipsis" style="font-size: 14px;">
                            <i v-if="getIconClass(item.rank)" class="fa fa-trophy pr-1" :class="getIconClass(item.rank)"></i><span>{{ item.rankString }}</span>&nbsp;-&nbsp;
                            <template v-for="(player, index) in item.players">
                                <a :href="'../User/UserDetails?userID=' + player.id" class="text-primary">{{ player.name }}</a>
                                {{ (item.players.length -1 != index) ? ', ' : '' }}
                            </template>&nbsp;-&nbsp;<small>{{ item.primaryTimeString }}</small>
                        </div>
                        <div class="py-1">
                            <span v-if="item.category?.name" class="badge badge-secondary font-weight-normal mr-1" style="font-size:12px;">{{ item.category?.name }}</span>
                            <span v-if="item.level?.name" class="pr-1"><span class="badge badge-secondary font-weight-normal mr-1" style="font-size:12px;">{{ item.level?.name }}</span></span>
                            <template v-for="(subCategoryVariableValueName, index) in item.subCategoryVariableValueNames">
                                <span class="badge badge-secondary font-weight-normal mr-1" style="font-size:12px;">{{ subCategoryVariableValueName }}</span>
                            </template>
                        </div>
                    </div>
                </div>
                <div class="body p-0 embed-responsive embed-responsive-16by9">
                    <iframe :src="item.videoLink"
                            loading="lazy"
                            frameborder="0"
                            autoplay="0"
                            scrolling="no"
                            allowfullscreen="true"></iframe>
                </div>
            </template>
            <template v-else>
                <div class="container pt-2 px-2 pb-0 d-flex">
                    <div class="p-0 col-1">
                        <div class="img-round">
                            <img :src="item.gameCoverImageLink" class="img-fluid" alt="Responsive image">
                        </div>
                    </div>
                    <div class="col-auto nowrap-elipsis pl-2 pr-0 align-self-end" style="max-width:60%;">
                        <a :href="'../Game/GameDetails?gameID=' + item.game.id" class="text-primary" style="font-size:14px;">{{ item.game.name }}</a>
                    </div>
                    <div class="col-auto pl-2 align-self-end">
                        &middot;
                        <small class="text-secondary pl-1">{{ item.relativeVerifyDateStringShort }}</small>
                    </div>
                    <div class="col-auto ml-auto p-0 align-self-start">
                        <button class="btn btn-secondary detail px-1 py-1" @click="showModal = true" style="font-size:12px;">Details</button>
                    </div>                      
                </div>
                <div class="container p-2 d-flex">
                    <div class="col-8 p-0 align-self-center">
                        <div class="text-secondary nowrap-elipsis" style="font-size: 14px;">
                            <i v-if="getIconClass(item.rank)" class="fa fa-trophy pr-1" :class="getIconClass(item.rank)"></i><span>{{ item.rankString }}</span>&nbsp;-&nbsp;<small>{{ item.primaryTimeString }}</small>
                        </div>
                        <div class="text-secondary nowrap-elipsis" style="font-size: 14px; ">
                            <template v-for="(player, index) in item.players">
                                <a :href="'../User/UserDetails?userID=' + player.id" class="text-primary">{{ player.name }}</a>
                                {{ (item.players.length -1 != index) ? ', ' : '' }}
                            </template>
                        </div>
                        <div class="py-1">
                            <span v-if="item.category?.name" class="badge badge-secondary font-weight-normal mr-1" style="font-size:12px;">{{ item.category?.name }}</span>
                            <span v-if="item.level?.name" class="pr-1"><span class="badge badge-secondary font-weight-normal mr-1" style="font-size:12px;">{{ item.level?.name }}</span></span>
                            <template v-for="(subCategoryVariableValueName, index) in item.subCategoryVariableValueNames">
                                <span class="badge badge-secondary font-weight-normal mr-1" style="font-size:12px;">{{ subCategoryVariableValueName }}</span>
                            </template>
                        </div>
                    </div>
                    <div class="col-sm-2 ml-auto align-self-center p-0">
                        <div class="image-container" @click="showVideo = !showVideo">
                            <img :src="item.videoThumbnailLink" />
                            <i class="play-icon fa fa-play"></i>
                        </div>
                    </div>
                </div>              
                <div v-if="showVideo" class="body p-0 embed-responsive embed-responsive-16by9">
                    <iframe :src="item.videoLink"
                            loading="lazy"
                            frameborder="0"
                            scrolling="no"
                            autoplay="1"
                            allowfullscreen="true"></iframe>
                </div>                
            </template>
            <input type="hidden" class="orderValue" :value="item.id" />
            <custom-modal v-model="showModal" v-if="showModal" contentclass="modal-lg">
                <template v-slot:title>
                    Details
                </template>
                <speedrun-edit :gameid="item.game.id.toString()" :speedrunid="item.id.toString()" :readonly="true" />
            </custom-modal>
        </div>
    </div>
</template>
<script>
    export default {
        name: "SpeedRunSummaryVue",
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
            }, 
        }       
    };
</script>
<style>
    .image-container {
        position: relative;
        width: 90px;
        margin-left:auto;
    }

    .image-container img{
        width: 90px;
    }

    .image-container .play-icon{
        cursor: pointer;
        position: absolute;
        top : 50%;
        left : 50%;
        transform: translate(-50%, -50%);
    }

    .nowrap-elipsis {
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
    }

    .gold {
        color: gold;
    }

    .silver {
        color: silver;
    }

    .bronze {
        color: #b08d57;
    }
</style>




