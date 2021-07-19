const speedRunListCategoryVue = {
    template: "#speedrun-list-category",
    data: function () {
        return {
            categoryid: "0"
        }
    },
    methods: {
        onCategoryChange: function (event) {
            Array.from(document.querySelectorAll('.category.active')).forEach((el) => el.classList.remove('active'));
            event.target.parentElement.classList.add("active");
        }
    }
};



