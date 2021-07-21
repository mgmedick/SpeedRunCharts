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
    template: '<input v-model="dt" name="date-picker" />',
    mounted: function () {
        var that = this;
        var datepicker = new Datepicker(this.$el, {
            buttonClass: 'btn',
            format: this.dtFormat
        });

        this.$el.addEventListener('changeDate', function (e) { that.dateChanged(e); });
    },
    methods: {
        dateChanged: function (e) {
            this.dt = e.target.value;
            this.$emit('dateChanged', this.dt, this.index);
        },
    }
};



