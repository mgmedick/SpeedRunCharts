<template>
    <div class="dropdown" :class="{ show : state }" @click="toggleDropdown">
        <button class="btn dropdown-toggle" :class="btnclasses" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" ref="btn">
            <slot name="text"></slot>
        </button>
        <div class="dropdown-menu" :class="[state ? 'show' : '', listclasses]" aria-labelledby="dropdownMenuButton" ref="list">
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
        updated: function(){
            if (this.$refs.list?.querySelectorAll('a.dropdown-item:not(.d-none).active').length > 0) {
                this.$refs.btn?.classList.add('active');         
            } else {
                this.$refs.btn?.classList.remove('active');         
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
<style>
    .dropdown-menu{
        overflow-y: auto;
        max-height: 300px;
    }
</style>



