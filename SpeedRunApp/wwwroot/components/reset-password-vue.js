const resetPasswordVue = {
    template: "#reset-password",
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
        submitForm() {
            var that = this;
            var formData = getFormData(this.form);
            this.loading = true;

            axios.post('/SpeedRun/ResetPassword', formData)
                .then((res) => {
                    that.showSuccess = res.data.success;
                    that.errorMessages = res.data.errorMessages;
                    that.loading = false;
                })
                .catch(err => { console.error(err); return Promise.reject(err); });
        }
    }
};




