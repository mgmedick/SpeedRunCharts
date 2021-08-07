<template>
    <div class="container mx-auto p-0" style="max-width:598px; margin-bottom:20px;">
        <div class="speedRunSummary bg-dark">
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
                    <div class="text-secondary" style="font-size:14px;">
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
                        scrolling="no"
                        allowfullscreen="true"></iframe>
            </div>
            <input type="hidden" class="orderValue" :value="item.id" />
            <custom-modal v-model="showModal" v-if="showModal" contentclass="modal-lg">
                <template v-slot:title>
                    Details
                </template>
                <speedrun-edit :gameid="item.game.id" :speedrunid="item.id" :readonly="true" />
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
        }
    };
</script>




