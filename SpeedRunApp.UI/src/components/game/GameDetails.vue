<template>
    <div class="container">
        <div v-if="gamedetailsvm.id == 0" class="mx-auto p-0" style="max-width:500px;">
            <div style="text-align:center;">
                <i class="fa fa-search fa-lg"></i>
                <div><span>Game not found, please search for the game by name and try again</span></div>
            </div>
        </div>
        <div v-else>
            <div class="d-flex">
                <div class="col-sm-2 p-0 align-self-end img-width" style="max-width:84px;">
                    <div class="img-width" style="max-width:84px;">
                        <div class="img-round">
                            <img :src="gamedetailsvm.coverImageUri" class="img-fluid" alt="Responsive image">
                        </div>
                    </div>
                </div>
                <div class="col-sm px-1 align-self-end">
                    <h5 class="mb-0 fw-bold">
                        {{ gamedetailsvm.displayName }}
                    </h5>
                </div>
            </div>
            <div>          
                <game-details-tabs :id="gamedetailsvm.id.toString()" :speedruncode="gamedetailsvm.speedRunCode"></game-details-tabs>
            </div>          
        </div>
    </div>
</template>
<script>
    export default {
        name: "GameDetails",
        props: {
            gamedetailsvm: Object
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


