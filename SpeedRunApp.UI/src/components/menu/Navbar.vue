<template>
    <div>
        <nav class="navbar navbar-expand-lg bg-dark">
            <a class="navbar-brand" href="#/" draggable="false" @click="onHomeClick">
                <img src="/dist/fonts/pie-chart.svg" width="30" height="30" class="d-inline-block align-top pr-1" alt="">
                SpeedRunCharts
            </a>
            <button id="btnToggleNavbar" class="navbar-toggler" type="button" @click="toggleNavbar = !toggleNavbar" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div id="navbarNav" class="navbar-collapse" :style="[ toggleNavbar ? null : { display:'none' } ]">
                <ul class="navbar-nav mr-auto">
                    <li class="nav-item">
                        <a class="nav-link" href="/Menu/About">About</a>
                    </li>
                    <li class="nav-item active pt-1 pb-1">
                        <a href="https://github.com/speedruncomorg/api" class="badge badge-primary p-2">Powered by speedrun.com API</a>
                    </li>
                </ul>
                <form class="form-inline">
                    <autocomplete v-model="searchText" @change="onChange" @search="onSearch" @selected="onSearchSelected" :options="searchResults" labelby="label" valueby="label" :isasync="true" :loading="searchLoading" :placeholder="'Search games, users'" style="width:100%"/>                
                </form>
                <div v-if="isauth">
                    <button-dropdown :btnclasses="'btn-secondary'" :listclasses="'dropdown-menu-sm-right'">
                        <template v-slot:text>
                            <span>
                                <i class="fa fa-user"></i><span class="pl-2">{{ username }}</span>
                            </span>
                        </template>
                        <template v-slot:options>
                            <div class="dropdown-item">
                                <div class="custom-control custom-switch">
                                    <input id="chkNightMode" type="checkbox" class="custom-control-input" data-toggle="toggle" v-model="isDarkTheme">
                                    <label class="custom-control-label pl-1" for="chkNightMode"><i class="fa fa-moon"></i><span class="pl-2">Night Mode</span></label>
                                </div>
                            </div>
                            <a href="/UserAccount/UserAccountDetails" class="dropdown-item"><i class="fa fa-cog"></i><span class="pl-2">Settings</span></a>
                            <a href="/Home/Logout" class="dropdown-item"><i class="fa fa-sign-out-alt"></i><span class="pl-2">Log out</span></a>
                        </template>
                    </button-dropdown>
                </div>
                <ul v-else class="navbar-nav">
                    <li class="nav-item">
                        <a class="nav-link" href="#" @click="showLoginModal = true">Log In</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#" @click="showSignUpModal = true">Sign Up</a>
                    </li>
                    <li class="nav-item">
                        <button-dropdown :btnclasses="'btn-secondary'" :listclasses="'dropdown-menu-sm-right'">
                            <template v-slot:text>
                                <span>
                                    <i class="fa fa-user"></i>
                                </span>
                            </template>
                            <template v-slot:options>
                                <div class="dropdown-item">
                                    <div class="custom-control custom-switch">
                                        <input id="chkNightMode" type="checkbox" class="custom-control-input" data-toggle="toggle" v-model="isDarkTheme">
                                        <label class="custom-control-label pl-1" for="chkNightMode"><i class="fa fa-moon"></i><span class="pl-2">Night Mode</span></label>
                                    </div>
                                </div>
                                <a class="dropdown-item" href="#" @click="showImportStatusModal = true"><i class="fa fa-calendar-check"></i><span class="pl-2">Import Status</span></a>
                                <a class="dropdown-item" href="#" @click="showLoginModal = true"><i class="fa fa-user"></i><span class="pl-2">Log In</span></a>
                                <a class="dropdown-item" href="#" @click="showSignUpModal = true"><i class="fa fa-clipboard"></i><span class="pl-2">Sign Up</span></a>
                            </template>
                        </button-dropdown> 
                    </li>                   
                </ul>
            </div>
        </nav>
        <modal v-if="showImportStatusModal" contentclass="cmv-modal-md" @close="showImportStatusModal = false">
            <template v-slot:title>
                Import Status
            </template>
            <import-status />
        </modal>          
        <modal v-if="showLoginModal" contentclass="cmv-modal-md" @close="showLoginModal = false">
            <template v-slot:title>
                Log In
            </template>
            <login @forgotpass="showResetModal = !(showLoginModal = false)" />
        </modal>
        <modal v-if="showResetModal" contentclass="cmv-modal-md" @close="showResetModal = false">
            <template v-slot:title>
                Reset Password
            </template>
            <reset-password />
        </modal>
        <modal v-if="showSignUpModal" contentclass="cmv-modal-md" @close="showSignUpModal = false">
            <template v-slot:title>
                Sign Up
            </template>
            <signup />
        </modal>      
    </div>   
