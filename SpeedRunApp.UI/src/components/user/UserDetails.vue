<template>
    <div>
        <div class="container d-flex p-0">
            <div class="col-auto p-0 align-self-end">
                <h5 class="m-0 font-weight-bold text-primary">{{ uservm.name }}</h5>
            </div>
            <div class="col-sm-2 pl-1 align-self-end">
                <a v-if="uservm.twitterProfile" :href="uservm.twitterProfile" class="pl-1" draggable="false"><i class="fab fa-twitter fa-lg" style="color: #1D9BF0;"></i></a>
                <a v-if="uservm.twitchProfile" :href="uservm.twitchProfile" class="pl-1" draggable="false"><i class="fab fa-twitch fa-lg" style="color: #6441a5;"></i></a>
                <a v-if="uservm.youtubeProfile" :href="uservm.youtubeProfile" class="pl-1" draggable="false"><i class="fab fa-youtube fa-lg" style="color: #FF0000;"></i></a>
            </div>
        </div>
        <div>
            <div class="container ml-0 p-0 mt-4">
                <h5 class="font-weight-bold mb-1">Details</h5>
                <div class="row no-gutters">
                    <div class="col-9">
                        <table>
                            <thead>
                                <tr>
                                    <th class="font-weight-normal" style="font-size:14px;">Total Runs</th>
                                    <th class="font-weight-normal" style="font-size:14px;">WRs</th>
                                    <th class="font-weight-normal" style="font-size:14px;">PBs</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>
                                        <span id="spnTotalSpeedRuns" class="form-control" style="width:100px; border:none; background:none;">{{ uservm.totalSpeedRuns }}</span>
                                    </td>
                                    <td>
                                        <span id="spnTotalWorldRecords" class="form-control" style="width:100px;  border:none; background:none;">{{ uservm.totalWorldRecords }}</span>
                                    </td>
                                    <td>
                                        <span id="spnTotalPersonalBests" class="form-control" style="width:100px; border:none; background:none;">{{ uservm.totalPersonalBests }}</span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="col-auto ml-auto">
                        <div class="btn btn-primary" @click="showUpdateUserModal = uservm.isChanged ? false : true" :class="{ 'disabled' : uservm.isChanged }" v-tippy="uservm.isChanged ? 'User Details and Runs are currently updating' : ''">
                            <div style="display:inline-block;">{{ uservm.isChanged ? 'Updating' : 'Update' }}</div>
                            <div v-if="uservm.isChanged" class="icon-elipsis-container"><span class="icon-elipsis"></span></div>  
                        </div>
                    </div>                     
                </div>
            </div>
            <userdetails-tab :id="uservm.id.toString()" :speedrunid="speedrunid"></userdetails-tab>
        </div>
        <modal v-if="showUpdateUserModal" contentclass="cmv-modal-md" @close="showUpdateUserModal = false" ref="updateModal">
            <template v-slot:title>
                Update User
            </template>
            <div class="container">
                <div>
                    <ul>
                        <li class="text-danger small font-weight-bold" v-for="errorMessage in errorMessages">{{ errorMessage }}</li>
                    </ul>
                </div>                
                <div class="form-group row no-gutters">
                    <span>Are you sure you want to update this User and their Runs?</span>
                    <div class="pt-3">
                        <i class="fa fa-exclamation-triangle fa-lg pr-1" style="color:#fd7e14;"></i><span>Please only update if the User Details (Name, Links, etc.) or Runs are out of date. It can take up 10 minutes for the import to complete the request.</span> 
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

    export default {
        name: "UserDetails",
        props: {
            uservm: Object,
            speedrunid: String            
        },
        data: function () {
            return {
                errorMessages: [],                
                showUpdateUserModal: false
            }
        },                            
        created: function () {
        },
        methods: {
            onUpdateClick() {
                var that = this;

                axios.post('/User/SetUserIsChanged', null,{ params: { userID: that.uservm.id } })
                    .then((res) => {
                        if (res.data.success) {
                            that.uservm.isChanged = true;
                            that.$refs.updateModal.close();                            
                        } else {
                            that.errorMessages = res.data.errorMessages;
                        }
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            }            
        }
    };
</script>


