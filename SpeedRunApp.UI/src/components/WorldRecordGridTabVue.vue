<template>
    <div v-if="!loading" id="divGridContainer" class="container-lg m-0 p-0">
        <div class="row no-gutters pl-3 pr-1 pt-1 pb-0 pr-0">
            <div class="col-sm-1 align-self-top pt-1">
                <label class="tab-row-name">Category Type:</label>
            </div>
            <div class="col pl-2 tab-list">
                <ul class="nav nav-pills">
                    <li class="nav-item p-1" v-for="(categoryType, categoryTypeIndex) in categoryTypes" :key="categoryType.id">
                        <a class="nav-link p-2" :class="{ 'active' : categoryTypeID == categoryType.id }" :data-value="categoryType.id" href="#/" data-toggle="pill" @click="onCategoryTypeClick">{{ categoryType.name }}</a>
                    </li>
                </ul>
            </div>
        </div>
        <div v-for="(categoryType, categoryTypeIndex) in categoryTypes" :key="categoryType.id">
            <div v-if="categoryTypeID == categoryType.id">
                <worldrecord-grid :gameid="gameid" :categorytypeid="categoryTypeID.toString()"></worldrecord-grid>
            </div>
        </div>
    </div>
</template>
<script>
    import axios from 'axios';

    export default {
        name: "WorldRecordGridTabVue",
        props: {
            gameid: String
        },
        data() {
            return {
                categoryTypes: [],
                categoryTypeID: '',
                loading: true
            }
        },
        created: function () {
            this.loadData();
        },
        methods: {
            loadData() {
                var that = this;
                this.loading = true;

                var prms = axios.get('../Game/GetWorldRecordGridTabs', { params: { gameID: this.gameid } })
                                .then(res => {
                                    that.categoryTypes = res.data;
                                    that.categoryTypeID = res.data[0].id;
                                    that.loading = false;
                                    return res;
                                })
                                .catch(err => { console.error(err); return Promise.reject(err); });

                return prms;
            },
            onCategoryTypeClick: function (event) {
                var value = event.target.getAttribute('data-value');
                this.categoryTypeID = value;
            }
        }
    };
</script>
<style>
    .tab-list .nav-link {
        background-color: #313131;
        font-size: 13px;
        font-weight: bold;
    }

/*    .tab-list .nav-link:hover {
        background-color: #2b2a2a;
    }
*/
   .tab-row-name {
        font-size: 14px !important;
        line-height: 18px;
        font-weight: bold;
    } 
</style>




