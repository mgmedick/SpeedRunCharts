<template>
    <div class="container mx-auto p-0" style="max-width:598px; margin-bottom:20px;">
        <div class="speedRunSummary bg-dark">
            <template v-if="isMediaLarge">
                <div class="container p-2 d-flex">
                    <div class="p-0 col-sm-2 align-self-center">
                        <div class="img-round">
                            <img :src="item.gameCoverImageLink" class="img-fluid" alt="Responsive image">
                        </div>
                    </div>
                    <div class="col-7 align-self-center">
                        <div>
                            <small class="text-muted">Verified {{ item.relativeVerifyDateString }}</small>
                        </div>
                        <div>
                            <a :href="'../Game/GameDetails?gameID=' + item.game.id" class="text-primary">{{ item.game.name }}</a>
                        </div>
                        <div class="text-secondary">
                            <small>{{ item.category.name }}{{ (item.subCategoryVariableValuesString) ? ' - ' : '' }}{{ item.subCategoryVariableValuesString }}</small>
                        </div>
                        <div class="text-secondary" style="font-size: 13px;">
                            {{ (item.rankString) ? item.rankString + ' - ' : '' }}
                            <template v-for="(player, index) in item.players">
                                <a :href="'../User/UserDetails?userID=' + player.id" class="text-secondary">{{ player.name }}</a>
                                {{ (item.players.length == index) ? ', ' : '' }}
                            </template>
                            &nbsp;-&nbsp;
                            {{ item.primaryTimeString }}
                        </div>
                    </div>
                    <div class="details p-0 col-auto ml-auto align-self-center">
                        <button class="btn btn-primary detail" @click="showModal = true" style="font-size:12px;">Details</button>
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
                    <div class="col-auto nowrap-elipsis pl-2 pr-0 align-self-center" style="max-width:60%">
                        <a :href="'../Game/GameDetails?gameID=' + item.game.id" class="text-primary">{{ item.game.name }}</a>
                    </div>
                    <div class="col-auto pl-2 align-self-center">
                        &middot;
                        <small class="text-secondary pl-1">{{ item.relativeVerifyDateStringShort }}</small>
                    </div>
                    <div class="col-auto ml-auto p-0 align-self-center">
                        <button class="btn btn-secondary detail px-1 py-1" @click="showModal = true" style="font-size:12px;">Details</button>
                    </div>                      
                </div>
                <div class="container p-2 d-flex">
                    <div class="col-8 p-0 align-self-center">
                        <div class="text-secondary nowrap-elipsis" style="font-size: 15px;">
                            <i v-if="getIconClass(item.rank)" class="fa fa-trophy pr-1" :class="getIconClass(item.rank)"></i><span class="pr-2">{{ item.rankString }}</span><small>{{ item.primaryTimeString }}</small>
                        </div>                      
                        <div class="text-secondary nowrap-elipsis">
                            <small>{{ item.category.name }}{{ (item.subCategoryVariableValuesString) ? ' - ' : '' }}{{ item.subCategoryVariableValuesString }}</small>
                        </div>
                        <div class="text-secondary nowrap-elipsis" style="font-size: 14px;">
                            <template v-for="(player, index) in item.players">
                                <a :href="'../User/UserDetails?userID=' + player.id" class="text-secondary">{{ player.name }}</a>
                                {{ (item.players.length == index) ? ', ' : '' }}
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
            isMediaLarge: function () {
                return window.innerWidth > 992;
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




