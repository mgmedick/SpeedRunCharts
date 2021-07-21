const speedRunSummaryVue = {
    template: "#speedrun-summary",
    props: {
        item: Object,
        index: Number
    },
    data() {
        return {
            showModal: false,
            exampleDate: new Date("05/15/2021").toISOString()
        }
    },
    methods: {
        showDetails: function () {
            this.showModal = true;
        }
    }
};



