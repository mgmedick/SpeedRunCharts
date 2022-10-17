<template>
    <div class="btn-dropdown dropdown" :class="{ show : state }" @click="toggleDropdown">
        <button class="btn dropdown-toggle" :class="btnclasses" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" ref="btn">
            <slot name="text"></slot>
        </button>
        <div class="dropdown-menu" :class="[listclasses]" aria-labelledby="dropdownMenuButton" ref="list" :style="[ state ? { display:'block' } : { display:'none' } ] ">
            <slot name="options"></slot>
        </div>
    </div>
</template>
<script>
    export default {
        name: "ButtonDropdownVue",
        props: {
            btnclasses: String,
            listclasses: String
        },
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
                    this.state = false;
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






