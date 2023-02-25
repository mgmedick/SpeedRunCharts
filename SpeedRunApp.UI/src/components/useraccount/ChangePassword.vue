<template>
    <div>
        <form v-if="islinkvalid" @submit.prevent="submitForm">
            <div>
                <ul>
                    <li v-for="errorMessage in errorMessages">{{ errorMessage }}</li>
                    <li v-for="error of v$.$errors" :key="error.$uid">{{ error.$message }}</li>
                </ul>
            </div>
            <div class="form-group row no-gutters">
                <label class="col-sm-4 col-form-label">Password</label>
                <div class="col-sm-auto">
                    <input type="password" class="form-control" autocomplete="off" v-model.trim="v$.form.Password.$model">
                </div>
            </div>
            <div class="form-group row no-gutters">
                <label class="col-sm-4 col-form-label">Confirm Password</label>
                <div class="col-sm-auto">
                    <input type="password" class="form-control" autocomplete="off" v-model.trim="v$.form.ConfirmPassword.$model">
                </div>
            </div>
            <div class="row no-gutters pt-1">
                <div class="form-group mx-auto">
                    <button type="submit" class="btn btn-primary">Submit</button>
                </div>
            </div>
            <div v-if="showSuccess" style="text-align:center;">
                <i class="fa fa-circle-check"></i>
                <div>
                    Successfully reset password.
                </div>
            </div>
        </form>
        <div v-else style="text-align:center;">
            <i class="fa fa-hourglass-end"></i>
            <div>
                Reset Password link has expired, please <a href="#" @click="showResetModal = true">Reset Password</a> to try again.
            </div>
        </div>
        <modal v-if="showResetModal" contentclass="cmv-modal-md" @close="showResetModal = false">
            <template v-slot:title>
                Reset Password
            </template>
            <div class="container">
                <reset-password />
            </div>
        </modal>
    </div>
</template>
<script>
    import { getFormData } from '../../js/common.js';
    import axios from 'axios';
    import useVuelidate from '@vuelidate/core';
    import { required, sameAs, helpers } from '@vuelidate/validators';
    const { withAsync } = helpers;

    const passwordFormat = helpers.regex(/^(?=.*[A-Za-z])(?=.*\d)[._()-\/#&$@+\w\s]{8,30}$/)

    const asyncPasswordNotMatches = async (value) => {
        if (value === '') return true;

        return await axios.get('/Home/PasswordNotMatches', { params: { password: value } })
            .then(res => {
                return res.data;
            })
            .catch(err => { console.error(err); return Promise.reject(err); });
    }

    export default {
        name: "ChangePassword",
        setup() {
            return { v$: useVuelidate() }
        },
        props: {
            islinkvalid: Boolean
        },
        data() {
            return {
                form: {
                    Password: '',
                    ConfirmPassword: ''
                },
                showResetModal: false,
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

                axios.post('/Home/ChangePassword', formData)
                    .then((res) => {
                        if (res.data.success) {
                            this.showSuccess = true;
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
                    Password: {
                        required: helpers.withMessage('Password is required', required),
                        passwordFormat: helpers.withMessage('Must be between 8 - 30 characters with at least 1 number and letter', passwordFormat),
                        passwordNotMatches: helpers.withMessage('Username already exists', withAsync(asyncPasswordNotMatches))

                    },
                    ConfirmPassword: {
                        sameAsPassword: helpers.withMessage('Must match Password', sameAs(this.form.Password))
                    }
                }
            }
        }
    };
</script>


