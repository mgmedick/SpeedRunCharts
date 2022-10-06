<template>
    <div class="speedRunSummaryContainer container mx-auto p-0" style="max-width:598px; margin-bottom:20px;">
        <div class="speedRunSummary">
            <div class="container pt-2 px-2 pb-0 d-flex">
                <div class="p-0 col-1 align-self-center">
                    <div style="max-width:37px;">
                        <div class="img-round">
                            <img :src="item.gameCoverImageLink" class="img-fluid" alt="Responsive image">
                        </div>
                    </div>
                </div>
                <div class="col-auto pl-2 pr-0 align-self-end" style="max-width: 70%;">
                    <div class="nowrap-elipsis align-self-start" style="font-size: 14px;">
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
            <div class="container px-2 pb-2 pt-1 d-flex">
                <div class="col-sm-9 p-0 align-self-center" style="overflow:hidden;">
                     <div>   
                        <div class="text-secondary nowrap-elipsis" style="font-size: 14px;">
                            <a :href="'/Game/GameDetails/' + item.game.abbr + '?speedRunID=' + item.speedRunComID" class="text-primary"><template v-if="item.rankString"><i v-if="getIconClass(item.rank)" class="fa fa-trophy pr-1" :class="getIconClass(item.rank)"></i><span>{{ item.rankString }}</span>&nbsp;-&nbsp;</template><small>{{ item.primaryTimeString }}</small></a>
                        </div>
                        <div class="text-secondary nowrap-elipsis" style="font-size: 14px;">
                            <template v-for="(player, index) in item.players">
                                <a :href="'/User/UserDetails/' + player.abbr + '?speedRunID=' + item.speedRunComID" class="text-primary">{{ player.name }}</a>
                                {{ (item.players.length -1 != index) ? ', ' : '' }}
                            </template>
                        </div>
                    </div>                            
                    <div class="py-1">
                        <span v-if="item.categoryType?.name" class="badge badge-secondary font-weight-normal mr-1 text-secondary">{{ item.categoryType?.name }}</span>
                        <span v-if="item.category?.name" class="badge badge-secondary font-weight-normal mr-1 text-secondary">{{ item.category?.name }}</span>
                        <span v-if="item.level?.name" class="badge badge-secondary font-weight-normal mr-1 text-secondary">{{ item.level?.name }}</span>
                        <template v-for="(subCategoryVariableValueName, index) in item.subCategoryVariableValueNames">
                            <span class="badge badge-secondary font-weight-normal mr-1 text-secondary">{{ subCategoryVariableValueName }}</span>
                        </template>
                    </div>                      
                </div>
                <div class="col-sm-3 ml-auto align-self-center p-0 show-sm image-container" @click="showVideoSm = !showVideoSm">
                    <img :src="item.videoThumbnailLink" />
                    <i class="play-icon fa fa-play"></i>
                </div>                    
            </div>
            <div v-if="isMediaMedium" class="body p-0 embed-responsive embed-responsive-16by9 show-md">
                <iframe :src="item.videoLink"
                        loading="lazy"           
                        frameborder="0"
                        autoplay="0"
                        scrolling="no"
                        width="100%"
                        height="auto"
                        allowfullscreen="true"></iframe>
            </div>            
            <div v-else-if="showVideoSm" class="body p-0 embed-responsive embed-responsive-16by9 show-sm">
                <iframe :src="item.videoLink"
                        loading="lazy"
                        frameborder="0"
                        autoplay="0"
                        scrolling="no"
                        width="100%"
                        height="auto"                        
                        allowfullscreen="true"></iframe>
            </div>
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
                showVideoSm: false
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
<style scoped>
    .image-container {
        position: relative;
        width: 90px;
        margin-left:auto;
    }

    .image-container img{
        width: 90px;
    }

    .image-container .play-icon {
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

    .badge {
        font-size: 85% !important;
    }

    @media (max-width: 768px) {
        .btn {
            font-size:11px;
            padding:4px 6px;
        }

        .show-md {
            display: none;
        }

        .show-sm {
            display: block;
        }
    }

    @media (min-width: 768px) {
        .show-md {
            display: block;
        }

        .show-sm {
            display: none;
        }
    }    
</style>




