<template>
    <div class="mx-auto">
        <h2 class="text-center mb-3">Change Password</h2>
        <div class="mx-auto" style="max-width:400px;">
            <form v-if="islinkvalid" @submit.prevent="submitForm">         
                <div class="mb-2">
                    <label for="txtPassword" class="form-label">New Password</label>
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
                    <label for="txtConfirmPassword" class="form-label">Confirm New Password</label>
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
                    <button type="submit" class="btn btn-primary">Change Password</button>
                </div>            
            </form>
            <div v-else class="text-center">
                <div class="m-3">
                    <font-awesome-icon icon="fa-solid fa-hourglass-end" size="2xl" />
                </div>
                <div>
                    <span>Reset Password link has expired, please try again.</span>
                    <div>
                        <a href="/ResetPassword/">Reset Password</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import { getFormData, successToast, errorToast } from '../../js/common.js';
    import axios from 'axios';
    import useVuelidate from '@vuelidate/core';
    import { required, sameAs, helpers } from '@vuelidate/validators'; 
    
    const { withAsync } = helpers;
    const passwordFormat = helpers.regex(/^(?=.*[A-Za-z])(?=.*\d)[._()-\/#&!$@+%^=[{\]};:>|?\w\s]{8,30}$/)

    export default {
        name: "ChangePassword",
        setup() {
            return { v$: useVuelidate() }
        },
        props: {
            islinkvalid: Boolean,
            email: String,
            emailtoken: String
        },
        data() {
            return {
                form: {
                    Password: '',
                    ConfirmPassword: '',
                    Email: this.email,
                    EmailToken: this.emailtoken
                },
                isShowPassword: false,
                isShowConfirmPassword: false,
                showResetModal: false,
                showSuccess: false
            }
        },
        computed: {
        },
        mounted: function () {
        },
        methods: {
            async submitForm() {
                const isValid = await this.v$.$validate()
                if (!isValid) return

                var that = this;
                var formData = getFormData(this.form);

                axios.post('/Home/ChangePassword', formData)
                    .then((res) => {
                        if (res.data.success) {
                            successToast("Password has been reset");
                        } else {
                            res.data.errorMessages.forEach(errorMsg => {
                                errorToast(errorMsg);                           
                            });     
                        }
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            },
            async passwordNotMatches(value) {  
                if (value === '') {
                    return true; 
                };

                return await axios.get('/Home/PasswordNotMatches', { params: { password: value, email: this.form.Email } })
                    .then(res => {
                        return res.data;
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            }                                                    
        },
        validations() {
            return {
                form: {
                    Password: {
                        required: helpers.withMessage('Password is required', required),
                        passwordFormat: helpers.withMessage('Must be between 8 - 30 characters with at least 1 number and letter', passwordFormat),
                        passwordNotMatches: helpers.withMessage('Password must differ from previous password', withAsync(this.passwordNotMatches))

                    },
                    ConfirmPassword: {
                        sameAsPassword: helpers.withMessage('Must match Password', sameAs(this.form.Password))
                    }
                }
            }
        }
    };
</script>


