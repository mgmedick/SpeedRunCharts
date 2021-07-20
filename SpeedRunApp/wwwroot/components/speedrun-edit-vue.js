const speedRunEditVue = {
    template: "#speedrun-edit",
    props: {
        gameid: Number,
        speedrunid: Number
    },
    data: function () {
        return {
            item: {},
            loading: false
        }
    },
    computed: {
        playerids: function () {
            return this.item.speedRunVM.players.map(i => i.id);
        }
    },
    created() {
        this.loadData();
        //document.querySelector('#.user-search').select2({
        //    dropdownAutoWidth: true,
        //    width: "300px",
        //    dropdownParent: "#editModal",
        //    minimumInputLength: 3,
        //    ajax: {
        //        url: '../SpeedRun/SearchUsers',
        //        dataType: 'json',
        //        processResults: function (data, params) {
        //            var results = $(data).map(function () { return { id: this.value, text: this.label } }).get();
        //            return { results: results, more: false };
        //        }
        //    }
        //});

    },
    methods: {
        loadData: function () {
            var that = this;
            this.loading = true;

            axios.get('../SpeedRun/GetEditSpeedRun', { params: { gameID: this.gameid, speedRunID: this.speedrunid, readonly: true } })
                .then(res => {
                    that.item = res.data;
                    that.loading = false;
                })
                .catch(err => { console.error(err); return Promise.reject(err); });
        }
    }
};



