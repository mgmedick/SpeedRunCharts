<template>
    <div class="speedRunSummary">
        <div class="row no-gutters p-2">
            <div class="col-1 p-0 align-self-center" style="max-width:37px;">
                <div class="img-round">
                    <img :src="item.gameCoverImageLink" class="img-fluid" alt="Responsive image">
                </div>
            </div>
            <div class="col-auto pl-2 pr-0 align-self-end">
                <div class="nowrap-elipsis align-self-start" style="font-size: 14px; font-weight: 500;">
                    <a :href="'/Game/GameDetails/' + encodeURIComponent(item.gameAbbr)" class="text-primary">{{ item.gameName }}</a>
                </div>
                <div class="align-self-end" style="line-height: 12px;">
                    <small class="text-secondary">{{ item.relativeVerifyDateString }}</small><span v-if="item.viewCountString">&nbsp;&middot;&nbsp;<small class="text-secondary">{{ item.viewCountString + " views" }}</small></span>
                </div>                    
            </div>
        </div>
        <div class="p-2 d-flex">
            <div class="col p-0 align-self-end" style="overflow:hidden;">
                <div>   
                    <div class="text-secondary nowrap-elipsis" style="font-size: 14px;">
                        <a :href="'/Game/GameDetails/' + encodeURIComponent(item.gameAbbr) + '?speedRunCode=' + item.code" class="text-primary"><template v-if="item.rankString"><i v-if="getIconClass(item.rank)" class="fa fa-trophy pr-1" :class="getIconClass(item.rank)"></i><span style="font-weight: 500;">{{ item.rankString }}</span>&nbsp;-&nbsp;</template><span style="font-size: 13px;">{{ item.primaryTimeString }}</span></a>
                    </div>
                    <div class="text-secondary font-weight-semibold" style="font-size: 14px;">
                        <template v-for="(player, index) in item.players">                               
                            <span v-if="player.colorLight && player.colorDark" class='username-text username-color-light' :style="'background: linear-gradient(to right,' + player.colorLight + ',' + (player.colorToLight || player.colorLight) + ');'">
                                <span class='username-text username-color-dark' :style="'background: linear-gradient(to right,' + player.colorDark + ',' + (player.colorToDark || player.colorDark) + ');'">
                                    <a :href="'/Player/PlayerDetails/' + encodeURIComponent(player.abbr) + '?speedRunCode=' + item.code" class="text-primary">{{ player.name }}</a>
                                </span>
                            </span>
                            <span v-else class="username-text">
                                <a :href="'/Player/PlayerDetails/' + encodeURIComponent(player.abbr) + '?speedRunCode=' + item.code">{{ player.name }}</a>
                            </span>
                            <span class="text-primary">{{ (item.players.length -1 == index) ? '' : ', ' }}</span>
                        </template>
                    </div>
                </div>                            
                <div>
                    <span v-if="item.categoryTypeName" class="badge badge-secondary font-weight-normal mr-1 mt-1 text-secondary" style="white-space:normal; text-align:left;">{{ item.categoryTypeName }}</span>
                    <span v-if="item.categoryName" class="badge badge-secondary font-weight-normal mr-1 mt-1 text-secondary" style="white-space:normal; text-align:left;">{{ item.categoryName }}</span>
                    <span v-if="item.levelName" class="badge badge-secondary font-weight-normal mr-1 mt-1 text-secondary" style="white-space:normal; text-align:left;">{{ item.levelName }}</span>
                    <template v-for="(subCategoryVariableValue, index) in item.subCategoryVariableValueNames">
                        <span class="badge badge-secondary font-weight-normal mr-1 mt-1 text-secondary" style="white-space:normal; text-align:left;">{{ subCategoryVariableValue }}</span>
                    </template>
                </div>                      
            </div>
            <div class="col align-self-end p-0 show-sm" @click="showVideo = !showVideo">
                <div :class="{ 'stretchy-wrapper' : item.isVideoThumbnailLowRes }">
                    <div class="embed-responsive embed-responsive-16by9">
                        <div class="embed-responsive-item">
                            <div style="position: relative;" :style="[ item.videoThumbnailLink ? null : { height:'100%' } ]">
                                <img :src="item.videoThumbnailLink" style="width: 100%; height: auto; overflow:hidden;"/>
                                <i class="play-icon fa fa-play fa-lg"></i>
                            </div>
                        </div>
                    </div>
                </div>                  
            </div>                    
        </div>
        <div class="body show-md">
            <div v-if="!showVideo" @click="showVideo = !showVideo;">
                <div :class="{ 'stretchy-wrapper' : item.isVideoThumbnailLowRes }">
                    <div class="embed-responsive embed-responsive-16by9">
                        <div class="embed-responsive-item">
                            <div style="position: relative;" :style="[ item.videoThumbnailLink ? null : { height:'100%' } ]">
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






