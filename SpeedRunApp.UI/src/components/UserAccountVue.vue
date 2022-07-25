<template>
    <div id="divUserAccount" class="container p-0">
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
                        <li class="text-danger small font-weight-bold" v-for="errorMessage in errorMessages">{{ errorMessage }}</li>
                    </ul>
                </div>
                <div class="form-group row no-gutters">
                    <label class="col-sm-1 col-form-label">Night Mode</label>
                    <div class="col-sm-auto">
                        <div class="custom-control custom-switch pt-2">
                            <input id="chkNightMode1" type="checkbox" class="custom-control-input" data-toggle="toggle" v-model="item.isDarkTheme">
                            <label class="custom-control-label pl-1" for="chkNightMode1"><span class="pl-2"></span></label>
                        </div>                   
                    </div>
                </div>
                <div class="form-group row no-gutters mb-2">
                    <label class="col-sm-1 col-form-label">Feeds</label>
                    <div class="col-sm-auto">
                        <div style="width:300px;">
                            <vue-next-select v-model="item.speedRunListCategoryIDs" :options="item.speedRunListCategories" label-by="displayName" value-by="id" multiple taggable>
                                <template #tag="{ option, remove }">
                                    <span v-tippy="option.description">{{ option.displayName }}</span>
                                    <img @click.stop="remove" src="data:image/svg+xml;base64,PHN2ZyBpZD0iZGVsZXRlIiBkYXRhLW5hbWU9ImRlbGV0ZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB2aWV3Qm94PSIwIDAgNTEyIDUxMiI+PHRpdGxlPmRlbGV0ZTwvdGl0bGU+PHBhdGggZD0iTTI1NiwyNEMzODMuOSwyNCw0ODgsMTI4LjEsNDg4LDI1NlMzODMuOSw0ODgsMjU2LDQ4OCwyNC4wNiwzODMuOSwyNC4wNiwyNTYsMTI4LjEsMjQsMjU2LDI0Wk0wLDI1NkMwLDM5Ny4xNiwxMTQuODQsNTEyLDI1Niw1MTJTNTEyLDM5Ny4xNiw1MTIsMjU2LDM5Ny4xNiwwLDI1NiwwLDAsMTE0Ljg0LDAsMjU2WiIgZmlsbD0iIzViNWI1ZiIvPjxwb2x5Z29uIHBvaW50cz0iMzgyIDE3Mi43MiAzMzkuMjkgMTMwLjAxIDI1NiAyMTMuMjkgMTcyLjcyIDEzMC4wMSAxMzAuMDEgMTcyLjcyIDIxMy4yOSAyNTYgMTMwLjAxIDMzOS4yOCAxNzIuNzIgMzgyIDI1NiAyOTguNzEgMzM5LjI5IDM4MS45OSAzODIgMzM5LjI4IDI5OC43MSAyNTYgMzgyIDE3Mi43MiIgZmlsbD0iIzViNWI1ZiIvPjwvc3ZnPg==" alt="delete tag" class="icon delete">
                                </template>
                            </vue-next-select>
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
</template>
<script>
    import axios from 'axios';
    import { getFormData } from '../js/common.js';

    export default {
        name: "UserAccountVue",
        props: {
            useraccountid: String
        },
        data() {
            return {
                item: {
                    userAccountID: 0,
                    username: '',
                    speedRunListCategoryIDs: [],
                    speedRunListCategories: [],
                    isDarkTheme: false
                },
                loading: false,
                errorMessages: []
            }
        },        
        created: function () {
            this.loadData();
        },
        methods: {
            loadData: function () {
                var that = this;
                this.loading = true;

                var prms = axios.get('/UserAccount/GetUserAccount', { params: { userAccountID: this.useraccountid } })
                    .then(res => {
                        that.item = res.data;
                        that.loading = false;

                        return res;
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });

                return prms;
            },
            submitForm: function () {
                var that = this;
                this.item.speedRunListCategoryIDs = this.item.speedRunListCategoryIDs.map(i => i);
                var formData = getFormData(this.item);
                this.loading = true;

                axios.post('/UserAccount/SaveUserAccount', formData)
                    .then((res) => {
                        if (res.data.success) {
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


