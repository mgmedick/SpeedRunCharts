<template>
    <div class="mx-auto">
        <h2 class="text-center mb-3">Welcome to speedruncharts.com</h2>
        <div class="mx-auto" style="max-width:400px;">
            <form @submit.prevent="onSubmit">
                <div id="divrecaptcha"></div>
                <div class="mb-3">
                    <label for="txtEmail" class="form-label">Email</label>
                    <input id="txtEmail" type="text" class="form-control" autocomplete="off" v-model.lazy="form.Email" @blur="v$.form.Email.$touch" aria-describedby="spnEmailErrors">
                    <div>
                        <div id="spnEmailErrors" class="form-text text-danger" v-for="error of v$.form.Email.$errors">{{ error.$message }}</div>
                    </div>
                </div>
                <div class="row g-2 justify-content-center mb-3 mx-auto">
                    <button id="btnSignUp" type="submit" class="btn btn-primary w-100" :disabled="loading">Sign Up</button>
                    <div class="text-center"><small class="fw-bold">OR</small></div>
                    <div class="fb-login-button" data-width="100%" data-size="large" data-button-type="continue_with" data-layout="rounded" data-auto-logout-link="false" data-use-continue-as="true" data-scope="public_profile,email" onlogin="checkFBLoginState();"></div>
                    <div ref="googleLoginBtn" class="p-0 w-100"></div>
                </div>        
                <div>
                    <div v-if="loading">
                        <div class="d-flex m-3">
                            <div class="mx-auto">
                                <i class="fas fa-spinner fa-spin fa-lg"></i>
                            </div>
                        </div>
                    </div>
                    <div v-else-if="showSuccess">
                        <div class="p-3 bg-dark">
                            <div class="mx-auto">
                                <div><span>To Create your account click the activation link in the email we just sent you.</span></div>
                                <br />
                                <div>Please allow up to 5 minutes for the email to arrive.</div>
                                <br />
                                <div>
                                    <span>If your email has still not arrived try these steps:</span>
                                    <ul class="ps-4">
                                        <li>Check your spam folder</li>
                                        <li>Try Sign Up again</li>
                                        <li>Use Sign In with Google/Facebook</li>
                                    </ul>
                                </div>
                            </div>
                        </div>
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
    import { required, email, helpers } from '@vuelidate/validators';
    
    const { withAsync } = helpers;

    export default {
        name: "SignUp",
        setup() {
            return { v$: useVuelidate() }
        },
        props: {
            signupvm: Object
        },        
        data() {
            return {
                form: {
                    Email: '',
                    Token: ''
                },
                loading: false,
                showSuccess: false,
                errorMessages: [],
                width: document.documentElement.clientWidth
            }
        },
        computed: {
        },
        mounted: function () {
            var that = this;
            this.createGoogleLoginScript().then(function() {
                window.google.accounts.id.initialize({
                    client_id: that.signupvm.gClientID,
                    callback: that.handleGoogleCredentialResponse,
                    auto_select: true
                });

                that.renderGoogleButton();
            });

            this.createFBLoginScript().then(function() {
                window.checkFBLoginState = that.checkFBLoginState;

                window.fbAsyncInit = function() {
                    FB.init({
                        appId: that.signupvm.fbClientID,
                        status: true,
                        cookie: true,
                        xfbml: true,
                        version: that.signupvm.fbApiVer
                    });                                                         
                };
            });  
            
            this.createGoogleRecaptchaScript().then(function() {
                grecaptcha.ready(function() {
                    grecaptcha.render('divrecaptcha', {
                        'sitekey' : that.signupvm.recaptchaKey,
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
                this.loading = true;

                axios.post('/Home/SignUp', formData)
                    .then((res) => {
                        if (res.data.success) {
                            that.showSuccess = res.data.success;
                        } else {
                            res.data.errorMessages.forEach(errorMsg => {
                                errorToast(errorMsg);                           
                            });                           
                        }
                        
                        that.loading = false;
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            },
            checkFBLoginState() {
                var that = this;
                this.loading = true;

                FB.getLoginStatus(function(response) {
                    that.handleFacebookCredentialResponse(response);
                });
            },
            async handleFacebookCredentialResponse(response) {
                var that = this;

                if (response && response.status == 'connected') { 
                        this.loginOrSignUpWithSocial(response.authResponse.accessToken, 2);
                } else {
                    res.data.errorMessages.forEach(errorMsg => {
                        errorToast(errorMsg);                           
                    });  
                    that.loading = false;                       
                }
            },           
            async handleGoogleCredentialResponse(response) {
                this.loginOrSignUpWithSocial(response.credential, 1);
            },            
            async loginOrSignUpWithSocial(accessToken, socialAccountTypeID) {
                var that = this;
                this.loading = true;

                axios.post('/Home/LoginOrSignUpWithSocial', null,{ params: { accessToken: accessToken, socialAccountTypeID: socialAccountTypeID } })
                    .then((res) => {
                        if (res.data.success) {
                            location.href = '/';
                        } else {
                            res.data.errorMessages.forEach(errorMsg => {
                                errorToast(errorMsg);                           
                            });                             
                        }
                        that.loading = false;
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
                var btnwidth = document.getElementById("btnSignUp").offsetWidth;   

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
            async emailNotExists(value) {  
                if (value === '') {
                    return true; 
                };

                return await axios.get('/Home/EmailNotExists', { params: { email: value } })
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
                        email: helpers.withMessage('Email not found', email),
                        emailNotExists: helpers.withMessage('Email already exists', withAsync(this.emailNotExists))
                    }
                }
            }
        }
    };
</script>


