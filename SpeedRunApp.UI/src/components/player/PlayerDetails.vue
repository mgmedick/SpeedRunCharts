<template>
    <div class="container-lg p-0">
        <div v-if="playerdetailsvm.id == 0" class="mx-auto" style="max-width:500px;">
            <div style="text-align:center;">
                <i class="fa fa-search fa-lg"></i>
                <div><span>Player not found, please search for the user by name and try again</span></div>
            </div>
        </div>
        <div v-else>
            <div class="d-flex">
                <div class="col-auto p-0 align-self-end">
                    <h5 class="m-0 fw-bold text-primary">{{ playerdetailsvm.name }}</h5>
                </div>
                <div class="col-sm-2 ps-1 align-self-end">
                    <a v-if="playerdetailsvm.twitterProfile" :href="playerdetailsvm.twitterProfile" class="ps-1" draggable="false"><i class="fab fa-twitter fa-lg" style="color: #1D9BF0;"></i></a>
                    <a v-if="playerdetailsvm.twitchProfile" :href="playerdetailsvm.twitchProfile" class="ps-1" draggable="false"><i class="fab fa-twitch fa-lg" style="color: #6441a5;"></i></a>
                    <a v-if="playerdetailsvm.youtubeProfile" :href="playerdetailsvm.youtubeProfile" class="ps-1" draggable="false"><i class="fab fa-youtube fa-lg" style="color: #FF0000;"></i></a>
                </div>
            </div>
            <div>
                <div class="mt-4">
                    <h5 class="fw-bold mb-1">Details</h5>
                    <div class="row no-gutters">
                        <div class="col">
                            <table>
                                <thead>
                                    <tr>
                                        <th class="fw-normal" style="font-size:14px; width:100px;">Total Runs</th>
                                        <th class="fw-normal" style="font-size:14px; width:100px;">WRs</th>
                                        <th class="fw-normal" style="font-size:14px; width:100px;">PBs</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>
                                            <span id="spnTotalSpeedRuns" class="form-control p-0" style="border:none; background:none;">{{ playerdetailsvm.totalSpeedRuns }}</span>
                                        </td>
                                        <td>
                                            <span id="spnTotalWorldRecords" class="form-control p-0" style="border:none; background:none;">{{ playerdetailsvm.totalWorldRecords }}</span>
                                        </td>
                                        <td>
                                            <span id="spnTotalPersonalBests" class="form-control p-0" style="border:none; background:none;">{{ playerdetailsvm.totalPersonalBests }}</span>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>                   
                    </div>
                </div>
                <player-details-tabs :id="playerdetailsvm.id.toString()" :speedruncode="playerdetailsvm.speedRunCode"></player-details-tabs>
            </div>  
        </div>      
    </div>
</template>
<script>
    export default {
        name: "PlayerDetails",
        props: {
            playerdetailsvm: Object
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


