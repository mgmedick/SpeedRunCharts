<template>
    <div class="dropdown" :class="{ show : state }" @click.stop="toggleDropdown">
        <button class="btn btn-secondary dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
            <slot name="text"></slot>
        </button>
        <ul class="dropdown-menu" :class="{ show : state }" aria-labelledby="dropdownMenuButton">
            <slot name="options"></slot>
        </ul>
    </div>
</template>
<script>
    export default {
        name: "ButtonDropdownVue",
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
</script>



