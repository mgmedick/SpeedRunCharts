<template>
    <div class="bg-light">
        <div class="show-lg">
            <div v-if="!showVideo" @click="showVideo = !showVideo;">
                <div :class="{ 'stretchy-wrapper' : item.isVideoThumbnailLowRes }" style="position:relative">
                    <div class="ratio ratio-16x9" style="overflow: hidden;">
                        <img :src="item.videoThumbnailLink" style="width: 100%; height: auto; overflow:hidden;" class="image-container1"/>
                    </div>
                    <div class="position-absolute top-0 bottom-0 start-0 end-0">
                        <div class="d-flex" style="height: 100%;">
                            <div class="mx-auto align-self-center" style="text-align: center;">
                                <i class="play-icon fa fa-play fa-5x"></i>
                            </div>
                        </div>
                    </div>                                       
                </div>                                                     
            </div>
            <div v-else class="ratio ratio-16x9">                    
                <iframe :src="item.embeddedVideoLinkAutoplay"
                    frameborder="0"
                    scrolling="no"
                    width="100%"
                    height="auto"                  
                    allowfullscreen="true"></iframe>                 
            </div>                
        </div>          
        <div class="row g-2 p-2 pb-0">
            <div class="col" style="max-width:50px;">
                <div class="img-round">
                    <img :src="item.gameCoverImageLink" class="img-fluid" alt="Responsive image">
                </div>
            </div>
            <div class="col-auto align-self-end">
                <div class="nowrap-elipsis align-self-start">
                    <a :href="'/Game/GameDetails/' + encodeURIComponent(item.gameAbbr)" class="text-primary text-decoration-none">{{ item.gameName }}</a>
                </div>
                <div class="align-self-end" style="line-height: 12px;">
                    <small class="text-secondary">{{ item.relativeVerifyDateString }}</small><span v-if="item.viewCountString">&nbsp;&middot;&nbsp;<small class="text-secondary">{{ item.viewCountString + " views" }}</small></span>
                </div>                    
            </div>
        </div>
        <div class="d-flex p-2">
            <div class="col p-0 align-self-end" style="overflow:hidden;">
                <div>   
                    <span class="text-secondary me-1">
                        <a :href="'/Game/GameDetails/' + encodeURIComponent(item.gameAbbr) + '?speedRunCode=' + item.code" class="text-primary text-decoration-none"><template v-if="item.rankString"><i v-if="getIconClass(item.rank)" class="fa fa-trophy pe-1" :class="getIconClass(item.rank)"></i><span>{{ item.rankString }}</span>&nbsp;-&nbsp;</template><span style="font-size: 13px;">{{ item.primaryTimeString }}</span></a>
                    </span>&nbsp;-&nbsp;
                    <span class="text-secondary fw-bold">
                        <template v-for="(player, index) in item.players">                               
                            <span v-if="player.colorLight && player.colorDark" class='playername-text playername-color-light' :style="'background: linear-gradient(to right,' + player.colorLight + ',' + (player.colorToLight || player.colorLight) + ');'">
                                <span class='playername-text playername-color-dark' :style="'background: linear-gradient(to right,' + player.colorDark + ',' + (player.colorToDark || player.colorDark) + ');'">
                                    <a :href="'/Player/PlayerDetails/' + encodeURIComponent(player.abbr) + '?speedRunCode=' + item.code" class="text-primary text-decoration-none">{{ player.name }}</a>
                                </span>
                            </span>
                            <span v-else class="playername-text">
                                <a :href="'/Player/PlayerDetails/' + encodeURIComponent(player.abbr) + '?speedRunCode=' + item.code">{{ player.name }}</a>
                            </span>
                            <span class="text-primary text-decoration-none">{{ (item.players.length -1 == index) ? '' : ', ' }}</span>
                        </template>
                    </span>
                </div>                            
                <div>
                    <span v-if="item.categoryTypeName" class="badge text-bg-primary me-1">{{ item.categoryTypeName }}</span>
                    <span v-if="item.categoryName" class="badge text-bg-primary me-1">{{ item.categoryName }}</span>
                    <span v-if="item.levelName" class="badge text-bg-primary me-1">{{ item.levelName }}</span>
                    <template v-for="(subCategoryVariableValue, index) in item.subCategoryVariableValueNames">
                        <span class="badge text-bg-primary me-1">{{ subCategoryVariableValue }}</span>
                    </template>
                </div>                      
            </div>
            <div class="col align-self-end p-0 show-md" @click="showVideo = !showVideo">
                <div :class="{ 'stretchy-wrapper' : item.isVideoThumbnailLowRes }" style="position:relative">
                    <div class="ratio ratio-16x9" style="overflow: hidden;">
                        <img :src="item.videoThumbnailLink" style="width: 100%; height: auto; overflow:hidden;"/>
                    </div>
                    <div class="position-absolute top-0 bottom-0 start-0 end-0">
                        <div class="d-flex" style="height: 100%;">
                            <div class="mx-auto align-self-center" style="text-align: center;">
                                <i class="play-icon fa fa-play fa-lg"></i>
                            </div>
                        </div>
                    </div>                                      
                </div>                                                             
            </div>                    
        </div>          
        <div v-if="showVideo" class="p-0 ratio ratio-16x9 show-md">
            <iframe :src="item.embeddedVideoLink"
                    frameborder="0"
                    scrolling="no"
                    width="100%"
                    height="auto"                        
                    allowfullscreen="true"></iframe> 
        </div>
        <input type="hidden" class="orderValue" :value="item.sortOrder" />
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
                showVideo: false
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






