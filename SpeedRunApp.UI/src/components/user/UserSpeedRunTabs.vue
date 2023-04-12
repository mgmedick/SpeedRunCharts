<template>
    <div v-if="loading">
        <div class="d-flex">
            <div class="mx-auto">
                <i class="fas fa-spinner fa-spin fa-lg"></i>
            </div>
        </div>
    </div> 
    <div v-else id="divSpeedRunGridTabContainer">
        <div class="row no-gutters pr-1 pt-1 pb-0">
            <div class="col tab-list">
                <ul class="nav nav-pills">
                    <li class="categoryType nav-item py-1 pr-1" v-for="(categoryType, categoryTypeIndex) in categoryTypes" :key="categoryType.id">
                        <a class="nav-link p-2" :class="{ 'active' : categoryTypeID == categoryType.id }" href="#/" data-type="categoryType" :data-value="categoryType.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ categoryType.name }}</a>
                    </li>
                    <button-dropdown v-show="false" class="more py-1 pr-1" :btnclasses="'btn-secondary'">
                        <template v-slot:text>
                            <span>More...</span>
                        </template>
                        <template v-slot:options>
                            <template v-for="(categoryType, categoryTypeIndex) in categoryTypes" :key="categoryType.id">
                                <a class="dropdown-item d-none" :class="{ 'active' : categoryTypeID == categoryType.id }" href="#/" data-type="categoryType" :data-value="categoryType.id" data-toggle="pill" draggable="false" @click="onTabClick">{{ categoryType.name }}</a>
                            </template>
                        </template>
                    </button-dropdown>                       
                </ul>
            </div>                    
        </div>
        <div class="row no-gutters pr-1 pt-0">
            <div class="col-auto pr-2">
                <i class="fas fa-info-circle fa-sm" v-tippy="'Hiding obsolete runs lists the Personal Bests'"></i>&nbsp;<label class="tab-row-name">Show Obsolete:</label>
            </div>
            <div class="col align-self-center">
                <div class="custom-control custom-switch">
                    <input id="chkShowAllData" type="checkbox" class="custom-control-input" data-toggle="toggle" v-model="showAllData">
                    <label class="custom-control-label" for="chkShowAllData"></label>
                </div>
            </div>                                                
        </div>         
        <div v-for="(categoryType, categoryTypeIndex) in categoryTypes" :key="categoryType.id">
            <div v-if="categoryTypeID == categoryType.id">
                <div v-for="(game, gameIndex) in items.filter(item => item.categoryTypes.filter(i => i.id == categoryType.id).length > 0)" :key="game.id" class="mt-4">
                    <div>
                        <div class="row no-gutters pb-1">
                            <div class="col-1 p-0" style="max-width:37px;">
                                <div class="img-round">
                                    <img :src="game.coverImageUri" class="img-fluid" alt="Responsive image">
                                </div>
                            </div>                            
                            <div class="col-11 pl-2 align-self-end">
                                <h6 class="font-weight-bold mb-0 nowrap-elipsis">{{ game.name }}</h6>
                            </div>
                        </div>
                        <user-speedrun-grid :userid="id" :gameabbr="game.abbr" :tabledata="tableData.filter(item => item.gameID == game.id && ((categoryType.id == 0 && !item.levelID) || (categoryType.id == 1 && item.levelID)))" :showmilliseconds="game.showMilliseconds" :variables="game.variables" :showalldata="showAllData"></user-speedrun-grid>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import axios from 'axios';

    export default {
        name: "UserSpeedRunTabs",
        props: {
            id: String,
            speedrunid: String
        },
        data() {
            return {
                items: [],
                categoryTypes: [],
                tableData: [],
                categoryTypeID: '',
                showAllData: false,
                showDetailModal: false,
                loading: true
            }
        },   
        mounted: function () {
            this.loadData();
            window.addEventListener('resize', this.resizeSRTabs);
        },       
        updated: function () {
            this.resizeSRTabs();
        },
        methods: {
            loadData() {
                var that = this;
                this.loading = true;

                var url = '/Game/GetUserSpeedRunTabsAndGridData?userID=' + this.id + '&speedRunID=' + this.speedrunid;
                var prms = axios.get(url)
                                .then(res => {
                                    that.items = res.data.tabItems;
                                    that.categoryTypes = res.data.categoryTypes,
                                    that.tableData = res.data.tableData,
                                    that.initSelected();
                                    that.loading = false;
                                    return res;
                                })
                                .catch(err => { console.error(err); return Promise.reject(err); });

                return prms;
            },
            initSelected: function () {
                this.categoryTypeID = this.categoryTypeID || this.categoryTypes[0].id;
            },            
            onTabClick: function (event) {
                var type = event.target.getAttribute('data-type');
                var value = event.target.getAttribute('data-value');

                switch(type) {
                    case 'categoryType':
                        this.categoryTypeID = value;
                        break;                                                                                                                                                                                            
                }
            },                 
            resizeSRTabs: function () {
                var rows = document.querySelectorAll('#divSpeedRunGridTabContainer .tab-list');

                for (var g = 0; g < rows.length; g++) {
                    var totalWidth = 0;
                    var tabitems = rows[g].querySelectorAll('li:not(.dropdown-item)');
                    var morediv =  rows[g].querySelector('.more');
                    var morebtn =  morediv.querySelector('.btn');
                    var moreItems = morediv.querySelectorAll('a.dropdown-item');
                    var moredrp = morediv.querySelector('.dropdown-menu');

                    for (var i = 0; i < tabitems.length; i++) {
                        tabitems[i].style.left = "-10000px";
                        tabitems[i].classList.remove('d-none');
                        totalWidth += tabitems[i].offsetWidth;
                        if (totalWidth > ((rows[g].offsetWidth - tabitems[i].offsetWidth) - 30)) {
                            tabitems[i].classList.add('d-none');
                            moreItems[i].classList.remove('d-none');
                        } else {
                            tabitems[i].classList.remove('d-none');
                            moreItems[i].classList.add('d-none');                     
                        }
                    }

                    var items = Array.from(morediv.querySelectorAll('a:not(.d-none)'));
                    if (items.length > 0) {
                        var item = items.find(i=>i.classList.contains('active'));
                        if (item) {
                            morebtn.innerHTML = item.innerHTML;
                            morebtn.classList.add('active');
                        } else {
                            morebtn.innerHTML = 'More...';
                            morebtn.classList.remove('active');
                        }
                        
                        morediv.style.display = 'block';
                    } else {
                        morediv.style.display = 'none';                       
                    }

                    var ww = document.documentElement.clientWidth;
                    var pos = this.getPosition(morediv);
                    if (pos.x > (ww / 2)) {
                        moredrp.classList.remove('dropdown-menu-left');
                        moredrp.classList.add('dropdown-menu-right');
                    } else {
                        moredrp.classList.remove('dropdown-menu-right');
                        moredrp.classList.add('dropdown-menu-left');
                    }
               }
            },
            getPosition: function (element) {
                var xPosition = 0;
                var yPosition = 0;

                while (element) {
                    xPosition += (element.offsetLeft - element.scrollLeft + element.clientLeft);
                    yPosition += (element.offsetTop - element.scrollTop + element.clientTop);
                    element = element.offsetParent;
                }
                return {
                    x: xPosition,
                    y: yPosition
                };
            }                      
        }
    };
</script>









