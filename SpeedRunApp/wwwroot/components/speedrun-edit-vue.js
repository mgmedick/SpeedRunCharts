const speedRunEditVue = {
    template: "#speedrun-edit",
    props: {
        gameid: Number,
        speedrunid: Number,
        readonly: Boolean
    },
    data: function () {
        return {
            item: {},
            loading: false
        }
    },
    computed: {
        playerids: function () {
            return this.item.speedRunVM.players.map(i => i.id.toString());
        }
    },
    created: function() {
        this.loadData().then(i => { this.init(); });       
    },
    methods: {
        loadData: function () {
            var that = this;
            this.loading = true;

            var prms = axios.get('../SpeedRun/GetEditSpeedRun', { params: { gameID: this.gameid, speedRunID: this.speedrunid } })
                            .then(res => {
                                that.item = res.data;
                                that.loading = false;

                                return res;
                            })
                            .catch(err => { console.error(err); return Promise.reject(err); });

            return prms;
        },
        init: function () {
            if (this.readonly) {
                Array.from(this.$el.querySelectorAll('#divSpeedRunEdit input[type=text], input[type=radio], select')).forEach((el) => el.disabled = true);
            }
        },
        save: function () { }
    }
};



