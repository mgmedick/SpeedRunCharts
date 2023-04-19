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
            <div>
                <div style="margin-bottom:10px;">
                    <div class="btn-group btn-group-toggle">
                        <label class="btn btn-primary btn-sm categorytype" :class="{ 'active' : !categorytypeid }">
                            <input type="radio" autocomplete="off" value="" v-model="categorytypeid" @change="onCategoryTypeChange">All
                        </label>
                        <label class="btn btn-primary btn-sm categorytype" :class="{ 'active' : categorytypeid == 0 }">
                            <input type="radio" autocomplete="off" value="0" v-model="categorytypeid" @change="onCategoryTypeChange">Full Game
                        </label>
                        <label class="btn btn-primary btn-sm categorytype" :class="{ 'active' : categorytypeid == 1 }">
                            <input type="radio" autocomplete="off" value="1" v-model="categorytypeid" @change="onCategoryTypeChange">Level
                        </label>                                                
                    </div>
                </div>                
                <div style="margin-bottom:20px;">
                    <div class="btn-group btn-group-toggle" style="display: block">
                        <label v-for="(item, itemIndex) in items" class="btn btn-primary btn-sm category" :class="{ 'active' : categoryid == item.id }" v-tippy="item.description">
                            <input type="radio" autocomplete="off" :value="item.id" v-model="categoryid" @change="onCategoryChange"><i :class="getIconClass(item.id)"></i>&nbsp;{{ item.displayName.replace(/ /g, '\u00a0') }}
                        </label>
                    </div>
                </div>
            </div>
            <div>
                <speedrun-list :categoryid="categoryid" :defaulttopamt="defaulttopamt" :categorytypeid="categorytypeid"></speedrun-list>
            </div>
        </div>
    </div>    
</template>
<script>
    import axios from 'axios';

    export default {
        name: 'SpeedRunListTab',
        props: {
            defaulttopamt: Number
        },
        data: function () {
            return {
                items: [],
                categoryid: sessionStorage.getItem("speedrunlistcategoryid") ?? null,
                categorytypeid: sessionStorage.getItem("speedrunlistcategorytypeid") ?? null,
                loading: true
            }
        },
        created() {
            var isPageReloaded = ((window.performance.navigation && window.performance.navigation.type === 1) ||
                        window.performance.getEntriesByType('navigation').map((nav) => nav.type).includes('reload'));

            if (isPageReloaded) {
                this.resetParams();                   
            }

            this.loadData();
        },
        methods: {
            loadData: function () {
                var that = this;
                this.loading = true;

                axios.get('/SpeedRun/GetSpeedRunListCategories')
                    .then(res => {
                        that.items = res.data;                   
                        if (!that.categoryid) {
                            that.categoryid = res.data[0]?.id;
                            sessionStorage.setItem("speedrunlistcategoryid", that.categoryid);                            
                        }
                        that.loading = false;
                        return res;
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            },
            resetParams: function() {
                this.categoryid = null;
                sessionStorage.removeItem("speedrunlistcategoryid");
                this.categorytypeid = null;
                sessionStorage.removeItem("speedrunlistcategorytypeid");                
            },            
            getIconClass: function (id) {
                var iconClass = '';

                switch (id) {
                    case 0:
                        iconClass = 'fa fa-certificate';
                        break;
                    case 1:
                        iconClass = 'fa fa-percentage';
                        break;
                    case 2:
                        iconClass = 'fa fa-award';
                        break;
                    case 3:
                        iconClass = 'fa fa-cubes';
                        break;
                    case 4:
                        iconClass = 'fa fa-star';
                        break;
                    case 5:
                        iconClass = 'fa fa-fire';
                        break;
                    case 7:
                        iconClass = 'fa fa-gamepad';
                        break;       
                    case 8:
                        iconClass = 'fa fa-lightbulb';
                        break;    
                    case 9:
                        iconClass = 'fa fa-chart-line';
                        break;                                                                                               
                }

                iconClass += " fa-sm";

                return iconClass;
            },                    
            onCategoryChange: function (event) {
                Array.from(document.querySelectorAll('.category.active')).forEach((el) => el.classList.remove('active'));
                event.target.parentElement.classList.add("active");
                sessionStorage.setItem("speedrunlistcategoryid", this.categoryid); 
            },                            
            onCategoryTypeChange: function (event) {
                Array.from(document.querySelectorAll('.categorytype.active')).forEach((el) => el.classList.remove('active'));
                event.target.parentElement.classList.add("active");
                sessionStorage.setItem("speedrunlistcategorytypeid", this.categorytypeid); 
            }                          
        }
    };
</script>





