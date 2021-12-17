<template>
    <div class="container mx-auto p-0" style="max-width:598px; margin-bottom:20px;">
        <div class="speedRunSummary bg-dark">
            <template v-if="isMediaLarge">
                <div class="header p-2 container d-flex">
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
                <div class="header p-1 container d-flex">
                    <div class="p-0 col-1">
                        <div class="img-round">
                            <img :src="item.gameCoverImageLink" class="img-fluid" alt="Responsive image">
                        </div>
                    </div>
                    <div class="nowrap-elipsis pl-2 align-self-center">
                        <a :href="'../Game/GameDetails?gameID=' + item.game.id" class="text-primary">{{ item.game.name }}</a>
                    </div>
                </div>
                <div class="header p-1 container d-flex">
                    <div class="col-8 p-0 align-self-center">
                        <div class="text-secondary nowrap-elipsis">
                            {{ item.rankString }}
                        </div>
                        <div class="text-secondary nowrap-elipsis" style="font-size:13px;">
                            {{ item.category.name }}{{ (item.subCategoryVariableValuesString) ? ' - ' : '' }}{{ item.subCategoryVariableValuesString }}
                        </div>
                        <div class="text-secondary nowrap-elipsis" style="font-size: 14px;">
                            <template v-for="(player, index) in item.players">
                                <a :href="'../User/UserDetails?userID=' + player.id" class="text-secondary">{{ player.name }}</a>
                                {{ (item.players.length == index) ? ', ' : '' }}
                            </template>
                            &nbsp;-&nbsp;
                            {{ item.primaryTimeString }}
                        </div>
                    </div>
                    <div class="col-sm-2 align-self-center p-0">
                        <div style="height:67.5px; width:90px; margin-left:auto;">
                            <img :src="item.videoThumbnailLink" style="height:67.5px; width:90px;" />
                        </div>
                    </div>
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
                showModal: false
            }
        },
        computed: {
            isMediaLarge: function () {
                return false;//window.innerWidth > 992;
            },
            rankColor: function () {
                var rankIcon = '';
                switch (item.rankString) {
                    case '1st':
                        rankIcon = ''
                        break;
                }
            }
        }
    };
</script>
<style>
    .nowrap-elipsis {
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
    }
</style>




