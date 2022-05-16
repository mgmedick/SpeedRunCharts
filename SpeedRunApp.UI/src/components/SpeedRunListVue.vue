<template>
    <div>
        <speedrun-summary v-for="(item, index) in items" :item="item" :index="index" :key="item.id"></speedrun-summary>
        <div v-if="loading">
            <div class="d-flex">
                <div class="mx-auto">
                    <i class="fas fa-spinner fa-spin fa-lg"></i>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import axios from 'axios'

    export default {
        name: 'SpeedRunListVue',
        props: {
            categoryid: String
        },
        data() {
            return {
                items: [],
                loading: true,
                throttleTimer: null,
                throttleDelay: 500,
                topamt: 20,
                offset: null
            }
        },
        created() {
            this.loadData();
            window.addEventListener('scroll', this.onWindowScroll);
        },
        watch: {
            categoryid: function (val, oldVal) {
                this.items = [];
                this.offset = null;
                this.loadData();
            }
        },
        methods: {
            reLoadData: function () {
                var orderValues = Array.from(document.querySelectorAll('.orderValue')).map(i => i.value);
                var offset = orderValues.length > 0 ? Math.min.apply(null, orderValues) : null;

                this.offset = offset;
                return this.loadData();
            },
            loadData: function () {
                var that = this;
                this.loading = true;

                var prms = axios.get('/SpeedRun/GetLatestSpeedRuns', { params: { category: this.categoryid, topAmount: this.topamt, orderValueOffset: this.offset } })
                    .then(res => {
                        that.items = that.items.concat(res.data);
                        that.loading = false;
                        return res;
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });

                return prms;
            },
            onWindowScroll: function () {
                var that = this;
                
                clearTimeout(this.throttleTimer);
                this.throttleTimer = setTimeout(function () {
                    var scollTop = document.documentElement.scrollTop + window.innerHeight;
                    var offsetHeight = document.documentElement.offsetHeight;
                    if(Math.floor(scollTop) === offsetHeight || Math.ceil(scollTop) === offsetHeight) {
                        that.reLoadData();
                    }
                }, this.throttleDelay);
            }   
        }
    };
</script>










