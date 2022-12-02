<template>
    <div class="vue-select direction-bottom" :data-disabled="disabled.toString()" :aria-disabled="disabled.toString()" >
        <div class="vue-select-header" tabIndex="-1" @click="onClick" @focus="onFocus">
            <ul class="vue-tags">
                <li v-for="(value, i) in model.filter(val => options.some(g => g[valueby] == val))" :key="i" class="vue-tag selected">
                    <slot name="tag" :index="i" :option="options.find(item => item.id == value)" :remove="onRemove">            
                        <span>{{ options.find(item => item.id == value)[labelby] }}</span>
                        <img src="data:image/svg+xml;base64,PHN2ZyBpZD0iZGVsZXRlIiBkYXRhLW5hbWU9ImRlbGV0ZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB2aWV3Qm94PSIwIDAgNTEyIDUxMiI+PHRpdGxlPmRlbGV0ZTwvdGl0bGU+PHBhdGggZD0iTTI1NiwyNEMzODMuOSwyNCw0ODgsMTI4LjEsNDg4LDI1NlMzODMuOSw0ODgsMjU2LDQ4OCwyNC4wNiwzODMuOSwyNC4wNiwyNTYsMTI4LjEsMjQsMjU2LDI0Wk0wLDI1NkMwLDM5Ny4xNiwxMTQuODQsNTEyLDI1Niw1MTJTNTEyLDM5Ny4xNiw1MTIsMjU2LDM5Ny4xNiwwLDI1NiwwLDAsMTE0Ljg0LDAsMjU2WiIgZmlsbD0iIzViNWI1ZiIvPjxwb2x5Z29uIHBvaW50cz0iMzgyIDE3Mi43MiAzMzkuMjkgMTMwLjAxIDI1NiAyMTMuMjkgMTcyLjcyIDEzMC4wMSAxMzAuMDEgMTcyLjcyIDIxMy4yOSAyNTYgMTMwLjAxIDMzOS4yOCAxNzIuNzIgMzgyIDI1NiAyOTguNzEgMzM5LjI5IDM4MS45OSAzODIgMzM5LjI4IDI5OC43MSAyNTYgMzgyIDE3Mi43MiIgZmlsbD0iIzViNWI1ZiIvPjwvc3ZnPg==" alt="delete tag" class="icon delete" @click.stop="onRemove(i)">                    
                    </slot>
                </li>
            </ul>
            <span v-if="loading" class="icon loading"><div></div></span>
            <span v-else class="icon arrow-downward" :class="{ 'active' : isFocus }"></span>            
        </div>
        <ul v-show="isOpen" class="vue-dropdown">
            <li v-for="(option, i) in options" :key="i" class="vue-dropdown-item" :class="{ 'selected' : model.some(g => g == option[valueby]), 'highlighted': i === arrowCounter }" @click="onSelect(option)">
                <span>{{ option[labelby] }}</span>                
            </li>
        </ul>
  </div>    
</template>
<script>
    export default {
        name: "Multiselect",
        props: {
            modelValue: {
                type: Array,
                default: () => []
            },
            options: {
                type: Array,
                default: () => []
            },            
            labelby: {
                type: String,
                required: true
            },            
            valueby: {
                type: String,
                required: true
            },
            minlength: Number,
            placeholder: String,
            loading: Boolean,
            disabled: Boolean
        },
        data() {
            return {
                model: this.modelValue,
                results: [],
                isOpen: false,
                isFocus: false,
                arrowCounter: -1,
            }
        },     
        watch: {
            model: function (val, oldVal) {
                this.$emit('update:modelValue', val);
            }     
        },                    
        mounted() {
            document.addEventListener('click', this.handleClickOutside)
        },
        destroyed() {
            document.removeEventListener('click', this.handleClickOutside)
        },               
        methods: {  
            onClick() {
                if (!this.disabled) {
                    this.isOpen = true;
                }
            },               
            onFocus() {
                if (!this.disabled) {
                    this.isFocus = true;
                }
            },                  
            onArrowDown() {
                if (this.arrowCounter < this.options.length) {
                    this.arrowCounter = this.arrowCounter + 1;
                }
            },
            onArrowUp() {
                if (this.arrowCounter > 0) {
                    this.arrowCounter = this.arrowCounter - 1;
                }
            },
            onEnter(e) {
                e.preventDefault();
                var option = this.options[this.arrowCounter];
                if (option) {
                    this.onSelect(option);
                }
            },    
            onSelect: function (option) {                   
                if (option.disabled) {
                    return false;
                } else {
                    var that = this;
                    this.arrowCounter = -1;

                    var index = this.model.findIndex(g => g == option[that.valueby]);
                    if (index > -1) {
                        this.model.splice(index, 1);
                    } else {
                        this.model.push(option[this.valueby]);                   
                    }
                }
            },         
            onRemove: function (index) {                  
                if (!this.disabled && this.model.length > index) {
                    this.model.splice(index, 1);
                }

                //this.$emit('selected', option);
            },                                  
            handleClickOutside(event) {
                if (!(this.$el == event.target || this.$el.contains(event.target))) {
                    this.isOpen = false;
                    this.isFocus = false;
                    this.arrowCounter = -1;
                }
            },
        }
    };
</script>

