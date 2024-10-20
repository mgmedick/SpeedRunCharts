<template>
    <div class="img-container">
        <div v-if="!showVideo" @click="showVideo = !showVideo;" class="stretchy-wrapper rounded" style="position:relative">
            <div class="ratio ratio-16x9" style="overflow: hidden;">
                <img :src="item.videoThumbnailLink" class="align-self-center" style="height:100%; width:100%; overflow:hidden;"/>
            </div>
        </div>
        <div v-else class="ratio ratio-16x9" style="overflow: hidden;">
            <iframe :src="item.embeddedVideoLinkAutoplay"
                        frameborder="0"
                        scrolling="no"
                        width="100%"
                        height="100%"                  
                        allowfullscreen="true"></iframe>  
        </div>                    
        <div class="d-flex g-2 py-2 px-sm-0 px-2">
            <div class="align-self-start" style="width: 40px; flex: none;">
                <div class="img-round">
                    <img :src="item.gameCoverImageLink" class="img-fluid" alt="Responsive image">
                </div>
            </div>
            <div class="px-2" style="overflow: hidden;">       
                <div class="row g-2 mb-2">
                    <div class="col-auto align-self-end">
                        <div class="nowrap-elipsis align-self-start">
                            <a :href="'/Game/GameDetails/' + encodeURIComponent(item.gameAbbr)" class="text-decoration-none text-reset" style="font-weight: 500;">{{ item.gameName }}</a>
                        </div>
                        <div class="align-self-end" style="line-height: 12px;">
                            <small class="text-muted">{{ item.relativeVerifyDateString }}</small><span v-if="item.viewCountString">&nbsp;&middot;&nbsp;<small class="text-muted">{{ item.viewCountString + " views" }}</small></span>
                        </div>                    
                    </div>
                </div>
                <div class="row g-2 mb-1">
                    <div class="col-auto">  
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
                </div>                       
                <div class="row g-1 mb-1">                    
                    <div v-if="item.categoryTypeName" class="col-auto">
                        <span class="badge border border-dark text-body fw-light">{{ item.categoryTypeName }}</span>
                    </div>
                    <div v-if="item.categoryName" class="col-auto">
                        <span v-if="item.categoryName" class="badge border border-dark text-body fw-light">{{ item.categoryName }}</span>
                    </div>
                    <div v-if="item.levelName" class="col-auto">
                        <span v-if="item.levelName" class="badge border border-dark text-body fw-light">{{ item.levelName }}</span>
                    </div>
                    <div v-for="(subCategoryVariableValue, index) in item.subCategoryVariableValueNames" class="col-auto">
                        <span class="badge border border-dark text-body fw-light">{{ subCategoryVariableValue }}</span>
                    </div>                
                </div>
            </div>
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






