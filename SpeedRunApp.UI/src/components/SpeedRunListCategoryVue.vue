<template>
    <div>
        <div>
            <div class="mx-auto p-2" style="max-width:598px; margin-bottom:20px;">
                <div class="btn-group btn-group-toggle pr-2">
                    <label v-for="(item, itemIndex) in items" class="btn btn-primary btn-sm font-weight-bold category" :class="{ 'active' : categoryid == item.id }" style="font-size:13px;" v-tippy="item.description">
                        <input type="radio" autocomplete="off" :value="item.id" v-model="categoryid" @click="onCategoryChange"><i :class="getIconClass(item.id)"></i>&nbsp;{{ item.displayName }}
                    </label>
                </div>
            </div>
        </div>
        <div>
            <speedrun-list :categoryid="categoryid.toString()"></speedrun-list>
        </div>
    </div>
</template>
<script>
    import axios from 'axios';

    export default {
        name: 'SpeedRunListCategoryVue',
        data: function () {
            return {
                items: [],
                categoryid: 0
            }
        },
        computed: {
            isMediaMedium: function () {
                return window.innerWidth > 768;
            }
        },
        created() {
            this.loadData();
        },
        methods: {
            loadData: function () {
                var that = this;
                this.loading = true;

                axios.get('../SpeedRun/GetSpeedRunListCategories')
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
                }

                iconClass += " fa-sm";
                // if (this.isMediaMedium) {
                //     iconClass += " fa-lg";
                // } else {
                //     iconClass += " fa-sm";
                // }

                return iconClass;
            },
            onCategoryChange: function (event) {
                Array.from(document.querySelectorAll('.category.active')).forEach((el) => el.classList.remove('active'));
                event.target.parentElement.classList.add("active");
            }
        }
    };
</script>
<style scoped>
    @media (max-width: 575px) {
        .category {
            font-size: 11px !important;
        }
    }    
</style>