</template>
<script>
    import axios from 'axios'
    import { setCookie } from '../../js/common';

    export default {
        name: "Navbar",
        props: {
            isauth: Boolean,
            isdarktheme: Boolean,
            username: String,
            userid: String
        },
        data: function () {
            return {
                searchText: null,
                searchResults: [],
                searchLoading: false,
                showImportStatusModal: false,
                showLoginModal: false,
                showResetModal: false,
                showSignUpModal: false,
                showDropdown: false,
                toggleNavbar: false,
                isDarkTheme: this.isdarktheme
            }
        },
        computed: {
        },
        watch: {
            isDarkTheme: function (val, oldVal) {
                var that = this;

                if (this.isauth) {
                    axios.post('/UserAccount/UpdateIsDarkTheme', null,{ params: { isDarkTheme: val } })
                        .then((res) => {
                            if (res.data.success) {
                                that.updateTheme(val);
                            }                                                                                   
                        })
                        .catch(err => { console.error(err); return Promise.reject(err); });        
                } else {
                    this.updateTheme(val);
                    var theme = val ? "theme-dark" : "theme-light";
                    setCookie("theme", theme);                  
                }
            }
        },
        created: function () {
        },        
        methods: {
            onInput: function(e){
                this.searchText = e;
            },
            onChange: function() {
                var a = this.searchText;
            },
            onSearch: function() {
                var that = this;
                this.searchLoading = true;
                               
                axios.get('/Menu/Search', { params: { term: this.searchText } })
                        .then(res => {
                            that.searchResults = res.data.reduce((flat, groupheader) => {
                                return flat
                                    .concat({
                                        label: groupheader.label,
                                        value: groupheader.subItems.map(method => method.value),
                                        isGroupHeader: true,
                                        disabled: true
                                    })
                                    .concat(groupheader.subItems.map(method => ({ label: method.label, value: method.value, category: groupheader.label })))
                            }, []);

                            if(that.searchResults.length == 0)
                            {
                                var noResult = { value: "", label: "No results found", category: null, disabled: true };
                                that.searchResults.push(noResult);
                            }

                            that.searchLoading = false;
                            return res;
                        })
                        .catch(err => { console.error(err); return Promise.reject(err); });
            },              
            onSearchSelected: function (result) {
                var controller;
                var action;

                if (result.category == 'Games') {
                    controller = "Game";
                    action = "GameDetails"
                } else {
                    controller = "User";
                    action = "UserDetails"
                }

                location.href = encodeURI('/' + controller + "/" + action + "/" + result.value);
            },
            onHomeClick: function() {
                if (window.location.pathname == '/') {
                    window.location.reload(true);
                } else {
                    sessionStorage.removeItem("speedrunlistcategoryid");
                    sessionStorage.removeItem("topamt");
                    sessionStorage.removeItem("offset");
                    sessionStorage.removeItem("scrolltop");
                    window.location.href = "/";
                }               
            },
            updateTheme: function(val){
                var el = document.body;

                if (val){
                    el.classList.remove("theme-light");
                    el.classList.add("theme-dark");
                } else {
                    el.classList.remove("theme-dark");
                    el.classList.add("theme-light");
                }
            }
        }
    };
</script>






