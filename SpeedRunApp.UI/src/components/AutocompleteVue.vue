<template>
    <div class="vue-select direction-bottom">
        <div class="vue-select-header">
            <div class="vue-input">
                <input type="text" :value="model" @input="model = $event.target.value" @focus="onFocus" @keydown.down="onArrowDown" @keydown.up="onArrowUp" @keydown.enter="onEnter($event)" :placeholder="placeholder"/>
                <span v-if="loading" class="icon loading"><div></div></span>
                <span v-else class="icon arrow-downward" :class="{ 'active' : isFocus }"></span>
            </div>
        </div>
        <ul v-show="isOpen" class="vue-dropdown">
            <li v-for="(result, i) in results" :key="i" class="vue-dropdown-item" :class="{ 'group' : result.isGroupHeader, 'highlighted': i === arrowCounter }" @click="onSearchSelected(result)">
                <span>{{ result[labelby] }}</span>
            </li>
        </ul>
  </div>    
</template>
<script>
    export default {
        name: "AutocompleteVue",
        props: {
            modelValue: String,
            minlength: Number,
            loading: Boolean,
            placeholder: String,
            labelby: {
                type: String,
                required: true
            },            
            valueby: {
                type: String,
                required: true
            },             
            items: Array
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
            items: function (val, oldVal) {
                this.results = val;
                this.isOpen = true;
            },
            model: function (val, oldVal) {
                this.$emit('update:modelValue', val); 

                this.$nextTick(function() {
                    if (val != oldVal && (!this.minlength || val.length >= this.minlength)) {
                        this.$emit('search'); 
                    }
                });
            }     
        },                    
        mounted() {
            document.addEventListener('click', this.handleClickOutside)
        },
        destroyed() {
            document.removeEventListener('click', this.handleClickOutside)
        },               
        methods: {    
            onFocus() {
                this.isFocus = !this.isFocus;
            },                  
            onArrowDown() {
                if (this.arrowCounter < this.results.length) {
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
                var result = this.results[this.arrowCounter];
                if (result) {
                    this.onSearchSelected(result);
                }
            },    
            onSearchSelected: function (result) {   
                if (result.disabled){
                    return false;
                } else {
                    this.isOpen = false;
                    this.arrowCounter = -1;                     
                    this.model = result[this.valueby];                   
                    this.$emit('selected', result);
                }
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





