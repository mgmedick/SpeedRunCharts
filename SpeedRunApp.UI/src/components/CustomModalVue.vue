<template>
    <vue-final-modal v-slot="{ params, close }" v-bind="$attrs" classes="modal-container" :content-class="'modal-dialog modal-dialog-centered ' + contentclass">
        <div class="modal-content">
            <div class="modal-header p-0">
                <slot name="header">
                    <div class="modal-title col-sm-10 align-self-center">
                        <slot name="title"></slot>
                    </div>
                </slot>
                <button type="button" class="close m-0" data-dismiss="modal" aria-label="Close" @click="close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <slot v-bind:params="params"></slot>
            </div>
        </div>
    </vue-final-modal>
</template>
<script>
    export default {
        name: "CustomModalVue",
        props: {
            contentclass: String
        }
    };
</script>

