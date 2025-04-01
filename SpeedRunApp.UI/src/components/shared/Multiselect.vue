<template>
    <div class="dropdown" :data-disabled="disabled.toString()" :aria-disabled="disabled.toString()">
        <div class="form-control" @click="onClick" style="min-height:48px;">
            <span v-for="(value, i) in model.filter(val => options.some(g => g[valueby] == val))" :key="i" class="fs-5">
                <span class="badge text-bg-secondary me-1 fw-normal">{{ options.find(item => item.id == value)[labelby] }}&nbsp;&nbsp;<span class="fas fa-times fa-sm" @click.stop="onRemove(i)" style="cursor:pointer"></span></span>
            </span>          
        </div>
        <ul class="dropdown-menu" style="width: 100%;" :style="[ isOpen ? { display:'block' } : { display:'none' } ]">
            <li v-for="(option, i) in options" :key="i">
                <a href="#/" class="dropdown-item" :class="{ 'active' : model.some(g => g == option[valueby]) }" @click="onSelect(option)">{{ option[labelby] }}</a>                
            </li>
        </ul>
  </div>    
</template>
<script>
    export default {
        name: "Multiselect",
        emits: ["update:modelValue"],
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
            disabled: Boolean
        },
        data() {
            return {
                model: this.modelValue,
                results: [],
                isOpen: false
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
            onSelect: function (option) {                   
                if (option.disabled) {
                    return false;
                } else {
                    var that = this;
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
            },                                  
            handleClickOutside(event) {
                if (!(this.$el == event.target || this.$el.contains(event.target))) {
                    this.isOpen = false;
                }
            },
        }
    };
</script>

