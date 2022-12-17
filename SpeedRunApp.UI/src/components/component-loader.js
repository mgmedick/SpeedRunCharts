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

// import gridTabWorldRecord from './speedrun/grids/GridTabWorldRecord.vue';
// import gridTabWorldRecordVariable from './speedrun/grids/GridTabWorldRecordVariable.vue';
// import worldRecordGrid from './speedrun/grids/WorldRecordGrid.vue';

import gameDetails from './game/GameDetails.vue';
import gameGridTab from './game/GameGridTab.vue';
import gameSpeedRunGridTab from './game/GameSpeedRunGridTab.vue';
import gameSpeedRunVariableGridTab from './game/GameSpeedRunVariableGridTab.vue';
import gameSpeedRunGrid from './game/GameSpeedRunGrid.vue';
import gameSpeedRunGridChartContainer from './game/GameSpeedRunGridChartContainer.vue';
import gameWorldRecordGridTab from './game/GameWorldRecordGridTab.vue';
import gameWorldRecordVariableGridTab from './game/GameWorldRecordVariableGridTab.vue';
import gameWorldRecordGrid from './game/GameWorldRecordGrid.vue';

import userDetails from './user/UserDetails.vue';
import userGridTab from './user/UserGridTab.vue';
import userSpeedRunGridTab from './user/UserSpeedRunGridTab.vue';
import userSpeedRunVariableGridTab from './user/UserSpeedRunVariableGridTab.vue';
import userSpeedRunGrid from './user/UserSpeedRunGrid.vue';
import userSpeedRunGridChartContainer from './user/UserSpeedRunGridChartContainer.vue';
import userPersonalBestGridTab from './user/UserPersonalBestGridTab.vue';
import userPersonalBestVariableGridTab from './user/UserPersonalBestVariableGridTab.vue';
import userPersonalBestGrid from './user/UserPersonalBestGrid.vue';

//import speedRunGridChartContainer from './charts/SpeedRunGridChartContainer.vue';
import gameSpeedRunGridPercentileChart from './charts/GameSpeedRunGridPercentileChart.vue';
import gameSpeedRunGridTopChart from './charts/GameSpeedRunGridTopChart.vue';
import gameSpeedRunGridWorldRecordChart from './charts/GameSpeedRunGridWorldRecordChart.vue';
import userSpeedRunGridPercentileChart from './charts/UserSpeedRunGridPercentileChart.vue';
import userSpeedRunGridTopChart from './charts/UserSpeedRunGridTopChart.vue';
import userSpeedRunGridPersonalBestChart from './charts/UserSpeedRunGridPersonalBestChart.vue';

// import speedRunGridPercentileChart from './charts/SpeedRunGridPercentileChart.vue';
// import speedRunGridTopChart from './charts/SpeedRunGridTopChart.vue';
// import speedRunGridWorldRecordChart from './charts/SpeedRunGridWorldRecordChart.vue';

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
        app.component("game-grid-tab", gameGridTab);
        app.component("game-speedrun-grid-tab", gameSpeedRunGridTab);
        app.component("game-speedrun-variable-grid-tab", gameSpeedRunVariableGridTab);
        app.component("game-speedrun-grid", gameSpeedRunGrid);
        app.component("game-speedrun-grid-chart-container", gameSpeedRunGridChartContainer);
        app.component("game-worldrecord-grid-tab", gameWorldRecordGridTab);
        app.component("game-worldrecord-variable-grid-tab", gameWorldRecordVariableGridTab);
        app.component("game-worldrecord-grid", gameWorldRecordGrid);

        app.component('userdetails', userDetails); 
        app.component("user-grid-tab", userGridTab);
        app.component("user-speedrun-grid-tab", userSpeedRunGridTab);
        app.component("user-speedrun-variable-grid-tab", userSpeedRunVariableGridTab);
        app.component("user-speedrun-grid", userSpeedRunGrid);

        app.component("user-speedrun-grid-chart-container", userSpeedRunGridChartContainer);
        app.component("user-personalbest-grid-tab", userPersonalBestGridTab);
        app.component("user-personalbest-variable-grid-tab", userPersonalBestVariableGridTab);        
        app.component("user-personalbest-grid", userPersonalBestGrid);

        app.component("game-speedrun-grid-worldrecord-chart", gameSpeedRunGridWorldRecordChart);
        app.component("game-speedrun-grid-percentile-chart", gameSpeedRunGridPercentileChart);
        app.component("game-speedrun-grid-top-chart", gameSpeedRunGridTopChart);

        app.component("user-speedrun-grid-personalbest-chart", userSpeedRunGridPersonalBestChart);
        app.component("user-speedrun-grid-percentile-chart", userSpeedRunGridPercentileChart);
        app.component("user-speedrun-grid-top-chart", userSpeedRunGridTopChart);

        app.component("reset-password", resetPassword);
        app.component("change-password", changePassword);
        app.component("login", login);
        app.component("signup", signUp);
        app.component("activate", activate);
        app.component("speedrun-edit", speedRunEdit);
        app.component("speedrun-list", speedRunList);
        app.component("speedrun-summary", speedRunSummary);
        // app.component("grid-tab-worldrecord-variable", gridTabWorldRecordVariable);        
        // app.component("speedrun-grid-chart-container", speedRunGridChartContainer);
        // app.component("speedrun-grid-worldrecord-chart", speedRunGridWorldRecordChart);
        // app.component("speedrun-grid-percentile-chart", speedRunGridPercentileChart);
        // app.component("speedrun-grid-top-chart", speedRunGridTopChart);
        // app.component("grid-tab-worldrecord", gridTabWorldRecord);
        // app.component("worldrecord-grid", worldRecordGrid);
        app.component("useraccount", userAccount);
        app.component("import-status", importStatus);

        app.mount('#vue-app');
        app.provide('app', 'Vue3');
    }
}




