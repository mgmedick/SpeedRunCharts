import { createApp } from "vue";
import VueTippy from "vue-tippy";

import buttonDropdown from './shared/ButtonDropdown.vue';
import autocomplete from './shared/Autocomplete.vue';
import multiselect from './shared/Multiselect.vue';
import modal from './shared/Modal.vue';

import navbar from './menu/Navbar.vue';
import importStatus from './menu/ImportStatus.vue';

import userAccount from './useraccount/UserAccount.vue';
import signUp from './useraccount/SignUp.vue';
import activate from './useraccount/Activate.vue';
import login from './useraccount/Login.vue';
import resetPassword from './useraccount/ResetPassword.vue';
import changePassword from './useraccount/ChangePassword.vue';

import speedRunListTab from './speedrun/SpeedRunListTab.vue';
import speedRunList from './speedrun/SpeedRunList.vue';
import speedRunSummary from './speedrun/SpeedRunSummary.vue';
import speedRunEdit from './speedrun/SpeedRunEdit.vue';

import gridTabContainer from './speedrun/grids/GridTabContainer.vue';
import gridTabSpeedRun from './speedrun/grids/GridTabSpeedRun.vue';
import gridTabSpeedRunVariable from './speedrun/grids/GridTabSpeedRunVariable.vue';
import gridTabWorldRecord from './speedrun/grids/GridTabWorldRecord.vue';
import speedRunGrid from './speedrun/grids/SpeedRunGrid.vue';
import worldRecordGrid from './speedrun/grids/WorldRecordGrid.vue';

import gameDetails from './game/GameDetails.vue';
import userDetails from './user/UserDetails.vue';

import speedRunGridChartContainer from './charts/SpeedRunGridChartContainer.vue';
import speedRunGridPercentileChart from './charts/SpeedRunGridPercentileChart.vue';
import speedRunGridTopChart from './charts/SpeedRunGridTopChart.vue';
import speedRunGridWorldRecordChart from './charts/SpeedRunGridWorldRecordChart.vue';

export default {
    loadComponents() {
        const app = createApp({
            components: {
                'speedrun-list-tab': speedRunListTab
            }
        })
        .use(VueTippy, { defaultProps: { allowHTML: true } });
        
        app.component("button-dropdown", buttonDropdown);
        app.component("navbar", navbar);
        app.component('autocomplete', autocomplete);
        app.component('multiselect', multiselect);
        app.component('modal', modal);  
        app.component('gamedetails', gameDetails);   
        app.component('userdetails', userDetails);        
        app.component("reset-password", resetPassword);
        app.component("change-password", changePassword);
        app.component("login", login);
        app.component("signup", signUp);
        app.component("activate", activate);
        app.component("speedrun-edit", speedRunEdit);
        app.component("speedrun-list", speedRunList);
        app.component("speedrun-summary", speedRunSummary);
        app.component("grid-tab-container", gridTabContainer);
        app.component("grid-tab-speedrun", gridTabSpeedRun);
        app.component("grid-tab-speedrun-variable", gridTabSpeedRunVariable);
        app.component("speedrun-grid", speedRunGrid);
        app.component("speedrun-grid-chart-container", speedRunGridChartContainer);
        app.component("speedrun-grid-worldrecord-chart", speedRunGridWorldRecordChart);
        app.component("speedrun-grid-percentile-chart", speedRunGridPercentileChart);
        app.component("speedrun-grid-top-chart", speedRunGridTopChart);
        app.component("grid-tab-worldrecord", gridTabWorldRecord);
        app.component("worldrecord-grid", worldRecordGrid);
        app.component("useraccount", userAccount);
        app.component("import-status", importStatus);

        app.mount('#vue-app');
        app.provide('app', 'Vue3');
    }
}




