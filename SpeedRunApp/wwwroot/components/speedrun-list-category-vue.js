const speedRunListCategoryVue = {
    template: "#speedrun-list-category",
    data: function () {
        return {
            categoryid: "0"
        }
    },
    methods: {
        onCategoryChange: function () {
            this.categoryid = document.querySelectorAll("input.category:checked")[0].value;
        }
    }
};



