const datepickerVue = {
    template: "#datepicker",
    props: {
        date: String,
        format: String
    },
    data: function () {
        return {
            dt: this.date ? this.date : '',
            dtFormat: this.format ? this.format : 'mm/dd/yyyy'
        };
    },
    template: '<input v-model="value" name="date-picker"/>',
    mounted: function() {        
        var datepicker = new Datepicker(this.$el, {
            buttonClass: 'btn',
            format: this.format
        });
    },
    methods: {
        dateChanged: function (event) {
            this.dt = event.target.value;
            this.$emit('selected', date);
        },
    }
};



