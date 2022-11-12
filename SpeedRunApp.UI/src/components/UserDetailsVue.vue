<template>
    <div>
        <div class="container d-flex p-0">
            <div class="col-auto p-0 align-self-end">
                <h5 class="m-0 font-weight-bold text-primary">{{ uservm.name }}</h5>
            </div>
            <div class="col-sm-2 pl-1 align-self-end">
                <a v-if="uservm.twitterProfile" :href="uservm.twitterProfile" class="pl-1"><i class="fab fa-twitter fa-lg" style="color: #1D9BF0;"></i></a>
                <a v-if="uservm.twitchProfile" :href="uservm.twitchProfile" class="pl-1"><i class="fab fa-twitch fa-lg" style="color: #6441a5;"></i></a>
                <a v-if="uservm.youtubeProfile" :href="uservm.youtubeProfile" class="pl-1"><i class="fab fa-youtube fa-lg" style="color: #FF0000;"></i></a>
            </div>
        </div>
        <div>
            <div class="container ml-0 p-0 mt-4">
                <h5 class="font-weight-bold mb-1">Details</h5>
                <div class="row no-gutters">
                    <div class="col-sm-auto pl-0">
                        <table class="table-responsive">
                            <thead>
                                <tr>
                                    <th class="font-weight-normal pr-2" style="font-size:14px;">Total Runs</th>
                                    <th class="font-weight-normal px-2" style="font-size:14px;">World Records</th>
                                    <th class="font-weight-normal px-2" style="font-size:14px;">Personal Bests</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td class="pr-2">
                                        <span id="spnTotalSpeedRuns" class="form-control" style="width:100px; border:none; background:none;">{{ uservm.totalSpeedRuns }}</span>
                                    </td>
                                    <td class="px-2">
                                        <span id="spnTotalWorldRecords" class="form-control" style="width:100px;  border:none; background:none;">{{ uservm.totalWorldRecords }}</span>
                                    </td>
                                    <td class="px-2">
                                        <span id="spnTotalPersonalBests" class="form-control" style="width:100px; border:none; background:none;">{{ uservm.totalPersonalBests }}</span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <grid-tab-container :isgame="false" :id="uservm.id.toString()" :speedrunid="speedrunid"></grid-tab-container>
        </div>
    </div>
</template>
<script>
    export default {
        name: "UserDetailstVue",
        props: {
            uservm: Object,
            speedrunid: String            
        },           
        created: function () {
        },
        methods: {
        }
    };
</script>


