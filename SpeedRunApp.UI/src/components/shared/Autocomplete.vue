<template>
    <div class="vue-select direction-bottom">
        <div class="vue-select-header">
            <div class="vue-input">
                <input type="text" :value="model" @input="model = $event.target.value" @click="onClick" @focus="onFocus" @keydown.down="onArrowDown" @keydown.up="onArrowUp" @keydown.enter="onEnter($event)" :placeholder="placeholder"/>
                <span v-if="loading" class="icon loading"><div></div></span>
                <span v-else class="icon arrow-downward" :class="{ 'active' : isFocus }"></span>
            </div>
        </div>
        <ul v-show="isOpen" class="vue-dropdown">
            <li v-for="(result, i) in results" :key="i" class="vue-dropdown-item" :class="{ 'group' : result.isGroupHeader, 'highlighted': i === arrowCounter }" @click="onSearchSelected(result)" @mouseover="arrowCounter = i">
                <span>{{ result[labelby] }}</span>
            </li>
        </ul>
  </div>    
</template>
<script>
    export default {
        name: "Autocomplete",
        emits: ["update:modelValue", "search", "selected"],
        props: {
            modelValue: String,
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
            minlength: {
                type: Number,
                default: 0
            }, 
            isasync: Boolean,               
            loading: Boolean,
            placeholder: String
        },
        data() {
            return {
                model: this.modelValue,
                results: [],
                isOpen: false,
                isFocus: false,
                arrowCounter: -1,
                throttleTimer: null,
                throttleDelay: 300                
            }
        },       
        watch: {
            options: function (val, oldVal) {
                this.results = val;
                this.isOpen = true;
            },       
            model: function (val, oldVal) {
                var that = this;
                this.$emit('update:modelValue', val); 

                this.$nextTick(function() {
                    if (val != oldVal) {
                        if (val.length >= that.minlength) {
                            if (that.isasync) {
                                clearTimeout(that.throttleTimer);
                                that.throttleTimer = setTimeout(function () {
                                    that.$emit('search'); 
                                }, that.throttleDelay);
                            } else {
                                that.filterResults();                            
                            }
                        }
                    }
                });
            }     
        },                    
        mounted() {
            if (!this.isasync) {
                this.results = this.options;
            }            
            document.addEventListener('click', this.handleClickOutside)
        },
        destroyed() {
            document.removeEventListener('click', this.handleClickOutside)
        },               
        methods: {    
            onClick() {
                if (!this.isasync) {
                    this.isOpen = true;
                }
            },             
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
            filterResults() {
                var that = this;

                this.results = this.options.filter((option) => {
                    return option[that.labelby].toLowerCase().indexOf(that.model.toLowerCase()) > -1;
                });

                if (this.results.length == 0) {
                    var noResult = { value: "", label: "No results found", category: null, disabled: true };
                    this.results.push(noResult);
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





