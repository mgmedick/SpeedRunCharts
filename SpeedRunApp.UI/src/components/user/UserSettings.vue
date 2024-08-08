<template>
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
                            <input id="chkNightMode1" type="checkbox" class="custom-control-input" data-toggle="toggle" v-model="item.isDarkTheme">
                            <label class="custom-control-label pl-1" for="chkNightMode1"><span class="pl-2"></span></label>
                        </div>                   
                    </div>
                </div>
                <div class="form-group row no-gutters mb-2">
                    <label class="col-sm-1 col-form-label">Feeds</label>
                    <div class="col-sm-auto">
                        <div style="width:300px;">
                            <multiselect v-model="item.speedRunListCategoryIDs" :options="item.speedRunListCategories" valueby="id" labelby="displayName">
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
</template>
<script>
    import axios from 'axios';
    import { getFormData } from '../../js/common.js';

    export default {
        name: "UserSettings",
        props: {
            uservm: Object
        },
        data() {
            return {
                item: {
                    userID: this.uservm.userID,
                    username: this.uservm.username,
                    speedRunListCategoryIDs: this.uservm.speedRunListCategoryIDs,
                    speedRunListCategories: this.uservm.speedRunListCategories,
                    isDarkTheme: this.uservm.isDarkTheme
                },
                loading: false,
                errorMessages: []
            }
        },        
        created: function () {
        },
        methods: {
            submitForm: function () {
                var that = this;
                this.item.speedRunListCategoryIDs = this.item.speedRunListCategoryIDs.map(i => i);
                var formData = getFormData(this.item);
                this.loading = true;

                axios.post('/User/SaveUser', formData)
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


