const speedRunSummaryVue = {
    template: "#speedrun-summary",
    props: {
        item: Object,
        index: Number
    },
    data() {
        return {
            showModal: false
        }
    },
    methods: {
        showDetails: function () {
            this.showModal = true;
        }
    }
};



