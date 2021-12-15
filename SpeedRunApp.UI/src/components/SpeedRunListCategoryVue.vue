<template>
    <div>
        <div>
            <div class="mx-auto p-2" style="max-width:598px; margin-bottom:20px;">
                <div class="btn-group btn-group-toggle pr-2">
                    <label v-for="(item, itemIndex) in items" class="btn btn-primary btn-sm category" :class="{ 'active' : categoryid == item.id }" v-tippy="item.description">
                        <input type="radio" autocomplete="off" :value="item.id" v-model="categoryid" @click="onCategoryChange"><i :class="getIconClass(item.id)"></i>&nbsp;{{ item.displayName }}
                    </label>
                </div>
            </div>
        </div>
        <div>
            <speedrun-list :categoryid="categoryid"></speedrun-list>
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
                        iconClass = 'fa fa-certificate fa-lg';
                        break;
                    case 1:
                        iconClass = 'fa fa-percentage fa-lg';
                        break;
                    case 2:
                        iconClass = 'fa fa-award fa-lg';
                        break;
                    case 3:
                        iconClass = 'fa fa-fire fa-lg';
                        break;
                    case 4:
                        iconClass = 'fa fa-star fa-lg';
                        break;
                }

                return iconClass;
            },
            onCategoryChange: function (event) {
                Array.from(document.querySelectorAll('.category.active')).forEach((el) => el.classList.remove('active'));
                event.target.parentElement.classList.add("active");
            }
        }
    };
</script>


