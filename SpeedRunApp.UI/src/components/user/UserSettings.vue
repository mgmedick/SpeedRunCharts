<template>
    <div class="mx-auto">
        <h2 class="text-center mb-1">User Settings</h2>
        <div class="mx-auto" style="max-width:400px;">
            <div v-if="loading">
                <div class="d-flex">
                    <div class="mx-auto">
                        <i class="fas fa-spinner fa-spin fa-lg"></i>
                    </div>
                </div>
            </div>    
            <div v-else>
                <h5 class="m-0 font-weight-bold m-0 text-center">{{ usersettingsvm.username }}</h5>
                <div>
                    <div>
                        <ul>
                            <li class="text-danger small font-weight-semibold" v-for="errorMessage in errorMessages">{{ errorMessage }}</li>
                        </ul>
                    </div>
                    <div class="form-group row no-gutters">
                        <label class="col-3 col-form-label">Night Mode</label>
                        <div class="col-auto">
                            <div class="custom-control custom-switch pt-2">
                                <input id="chkNightMode1" type="checkbox" class="custom-control-input" data-toggle="toggle" v-model="isDarkTheme" @change="onUpdateIsDarkTheme">
                                <label class="custom-control-label pl-1" for="chkNightMode1"><span class="pl-2"></span></label>
                            </div>                   
                        </div>
                    </div>
                    <div class="form-group row no-gutters mb-2">
                        <label class="col-2 col-form-label">Lists</label>
                        <div class="col-auto">
                            <div style="width:300px;">
                                <multiselect v-model="summaryListIDs" :options="summaryLists" valueby="id" labelby="displayName">
                                    <template #tag="{ index, option, remove }">
                                        <span v-tippy="option.description">{{ option.displayName }}</span>&nbsp;
                                        <span class="fas fa-times fa-sm" @click.stop="remove(index)" style="cursor:pointer;"></span>
                                    </template>
                                </multiselect>
                            </div>
                        </div>
                    </div>
                </div>
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
                summaryListIDs: this.usersettingsvm.summaryListIDs,
                isDarkTheme: this.usersettingsvm.isDarkTheme,
                summaryLists: this.usersettingsvm.summaryLists,
                loading: false,
                errorMessages: []
            }
        },
        watch: {
            summaryListIDs: {
                handler (val, oldVal) {
                    var that = this;
                    var formData = getFormData({ summaryListIDs: val });
                    var config = { headers: { 'RequestVerificationToken': that.getCsrfToken() } };
                    this.loading = true;

                    axios.post('/User/SaveUserSummaryLists', formData, config)
                        .then((res) => {
                            if (res.data.success) {
                                that.loading = false;
                            } else {
                                that.errorMessages = res.data.errorMessages;
                            }
                        })
                        .catch(err => { console.error(err); return Promise.reject(err); });
                },
                deep: true
            }
        },           
        created: function () {
        },
        methods: {
            onUpdateIsDarkTheme(e) {
                var that = this;

                axios.post('/User/UpdateIsDarkTheme', null,{ params: { isDarkTheme: this.isDarkTheme } })
                        .then((res) => {
                            if (res.data.success) {
                                that.updateTheme(this.isDarkTheme);
                            }                                                                                   
                        })
                        .catch(err => { console.error(err); return Promise.reject(err); }); 
            },                            
            updateTheme: function(val){
                var el = document.body;

                if (val){
                    el.classList.remove("theme-light");
                    el.classList.add("theme-dark");
                } else {
                    el.classList.remove("theme-dark");
                    el.classList.add("theme-light");
                }
            }
        }
    };
</script>


