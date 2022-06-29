const datepickerVue = {
    template: '<input type="text" v-model="dt" name="date-picker" />',
    props: {
        date: String,
        format: String,
        container: String
    },
    data: function () {
        return {
            dt: this.date ? this.date : '',
            dtFormat: this.format ? this.format : 'mm/dd/yyyy',
            dtContainer: this.container ? this.container : 'body'
        };
    },
    mounted: function () {
        var that = this;
        var datepicker = new Datepicker(this.$el, {
            buttonClass: 'btn',
            format: that.dtFormat,
            container: that.dtContainer
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



