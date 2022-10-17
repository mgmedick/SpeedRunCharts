<template>
    <transition name="modal-fade">
      <div class="modal-backdrop">
        <div class="modal modal-lg" role="dialog" aria-labelledby="modalTitle" aria-describedby="modalDescription">
            <header class="modal-header" id="modalTitle">
                <slot name="header">
                </slot>
                <button type="button" class="btn-close" @click="close" aria-label="Close modal">&times;</button>
            </header>
            <section class="modal-body" id="modalDescription">
                <slot name="body">
                </slot>
            </section>
            <footer class="modal-footer">
                <slot name="footer">
                </slot>
            </footer>
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
        methods: {
            close() {
                this.$emit('close');
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
    } 
</style>

