<template>
    <div class="mx-auto">
        <h2 class="text-center mb-1">User Settings</h2>
        <div class="mx-auto" style="max-width:400px;">  
            <h5 class="m-0 fw-bold m-0 text-center">{{ usersettingsvm.username }}</h5>
            <div>
                <div>
                    <ul>
                        <li class="text-danger small fw-bold" v-for="errorMessage in errorMessages">{{ errorMessage }}</li>
                    </ul>
                </div>
                <div class="form-group row no-gutters">
                    <label class="col-3 col-form-label">Night Mode</label>
                    <div class="col-auto">
                        <div class="form-check form-switch pt-2">
                            <input id="chkNightMode" class="form-check-input" type="checkbox" v-model="isDarkTheme" @change="onUpdateIsDarkTheme">
                            <label class="form-check-label" for="chkNightMode"><span class="ps-2"></span></label>
                        </div>                                     
                    </div>
                </div>
                <div class="form-group row no-gutters mb-2">
                    <label class="col-2 col-form-label">Lists</label>
                    <div class="col-auto">
                        <div style="width:300px;">
                            <multiselect v-model="summaryListIDs" :options="summaryLists" valueby="id" labelby="displayName">
                                <template #tag="{ index, option, remove }">
                                    <span data-bs-toggle="tooltip" :data-bs-title="option.description">{{ option.displayName }}</span>&nbsp;
                                    <span class="fas fa-times fa-sm" @click.stop="remove(index)" style="cursor:pointer;"></span>
                                </template>
                            </multiselect>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import axios from 'axios';
    import { getFormData, successToast, errorToast } from '../../js/common.js';
    import { Tooltip } from 'bootstrap';

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
                errorMessages: []
            }
        },
        watch: {
            summaryListIDs: {
                handler (val, oldVal) {
                    var that = this;
                    var formData = getFormData({ summaryListIDs: val });
                    var config = { headers: { 'RequestVerificationToken': that.getCsrfToken() } };

                    axios.post('/User/SaveUserSummaryLists', formData, config)
                    .then((res) => {
                        if (res.data.success) {
                            successToast("Updated user lists");                           
                        } else {
                            res.data.errorMessages.forEach(errorMsg => {
                                errorToast(errorMsg);                           
                            });                                
                        }
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
                },
                deep: true
            }
        },           
        created: function () {
        },
        mounted: function () {
            this.$el.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(el => {
                new Tooltip(el);                        
            });
        },
        methods: {
            onUpdateIsDarkTheme(e) {
                var that = this;

                axios.post('/Home/UpdateIsDarkTheme', null,{ params: { isDarkTheme: this.isDarkTheme } })
                        .then((res) => {
                            if (res.data.success) {
                                that.updateTheme(this.isDarkTheme);
                            } else {
                                res.data.errorMessages.forEach(errorMsg => {
                                    errorToast(errorMsg);                           
                                });                                
                            }                                                                                
                        })
                        .catch(err => { console.error(err); return Promise.reject(err); }); 
            },                            
            updateTheme: function(val){
                var el = document.body;

                if (val){
                    el.dataset.bsTheme = "dark";
                } else {
                    el.dataset.bsTheme = "light";
                }
            }
        }
    };
</script>


