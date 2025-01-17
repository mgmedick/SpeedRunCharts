<template>
    <div class="mb-lg-5">
        <a :href="'/SpeedRun/SpeedRunDetails/' + encodeURIComponent(item.code)" @mouseover="onMouseOver" @mouseleave="onMouseLeave" @touchstart="onVideoTouchStart">
            <div v-if="showVideo" class="ratio ratio-16x9 iframe-wrapper" style="overflow: hidden;">
                <iframe ref="frame" 
                            :src="item.embeddedVideoLinkAutoplay"
                            frameborder="0"
                            scrolling="no"
                            width="100%"
                            height="100%"
                            allowfullscreen="true"></iframe>
            </div>
            <div v-else class="stretchy-wrapper rounded" style="position:relative">
                <div class="ratio ratio-16x9" style="overflow: hidden;">
                    <img :src="item.videoThumbnailLink" class="align-self-center" style="height:100%; width:100%; overflow:hidden;"/>
                </div>
            </div>                
        </a>
        <div class="d-flex g-2 py-2 px-sm-0 px-2">
            <div class="align-self-start" style="width: 40px; flex: none;">
                <div class="img-round">
                    <img :src="item.gameCoverImageLink" class="img-fluid" alt="Responsive image">
                </div>
            </div>
            <div class="px-2" style="overflow: hidden;">       
                <div class="mb-1">
                    <div class="nowrap-elipsis align-self-start">
                        <a :href="'/Game/GameDetails/' + encodeURIComponent(item.gameAbbr) + '?speedRunCode=' + item.code" class="text-decoration-none text-reset" style="font-weight: 500;">{{ item.gameName }}</a>
                    </div>
                    <div class="align-self-end" style="line-height: 12px;">
                        <small class="text-muted">{{ item.relativeVerifyDateString }}</small><span v-if="item.viewCountString">&nbsp;&middot;&nbsp;<small class="text-muted">{{ item.viewCountString + " views" }}</small></span>
                    </div>                    
                </div>
                <div class="nowrap-elipsis mb-1">  
                    <span class="me-1">
                        <a :href="'/Game/GameDetails/' + encodeURIComponent(item.gameAbbr) + '?speedRunCode=' + item.code" class="text-decoration-none text-reset"><template v-if="item.rankString"><i v-if="getIconClass(item.rank)" class="fa fa-trophy pe-1" :class="getIconClass(item.rank)"></i><span>{{ item.rankString }}</span>&nbsp;-&nbsp;</template><span style="font-size: 13px;">{{ item.primaryTimeString }}</span></a>
                    </span>&nbsp;-&nbsp;
                    <span>
                        <template v-for="(player, index) in item.players">                               
                            <span v-if="player.colorLight && player.colorDark" class='playername-text playername-color-light' :style="'background: linear-gradient(to right,' + player.colorLight + ',' + (player.colorToLight || player.colorLight) + ');'">
                                <span class='playername-color-dark' :style="'background: linear-gradient(to right,' + player.colorDark + ',' + (player.colorToDark || player.colorDark) + ');'">
                                    <a :href="'/Player/PlayerDetails/' + encodeURIComponent(player.abbr) + '?speedRunCode=' + item.code" class="text-primary text-decoration-none">{{ player.name }}</a>
                                </span>
                            </span>
                            <span v-else class="playername-text">
                                <a :href="'/Player/PlayerDetails/' + encodeURIComponent(player.abbr) + '?speedRunCode=' + item.code">{{ player.name }}</a>
                            </span>
                            <span class="text-decoration-none">{{ (item.players.length -1 == index) ? '' : ', ' }}</span>
                        </template>
                    </span>
                </div>
                <div class="nowrap-elipsis mb-1">                    
                    <span v-if="item.categoryTypeName" class="badge rounded-pill text-bg-secondary me-1 fw-500">{{ item.categoryTypeName }}</span>
                    <span v-if="item.categoryName" class="badge rounded-pill text-bg-secondary me-1 fw-500">{{ item.categoryName }}</span>
                    <span v-if="item.levelName" class="badge rounded-pill text-bg-secondary me-1 fw-500">{{ item.levelName }}</span>
                    <span v-for="(subCategoryVariableValue, index) in item.subCategoryVariableValueNames" class="badge rounded-pill text-bg-secondary me-1 fw-500">{{ subCategoryVariableValue }}</span>        
                </div>
            </div>
        </div>    
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
                showVideo: false,
                mouseOver: false,
                touchStart: false,
                throttleTimer: null,
                throttleDelay: 300                
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
            onVideoTouchStart(event) {
                var that = this;
                that.touchStart = true;                  
            },                  
            onMouseOver() {
                var that = this;
                if (!that.touchStart) {
                    that.mouseOver = true;

                    clearTimeout(that.throttleTimer);
                    that.throttleTimer = setTimeout(function () {
                        if (that.mouseOver) {
                            that.showVideo = true;
                        }
                    }, that.throttleDelay);    
                }            
            },            
            onMouseLeave() {
                this.mouseOver = false;
                this.showVideo = false;
                this.touchStart = false;
            }
        }       
    };
</script>






