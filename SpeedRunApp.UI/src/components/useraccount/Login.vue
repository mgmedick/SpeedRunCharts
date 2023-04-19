<template>
    <form @submit.prevent="submitForm">
        <div>
            <ul>
                <li v-for="errorMessage in errorMessages" class="text-danger small font-weight-semibold">{{ errorMessage }}</li>
            </ul>
        </div>
        <div class="form-group row no-gutters">
            <label class="col-sm-3 col-form-label">Username</label>
            <div class="col-sm-auto">
                <input type="text" class="form-control" autocomplete="off" v-model.lazy="form.Username" @blur="v$.form.Username.$touch">
                <span class="text-danger small font-weight-semibold" v-for="error of v$.form.Username.$errors">{{ error.$message }}</span>
            </div>
        </div>
        <div class="form-group row no-gutters">
            <label class="col-sm-3 col-form-label">Password</label>
            <div class="col-sm-auto">
                <input type="password" class="form-control" autocomplete="off" v-model.lazy="form.Password" @blur="v$.form.Password.$touch">
                <span class="text-danger small font-weight-semibold" v-for="error of v$.form.Password.$errors">{{ error.$message }}</span>
            </div>
        </div>
        <div class="row no-gutters pt-1">
            <div class="form-group mx-auto">
                <button type="submit" class="btn btn-primary">Log In</button>
                <button type="button" class="btn btn-secondary ml-2" @click="$emit('forgotpass', close)">Forgot Password</button>
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
        name: "Login",
        emits: ["forgotpass"],
        setup() {
            return { v$: useVuelidate() }
        },
        data() {
            return {
                form: {
                    Username: '',
                    Password: ''
                },
                errorMessages: []
            }
        },
        methods: {
            async submitForm() {
                const isValid = await this.v$.$validate()
                if (!isValid) return

                var that = this;
                var formData = getFormData(this.form);

                axios.post('/Home/Login', formData)
                    .then((res) => {
                        if (res.data.success) {
                            location.reload();
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
                        activeUsernameExists: helpers.withMessage('Invalid Username', withAsync(asyncActiveUsernameExists))
                    },
                    Password: {
                        required: helpers.withMessage('Password is required', required)
                    }
                }
            }
        }
    };
</script>


