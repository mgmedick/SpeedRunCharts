<template>
    <transition name="cmv-modal-fade">
      <div class="cmv-modal" style="z-index: 1000;">
        <div class="cmv-modal-backdrop"></div>
        <div class="cmv-modal-container" role="dialog" aria-expanded="true" aria-modal="true" tabindex="-1">
            <div :class="'cmv-modal-dialog cmv-modal-dialog-centered ' + contentclass">
                <div class="cmv-modal-content">
                    <div class="cmv-modal-header p-0">
                        <slot name="header">
                            <div class="cmv-modal-title col-sm-10 align-self-center">
                                <slot name="title"></slot>
                            </div>
                        </slot>
                        <button type="button" class="cmv-close m-0" data-dismiss="modal" aria-label="Close" @click="close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div :class="'cmv-modal-body ' + bodyclass">
                        <slot v-bind:params="params"></slot>
                    </div>
                </div>
            </div>
        </div>
      </div>
    </transition>
  </template>  
<script>
    export default {
        name: "ModalVue",
        props: {
            contentclass: String,
            bodyclass: String
        },           
        mounted() {
            document.body.style.overflow = "hidden";
        },
        destroyed() {
            document.body.style.removeProperty('overflow')
        },              
        methods: {
            close() {
                this.$emit('close');
                document.body.style.removeProperty('overflow')
            },
        },        
    };
</script>


