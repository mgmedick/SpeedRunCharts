<template>
    <nav class="navbar navbar-expand-lg bg-body-tertiary">
        <div class="container-fluid">
            <a class="navbar-brand" href="#/" draggable="false" @click="onHomeClick">
                <img src="/dist/fonts/pie-chart.svg" width="30" height="30" class="d-inline-block align-top pe-1" alt="">
                SpeedRunCharts
            </a>
            <button id="btnToggleNavbar" class="navbar-toggler" type="button" @click="toggleNavbar = !toggleNavbar" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div id="navbarNav" class="navbar-collapse" :style="[ toggleNavbar ? null : { display:'none' } ]">
                <ul class="navbar-nav me-auto">
                    <li class="nav-item">
                        <a class="nav-link" href="/Menu/About">About</a>
                    </li>                    
                </ul>
                <input type="search" class="form-control" style="max-width: 300px;" placeholder="Search games, users" @click="onSearchClick" readonly>
                <div v-if="isauth">
                    <div class="btn-group">
                        <button class="btn btn-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                            <span>
                                <i class="fa fa-user"></i>
                            </span>
                        </button>
                        <ul class="dropdown-menu dropdown-menu-end">
                            <li>
                                <div class="dropdown-item">
                                    <div class="form-check form-switch">
                                        <input id="chkNightMode" class="form-check-input" type="checkbox" v-model="isDarkTheme">
                                        <label class="form-check-label" for="chkNightMode"><i class="fa fa-moon"></i><span class="ps-2">Night Mode</span></label>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <a href="/User/UserSettings" class="dropdown-item"><i class="fa fa-cog"></i><span class="ps-2">Settings</span></a>
                            </li>
                            <li>
                                <a href="/Home/Logout" class="dropdown-item"><i class="fa fa-sign-out-alt"></i><span class="ps-2">Log out</span></a>
                            </li>
                        </ul>
                    </div>                     
                </div>
                <ul v-else class="navbar-nav">
                    <li class="nav-item">
                        <a class="nav-link" href="/Home/Login">Log In</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/Home/SignUp">Sign Up</a>
                    </li>
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                            Options
                        </a>
                        <ul class="dropdown-menu dropdown-menu-end">
                            <li>
                                <div class="dropdown-item">
                                    <div class="form-check form-switch">
                                        <input id="chkNightMode" class="form-check-input" type="checkbox" v-model="isDarkTheme">
                                        <label class="form-check-label" for="chkNightMode"><i class="fa fa-moon"></i><span class="ps-2">Night Mode</span></label>
                                    </div>
                                </div>
                            </li>
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
        <div ref="searchmodal" class="modal modal-lg" tabindex="-1">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Search</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button> 
                    </div>
                    <div class="modal-body">
                        <div>
                            <button type="button" class="btn btn-primary btn-sm" :class="{ 'active' : searchTypeID == 0 }" @click="onSearchTypeClick(0)">Games</button>
                            <button type="button" class="btn btn-primary btn-sm ms-1" :class="{ 'active' : searchTypeID == 1 }" @click="onSearchTypeClick(1)">Players</button>                            
                        </div>
                        <div class="mt-3">
                            <autocomplete ref="searchautocomplete" v-model="searchText" @search="onSearch" @selected="onSearchSelected" :options="searchResults" :isasync="true" :isimgresults="true" :isimgcircle="searchTypeID == 1" :loading="searchLoading" :placeholder="searchTypeID == 0 ? 'Search games' : 'Search players'" />                        
                        </div>
                    </div>
                </div>
            </div>
        </div>                                 
    </nav>           
</template>
<script>
    import axios from 'axios'
    import { setCookie } from '../../js/common';
    import { Modal } from 'bootstrap';

    export default {
        name: "Navbar",
        props: {
            isauth: Boolean,
            username: String,
            userid: String,
            isdarktheme: Boolean
        },
        data: function () {
            return {
                searchText: null,
                searchResults: [],
                searchLoading: false,
                showImportStatusModal: false,
                showDropdown: false,
                toggleNavbar: false,
                isDarkTheme: this.isdarktheme,
                searchTypeID: 0
            }
        },
        computed: {
        },
        watch: {
            isDarkTheme: function (val, oldVal) {
                var that = this;

                if (this.isauth) {
                    axios.post('/Home/UpdateIsDarkTheme', null,{ params: { isDarkTheme: val } })
                        .then((res) => {
                            if (res.data.success) {
                                that.updateTheme(this.isDarkTheme);
                            } else {
                                res.data.errorMessages.forEach(errorMsg => {
                                    errorToast(errorMsg);                           
                                });                                
                            }                                                                                 
                        })
                        .catch(err => { console.error(err); return Promise.reject(err); });        
                } else {
                    this.updateTheme(val);
                    var theme = val ? "dark" : "light";
                    setCookie("theme", theme);                  
                }
            }
        },
        created: function () {
        },      
        mounted: function() {
            var that = this;

            that.$refs.searchmodal.addEventListener('hidden.bs.modal', event => {
                that.$refs.searchautocomplete.clear();
            });                  
        },
        methods: {
            onSearchClick(e){
                this.searchText = null;

                this.$nextTick(function() {
                    new Modal(this.$refs.searchmodal).show();
                });  
            }, 
            onSearchTypeClick: function (searchTypeID) {
                this.searchTypeID = searchTypeID;
                this.searchLoading = true;

                this.$nextTick(function() {
                    this.$refs.searchautocomplete.reload(this.searchText);
                });  
            },             
            onSearch: function() {
                var that = this;
                this.searchLoading = true;
                               
                var loc = this.searchTypeID == 0 ? '/Game/SearchGames' : '/Player/SearchPlayers'
                axios.get(loc, { params: { term: this.searchText } })
                        .then(res => {
                            that.searchResults = res.data.reduce((flat, groupheader) => {
                                return flat
                                    .concat({
                                        label: groupheader.label,
                                        value: groupheader.value,
                                        isGroupHeader: true
                                    })
                                    .concat(groupheader.subItems)
                            }, []);
                            that.searchLoading = false;

                            return res;
                        })
                        .catch(err => { console.error(err); return Promise.reject(err); });
            },    
            onSearchSelected: function (result) {
                var loc = this.searchTypeID == 0 ? '/Game/GameDetails/' : '/Player/PlayerDetails/'
                location.href = encodeURI(loc + result.value);
            },                                                                
            onHomeClick: function() {
                if (window.location.pathname == '/') {
                    window.location.reload(true);
                } else {
                    sessionStorage.removeItem("speedrunsummarylistid");
                    sessionStorage.removeItem("topamt");
                    sessionStorage.removeItem("offset");
                    sessionStorage.removeItem("scrolltop");
                    window.location.href = "/";
                }               
            },
            updateTheme: function(val){
                var el = document.body;

                if (val){
                    el.dataset.bsTheme = "dark";
                } else {
                    el.dataset.bsTheme = "light";
                }
            }
        }
    };
</script>






