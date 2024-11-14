<template>
    <div class="mx-auto">
        <h2 class="text-center mb-3">Welcome to speedruncharts.com</h2>
        <div class="mx-auto" style="max-width:400px;">
            <form @submit.prevent="onSubmit">
                <div id="divrecaptcha"></div>
                <div class="mb-2">
                    <label for="txtEmail" class="form-label">Email</label>
                    <input id="txtEmail" type="text" class="form-control" autocomplete="off" v-model.lazy="form.Email" @blur="v$.form.Email.$touch" aria-describedby="spnEmailErrors">
                    <div>
                        <div id="spnEmailErrors" class="form-text text-danger" v-for="error of v$.form.Email.$errors">{{ error.$message }}</div>
                    </div>
                </div>
                <div class="mb-3">
                    <label for="txtPassword" class="form-label">Password</label>
                    <div class="d-flex">
                        <input id="txtPassword" :type="isShowPassword ? 'text' : 'password'" class="form-control" autocomplete="off" v-model.lazy="form.Password" @blur="v$.form.Password.$touch" aria-describedby="spnPasswordErrors">
                        <div class="align-self-center text-muted" style="margin-left: -35px;" role="button" @click="isShowPassword = !isShowPassword">
                            <i v-if="isShowPassword" class="fas a-eye-slash"></i>
                            <i v-else class="fas fa-eye"></i>
                        </div>
                    </div>                     
                    <div>
                        <div id="spnPasswordErrors" class="form-text text-danger" v-for="error of v$.form.Password.$errors">{{ error.$message }}</div>
                    </div>
                    <div>
                        <a :href="'/Home/ResetPassword?email=' + form.Email" class="link-dark small">Forgot your password?</a>
                    </div>
                </div>   
                <div class="row g-2 justify-content-center mx-auto">
                    <button id="btnLogin" type="submit" class="btn btn-primary w-100">Log In</button>
                    <div class="text-center"><small class="fw-bold">OR</small></div>
                    <div class="fb-login-button" data-width="100%" data-size="large" data-button-type="continue_with" data-layout="rounded" data-auto-logout-link="false" data-use-continue-as="true" data-scope="public_profile,email" onlogin="checkFBLoginState();" style="color-scheme: light"></div>
                    <div ref="googleLoginBtn" class="p-0 w-100" style="color-scheme: light"></div>
                </div>
                <div ref="loadingmodal" class="modal" tabindex="-1">
                    <div class="modal-dialog modal-dialog-centered justify-content-center" style="color: #fff;">
                        <i class="fas fa-spinner fa-spin fa-lg"></i>
                    </div>
                </div>                               
            </form>
        </div>
    </div>
