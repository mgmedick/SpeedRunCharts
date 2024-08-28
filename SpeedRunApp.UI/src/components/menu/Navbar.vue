<template>
    <nav class="navbar navbar-expand-lg bg-dark">
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
                    <li class="nav-item active pt-1 pb-1">
                        <a href="https://github.com/speedruncomorg/api" class="badge badge-primary p-2">Powered by speedrun.com API</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/Menu/About">About</a>
                    </li>                    
                </ul>
                <form class="form-inline mb-2 mb-lg-0 me-2">
                    <!-- <autocomplete v-model="searchText" @change="onChange" @search="onSearch" @selected="onSearchSelected" :options="searchResults" labelby="label" valueby="label" :isasync="true" :loading="searchLoading" :placeholder="'Search games, users'" style="width:100%"/>                 -->
                    <autocomplete v-model="searchText" @search="onSearch" @selected="onSearchSelected" :options="searchResults" :isasync="true" :isimgresults="false" :loading="searchLoading" :placeholder="'Search games, users'" style="width:300px;"/>    
                </form>
                <div v-if="isauth">
                    <button-dropdown :btnclasses="'btn-secondary'" :listclasses="'dropdown-menu-sm-right'">
                        <template v-slot:text>
                            <span>
                                <i class="fa fa-user"></i><span class="ps-2">{{ username }}</span>
                            </span>
                        </template>
                        <template v-slot:options>
                            <div class="dropdown-item">
                                <div class="form-check form-switch">
                                    <input id="chkNightMode" class="form-check-input" type="checkbox" v-model="isDarkTheme">
                                    <label class="form-check-label" for="chkNightMode"><i class="fa fa-moon"></i><span class="ps-2">Night Mode</span></label>
                                </div>
                            </div>
                            <a href="/User/UserSettings" class="dropdown-item"><i class="fa fa-cog"></i><span class="ps-2">Settings</span></a>
                            <a href="/Home/Logout" class="dropdown-item"><i class="fa fa-sign-out-alt"></i><span class="ps-2">Log out</span></a>
                        </template>
                    </button-dropdown>
                </div>
                <ul v-else class="navbar-nav">
                    <li class="nav-item">
                        <a class="nav-link" href="/Home/Login">Log In</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/Home/SignUp">Sign Up</a>
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
                                        <label class="custom-control-label ps-1" for="chkNightMode"><i class="fa fa-moon"></i><span class="ps-2">Night Mode</span></label>
                                    </div>
                                </div>
                                <a class="dropdown-item" href="#" @click="showImportStatusModal = true"><i class="fa fa-calendar-check"></i><span class="ps-2">Import Status</span></a>
                                <a class="dropdown-item" href="/Home/Login"><i class="fa fa-user"></i><span class="ps-2">Log In</span></a>
                                <a class="dropdown-item" href="/Home/SignUp"><i class="fa fa-clipboard"></i><span class="ps-2">Sign Up</span></a>
                            </template>
                        </button-dropdown> 
                    </li>                   
                </ul>
            </div>
        </div>                        
    </nav>           
</template>
<script>
    import axios from 'axios'
    import { setCookie } from '../../js/common';

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
                isDarkTheme: this.isdarktheme
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
                    action = "PlayerDetails"
                }

                location.href = encodeURI('/' + controller + "/" + action + "/" + result.value);
            },           
            // onSearch: function() {
            //     var that = this;
            //     this.searchLoading = true;
                               
            //     axios.get('/Menu/Search', { params: { term: this.searchText } })
            //             .then(res => {
            //                 that.searchResults = res.data.reduce((flat, groupheader) => {
            //                     return flat
            //                         .concat({
            //                             label: groupheader.label,
            //                             value: groupheader.subItems.map(method => method.value),
            //                             isGroupHeader: true,
            //                             disabled: true
            //                         })
            //                         .concat(groupheader.subItems.map(method => ({ label: method.label, value: method.value, category: groupheader.label })))
            //                 }, []);

            //                 if(that.searchResults.length == 0)
            //                 {
            //                     var noResult = { value: "", label: "No results found", category: null, disabled: true };
            //                     that.searchResults.push(noResult);
            //                 }

            //                 that.searchLoading = false;
            //                 return res;
            //             })
            //             .catch(err => { console.error(err); return Promise.reject(err); });
            // },              
            // onSearchSelected: function (result) {
            //     var controller;
            //     var action;

            //     if (result.category == 'Games') {
            //         controller = "Game";
            //         action = "GameDetails"
            //     } else {
            //         controller = "User";
            //         action = "PlayerDetails"
            //     }

            //     location.href = encodeURI('/' + controller + "/" + action + "/" + result.value);
            // },
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






