import { createApp } from "vue";
import VueTippy from "vue-tippy";

import buttonDropdown from './shared/ButtonDropdown.vue';
import autocomplete from './shared/Autocomplete.vue';
import multiselect from './shared/Multiselect.vue';
import modal from './shared/Modal.vue';

import navbar from './menu/Navbar.vue';
import importStatus from './menu/ImportStatus.vue';

import userSettings from './user/UserSettings.vue';
import signUp from './user/SignUp.vue';
import activate from './user/Activate.vue';
import login from './user/Login.vue';
import resetPassword from './user/ResetPassword.vue';
import changePassword from './user/ChangePassword.vue';

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

import playerDetails from './player/PlayerDetails.vue';
import playerTabs from './player/PlayerTabs.vue';
import playerSpeedRunTabs from './player/PlayerSpeedRunTabs.vue';
import playerSpeedRunGrid from './player/PlayerSpeedRunGrid.vue';
import playerSpeedRunChartContainer from './player/PlayerSpeedRunChartContainer.vue';
import playerChartTabs from './player/PlayerChartTabs.vue';
import playerChartContainer from './player/PlayerChartContainer.vue';

import gameSpeedRunCountDonutChart from './charts/GameSpeedRunCountDonutChart.vue';
import gameSpeedRunCountLineChart from './charts/GameSpeedRunCountLineChart.vue';
import gameSpeedRunCountBarChart from './charts/GameSpeedRunCountBarChart.vue';
import leaderboardPercentileChart from './charts/LeaderboardPercentileChart.vue';
import leaderboardTopChart from './charts/LeaderboardTopChart.vue';
import leaderboardTopLineChart from './charts/LeaderboardTopLineChart.vue';
import leaderboardWorldRecordChart from './charts/LeaderboardWorldRecordChart.vue';
import playerSpeedRunCountDonutChart from './charts/PlayerSpeedRunCountDonutChart.vue';
import playerSpeedRunCountBarChart from './charts/PlayerSpeedRunCountBarChart.vue';
import playerSpeedRunCountLineChart from './charts/PlayerSpeedRunCountLineChart.vue';
import playerSpeedRunPercentileChart from './charts/PlayerSpeedRunPercentileChart.vue';
import playerSpeedRunTopChart from './charts/PlayerSpeedRunTopChart.vue';
import playerSpeedRunPersonalBestChart from './charts/PlayerSpeedRunPersonalBestChart.vue';

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

        app.component('playerdetails', playerDetails); 
        app.component("player-tabs", playerTabs);
        app.component("player-speedrun-grid", playerSpeedRunGrid);
        app.component("player-speedrun-tabs", playerSpeedRunTabs);
        app.component("player-speedrun-chart-container", playerSpeedRunChartContainer);
        app.component("player-chart-tabs", playerChartTabs);
        app.component("player-chart-container", playerChartContainer);
        
        app.component("game-speedrun-count-doughnut-chart", gameSpeedRunCountDonutChart);
        app.component("game-speedrun-count-line-chart", gameSpeedRunCountLineChart);
        app.component("game-speedrun-count-bar-chart", gameSpeedRunCountBarChart);
        app.component("leaderboard-worldrecord-chart", leaderboardWorldRecordChart);
        app.component("leaderboard-percentile-chart", leaderboardPercentileChart);
        app.component("leaderboard-top-chart", leaderboardTopChart);
        app.component("leaderboard-top-line-chart", leaderboardTopLineChart);
   
        app.component("player-speedrun-count-donut-chart", playerSpeedRunCountDonutChart);         
        app.component("player-speedrun-count-bar-chart", playerSpeedRunCountBarChart);        
        app.component("player-speedrun-count-line-chart", playerSpeedRunCountLineChart);         
        app.component("player-speedrun-personalbest-chart", playerSpeedRunPersonalBestChart);
        app.component("player-speedrun-percentile-chart", playerSpeedRunPercentileChart);
        app.component("player-speedrun-top-chart", playerSpeedRunTopChart);

        app.component("reset-password", resetPassword);
        app.component("change-password", changePassword);
        app.component("login", login);
        app.component("signup", signUp);
        app.component("activate", activate);
        app.component("speedrun-edit", speedRunEdit);
        app.component("speedrun-list", speedRunList);
        app.component("speedrun-summary", speedRunSummary);
        app.component("user-settings", userSettings);
        app.component("import-status", importStatus);

        app.mount('#vue-app');
        app.provide('app', 'Vue3');
    }
}




