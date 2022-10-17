<template>
    <transition name="modal-fade">
      <div class="cmv-modal" style="z-index: 1000;">
        <div class="cmv-modal-backdrop"></div>
        <div class="cmv-modal-container" role="dialog" aria-expanded="true" aria-modal="true" tabindex="-1">
            <div class="cmv-modal-dialog cmv-modal-dialog-centered cmv-modal-lg">
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
                    <div class="cmv-modal-body">
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
            contentclass: String
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
<style scoped>
    .theme-dark {   
        --bs-border-color10: #444;                  
        --bs-text-color2: #fff;  
        --bs-background-color7: #303030;    
    }

    .theme-light {        
        --bs-border-color10: #dee2e6;     
        --bs-text-color2: #000;
        --bs-background-color7: #fff;    
    }

    .cmv-modal {
        top: 0;
        right: 0;
        bottom: 0;
        left: 0;
        position: fixed;
    }

    .cmv-modal-backdrop {
        background-color: rgba(0, 0, 0, 0.5);
        position: absolute;
        top: 0;
        right: 0;
        bottom: 0;
        left: 0; 
    }
    
    .cmv-modal-container {
        position: absolute; 
        top: 0;
        right: 0;
        bottom: 0;
        left: 0;  
        outline: none;          
        overflow-x: hidden;
        overflow-y: auto;                                  
    }

    .cmv-modal-dialog {
        position: relative;
        width: auto;
        margin: 0.5rem;
        pointer-events: none;
    }

    /* .modal.fade .modal-dialog {
        transition: -webkit-transform 0.3s ease-out;
        transition: transform 0.3s ease-out;
        transition: transform 0.3s ease-out, -webkit-transform 0.3s ease-out;
        -webkit-transform: translate(0, -50px);
        transform: translate(0, -50px);
    }

    @media (prefers-reduced-motion: reduce) {
        .modal.fade .modal-dialog {
            transition: none;
        }
    }

    .modal.show .modal-dialog {
        -webkit-transform: none;
        transform: none;
    }

    .modal.modal-static .modal-dialog {
        -webkit-transform: scale(1.02);
        transform: scale(1.02);
    } */

    .cmv-modal-dialog-centered {
        display: -ms-flexbox;
        display: flex;
        -ms-flex-align: center;
        align-items: center;
        min-height: calc(100% - 1rem);
    }

    .cmv-modal-dialog-centered::before {
        display: block;
        height: calc(100vh - 1rem);
        height: -webkit-min-content;
        height: -moz-min-content;
        height: min-content;
        content: "";
    }

    .cmv-modal-content {
        position: relative;
        display: -ms-flexbox;
        display: flex;
        -ms-flex-direction: column;
        flex-direction: column;
        width: 100%;
        pointer-events: auto;
        background-color: #fff;
        background-clip: padding-box;
        background-color: var(--bs-background-color7);        
        border: 1px solid;
        border-radius: 0.3rem;
        border-color: var(--bs-border-color10);        
        outline: 0;
    }

    .cmv-modal-header {
        display: -ms-flexbox;
        display: flex;
        -ms-flex-align: start;
        align-items: flex-start;
        -ms-flex-pack: justify;
        justify-content: space-between;
        padding: 1rem 1rem;
        border-bottom: 1px solid;
        border-bottom-color: var(--bs-border-color10);        
        border-top-left-radius: calc(0.3rem - 1px);
        border-top-right-radius: calc(0.3rem - 1px);
    }

    .cmv-modal-title {
        margin-bottom: 0;
        line-height: 1.5;
    }

    .cmv-modal-header .cmv-close {
        padding: 1rem 1rem;
        margin: -1rem -1rem -1rem auto;
    }   

    .cmv-close {
        float: right;
        font-size: 1.40625rem;
        font-weight: 700;
        line-height: 1;
        color: var(--bs-text-color2);
        text-shadow: none;
        opacity: .5;
    }

    .cmv-close:hover {
        color: var(--bs-text-color2);
        text-decoration: none;
    }

    .cmv-close:not(:disabled):not(.disabled):hover, .cmv-close:not(:disabled):not(.disabled):focus {
        opacity: .75;
    }

    button.cmv-close {
        padding: 0;
        background-color: transparent;
        border: 0;
    }

    .cmv-modal-body {
        position: relative;
        -ms-flex: 1 1 auto;
        flex: 1 1 auto;
        padding: 1rem;
    }

    /* .modal-dialog-centered.modal-dialog-scrollable {
        -ms-flex-direction: column;
        flex-direction: column;
        -ms-flex-pack: center;
        justify-content: center;
        height: 100%;
    }

    .modal-dialog-centered.modal-dialog-scrollable .modal-content {
        max-height: none;
    }

    .modal-dialog-centered.modal-dialog-scrollable::before {
        content: none;
    } */

    @media (min-width: 576px) {
        .cmv-modal-dialog {
            max-width: 500px;
            margin: 1.75rem auto;
        }
        .cmv-modal-dialog-centered {
            min-height: calc(100% - 3.5rem);
        }
        .cmv-modal-dialog-centered::before {
            height: calc(100vh - 3.5rem);
            height: -webkit-min-content;
            height: -moz-min-content;
            height: min-content;
        }
        .cmv-modal-sm {
            max-width: 300px;
        }
    }
    
    @media (min-width: 992px) {
        .cmv-modal-lg,
        .cmv-modal-xl {
            max-width: 800px;
        }
    }    

    /*
    .vfm--fixed {
        position: fixed;
    }
    .vfm--absolute {
        position: absolute;
    }
    .vfm--inset {
        top: 0;
        right: 0;
        bottom: 0;
        left: 0;
    }
    .vfm--overlay {
        background-color: rgba(0, 0, 0, 0.5);
    }
    .vfm--prevent-none {
        pointer-events: none;
    }
    .vfm--prevent-auto {
        pointer-events: auto;
    }
    .vfm--outline-none:focus {
        outline: none;
    }
    .vfm-enter-active,
    .vfm-leave-active {
        transition: opacity 0.2s;
    }
    .vfm-enter-from,
    .vfm-leave-to {
        opacity: 0;
    }

    .vfm--touch-none {
        touch-action: none;
    }
    .vfm--select-none {
        user-select: none;
    }

    .vfm--resize-tr,
    .vfm--resize-br,
    .vfm--resize-bl,
    .vfm--resize-tl {
        width: 12px;
        height: 12px;
        z-index: 10;
    }

    .vfm--resize-t {
        top: -6px;
        left: 0;
        width: 100%;
        height: 12px;
        cursor: ns-resize;
    }

    .vfm--resize-tr {
        top: -6px;
        right: -6px;
        cursor: nesw-resize;
    }
    .vfm--resize-r {
        top: 0;
        right: -6px;
        width: 12px;
        height: 100%;
        cursor: ew-resize;
    }
    .vfm--resize-br {
        bottom: -6px;
        right: -6px;
        cursor: nwse-resize;
    }
    .vfm--resize-b {
        bottom: -6px;
        left: 0;
        width: 100%;
        height: 12px;
        cursor: ns-resize;
    }
    .vfm--resize-bl {
        bottom: -6px;
        left: -6px;
        cursor: nesw-resize;
    }
    .vfm--resize-l {
        top: 0;
        left: -6px;
        width: 12px;
        height: 100%;
        cursor: ew-resize;
    }
    .vfm--resize-tl {
        top: -6px;
        left: -6px;
        cursor: nwse-resize;
    }
    */

    /*
    .modal-backdrop {
        position: absolute;
        top: 0;
        bottom: 0;
        left: 0;
        right: 0;
        background-color: rgba(0, 0, 0, 0.3);
        display: flex;
        justify-content: center;
        align-items: center;
        overflow-x: hidden;
        overflow-y: auto;
    }
    
    .modal { 
        overflow-x: auto;
        display: flex;
        flex-direction: column;
        max-height: 90%;
        min-height: calc(100% - 3.5rem);
        position: relative !important;
        border: 1px solid;
        border-radius: 0.25rem;    
        background-color: var(--bs-background-color7);  
        border-color: var(--bs-border-color10);               
    }
    
    .modal-header,
    .modal-footer {
        padding: 15px;
        display: flex;
    }

    .modal-header {
        position: relative;
        border-bottom: 1px solid;
        border-bottom-color: var(--bs-border-color10);
        justify-content: space-between;
    }

    .modal-footer {
        border-top: 1px solid;
        border-top-color:  var(--bs-border-color10);
        flex-direction: column;
    }

    .modal-body {
        position: relative;
        flex: 1 1 auto;
        padding: 1rem;
    } 

    .btn-close {
        position: absolute;
        top: 0;
        right: 0;
        border: none;
        font-size: 20px;
        font-weight: bold;
        line-height: 1;
        padding: 10px;
        cursor: pointer;
        color: var(--bs-text-color2);
        text-shadow: none;
        background: transparent;
        opacity: 0.5;
    }

    .modal-fade-enter,
    .modal-fade-leave-to {
        opacity: 0;
    }

    .modal-fade-enter-active,
    .modal-fade-leave-active {
        transition: opacity .5s ease;
    }

    .modal-lg, .modal-xl {
        max-width: 800px;
    }    */


    .modal-fade-enter,
    .modal-fade-leave-to {
        opacity: 0;
    }

    .modal-fade-enter-active,
    .modal-fade-leave-active {
        transition: opacity .5s ease;
    }    

</style>

