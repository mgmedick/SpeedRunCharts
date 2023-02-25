<template>
    <form @submit.prevent="submitForm">
        <div>
            <ul>
                <li v-for="errorMessage in errorMessages">{{ errorMessage }}</li>
                <li v-for="error of v$.$errors">{{ error.$message }}</li>
            </ul>
        </div>
        <div class="form-group row no-gutters">
            <label class="col-sm-3 col-form-label">Username</label>
            <div class="col-sm-auto">
                <input type="text" name="Username" class="form-control" autocomplete="off" v-model.lazy="form.Username" @blur="v$.form.Username.$touch">
            </div>
        </div>
        <div class="row no-gutters pt-1">
            <div class="form-group mx-auto">
                <button type="submit" class="btn btn-primary">Reset Password</button>
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
                        <span>To Reset your password click the reset password link in the email we just sent you.</span>
                        <br />
                        <br />
                        <span>If your email has not arrived try these steps:</span>
                        <ul class="pl-4">
                            <li>Wait 30 mins</li>
                            <li>Check your spam folder</li>
                            <li>Try Reset Password again</li>
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
    import { required, helpers } from '@vuelidate/validators';
    const { withAsync } = helpers;

    const asyncActiveUsernameExists = async (value) => {
        if (value === '') return true;

        return await axios.get('/Home/ActiveUsernameExists', { params: { username: value } })
            .then(res => {
                return res.data;
            })
            .catch(err => { console.error(err); return Promise.reject(err); });
    }

    export default {
        name: "ResetPassword",
        setup() {
            return { v$: useVuelidate() }
        },
        data() {
            return {
                form: {
                    Username: ''
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

                axios.post('/Home/ResetPassword', formData)
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
                    Username: {
                        required: helpers.withMessage('Username is required', required),
                        activeUsernameExists: helpers.withMessage('Invalid Username', withAsync(asyncActiveUsernameExists))
                    }
                }
            }
        }
    };
</script>


