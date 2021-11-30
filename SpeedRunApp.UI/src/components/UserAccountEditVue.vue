<template>
    <div>
        <div v-if="loading">
            <div class="d-flex">
                <div class="mx-auto">
                    <i class="fas fa-spinner fa-spin fa-lg"></i>
                </div>
            </div>
        </div>
        <div v-else id="divSpeedRunEdit" class="container p-0">
            <form @submit.prevent="submitForm">
                <div class="form-group row no-gutters mb-2">
                    <label class="col-sm-2 col-form-label">Feeds</label>
                    <div class="col-sm-10">
                        <div style="width:300px;">
                            <vue-next-select name="FeedIDs" v-model="feedIDs" :options="item.feeds" label-by="name" value-by="id" :disabled="readonly" multiple taggable></vue-next-select>
                        </div>
                    </div>
                </div>
                <input type="hidden" name="UserID" class="form-control" autocomplete="off" v-model="item.userID" />
                <div class="row no-gutters pt-1" v-if="!readonly">
                    <div class="form-group">
                        <input id="btnSave" type="button" class="btn btn-primary" value="Save" @click="save" />
                    </div>
                </div>
            </form>
        </div>
    </div>   
</template>
<script>
    import { getFormData } from '../js/common.js';
    import axios from 'axios';
    //import useVuelidate from '@vuelidate/core';

    export default {
        name: 'UserAccountEditVue',
        //setup() {
        //    return { v$: useVuelidate() }
        //},
        props: {
            userid: String
        },
        data: function () {
            return {
                item: {},
                loading: false,
                feedIDs: []
            }
        },
        computed: {
            playerids: function () {
                return this.item.speedRunVM.players.map(i => i.id);
            }
        },
        created: function () {
            this.loadData().then(i => { this.init(); });
        },
        methods: {
            loadData: function () {
                var that = this;
                this.loading = true;

                var prms = axios.get('../UserAccount/UserAccountSettings', { params: { userID: this.userid } })
                    .then(res => {
                        that.item = res.data;
                        that.feedids = res.data.feedIDs;
                        that.loading = false;

                        return res;
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });

                return prms;
            },
            async submitForm() {
                //const isValid = await this.v$.$validate()
                //if (!isValid) return

                var that = this;
                var formData = getFormData(this.form);

                axios.post('/UserAccount/UserAccountSettings', formData)
                    .then((res) => {
                        if (res.data.success) {
                            this.showSuccess = true;
                        } else {
                            that.errorMessages = res.data.errorMessages;
                        }
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            }
        }
    };
</script>


