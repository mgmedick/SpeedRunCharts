<template>
    <div class="mx-auto">
        <h2 class="text-center mb-3">Create Account</h2>
        <div class="mx-auto" style="max-width:400px;">
            <form v-if="islinkvalid" @submit.prevent="submitForm" autocomplete="off">
                <div class="mb-2">
                    <label for="txtEmail" class="form-label">Email</label>
                    <input id="txtEmail" type="text" class="form-control" v-model.lazy="form.Email" disabled>
                </div>              
                <div class="mb-2">
                    <label for="txtUserName" class="form-label">Username</label>
                    <input id="txtUserName" type="text" class="form-control" autocomplete="off" v-model.lazy="form.Username" @blur="v$.form.Username.$touch" aria-describedby="spnUserNameErrors">
                    <div>
                        <div id="spnUserNameErrors" class="form-text text-danger" v-for="error of v$.form.Username.$errors">{{ error.$message }}</div>
                    </div>
                </div>
                <div class="mb-2">
                    <label for="txtPassword" class="form-label">Password</label>
                    <div class="d-flex">
                        <input id="txtPassword" :type="isShowPassword ? 'text' : 'password'" class="form-control" autocomplete="off" v-model.lazy="form.Password" @blur="v$.form.Password.$touch" aria-describedby="spnPasswordErrors">
                        <div class="align-self-center text-muted" style="margin-left: -35px;" role="button" @click="isShowPassword = !isShowPassword">
                            <font-awesome-icon v-if="isShowPassword" icon="fa-solid fa-eye-slash"/>
                            <font-awesome-icon v-else icon="fa-solid fa-eye"/>
                        </div>
                    </div>
                    <div>
                        <div id="spnPasswordErrors" class="form-text text-danger" v-for="error of v$.form.Password.$errors">{{ error.$message }}</div>
                    </div>
                </div>
                <div class="mb-3">
                    <label for="txtConfirmPassword" class="form-label">Confirm Password</label>
                    <div class="d-flex">
                        <input id="txtConfirmPassword" :type="isShowConfirmPassword ? 'text' : 'password'" class="form-control" autocomplete="off" v-model.lazy="form.ConfirmPassword" @blur="v$.form.ConfirmPassword.$touch" aria-describedby="spnConfirmPasswordErrors">
                        <div class="align-self-center text-muted" style="margin-left: -35px;" role="button" @click="isShowConfirmPassword = !isShowConfirmPassword">
                            <font-awesome-icon v-if="isShowConfirmPassword" icon="fa-solid fa-eye-slash"/>
                            <font-awesome-icon v-else icon="fa-solid fa-eye"/>
                        </div>
                    </div>                    
                    <div>
                        <div id="spnConfirmPasswordErrors" class="form-text text-danger" v-for="error of v$.form.ConfirmPassword.$errors">{{ error.$message }}</div>
                    </div>
                </div>
                <div class="row g-2 justify-content-center mb-3 mx-auto">
                    <button type="submit" class="btn btn-primary d-flex justify-content-center align-items-center">Submit</button>
                </div> 
                <div ref="loadingmodal" class="modal" tabindex="-1">
                    <div class="modal-dialog modal-dialog-centered justify-content-center" style="color: #fff;">
                        <font-awesome-icon icon="fa-solid fa-spinner" spin size="2xl" />
                    </div>
                </div>                                    
            </form>
            <div v-else class="text-center">
                <div class="m-3">
                    <font-awesome-icon icon="fa-solid fa-hourglass-end" size="2xl" />
                </div>
                <div>
                    <span>Activation link has expired, please try again.</span>
                    <div>
                        <a href="/Signup">Sign Up</a>
                    </div>
                </div>
            </div>   
        </div> 
    </div>
</template>
<script>
    import { getFormData, errorToast } from '../../js/common.js';
    import axios from 'axios';
    import { Modal } from 'bootstrap';
    import useVuelidate from '@vuelidate/core';
    import { required, helpers, sameAs } from '@vuelidate/validators';
    
    const { withAsync } = helpers;
    const usernameFormat = helpers.regex(/^[._()-\/#&$@+\w\s]{3,30}$/)
    const passwordFormat = helpers.regex(/^(?=.*[A-Za-z])(?=.*\d)[._()-\/#&!$@+\w\s]{8,30}$/)

    export default {
        name: "Activate",
        props: {
            islinkvalid: Boolean,
            email: String,
            emailtoken: String
        },
        setup() {
            return { v$: useVuelidate() }
        },
        data() {
            return {
                form: {
                    Email: this.email,
                    EmailToken: this.emailtoken,
                    Username: '',
                    Password: '',
                    ConfirmPassword: ''
                },
                isShowPassword: false,
                isShowConfirmPassword: false
            }
        },
        computed: {
        },
        created: function () {
        },
        mounted: function () {
        },        
        methods: {
            async submitForm() {
                const isValid = await this.v$.$validate()
                if (!isValid) return

                var that = this;
                var formData = getFormData(this.form);
                new Modal(this.$refs.loadingmodal).show();

                axios.post('/Home/Activate', formData)
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
            async usernameNotExists(value) {  
                if (value === '') {
                    return true; 
                };

                return await axios.get('/Home/UsernameNotExists', { params: { username: value } })
                    .then(res => {
                        return Promise.resolve(res.data);
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            }                                                                                                                                                        
        },
        validations() {
            return {
                form: {
                    Username: {
                        required: helpers.withMessage('Username is required', required),
                        usernameFormat: helpers.withMessage('Must be between 3 - 30 characters', usernameFormat),
                        usernameNotExists: helpers.withMessage('Username already exists', withAsync(this.usernameNotExists))
                    },
                    Password: {
                        required: helpers.withMessage('Password is required', required),
                        passwordFormat: helpers.withMessage('Must be between 8 - 30 characters with at least 1 number and letter', passwordFormat)
                    },
                    ConfirmPassword: {
                        sameAsPassword: helpers.withMessage('Must match Password', sameAs(this.form.Password))
                    }
                }
            }
        }
    };
</script>


