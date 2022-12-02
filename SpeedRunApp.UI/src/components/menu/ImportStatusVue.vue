<template>
    <div>
        <div v-if="loading">
            <div class="d-flex">
                <div class="mx-auto">
                    <i class="fas fa-spinner fa-spin fa-lg"></i>
                </div>
            </div>
        </div>
        <div v-else id="divImportStatus" class="container p-0">
            <div class="form-group row no-gutters mb-2">
                <label class="col-sm-4 col-form-label">Last Import</label>
                <div class="col-sm-8">
                    <input type="datetime-local" disabled class="form-control" style="width:240px;" :value="getFormattedDateString(item.importLastRunDate)"/>
                </div>
            </div>
            <div class="form-group row no-gutters mb-2">
                <label class="col-sm-4 col-form-label">Last Nightly Update</label>
                <div class="col-sm-8">
                    <input type="datetime-local" disabled class="form-control" style="width:240px;" :value="getFormattedDateString(item.importLastUpdateSpeedRunsDate)"/>
                </div>
            </div>         
            <div class="form-group row no-gutters mb-2">
                <label class="col-sm-4 col-form-label">Last Full Import</label>
                <div class="col-sm-8">
                    <input type="datetime-local" disabled class="form-control" style="width:240px;" :value="getFormattedDateString(item.importLastBulkReloadDate)"/>
                </div>
            </div>                 
        </div> 
    </div>   
</template>
<script>
    import axios from 'axios'
    import { getDateTimeLocalString } from '../../js/common.js';

    export default {
        name: 'ImportStatusVue',
        data: function () {
            return {
                item: {},
                loading: false
            }
        },      
        created: function () {
            this.loadData();
        },
        methods: {
            loadData: function () {
                var that = this;
                this.loading = true;

                var prms = axios.get('/Menu/GetImportStatus')
                    .then(res => {
                        that.item = res.data;
                        that.loading = false;

                        return res;
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });

                return prms;
            },
            getFormattedDateString: function (value) {
                return getDateTimeLocalString(value);
            }
        }        
    };
</script>


