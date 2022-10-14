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
            categoryid: String,
            defaulttopamt: Number
        },
        data() {
            return {
                items: [],
                loading: true,
                throttleTimer: null,
                throttleDelay: 500,
                topamt: sessionStorage.getItem("topamt") ?? this.defaulttopamt,
                offset: sessionStorage.getItem("offset") ?? null
            }
        },
        created() {
            var isPageReloaded = ((window.performance.navigation && window.performance.navigation.type === 1) ||
                        window.performance.getEntriesByType('navigation').map((nav) => nav.type).includes('reload'));

            if (isPageReloaded) {
                this.resetParams();                   
            }
            
            this.loadData().then(function() {                               
                if (sessionStorage.scrolltop) {
                    document.documentElement.scrollTop = sessionStorage.getItem("scrolltop");
                }
            });
            window.addEventListener('scroll', this.onWindowScroll);
            window.addEventListener('beforeunload', this.onBeforeUnload);          
        },             
        watch: {
            categoryid: function (val, oldVal) {
                this.resetParams();        
                this.loadData();
            }
        },
        methods: {
            reLoadData: function () {
                var that = this;
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
            resetParams: function() {
                this.items = [];
                this.offset = null;
                this.topamt = this.defaulttopamt;
                sessionStorage.removeItem("topamt");
                sessionStorage.removeItem("offset");
                sessionStorage.removeItem("scrolltop");
            },
            onWindowScroll: function () {
                var that = this;
                
                clearTimeout(this.throttleTimer);
                this.throttleTimer = setTimeout(function () {
                    var scollTop = document.documentElement.scrollTop + window.innerHeight;
                    var offsetHeight = document.documentElement.offsetHeight;
                    if(Math.floor(scollTop) >= offsetHeight || Math.ceil(scollTop) >= offsetHeight) {
                        that.reLoadData();
                    }
                }, this.throttleDelay);
            },
            onBeforeUnload: function() {
                sessionStorage.setItem("scrolltop", document.documentElement.scrollTop);

                var orderValue = Array.from(document.querySelectorAll('.orderValue')).map(i => i.value)[0];
                sessionStorage.setItem("offset", parseInt(orderValue) + 1);

                if (this.items.length > this.topamt) {
                    sessionStorage.setItem("topamt", this.items.length);
                }
            }
        }
    };
</script>










