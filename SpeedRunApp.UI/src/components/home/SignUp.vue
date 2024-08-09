<template>
    <form @submit.prevent="submitForm">
        <div>
            <ul>
                <li class="text-danger small font-weight-semibold" v-for="errorMessage in errorMessages">{{ errorMessage }}</li>
            </ul>
        </div>
        <div class="form-group row no-gutters">
            <label class="col-sm-3 col-form-label">Email</label>
            <div class="col">
                <input type="text" name="Email" class="form-control" autocomplete="off" v-model.lazy="form.Email" @blur="v$.form.Email.$touch" style="width:100%;">
                <span class="text-danger small font-weight-semibold" v-for="error of v$.form.Email.$errors">{{ error.$message }}</span>
            </div>
        </div>
        <div class="row no-gutters pt-1">
            <div class="form-group mx-auto">
                <button type="submit" class="btn btn-primary">Sign Up</button>
            </div>
        </div>
        <div>
            <div v-if="loading">
                <div class="d-flex p-3">
                    <div class="mx-auto align-self-center">
                        <i class="fa fa-spinner fa-spin fa-lg"></i>
                    </div>
                </div>
            </div>
            <div v-else-if="showSuccess">
                <div class="container p-3" style="max-width: 400px;">
                    <div class="mx-auto">
                        <span>To Activate your account click the activation link in the email we just sent you.</span>
                        <br />
                        <br />
                        <span>If your email has not arrived try these steps:</span>
                        <ul class="pl-4">
                            <li>Wait 30 mins</li>
                            <li>Check your spam folder</li>
                            <li>Try Sign Up again</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </form>
</template>
<script>
    import { getFormData } from '../../js/common.js';
    import axios from 'axios';
    import useVuelidate from '@vuelidate/core';
    import { required, email, helpers } from '@vuelidate/validators';
    const { withAsync } = helpers;

    const asyncEmailNotExists = async (value) => {
        if (value === '') return true;

        return await axios.get('/Home/EmailNotExists', { params: { email: value } })
            .then(res => {
                return res.data;
            })
            .catch(err => { console.error(err); return Promise.reject(err); });
    }

    export default {
        name: "SignUp",
        setup() {
            return { v$: useVuelidate() }
        },
        data() {
            return {
                form: {
                    Email: ''
                },
                loading: false,
                showSuccess: false,
                errorMessages: []
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

                axios.post('/Home/SignUp', formData)
                    .then((res) => {
                        that.showSuccess = res.data.success;
                        that.errorMessages = res.data.errorMessages;
                        that.loading = false;
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            }
        },
        validations() {
            return {
                form: {
                    Email: {
                        required: helpers.withMessage('Email is required', required),
                        email: helpers.withMessage('Invalid Email', email),
                        emailNotExists: helpers.withMessage('Email already exists', withAsync(asyncEmailNotExists))
                    }
                }
            }
        }
    };
</script>


