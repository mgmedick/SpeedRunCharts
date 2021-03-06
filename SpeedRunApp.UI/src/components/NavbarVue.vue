﻿<template>
    <div>
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
            <a class="navbar-brand" href="/">
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
                    <li class="nav-item active p-2">
                        <a href="https://github.com/speedruncomorg/api" class="badge badge-primary nav-link">Powered by speedrun.com API</a>
                    </li>
                </ul>
                <form class="form-inline">
                    <vue-next-select v-model="searchSelected"
                                    :options="searchOptions"
                                    :loading="searchLoading"
                                    searchable
                                    @search:input="onSearchGames"
                                    @selected="onSearchSelected"
                                    group-by="isConstructor"
                                    label-by="label"
                                    value-by="value"
                                    clear-on-select
                                    close-on-select
                                    openDirection="bottom"
                                    placeholder="Search games, users"
                                    :style="{ width:100 + '%' }" />
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
                                    <input id="chkNightMode" type="checkbox" class="custom-control-input" data-toggle="toggle" v-model="isdarktheme">
                                    <label class="custom-control-label pl-1" for="chkNightMode"><i class="fa fa-moon"></i><span class="pl-2">Night Mode</span></label>
                                </div>
                            </div>
                            <a href="/UserAccount/UserAccountDetails" class="dropdown-item"><i class="fa fa-cog"></i><span class="pl-2">Settings</span></a>
                            <a href="/SpeedRun/Logout" class="dropdown-item"><i class="fa fa-sign-out-alt"></i><span class="pl-2">Log out</span></a>
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
                                        <input id="chkNightMode" type="checkbox" class="custom-control-input" data-toggle="toggle" v-model="isdarktheme">
                                        <label class="custom-control-label pl-1" for="chkNightMode"><i class="fa fa-moon"></i><span class="pl-2">Night Mode</span></label>
                                    </div>
                                </div>
                                <a class="dropdown-item" href="#" @click="showLoginModal = true"><i class="fa fa-user"></i><span class="pl-2">Log In</span></a>
                                <a class="dropdown-item" href="#" @click="showSignUpModal = true"><i class="fa fa-clipboard"></i><span class="pl-2">Sign Up</span></a>
                            </template>
                        </button-dropdown> 
                    </li>                   
                </ul>
            </div>
        </nav>
        <custom-modal v-model="showLoginModal" v-if="showLoginModal" contentclass="modal-md">
            <template v-slot:title>
                Log In
            </template>
            <login @forgotpass="showResetModal = !(showLoginModal = false)" />
        </custom-modal>
        <custom-modal v-model="showResetModal" v-if="showResetModal" contentclass="modal-md">
            <template v-slot:title>
                Reset Password
            </template>
            <reset-password />
        </custom-modal>
        <custom-modal v-model="showSignUpModal" v-if="showSignUpModal" contentclass="modal-md">
            <template v-slot:title>
                Sign Up
            </template>
            <signup />
        </custom-modal>
    </div>   
</template>
<script>
    import axios from 'axios'
    import { setCookie } from '../js/common.js';

    export default {
        name: "NavbarVue",
        props: {
            isauth: Boolean,
            isdarktheme: Boolean,
            username: String,
            userid: String
        },
        data: function () {
            return {
                searchSelected: null,
                searchOptions: [],
                searchLoading: false,
                showLoginModal: false,
                showResetModal: false,
                showSignUpModal: false,
                showDropdown: false,
                toggleNavbar: false
            }
        },
        computed: {
        },
        watch: {
            isdarktheme: function (val, oldVal) {
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
            onSearchGames: function (e) {
                if (e.target.value) {
                    var that = this;
                    this.searchLoading = true;

                    axios.get('/Menu/Search', { params: { term: e.target.value } })
                        .then(res => {
                            that.searchOptions = res.data.reduce((flat, constructor) => {
                                return flat
                                    .concat({
                                        label: constructor.label,
                                        value: constructor.subItems.map(method => method.value),
                                        isConstructor: true
                                    })
                                    .concat(constructor.subItems.map(method => ({ label: method.label, value: method.value, category: constructor.label })))
                            }, []);
                            that.searchLoading = false;

                            return res;
                        })
                        .catch(err => { console.error(err); return Promise.reject(err); });
                }
            },
            onSearchSelected: function (option) {
                var controller;
                var action;
                var params;

                if (option.category == 'Games') {
                    controller = "Game";
                    action = "GameDetails"
                } else {
                    controller = "User";
                    action = "UserDetails"
                } 

                location.href = encodeURI('/' + controller + "/" + action + "/" + option.value);
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
<style scoped>
    @media (min-width: 992px) {
        :deep(.vue-select) {
            min-width: 400px !important;
            margin-right: 8px;
        }

        :deep(.vue-dropdown) {
            min-width: 400px !important;
            max-width: 500px;
            width: auto !important;
        }
    }
</style>





