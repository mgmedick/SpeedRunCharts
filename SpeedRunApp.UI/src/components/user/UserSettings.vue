<template>
    <div class="container-lg p-0">
        <h5 class="m-0 font-weight-bold m-0">@{{ usersettingsvm.username }}</h5>
        <div>
            <div v-if="loading">
                <div class="d-flex">
                    <div class="mx-auto">
                        <i class="fas fa-spinner fa-spin fa-lg"></i>
                    </div>
                </div>
            </div>    
            <div v-else>
                <form @submit.prevent="submitForm">
                    <div>
                        <ul>
                            <li class="text-danger small font-weight-semibold" v-for="errorMessage in errorMessages">{{ errorMessage }}</li>
                        </ul>
                    </div>
                    <div class="form-group row no-gutters">
                        <label class="col-sm-1 col-form-label">Night Mode</label>
                        <div class="col-sm-auto">
                            <div class="custom-control custom-switch pt-2">
                                <input id="chkNightMode1" type="checkbox" class="custom-control-input" data-toggle="toggle" v-model="form.isDarkTheme">
                                <label class="custom-control-label pl-1" for="chkNightMode1"><span class="pl-2"></span></label>
                            </div>                   
                        </div>
                    </div>
                    <div class="form-group row no-gutters mb-2">
                        <label class="col-sm-1 col-form-label">Lists</label>
                        <div class="col-sm-auto">
                            <div style="width:300px;">
                                <multiselect v-model="form.summaryListIDs" :options="speedRunSummaryLists" valueby="id" labelby="displayName">
                                    <template #tag="{ index, option, remove }">
                                        <span v-tippy="option.description">{{ option.displayName }}</span>&nbsp;
                                        <span class="fas fa-times fa-sm" @click.stop="remove(index)" style="cursor:pointer;"></span>
                                    </template>
                                </multiselect>
                            </div>
                        </div>
                    </div>
                    <div class="row no-gutters pt-1" style="width:50%;">
                        <div class="form-group mx-auto">
                            <button type="submit" class="btn btn-primary">Save</button>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>
</template>
<script>
    import axios from 'axios';
    import { getFormData } from '../../js/common.js';

    export default {
        name: "UserSettings",
        props: {
            usersettingsvm: Object
        },
        data() {
            return {
                form: { 
                    userID: this.usersettingsvm.userID,
                    username: this.usersettingsvm.username,
                    summaryListIDs: this.usersettingsvm.summaryListIDs,
                    isDarkTheme: this.usersettingsvm.isDarkTheme
                },
                speedRunSummaryLists: this.usersettingsvm.speedRunSummaryLists,
                loading: false,
                errorMessages: []
            }
        },        
        created: function () {
        },
        methods: {
            submitForm: function () {
                var that = this;
                var formData = getFormData(this.form);
                this.loading = true;

                axios.post('/User/UserSettings', formData)
                    .then((res) => {
                        if (res.data.success) {
                            that.loading = false;
                            location.reload();
                        } else {
                            that.errorMessages = res.data.errorMessages;
                        }
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            }
        }
    };
</script>


