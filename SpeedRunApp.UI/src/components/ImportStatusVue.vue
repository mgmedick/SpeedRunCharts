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
                    <input type="datetime-local" disabled class="form-control" style="width:240px;" :value="item.importLastRunDateString"/>
                </div>
            </div>
            <div class="form-group row no-gutters mb-2">
                <label class="col-sm-4 col-form-label">Last Nightly Update</label>
                <div class="col-sm-8">
                    <input type="datetime-local" disabled class="form-control" style="width:240px;" :value="item.importLastUpdateSpeedRunsDateString"/>
                </div>
            </div>         
            <div class="form-group row no-gutters mb-2">
                <label class="col-sm-4 col-form-label">Last Full Import</label>
                <div class="col-sm-8">
                    <input type="datetime-local" disabled class="form-control" style="width:240px;" :value="item.importLastBulkReloadDateString"/>
                </div>
            </div>                 
        </div> 
    </div>   
</template>
<script>
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
            }
        }
    };
</script>


