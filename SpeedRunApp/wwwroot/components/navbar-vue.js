const navbarVue = {
    template: "#navbar",
    props: {
        isauth: Boolean,
        isdarktheme: Boolean,
        username: String
    },
    data: function () {
        return {
            searchSelected: null,
            searchOptions: [],
            searchLoading: false,
            showLoginModal: false,
            showResetModal: false,
            showDropdown: false
        }
    },
    computed: {
    },
    created: function() {
    },
    methods: {
        onSearchGames: function (e) {
            if (e.target.value) {
                var that = this;
                this.searchLoading = true;

                axios.get('../Menu/Search', { params: { term: e.target.value } })
                    .then(res => {
                        that.searchOptions = res.data.reduce((flat, constructor) => {
                            return flat
                                .concat({
                                    label: constructor.label,
                                    value: constructor.subItems.map(method => method.value),
                                    isConstructor: true
                                })
                                .concat(constructor.subItems.map(method => ({ label: method.label, value: method.value })))
                        }, []);
                        that.searchLoading = false;

                        return res;
                    })
                    .catch(err => { console.error(err); return Promise.reject(err); });
            }
        },
        onSearchSelected: function (option) {
            var controller = "Game";
            var action = "GameDetails";
            var params = "gameID=" + option.value;

            location.href = encodeURI('../' + controller + "/" + action + "?" + params);
        },
        onForgotPassword: function (option) {

        }
    }
};



