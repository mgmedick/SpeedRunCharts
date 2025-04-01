<template>
    <div class="mx-auto">
        <h2 class="text-center mb-1">User Settings</h2>
        <div class="mx-auto" style="max-width:400px;">  
            <h5 class="m-0 fw-bold text-center">{{ usersettingsvm.username }}</h5>
            <div>
                <ul>
                    <li class="text-danger small fw-bold" v-for="errorMessage in errorMessages">{{ errorMessage }}</li>
                </ul>
            </div>
            <div class="row g-3 align-items-center mb-2"> 
                <div class="col-auto align-self-end">
                    <label class="form-label">Night Mode</label>
                </div>       
                <div class="col-auto">
                    <div class="form-check form-switch">
                        <input id="chkNightMode" class="form-check-input" type="checkbox" v-model="isDarkTheme" @change="onUpdateIsDarkTheme">
                        <label class="form-check-label" for="chkNightMode"><span class="ps-2"></span></label>
                    </div>    
                </div>                                        
            </div>
            <div>
                <label class="form-label">Lists</label>
                <multiselect v-model="summaryListIDs" :options="summaryLists" valueby="id" labelby="displayName" style="width: 300px"/>                 
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
                searchText: null,
                searchResults: [],
                searchLoading: false,
                summaryListIDs: this.usersettingsvm.summaryListIDs ?? [],
                isDarkTheme: this.usersettingsvm.isDarkTheme,
                summaryLists: this.usersettingsvm.summaryLists,
                throttleTimer: null,
                throttleDelay: 300,
                errorMessages: []
            }
        },
        watch: {
            summaryListIDs: {
                handler (val, oldVal) {
                    this.updateUserSummaryListsDelay();
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
            window.addEventListener('themeUpdate', this.onThemeUpdate);
        },
        destroyed() {
            window.removeEventListener('themeUpdate', this.onThemeUpdate);
        },           
        methods: {  
            onUpdateIsDarkTheme(e) {
                var that = this;
                that.updateTheme(this.isDarkTheme);

                axios.post('/Home/UpdateIsDarkTheme', null,{ params: { isDarkTheme: this.isDarkTheme } })
                        .then((res) => {
                            if (!res.data.success) {
                                res.data.errorMessages.forEach(errorMsg => {
                                    errorToast(errorMsg);                           
                                });  
                            }                                                                            
                        })
                        .catch(err => { console.error(err); return Promise.reject(err); }); 
            },                            
            updateTheme: function(val){
                var el = document.documentElement;

                if (val){
                    el.dataset.bsTheme = "dark";
                } else {
                    el.dataset.bsTheme = "light";
                }
            },
            updateUserSummaryListsDelay() {
                var that = this;

                clearTimeout(that.throttleTimer);
                    that.throttleTimer = setTimeout(function () {
                            that.updateUserSummaryLists();
                }, that.throttleDelay);     
            },
            updateUserSummaryLists() {
                var that = this;
                var formData = getFormData({ summaryListIDs: that.summaryListIDs });
                var config = { headers: { 'RequestVerificationToken': that.getCsrfToken() } };

                return axios.post('/User/SaveUserSummaryLists', formData, config)
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
            onThemeUpdate() {
                this.isDarkTheme = document.documentElement.dataset.bsTheme == 'dark';
            }  
        }
    };
</script>


