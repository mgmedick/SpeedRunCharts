//import { createApp } from "vue/dist/vue.esm-bundler";
import { createApp } from "vue";
import VueTippy from "vue-tippy";

import buttonDropdownVue from './ButtonDropdownVue.vue';
import navbarVue from './NavbarVue.vue';
import autocompleteVue from './AutocompleteVue.vue';
import multiselectVue from './MultiselectVue.vue';
import modalVue from './ModalVue.vue';
import gameDetailsVue from './GameDetailsVue.vue';
import userDetailsVue from './UserDetailsVue.vue';
import resetPasswordVue from './ResetPasswordVue.vue';
import changePasswordVue from './ChangePasswordVue.vue';
import loginVue from './LoginVue.vue';
import signUpVue from './SignUpVue.vue';
import activateVue from './ActivateVue.vue';
import speedRunEditVue from './SpeedRunEditVue.vue';
import speedRunListVue from './SpeedRunListVue.vue';
import speedRunSummaryVue from './SpeedRunSummaryVue.vue';
import speedRunListCategoryVue from './SpeedRunListCategoryVue.vue';
import gridTabContainerVue from './GridTabContainerVue.vue';
import speedRunGridTabVue from './SpeedRunGridTabVue.vue';
import speedRunGridTabVariableVue from './SpeedRunGridTabVariableVue.vue';
import speedRunGridVue from './SpeedRunGridVue.vue';
import speedRunGridChartVue from './SpeedRunGridChartVue.vue';
import worldRecordChartVue from './charts/WorldRecordChartVue.vue';
import speedRunPercentileChartVue from './charts/SpeedRunPercentileChartVue.vue';
import topSpeedRunChartVue from './charts/TopSpeedRunChartVue.vue';
import worldRecordGridVue from './WorldRecordGridVue.vue';
import worldRecordGridTabVue from './WorldRecordGridTabVue.vue';
import userAccountVue from './UserAccountVue.vue';
import importStatusVue from './ImportStatusVue.vue';

export default {
    loadComponents() {
        const app = createApp({
            components: {
                'speedrun-list-category': speedRunListCategoryVue
            }
        })
        .use(VueTippy, { defaultProps: { allowHTML: true } });
        
        app.component("button-dropdown", buttonDropdownVue);
        app.component("navbar", navbarVue);
        app.component('autocomplete', autocompleteVue);
        app.component('multiselect', multiselectVue);
        app.component('modal', modalVue);  
        app.component('gamedetails', gameDetailsVue);   
        app.component('userdetails', userDetailsVue);        
        app.component("reset-password", resetPasswordVue);
        app.component("change-password", changePasswordVue);
        app.component("login", loginVue);
        app.component("signup", signUpVue);
        app.component("activate", activateVue);
        app.component("speedrun-edit", speedRunEditVue);
        app.component("speedrun-list", speedRunListVue);
        app.component("speedrun-summary", speedRunSummaryVue);
        app.component("grid-tab-container", gridTabContainerVue);
        app.component("speedrun-grid-tab", speedRunGridTabVue);
        app.component("speedrun-grid-tab-variable", speedRunGridTabVariableVue);
        app.component("speedrun-grid", speedRunGridVue);
        app.component("speedrun-grid-chart", speedRunGridChartVue);
        app.component("worldrecord-chart", worldRecordChartVue);
        app.component("speedrun-percentile-chart", speedRunPercentileChartVue);
        app.component("top-speedrun-chart", topSpeedRunChartVue);
        app.component("worldrecord-grid-tab", worldRecordGridTabVue);
        app.component("worldrecord-grid", worldRecordGridVue);
        app.component("personalbest-grid-tab", worldRecordGridTabVue);
        app.component("useraccount", userAccountVue);
        app.component("import-status", importStatusVue);

        app.mount('#vue-app');
        app.provide('app', 'Vue3');
    }
}




