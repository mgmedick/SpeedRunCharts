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
import gameTabs from './game/GameTabs.vue';
import leaderboardGrid from './game/LeaderboardGrid.vue';
import leaderboardTabs from './game/LeaderboardTabs.vue';
import leaderboardTabsVariable from './game/LeaderboardTabsVariable.vue';
import leaderboardChartContainer from './game/LeaderboardChartContainer.vue';
import worldRecordTabs from './game/WorldRecordTabs.vue';
import worldRecordGrid from './game/WorldRecordGrid.vue';

import userDetails from './user/UserDetails.vue';
import userTabs from './user/UserTabs.vue';
import userSpeedRunTabs from './user/UserSpeedRunTabs.vue';
import userSpeedRunTabsVariable from './user/UserSpeedRunTabsVariable.vue';
import userSpeedRunGrid from './user/UserSpeedRunGrid.vue';
import userSpeedRunChartContainer from './user/UserSpeedRunChartContainer.vue';
import personalBestGrid from './user/PersonalBestGrid.vue';
import personalBestTabs from './user/PersonalBestTabs.vue';

import leaderboardGridPercentileChart from './charts/LeaderboardGridPercentileChart.vue';
import leaderboardGridTopChart from './charts/LeaderboardGridTopChart.vue';
import leaderboardGridWorldRecordChart from './charts/LeaderboardGridWorldRecordChart.vue';
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
        app.component("game-tabs", gameTabs);
        app.component("leaderboard-tabs", leaderboardTabs);
        app.component("leaderboard-tabs-variable", leaderboardTabsVariable);
        app.component("leaderboard-grid", leaderboardGrid);
        app.component("leaderboard-chart-container", leaderboardChartContainer);
        app.component("worldrecord-tabs", worldRecordTabs);
        app.component("worldrecord-grid", worldRecordGrid);

        app.component('userdetails', userDetails); 
        app.component("user-tabs", userTabs);
        app.component("user-speedrun-grid", userSpeedRunGrid);
        app.component("user-speedrun-tabs", userSpeedRunTabs);
        app.component("user-speedrun-tabs-variable", userSpeedRunTabsVariable);
        app.component("user-speedrun-chart-container", userSpeedRunChartContainer);
        app.component("personalbest-grid", personalBestGrid);        
        app.component("personalbest-tabs", personalBestTabs);

        app.component("leaderboard-grid-worldrecord-chart", leaderboardGridWorldRecordChart);
        app.component("leaderboard-grid-percentile-chart", leaderboardGridPercentileChart);
        app.component("leaderboard-grid-top-chart", leaderboardGridTopChart);

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




