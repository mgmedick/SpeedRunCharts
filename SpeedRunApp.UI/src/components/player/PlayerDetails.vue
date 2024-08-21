<template>
    <div>
        <div class="d-flex">
            <div class="col-auto p-0 align-self-end">
                <h5 class="m-0 font-weight-bold text-primary">{{ playervm.name }}</h5>
            </div>
            <div class="col-sm-2 pl-1 align-self-end">
                <a v-if="playervm.twitterProfile" :href="playervm.twitterProfile" class="pl-1" draggable="false"><i class="fab fa-twitter fa-lg" style="color: #1D9BF0;"></i></a>
                <a v-if="playervm.twitchProfile" :href="playervm.twitchProfile" class="pl-1" draggable="false"><i class="fab fa-twitch fa-lg" style="color: #6441a5;"></i></a>
                <a v-if="playervm.youtubeProfile" :href="playervm.youtubeProfile" class="pl-1" draggable="false"><i class="fab fa-youtube fa-lg" style="color: #FF0000;"></i></a>
            </div>
        </div>
        <div>
            <div class="mt-4">
                <h5 class="font-weight-bold mb-1">Details</h5>
                <div class="row no-gutters">
                    <div class="col">
                        <table>
                            <thead>
                                <tr>
                                    <th class="font-weight-normal" style="font-size:14px; width:100px;">Total Runs</th>
                                    <th class="font-weight-normal" style="font-size:14px; width:100px;">WRs</th>
                                    <th class="font-weight-normal" style="font-size:14px; width:100px;">PBs</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>
                                        <span id="spnTotalSpeedRuns" class="form-control p-0" style="border:none; background:none;">{{ playervm.totalSpeedRuns }}</span>
                                    </td>
                                    <td>
                                        <span id="spnTotalWorldRecords" class="form-control p-0" style="border:none; background:none;">{{ playervm.totalWorldRecords }}</span>
                                    </td>
                                    <td>
                                        <span id="spnTotalPersonalBests" class="form-control p-0" style="border:none; background:none;">{{ playervm.totalPersonalBests }}</span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>                   
                </div>
            </div>
            <player-details-tabs :id="playervm.id.toString()" :speedruncode="playervm.speedRunCode"></player-details-tabs>
        </div>        
    </div>
</template>
<script>
    export default {
        name: "PlayerDetails",
        props: {
            playervm: Object
        },
        data: function () {
            return {
                errorMessages: []
            }
        },                            
        created: function () {
        },
        methods: {          
        }
    };
</script>


