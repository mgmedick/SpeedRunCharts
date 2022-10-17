<template>
    <div>
        <div>
            <div class="mx-auto" style="max-width:598px; margin-bottom:20px;">
                <div class="btn-group btn-group-toggle pr-2">
                    <label v-for="(item, itemIndex) in items" class="btn btn-primary btn-sm font-weight-bold category" :class="{ 'active' : categoryid == item.id }" style="font-size:13px;" v-tippy="item.description">
                        <input type="radio" autocomplete="off" :value="item.id" v-model="categoryid" @change="onCategoryChange"><i :class="getIconClass(item.id)"></i>&nbsp;{{ item.displayName.replace(/ /g, '\u00a0') }}
                    </label>
                </div>
            </div>
        </div>
        <div>
            <speedrun-list :categoryid="categoryid.toString()" :defaulttopamt="defaulttopamt"></speedrun-list>
        </div>
    </div>
</template>
<script>
    import axios from 'axios';

    export default {
        name: 'SpeedRunListCategoryVue',
        props: {
            defaulttopamt: Number
        },
        data: function () {
            return {
                items: [],
                categoryid: sessionStorage.getItem("speedrunlistcategoryid") ?? 0
            }
        },
        created() {
            this.loadData();
        },
        methods: {
            loadData: function () {
                var that = this;
                this.loading = true;

                axios.get('/SpeedRun/GetSpeedRunListCategories')
                    .then(res => {
                        that.items = res.data;
                        that.loading = false;
                        return res;
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
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
            }
        }
    };
</script>