</template>
<script>
    import { getFormData, successToast, errorToast } from '../../js/common.js';
    import axios from 'axios';
    import useVuelidate from '@vuelidate/core';
    import { required, helpers } from '@vuelidate/validators';
    import { Modal } from 'bootstrap';
    
    const { withAsync } = helpers;

    export default {
        name: "Login",
        setup() {
            return { v$: useVuelidate() }
        },
        props: {
            loginvm: Object
        },           
        data() {
            return {
                form: {
                    Email: '',
                    Password: '',
                    Token: ''
                },
                isShowPassword: false,
                errorMessages: [],
                width: document.documentElement.clientWidth              
            }
        },
        mounted: function () {
            var that = this;

            this.createGoogleLoginScript().then(function() {
                window.google.accounts.id.initialize({
                    client_id: that.loginvm.gClientID,
                    callback: that.handleGoogleCredentialResponse,
                    auto_select: true
                });

                that.renderGoogleButton();
            });

            this.createFBLoginScript().then(function() {
                window.checkFBLoginState = that.checkFBLoginState;

                window.fbAsyncInit = function() {
                    FB.init({
                        appId: that.loginvm.fbClientID,
                        status: true,
                        cookie: true,
                        xfbml: true,
                        version: that.loginvm.fbApiVer
                    });                                        
                };
            }); 
            
            this.createGoogleRecaptchaScript().then(function() {
                grecaptcha.ready(function() {
                    grecaptcha.render('divrecaptcha', {
                        'sitekey' : that.loginvm.recaptchaKey,
                        'callback' : that.submitForm,
                        'size' : 'invisible'
                    });          
                });      
            });             
        },  
        destroyed() {
        },                     
        methods: {
            async onSubmit() {
                const isValid = await this.v$.$validate();
                                
                if (!isValid) {
                    return;
                } else {
                    grecaptcha.execute();
                }
            },           
            submitForm(token) {
                var that = this;
                this.form.Token = token;
                var formData = getFormData(this.form);
                new Modal(this.$refs.loadingmodal).show();

                axios.post('/Home/Login', formData)
                    .then((res) => {
                        if (res.data.success) {
                            location.href = '/';
                        } else {
                            res.data.errorMessages.forEach(errorMsg => {
                                errorToast(errorMsg);                           
                            }); 
                        }

                        Modal.getInstance(that.$refs.loadingmodal).hide();                                           
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            },
            checkFBLoginState() {
                var that = this;
                new Modal(this.$refs.loadingmodal).show();

                FB.getLoginStatus(function(response) {
                    that.handleFacebookCredentialResponse(response);
                });
            },
            async handleFacebookCredentialResponse(response) {
                var that = this;

                if (response && response.status == 'connected') { 
                        this.loginOrSignUpWithSocial(response.authResponse.accessToken, 2);
                } else {
                    errorToast("Failed to connect");   
                    Modal.getInstance(that.$refs.loadingmodal).hide();                                           
                }
            },            
            async handleGoogleCredentialResponse(response) {
                new Modal(this.$refs.loadingmodal).show();
                this.loginOrSignUpWithSocial(response.credential, 1);
            },
            async loginOrSignUpWithSocial(accessToken, socialAccountTypeID) {
                var that = this;

                axios.post('/Home/LoginOrSignUpWithSocial', null,{ params: { accessToken: accessToken, socialAccountTypeID: socialAccountTypeID } })
                    .then((res) => {
                        if (res.data.success) {
                            location.href = '/';
                        } else {
                            res.data.errorMessages.forEach(errorMsg => {
                                errorToast(errorMsg);                           
                            });                          
                        }

                        Modal.getInstance(that.$refs.loadingmodal).hide();                                           
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            },        
            createFBLoginScript() {
                return new Promise((resolve, reject) => {
                    let scriptHTML = document.createElement('script');
                    scriptHTML.type = 'text/javascript';
                    scriptHTML.async = true;
                    scriptHTML.defer = true;
                    scriptHTML.src = 'https://connect.facebook.net/en_US/sdk.js';
                    document.getElementsByTagName('head')[0].appendChild(scriptHTML);
                    scriptHTML.onload = function () {
                        resolve();
                    }
                });
            },              
            createGoogleLoginScript() {
                return new Promise((resolve, reject) => {
                    let scriptHTML = document.createElement('script');
                    scriptHTML.type = 'text/javascript';
                    scriptHTML.async = true;
                    scriptHTML.defer = true;
                    scriptHTML.src = 'https://accounts.google.com/gsi/client';
                    document.getElementsByTagName('head')[0].appendChild(scriptHTML);
                    scriptHTML.onload = function () {
                        resolve();
                    }
                });
            },     
            createGoogleRecaptchaScript() {
                var that = this;

                return new Promise((resolve, reject) => {
                    let scriptHTML = document.createElement('script');
                    scriptHTML.type = 'text/javascript';
                    scriptHTML.async = true;
                    scriptHTML.defer = true;
                    scriptHTML.src = 'https://www.google.com/recaptcha/api.js?render=explicit';
                    document.getElementsByTagName('head')[0].appendChild(scriptHTML);
                    scriptHTML.onload = function () {
                        resolve();
                    }
                });
            },                           
            renderGoogleButton() {
                var btnwidth = document.getElementById("btnLogin").offsetWidth;   

                const options = {
                    type: 'standard',
                    shape: 'pill',
                    theme: 'outline',
                    text: 'signin_with',
                    size: 'large',
                    logo_alignment: 'left',
                    width: btnwidth
                }

                window.google.accounts.id.renderButton(this.$refs.googleLoginBtn, options);
            },
            async activeEmailExists(value) {  
                if (value === '') {
                    return true; 
                };

                return await axios.get('/Home/ActiveEmailExists', { params: { email: value } })
                    .then(res => {
                        return res.data;
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            }                                                
        },
        validations() {
            return {
                form: {
                    Email: {
                        required: helpers.withMessage('Email is required', required),
                        activeEmailExists: helpers.withMessage('Email not found', withAsync(this.activeEmailExists))
                    },
                    Password: {
                        required: helpers.withMessage('Password is required', required)
                    }
                }
            }
        }
    };
</script>


