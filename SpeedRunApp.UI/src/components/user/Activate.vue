<template>
    <div>
        <form v-if="islinkvalid" @submit.prevent="submitForm" autocomplete="off">
            <div>
                <ul>
                    <li class="text-danger small font-weight-semibold" v-for="errorMessage in errorMessages">{{ errorMessage }}</li>
                </ul>
            </div>
            <div class="form-group row no-gutters">
                <label class="col-sm-4 col-form-label">Username</label>
                <div class="col-sm-auto">
                    <input type="text" name="Username" class="form-control" autocomplete="off" v-model.lazy="form.Username" @blur="v$.form.Username.$touch" />
                    <span class="text-danger small font-weight-semibold" v-for="error of v$.form.Username.$errors">{{ error.$message }}</span>
                </div>
            </div>
            <div class="form-group row no-gutters">
                <label class="col-sm-4 col-form-label">Password</label>
                <div class="col-sm-auto">
                    <input type="password" class="form-control" autocomplete="new-password" v-model.lazy="form.Password" @blur="v$.form.Password.$touch">
                    <span class="text-danger small font-weight-semibold" v-for="error of v$.form.Password.$errors">{{ error.$message }}</span>
                </div>
            </div>
            <div class="form-group row no-gutters">
                <label class="col-sm-4 col-form-label">Confirm Password</label>
                <div class="col-sm-auto">
                    <input type="password" class="form-control" autocomplete="new-password" v-model.lazy="form.ConfirmPassword" @blur="v$.form.ConfirmPassword.$touch">
                    <span class="text-danger small font-weight-semibold" v-for="error of v$.form.ConfirmPassword.$errors">{{ error.$message }}</span>
                </div>
            </div>
            <div class="row no-gutters pt-1">
                <div class="form-group mx-auto">
                    <button type="submit" class="btn btn-primary">Submit</button>
                </div>
            </div>
        </form>
        <div v-else style="text-align:center;">
            <i class="fa fa-hourglass-end"></i>
            <div>
                Activation link has expired, please <a href="#" @click="showSignUpModal = true">Sign Up</a> to try again.
            </div>
        </div>
        <modal v-if="showSignUpModal" contentclass="cmv-modal-md" @close="showSignUpModal = false">
            <template v-slot:title>
                Sign Up
            </template>
            <div class="container">
                <signup />
            </div>
        </modal>        
    </div>
</template>
<script>
    import { getFormData } from '../../js/common.js';
    import axios from 'axios';
    import useVuelidate from '@vuelidate/core';
    import { required, helpers, sameAs } from '@vuelidate/validators';
    const { withAsync } = helpers;

    const usernameFormat = helpers.regex(/^[._()-\/#&$@+\w\s]{3,30}$/)
    const passwordFormat = helpers.regex(/^(?=.*[A-Za-z])(?=.*\d)[._()-\/#&$@+\w\s]{8,30}$/)

    const asyncUsernameNotExists = async (value) => {  
        if(value === '') { return true; };            

        return await axios.get('/Home/UsernameNotExists', { params: { username: value } })
            .then(res => {
                return Promise.resolve(res.data);
            })
            .catch(err => { console.error(err); return Promise.reject(err); });
    }

    export default {
        name: "Activate",
        props: {
            islinkvalid: Boolean
        },
        setup() {
            return { v$: useVuelidate() }
        },
        data() {
            return {
                form: {
                    Username: '',
                    Password: '',
                    ConfirmPassword: ''
                },
                errorMessages: [],
                showSignUpModal: false
            }
        },
        computed: {
        },
        created: function () {
        },
        methods: {
            async submitForm() {
                const isValid = await this.v$.$validate()
                if (!isValid) return

                var that = this;
                var formData = getFormData(this.form);
                this.loading = true;

                axios.post('/Home/Activate', formData)
                    .then((res) => {
                        if (res.data.success) {
                            location.href = '/';
                        } else {
                            that.errorMessages = res.data.errorMessages;
                        }
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
                        usernameNotExists: helpers.withMessage('Username already exists', withAsync(asyncUsernameNotExists))
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


