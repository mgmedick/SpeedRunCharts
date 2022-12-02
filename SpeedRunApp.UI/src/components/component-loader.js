import { createApp } from "vue";
import VueTippy from "vue-tippy";

import buttonDropdownVue from './shared/ButtonDropdownVue.vue';
import autocompleteVue from './shared/AutocompleteVue.vue';
import multiselectVue from './shared/MultiselectVue.vue';
import modalVue from './shared/ModalVue.vue';

import navbarVue from './menu/NavbarVue.vue';
import importStatusVue from './menu/ImportStatusVue.vue';

import userAccountVue from './useraccount/UserAccountVue.vue';
import signUpVue from './useraccount/SignUpVue.vue';
import activateVue from './useraccount/ActivateVue.vue';
import loginVue from './useraccount/LoginVue.vue';
import resetPasswordVue from './useraccount/ResetPasswordVue.vue';
import changePasswordVue from './useraccount/ChangePasswordVue.vue';

import speedRunEditVue from './speedrun/SpeedRunEditVue.vue';
import speedRunListVue from './speedrun/SpeedRunListVue.vue';
import speedRunSummaryVue from './speedrun/SpeedRunSummaryVue.vue';
import speedRunListCategoryVue from './speedrun/SpeedRunListCategoryVue.vue';
import speedRunGridVue from './speedrun/SpeedRunGridVue.vue';
import speedRunGridChartVue from './speedrun/SpeedRunGridChartVue.vue';
import worldRecordGridVue from './speedrun/WorldRecordGridVue.vue';

import gameDetailsVue from './game/GameDetailsVue.vue';
import gridTabContainerVue from './game/GridTabContainerVue.vue';
import speedRunGridTabVue from './game/SpeedRunGridTabVue.vue';
import speedRunGridTabVariableVue from './game/SpeedRunGridTabVariableVue.vue';
import worldRecordGridTabVue from './game/WorldRecordGridTabVue.vue';

import userDetailsVue from './user/UserDetailsVue.vue';

import worldRecordChartVue from './charts/WorldRecordChartVue.vue';
import speedRunPercentileChartVue from './charts/SpeedRunPercentileChartVue.vue';
import topSpeedRunChartVue from './charts/TopSpeedRunChartVue.vue';

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




