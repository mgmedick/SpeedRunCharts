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

import gameDetails from './game/GameDetails.vue';
import gameDetailsTab from './game/GameDetailsTab.vue';
import gameSpeedRunGridTab from './game/GameSpeedRunGridTab.vue';
import gameSpeedRunGridTabVariable from './game/GameSpeedRunGridTabVariable.vue';
import gameSpeedRunGrid from './game/GameSpeedRunGrid.vue';
import gameSpeedRunGridCharts from './game/GameSpeedRunGridCharts.vue';
import gameWorldRecordGridTab from './game/GameWorldRecordGridTab.vue';
import gameWorldRecordGrid from './game/GameWorldRecordGrid.vue';

import userDetails from './user/UserDetails.vue';
import userDetailsTab from './user/UserDetailsTab.vue';
import userSpeedRunGridTab from './user/UserSpeedRunGridTab.vue';
import userSpeedRunGridTabVariable from './user/UserSpeedRunGridTabVariable.vue';
import userSpeedRunGrid from './user/UserSpeedRunGrid.vue';
import userSpeedRunGridCharts from './user/UserSpeedRunGridCharts.vue';
import userPersonalBestGridTab from './user/UserPersonalBestGridTab.vue';
import userPersonalBestGrid from './user/UserPersonalBestGrid.vue';

import gameSpeedRunGridPercentileChart from './charts/GameSpeedRunGridPercentileChart.vue';
import gameSpeedRunGridTopChart from './charts/GameSpeedRunGridTopChart.vue';
import gameSpeedRunGridWorldRecordChart from './charts/GameSpeedRunGridWorldRecordChart.vue';
import userSpeedRunGridPercentileChart from './charts/UserSpeedRunGridPercentileChart.vue';
import userSpeedRunGridTopChart from './charts/UserSpeedRunGridTopChart.vue';
import userSpeedRunGridPersonalBestChart from './charts/UserSpeedRunGridPersonalBestChart.vue';

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
        app.component("gamedetails-tab", gameDetailsTab);
        app.component("game-speedrun-grid-tab", gameSpeedRunGridTab);
        app.component("game-speedrun-grid-tab-variable", gameSpeedRunGridTabVariable);
        app.component("game-speedrun-grid", gameSpeedRunGrid);
        app.component("game-speedrun-grid-charts", gameSpeedRunGridCharts);
        app.component("game-worldrecord-grid-tab", gameWorldRecordGridTab);
        app.component("game-worldrecord-grid", gameWorldRecordGrid);

        app.component('userdetails', userDetails); 
        app.component("userdetails-tab", userDetailsTab);
        app.component("user-speedrun-grid-tab", userSpeedRunGridTab);
        app.component("user-speedrun-grid-tab-variable", userSpeedRunGridTabVariable);
        app.component("user-speedrun-grid", userSpeedRunGrid);

        app.component("user-speedrun-grid-charts", userSpeedRunGridCharts);
        app.component("user-personalbest-grid-tab", userPersonalBestGridTab);
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
        app.component("useraccount", userAccount);
        app.component("import-status", importStatus);

        app.mount('#vue-app');
        app.provide('app', 'Vue3');
    }
}




