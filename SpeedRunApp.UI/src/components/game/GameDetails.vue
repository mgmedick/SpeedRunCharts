<template>
    <div>
        <div class="d-flex">
            <div class="col-sm-2 p-0 align-self-end img-width" style="max-width:84px;">
                <div class="img-width" style="max-width:84px;">
                    <div class="img-round">
                        <img :src="gamevm.coverImageUri" class="img-fluid" alt="Responsive image">
                    </div>
                </div>
            </div>
            <div class="col-sm px-1 align-self-end">
                <h5 class="m-0 font-weight-bold text-primary">
                    {{ gamevm.displayName }}
                </h5>
            </div>
        </div>
        <div>          
            <game-detail-tabs :id="gamevm.id.toString()" :speedruncode="gamevm.speedRunCode"></game-detail-tabs>
        </div>          
    </div>
</template>
<script>
    export default {
        name: "GameDetails",
        props: {
            gamevm: Object
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


