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
import gameChartTabs from './game/GameChartTabs.vue';
import gameChartContainer from './game/GameChartContainer.vue';
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
import userSpeedRunGrid from './user/UserSpeedRunGrid.vue';
import userSpeedRunChartContainer from './user/UserSpeedRunChartContainer.vue';
import userChartTabs from './user/UserChartTabs.vue';
import userChartContainer from './user/UserChartContainer.vue';

import gameSpeedRunCountDonutChart from './charts/GameSpeedRunCountDonutChart.vue';
import gameSpeedRunCountLineChart from './charts/GameSpeedRunCountLineChart.vue';
import gameSpeedRunCountBarChart from './charts/GameSpeedRunCountBarChart.vue';
import leaderboardPercentileChart from './charts/LeaderboardPercentileChart.vue';
import leaderboardTopChart from './charts/LeaderboardTopChart.vue';
import leaderboardTopLineChart from './charts/LeaderboardTopLineChart.vue';
import leaderboardWorldRecordChart from './charts/LeaderboardWorldRecordChart.vue';
import userSpeedRunCountDonutChart from './charts/UserSpeedRunCountDonutChart.vue';
import userSpeedRunCountBarChart from './charts/UserSpeedRunCountBarChart.vue';
import userSpeedRunCountLineChart from './charts/UserSpeedRunCountLineChart.vue';
import userSpeedRunPercentileChart from './charts/UserSpeedRunPercentileChart.vue';
import userSpeedRunTopChart from './charts/UserSpeedRunTopChart.vue';
import userSpeedRunPersonalBestChart from './charts/UserSpeedRunPersonalBestChart.vue';

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
        app.component("game-chart-tabs", gameChartTabs);
        app.component("game-chart-container", gameChartContainer);
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
        app.component("user-speedrun-chart-container", userSpeedRunChartContainer);
        app.component("user-chart-tabs", userChartTabs);
        app.component("user-chart-container", userChartContainer);
        
        app.component("game-speedrun-count-doughnut-chart", gameSpeedRunCountDonutChart);
        app.component("game-speedrun-count-line-chart", gameSpeedRunCountLineChart);
        app.component("game-speedrun-count-bar-chart", gameSpeedRunCountBarChart);
        app.component("leaderboard-worldrecord-chart", leaderboardWorldRecordChart);
        app.component("leaderboard-percentile-chart", leaderboardPercentileChart);
        app.component("leaderboard-top-chart", leaderboardTopChart);
        app.component("leaderboard-top-line-chart", leaderboardTopLineChart);
   
        app.component("user-speedrun-count-donut-chart", userSpeedRunCountDonutChart);         
        app.component("user-speedrun-count-bar-chart", userSpeedRunCountBarChart);        
        app.component("user-speedrun-count-line-chart", userSpeedRunCountLineChart);         
        app.component("user-speedrun-personalbest-chart", userSpeedRunPersonalBestChart);
        app.component("user-speedrun-percentile-chart", userSpeedRunPercentileChart);
        app.component("user-speedrun-top-chart", userSpeedRunTopChart);

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




