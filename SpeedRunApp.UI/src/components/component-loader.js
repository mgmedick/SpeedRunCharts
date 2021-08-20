import { createApp } from "vue/dist/vue.esm-bundler";
import { vfmPlugin } from 'vue-final-modal';
import vueNextSelect from 'vue-next-select'
//import VueTippy from 'vue-tippy'

import buttonDropdownVue from './ButtonDropdownVue.vue';
import navbarVue from './NavbarVue.vue';
import customModalVue from './CustomModalVue.vue';
import resetPasswordVue from './ResetPasswordVue.vue';
import changePasswordVue from './ChangePasswordVue.vue';
import loginVue from './LoginVue.vue';
import signUpVue from './SignUpVue.vue';
import activateVue from './ActivateVue.vue';
import speedRunEditVue from './SpeedRunEditVue.vue';
import speedRunListVue from './SpeedRunListVue.vue';
import speedRunSummaryVue from './SpeedRunSummaryVue.vue';
import speedRunListCategoryVue from './SpeedRunListCategoryVue.vue';
import speedRunGridTabVue from './SpeedRunGridTabVue.vue';
import speedRunGridTabVariableVue from './SpeedRunGridTabVariableVue.vue';
import speedRunGridVue from './SpeedRunGridVue.vue';

export default {
    loadComponents() {
        const app = createApp({
            components: {
                'speedrun-list-category': speedRunListCategoryVue
            }
        }).use(vfmPlugin);

        app.component("button-dropdown", buttonDropdownVue);
        app.component("navbar", navbarVue);
        app.component('vue-next-select', vueNextSelect);
        app.component('custom-modal', customModalVue);
        app.component("reset-password", resetPasswordVue);
        app.component("change-password", changePasswordVue);
        app.component("login", loginVue);
        app.component("signup", signUpVue);
        app.component("activate", activateVue);
        app.component("speedrun-edit", speedRunEditVue);
        app.component("speedrun-list", speedRunListVue);
        app.component("speedrun-summary", speedRunSummaryVue);
        app.component("speedrun-grid-tab", speedRunGridTabVue);
        app.component("speedrun-grid-tab-variable", speedRunGridTabVariableVue);
        app.component("speedrun-grid", speedRunGridVue);

        app.mount('#vue-app');
        app.provide('app', 'Vue3');
    }
}




