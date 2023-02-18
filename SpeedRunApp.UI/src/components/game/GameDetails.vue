<template>
    <div>
        <div class="container ml-0 p-0 d-flex">
            <div class="col-sm-2 p-0 align-self-end img-width" style="max-width:84px;">
                <div class="img-width" style="max-width:84px;">
                    <div class="img-round">
                        <img :src="gamevm.coverImageUri" class="img-fluid" alt="Responsive image">
                    </div>
                </div>
            </div>
            <div class="col-sm px-1 align-self-end">
                <h5 class="m-0 font-weight-bold text-primary">
                    {{ gameDisplayName }}
                </h5>
            </div>
        </div>
        <div>          
            <div class="container ml-0 p-0 mt-4">
                <h5 class="font-weight-bold mb-2">Details</h5>
                <div>
                    <div class="row no-gutters">
                        <div v-if="gamevm.platformsString" class="col-8">
                            <label for="spnPlatforms" class="col-form-label pt-0 pb-1 font-weight-bold">Platforms</label>
                            <div>
                                <span id="spnPlatforms" class="form-control" style="max-width: 300px; width: auto; height: auto; font-size:14px; border:none; background:none;">{{ gamevm.platformsString }}</span>
                            </div>
                        </div>
                        <div class="col-auto ml-auto">
                            <div class="btn btn-primary" @click="showUpdateGameModal = gamevm.isChanged ? false : true" :class="{ 'disabled' : gamevm.isChanged }" v-tippy="gamevm.isChanged ? 'Game Details and Runs are currently updating' : ''">
                                <div style="display:inline-block;">{{ gamevm.isChanged ? 'Updating' : 'Update' }}</div>
                                <div v-if="gamevm.isChanged" class="icon-elipsis-container"><span class="icon-elipsis"></span></div>  
                            </div>
                        </div>                        
                    </div>
                    <div v-if="gamevm.moderators && gamevm.moderators.length > 0" class="row no-gutters pt-1">
                        <div class="col-auto">
                            <label for="spnModerators" class="col-form-label pt-0 pb-1 font-weight-bold">Moderators</label>
                            <div>
                                <span id="spnModerators" class="form-control" style="max-width: 300px; width:auto; height:auto; font-size:14px; border:none; background:none;">                             
                                    <template v-for="(moderator, index) in gamevm.moderators" :key="index">
                                        <a :href="'/User/UserDetails/' + moderator.abbr" class="text-primary" draggable="false">{{ moderator.name }}</a>{{ (gamevm.moderators.length -1 == index) ? '' : ', ' }}
                                    </template>                            
                                </span>
                            </div>
                        </div>
                    </div>
                </div>            
            </div>
            <game-tabs :id="gamevm.id.toString()" :speedrunid="speedrunid"></game-tabs>
        </div>
        <modal v-if="showUpdateGameModal" contentclass="cmv-modal-md" @close="showUpdateGameModal = false" ref="updateModal">
            <template v-slot:title>
                Update Game
            </template>
            <div class="container">
                <div>
                    <ul>
                        <li class="text-danger small font-weight-bold" v-for="errorMessage in errorMessages">{{ errorMessage }}</li>
                    </ul>
                </div>                
                <div class="form-group row no-gutters">
                    <span>Are you sure you want to update this Game and its Runs?</span>                   
                    <div class="pt-3">
                        <i class="fa fa-exclamation-triangle fa-lg pr-1" style="color:#fd7e14;"></i><span>This site is kept up to date automatically. Only update if Leaderboard ranks are out of date or a Category is missing. It can take up 10 minutes for the import to complete the request.</span> 
                    </div>                     
                </div>
                <div class="row no-gutters pt-1">
                    <div class="form-group mx-auto">
                        <button type="button" class="btn btn-primary" @click="onUpdateClick">Update</button>
                        <button type="button" class="btn btn-secondary ml-2" @click="$refs.updateModal.close()">Cancel</button>
                    </div>
                </div>
            </div>         
        </modal>           
    </div>
</template>
<script>
    import axios from 'axios';
    import { getDateTimeLocalString } from '../../js/common.js';

    export default {
        name: "GameDetails",
        props: {
            gamevm: Object,
            speedrunid: String
        },  
        data: function () {
            return {
                errorMessages: [],                
                showUpdateGameModal: false,
                showGameCharts: true               
            }
        },            
        computed: {
            gameDisplayName: function () {
                var result = this.gamevm.name;
                if (this.gamevm.yearOfRelease){
                    result += " (" + this.gamevm.yearOfRelease + ")"
                }
                return result;
            }
        },                
        created: function () {
        },
        methods: {
            onUpdateClick() {
                var that = this;

                axios.post('/Game/SetGameIsChanged', null,{ params: { gameID: that.gamevm.id } })
                    .then((res) => {
                        if (res.data.success) {
                            that.gamevm.isChanged = true;
                            that.$refs.updateModal.close();                            
                        } else {
                            that.errorMessages = res.data.errorMessages;
                        }
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            },
            onShowGameChartsClick: function (event) {
                this.showGameCharts = !this.showGameCharts;
            },
            getFormattedDateString: function (value) {
                return getDateTimeLocalString(value);
            }                  
        }
    };
</script>


