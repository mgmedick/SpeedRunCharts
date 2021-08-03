const { required } = window.validators;

const loginVue = {
    template: "#login",
    data() {
        return {
            form: {
                Username: '',
                Password: '',
            },
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

            axios.post('/SpeedRun/Login', formData)
                .then((res) => {
                    if (res.data.success) {
                        location.reload();
                    } else {
                        that.isError = true;
                        that.errorMessages = res.data.errorMessages;
                    }
                })
                .catch(err => { console.error(err); return Promise.reject(err); });
        }
    },
    validations: {
        Username: {
            required
        },
        Password: {
            required
        }
    }
};





