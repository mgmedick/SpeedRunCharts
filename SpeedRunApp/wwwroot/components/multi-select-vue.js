const multiSelectVue = {
    template: "#multi-select",
    props: {
        selected: Array,
        width: String,
        options: Array
    },
    data: function () {
        return {
            values: this.selected != null ? this.selected : [],
            choices: null
        };
    },
    watch: {
        options: {
            deep: true,
            handler() {
                this.refreshChoices();
            }
        },
        selected: {
            deep: true,
            handler() {
                this.refreshSelected();
            }
        }
    },
    mounted: function () {

        var items = [];
        var $that = this;

        if (this.options != null) {
            items = this.options.map(function (o) {
                return { value: o.id + '', label: o.name, selected: false, disabled: false };
            });
        }

        this.choices = new Choices(this.$el, {
            choices: items,
            removeItemButton: true,
            searchFloor: 2,
            searchResultLimit: 25,
            searchFields: ['label']
        });

        this.choices.setChoiceByValue(this.values);

        this.$el.addEventListener('change',
            function (event) {
                $that.values = Array.apply(null, event.target.options).map(function (o) { return o.value });
                $that.$emit('items-changed', $that.values);
            });
    },
    methods: {
        refreshChoices: function () {
            var items = [];
            var $that = this;

            if (this.options != null) {
                items = this.options.map(function (o) {
                    return { value: o.id + '', label: o.name, selected: $that.values.indexOf(o.id + '') > -1, disabled: false };
                });
            }

            this.choices.setChoices(items, 'value', 'label', false);
        },
        refreshSelected: function () {
            this.values = this.selected;
            this.choices.setChoiceByValue(this.values);
        },

    }
};



