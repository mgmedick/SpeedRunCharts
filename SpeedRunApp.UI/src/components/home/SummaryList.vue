<template>
    <div>
        <div class="row row-cols-lg-5 row-cols-sm-2 row-cols-1 g-3 gy-4">
            <!-- <div class="col-xl-auto col-md-3 col-sm-4 col-12 default-img-container">
                <div class="position-relative" style="overflow: hidden;">              
                    <svg :width="imgWidth" :height="imgHeight" class="img-fluid default-img">
                        <rect :width="imgWidth" :height="imgHeight" style="fill: #f8f9fb;" />
                    </svg>                        
                </div>                                                   
            </div>            -->
            <div v-for="(item, index) in items" class="col" :key="item.id">
                <speedrun-summary :item="item" :index="index"></speedrun-summary>
                <!-- <div class="bg-light thumbnail-img">
                    <div class="ratio ratio-16x9" style="overflow: hidden;">
                        <img v-if="!showVideo" @click="showVideo = !showVideo;" :src="item.videoThumbnailLink" class="img-fluid align-self-center rounded" style="height:100%; width:100%; overflow:hidden;"/>
                        <iframe v-else :src="item.embeddedVideoLinkAutoplay"
                            frameborder="0"
                            scrolling="no"
                            width="100%"
                            height="100%"                  
                            allowfullscreen="true"></iframe>     
                    </div>
                    <div class="p-2">       
                        <div class="row g-2">
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
                        <div class="row g-2">
                            <div class="col-auto">  
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
                        </div>                       
                        <div class="row g-1">                    
                            <div v-if="item.categoryTypeName" class="col-auto">
                                <span class="badge text-bg-primary">{{ item.categoryTypeName }}</span>
                            </div>
                            <div v-if="item.categoryName" class="col-auto">
                                <span v-if="item.categoryName" class="badge text-bg-primary">{{ item.categoryName }}</span>
                            </div>
                            <div v-if="item.levelName" class="col-auto">
                                <span v-if="item.levelName" class="badge text-bg-primary">{{ item.levelName }}</span>
                            </div>
                            <div v-for="(subCategoryVariableValue, index) in item.subCategoryVariableValueNames" class="col-auto">
                                <span class="badge text-bg-primary">{{ subCategoryVariableValue }}</span>
                            </div>                
                        </div>
                    </div>               
                </div>          
                <input type="hidden" class="orderValue" :value="item.sortOrder" /> -->
            </div>
        </div>
        <div v-if="loading">
            <div class="d-flex">
                <div class="mx-auto">
                    <i class="fas fa-spinner fa-spin fa-lg"></i>
                </div>
            </div>
        </div>           
    </div> 
</template>
<script>
    import axios from 'axios'

    export default {
        name: 'SummaryList',
        props: {
            summarylistid: Number,
            defaulttopamt: Number,            
            categorytypeid: Number            
        },
        data() {
            return {
                items: [],
                loading: true,
                throttleTimer: null,
                throttleDelay: 500,
                // showVideo: false,
                width: document.documentElement.clientWidth,
                height: document.documentElement.clientHeight,
                imgWidth: 365,
                imgHeight: 205,
                topamt: sessionStorage.getItem("topamt") ?? this.defaulttopamt,
                offset: sessionStorage.getItem("offset") ?? null
            }
        },
        watch: {                   
            summarylistid: function (val, oldVal) {
                this.resetParams();        
                this.loadData();
            },
            categorytypeid: function (val, oldVal) {
                this.resetParams();        
                this.loadData();
            }                 
        },        
        created() {
            var isPageReloaded = ((window.performance.navigation && window.performance.navigation.type === 1) ||
                        window.performance.getEntriesByType('navigation').map((nav) => nav.type).includes('reload'));

            if (isPageReloaded) {
                this.resetParams();                   
            }
            
            this.loadData().then(function() {                               
                if (sessionStorage.scrolltop) {
                    document.documentElement.scrollTop = sessionStorage.getItem("scrolltop");
                }
            });
            window.addEventListener('scroll', this.onWindowScroll);
            window.addEventListener('beforeunload', this.onBeforeUnload);          
        },        
        mounted() {
            // window.addEventListener('resize', this.onResize);
        },  
        destroyed() {
            // window.removeEventListener('resize', this.onResize);    
        },            
        methods: {
            reLoadData: function () {
                var that = this;
                var orderValues = Array.from(document.querySelectorAll('.orderValue')).map(i => i.value);
                var offset = orderValues.length > 0 ? Math.min.apply(null, orderValues) : null;

                this.offset = offset;
                return this.loadData();
            },
            loadData: function () {
                var that = this;
                this.loading = true;

                var prms = axios.get('/Home/GetSummaryListResults', { params: { summaryListID: this.summarylistid, topAmount: this.topamt, orderValueOffset: this.offset, categoryTypeID: this.categorytypeid } })
                    .then(res => {
                        that.items = that.items.concat(res.data);    
                        that.loading = false;

                        that.$nextTick(function() {
                            that.resizeColumns();                        
                        });

                        return res;
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });

                return prms;
            },
            resetParams: function() {
                this.items = [];
                this.offset = null;
                this.topamt = this.defaulttopamt;
                sessionStorage.removeItem("topamt");
                sessionStorage.removeItem("offset");
                sessionStorage.removeItem("scrolltop");
            },
            onWindowScroll: function () {
                var that = this;
                var scollTop = document.documentElement.scrollTop + window.innerHeight;
                var offsetHeight = document.documentElement.offsetHeight;
                if (Math.ceil(scollTop) >= offsetHeight && !that.loading) {
                    that.reLoadData();
                }                
            },
            onBeforeUnload: function() {
                sessionStorage.setItem("scrolltop", document.documentElement.scrollTop);

                var orderValue = Array.from(document.querySelectorAll('.orderValue')).map(i => i.value)[0];
                sessionStorage.setItem("offset", parseInt(orderValue) + 1);

                if (this.items.length > this.topamt) {
                    sessionStorage.setItem("topamt", this.items.length);
                }
            },
            onResize: function() {
                var that = this;
                if (that.width != document.documentElement.clientWidth || that.height != document.documentElement.clientHeight) {     
                    that.width = document.documentElement.clientWidth;
                    that.height = document.documentElement.clientHeight;   

                    that.$nextTick(function() {
                        that.resizeColumns(); 
                    });                   
                }
            },  
            resizeColumns() {
                // var that = this;
                // document.querySelector('.default-img-container').classList.remove('d-none');

                // var rect = document.querySelector('.default-img-container .default-img').getBoundingClientRect();
                // var defaultwidth = rect.width;
                // if (defaultwidth > 0) {
                //     document.querySelectorAll('.img-container').forEach(img => {
                //         img.style.width = defaultwidth + 'px';
                //     });
                // }

                // if (defaultwidth > 0) {
                //     document.querySelector('.default-img-container').classList.add('d-none');
                // }
            },
            // getIconClass: function (rank) {
            //     var iconClass = '';

            //     switch (rank) {
            //         case 1:
            //             iconClass = 'gold';
            //             break;
            //         case 2:
            //             iconClass = 'silver';
            //             break;
            //         case 3:
            //             iconClass = 'bronze';
            //             break;
            //     }

            //     return iconClass;
            // }                        
        }
    };
</script>










