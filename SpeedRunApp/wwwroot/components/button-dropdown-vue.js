const buttonDropdownVue = {
    template: '#button-dropdown',
    data() {
        return {
            state: false
        }
    },
    methods: {
        toggleDropdown(e) {
            this.state = !this.state
        },
        close(e) {
            if (!this.$el.contains(e.target)) {
                this.state = false
            }
        }
    },
    mounted() {
        document.addEventListener('click', this.close)
    },
    beforeDestroy() {
        document.removeEventListener('click', this.close)
    }
};



