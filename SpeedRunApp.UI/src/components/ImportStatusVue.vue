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
                    <span class="form-control" style="width:170px;">{{ getDateString(item.importLastRunDate) }}</span>
                </div>
            </div>
            <div class="form-group row no-gutters mb-2">
                <label class="col-sm-4 col-form-label">Last Nightly Update</label>
                <div class="col-sm-8">
                    <span class="form-control" style="width:170px;">{{ getDateString(item.importLastUpdateSpeedRunsDate) }}</span>
                </div>
            </div>         
            <div class="form-group row no-gutters mb-2">
                <label class="col-sm-4 col-form-label">Last Full Import</label>
                <div class="col-sm-8">
                    <span class="form-control" style="width:170px;">{{ getDateString(item.importLastBulkReloadDate) }}</span>
                </div>
            </div>                 
        </div> 
    </div>   
</template>
<script>
    const dayjs = require('dayjs');
    import axios from 'axios'
    
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

                var prms = axios.get('/SpeedRun/GetImportStatus')
                    .then(res => {
                        that.item = res.data;
                        that.loading = false;

                        return res;
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });

                return prms;
            },
            getDateString: function(value) {
                return  value ? dayjs(value).format("M/DD/YYYY h:mm A") : "";
            }
        }
    };
</script>


