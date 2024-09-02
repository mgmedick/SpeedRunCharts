<template>
    <div class="dropdown">
        <div>
            <input type="search" class="form-control" :value="model" @input="model = $event.target.value" @click="onClick" @focus="onFocus" @keydown.down="onArrowDown" @keydown.up="onArrowUp" @keydown.enter="onEnter($event)" :placeholder="placeholder" />
        </div>
        <div v-if="isimgresults" class="container p-0">           
            <div class="row g-3 mt-3">
                <div class="col-lg-2 col-md-3 col-4 d-none">
                    <div class="position-relative default-image-container" role="button">               
                        <svg :width="imgWidth" :height="imgHeight" class="img-fluid default-image">
                            <rect :width="imgWidth" :height="imgHeight" style="fill: none;" />
                        </svg>                        
                    </div>
                </div>
                <div v-if="results.length > 0" v-for="(result, i) in results" class="col-lg-2 col-md-3 col-4" :class="{ 'highlighted': i === arrowCounter }">
                    <div @click="onSearchSelected(result)" role="button" class="position-relative image-container rounded d-flex" style="overflow: hidden; background: linear-gradient(45deg,#dbdde3,#fff);">
                        <img v-if="result.imagePath" :src="result.imagePath" class="img-fluid align-self-center" :style="[ result.imagePath?.indexOf('nocover.png') > -1 ? { opacity:'0.5' } : null ]" alt="Responsive image">
                    </div>
                    <div class="text-xs">
                        <span> {{ result.label }}</span>
                        <div v-if="result.labelSecondary" class="text-muted">
                            <span>{{ result.labelSecondary }}</span>
                        </div>
                    </div>
                </div>
                <div v-else-if="model && model.length >= minlength && !loading && results.length == 0">
                    <span>No results found</span>
                </div>
            </div>
        </div>
        <ul v-else class="dropdown-menu" style="width: 100%;" :style="[ isOpen ? { display:'block' } : { display:'none' } ]">
            <li v-if="results.length > 0" v-for="(result, i) in results" :key="i" class="dropdown-item" :class="{ 'dropdown-header' : result.isGroupHeader, 'highlighted': i === arrowCounter }" @click="onSearchSelected(result)" @mouseover="arrowCounter = i">
                <span>{{ result.label }}</span>
            </li>
            <div v-else-if="model && model.length >= minlength && !loading && results.length == 0">
                <span>No results found</span>
            </div>            
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
            minlength: {
                type: Number,
                default: 0
            }, 
            isimgresults: Boolean,
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
                throttleDelay: 300,
                imgWidth: 207,
                imgHeight: 276
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
                    if (val) {
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
                    } else {
                        that.results = [];
                    }
                });
            },
            loading: function (val, oldVal) {
                var that = this;

                if (this.isimgresults && !val && val != oldVal) {
                    this.$nextTick(function() {
                        that.resizeColumns();
                    });
                }
            }     
        },                    
        mounted() {
            var that = this;

            if (!this.isasync) {
                this.results = this.options;
            }       
            
            if (this.isimgresults) {
                window.addEventListener('resize', this.onResize);
            }

            document.addEventListener('click', this.handleClickOutside);                       
        },
        destroyed() {
            window.removeEventListener('resize', this.onResize);     
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
                    this.$emit('selected', result);
                }
            },
            onResize: function() {
                var that = this;
                if (that.width != document.documentElement.clientWidth || that.height != document.documentElement.clientHeight) {     
                    that.width = document.documentElement.clientWidth;
                    that.height = document.documentElement.clientHeight;   

                    that.$nextTick(function() {
                        that.resizeColumns();
                    });
                }
            },                       
            filterResults() {
                var that = this;

                this.results = this.options.filter((option) => {
                    return option.label.toLowerCase().indexOf(that.model.toLowerCase()) > -1;
                });           
            },                                    
            handleClickOutside(event) {
                if (!(this.$el == event.target || this.$el.contains(event.target))) {
                    this.isOpen = false;
                    this.isFocus = false;
                    this.arrowCounter = -1;
                }
            },
            clear() {
                this.model = "";
                this.results = [];
            },
            resizeColumns() {
                var that = this;
                document.querySelector('.default-image-container').parentElement.classList.remove('d-none');

                var defaultheight = document.querySelector('.default-image-container .default-image')?.clientHeight;
                if (defaultheight > 0) {
                    document.querySelectorAll('.image-container').forEach(item => {
                        item.style.height = defaultheight + 'px';
                    });
                }

                var defaultwidth = document.querySelector('.default-image-container .default-image')?.clientWidth;
                if (defaultwidth > 0) {
                    document.querySelectorAll('.image-container').forEach(item => {
                        item.style.width = defaultwidth + 'px';
                    });
                }

                if (defaultheight > 0 && defaultwidth > 0) {
                    document.querySelector('.default-image-container').parentElement.classList.add('d-none');
                }
            }              
        }
    };
</script>





