import { createApp } from "vue";

import autocomplete from './shared/Autocomplete.vue';
import multiselect from './shared/Multiselect.vue';

import navbar from './menu/Navbar.vue';
import about from './menu/About.vue';

import index from './home/Index.vue';
import signUp from './home/SignUp.vue';
import activate from './home/Activate.vue';
import login from './home/Login.vue';
import resetPassword from './home/ResetPassword.vue';
import changePassword from './home/ChangePassword.vue';
import summaryListTabs from './home/SummaryListTabs.vue';
import summaryList from './home/SummaryList.vue';
import speedRunSummary from './home/SpeedRunSummary.vue';
import speedRunDetails from './home/SpeedRunDetails.vue';

import userSettings from './user/UserSettings.vue';

import gameDetails from './game/GameDetails.vue';
import gameDetailTabs from './game/GameDetailTabs.vue';
import gameSummaryChartTabs from './game/GameSummaryChartTabs.vue';
import gameSummaryCharts from './game/GameSummaryCharts.vue';
import leaderboardGrid from './game/LeaderboardGrid.vue';
import leaderboardTabs from './game/LeaderboardTabs.vue';
import leaderboardTabsVariable from './game/LeaderboardTabsVariable.vue';
import leaderboardCharts from './game/LeaderboardCharts.vue';
import worldRecordTabs from './game/WorldRecordTabs.vue';
import worldRecordGrid from './game/WorldRecordGrid.vue';

import gameSummaryDonutChart from './game/charts/GameSummaryDonutChart.vue';
import gameSummaryLineChart from './game/charts/GameSummaryLineChart.vue';
import gameSummaryBarChart from './game/charts/GameSummaryBarChart.vue';
import leaderboardPercentileChart from './game/charts/LeaderboardPercentileChart.vue';
import leaderboardTopChart from './game/charts/LeaderboardTopChart.vue';
import leaderboardTopLineChart from './game/charts/LeaderboardTopLineChart.vue';
import leaderboardWorldRecordChart from './game/charts/LeaderboardWorldRecordChart.vue';

import playerDetails from './player/PlayerDetails.vue';
import playerDetailTabs from './player/PlayerDetailTabs.vue';
import playerSpeedRunTabs from './player/PlayerSpeedRunTabs.vue';
import playerSpeedRunGrid from './player/PlayerSpeedRunGrid.vue';
import playerSpeedRunCharts from './player/PlayerSpeedRunCharts.vue';
import playerSummaryChartTabs from './player/PlayerSummaryChartTabs.vue';
import playerSummaryCharts from './player/PlayerSummaryCharts.vue';

import playerSummaryDonutChart from './player/charts/PlayerSummaryDonutChart.vue';
import playerSummaryBarChart from './player/charts/PlayerSummaryBarChart.vue';
import playerSummaryLineChart from './player/charts/PlayerSummaryLineChart.vue';
import playerSpeedRunPercentileChart from './player/charts/PlayerSpeedRunPercentileChart.vue';
import playerSpeedRunTopChart from './player/charts/PlayerSpeedRunTopChart.vue';
import playerSpeedRunPersonalBestChart from './player/charts/PlayerSpeedRunPersonalBestChart.vue';

export default {
    loadComponents() {
        const app = createApp({
            // components: {
            //     'summary-list-tabs': summaryListTabs
            // }
        });
        
        app.component("summary-list-tabs", summaryListTabs);
        app.component("navbar", navbar);
        app.component("about", about);
        app.component('autocomplete', autocomplete);
        app.component('multiselect', multiselect);

        app.component('index', index);        
        app.component("activate", activate);   
        app.component("change-password", changePassword);
        app.component("login", login);
        app.component("reset-password", resetPassword);
        app.component("signup", signUp);
        app.component("summary-list", summaryList);
        app.component("speedrun-summary", speedRunSummary);
        app.component("speedrun-details", speedRunDetails);

        app.component("user-settings", userSettings);

        app.component('game-details', gameDetails); 
        app.component("game-detail-tabs", gameDetailTabs);         
        app.component("game-summary-chart-tabs", gameSummaryChartTabs);
        app.component("game-summary-charts", gameSummaryCharts);
        app.component("leaderboard-tabs", leaderboardTabs);
        app.component("leaderboard-tabs-variable", leaderboardTabsVariable);
        app.component("leaderboard-grid", leaderboardGrid);
        app.component("leaderboard-charts", leaderboardCharts);
        app.component("worldrecord-tabs", worldRecordTabs);
        app.component("worldrecord-grid", worldRecordGrid);

        app.component("game-summary-doughnut-chart", gameSummaryDonutChart);
        app.component("game-summary-line-chart", gameSummaryLineChart);
        app.component("game-summary-bar-chart", gameSummaryBarChart);
        app.component("leaderboard-worldrecord-chart", leaderboardWorldRecordChart);
        app.component("leaderboard-percentile-chart", leaderboardPercentileChart);
        app.component("leaderboard-top-chart", leaderboardTopChart);
        app.component("leaderboard-top-line-chart", leaderboardTopLineChart);

        app.component('player-details', playerDetails); 
        app.component("player-detail-tabs", playerDetailTabs);
        app.component("player-speedrun-grid", playerSpeedRunGrid);
        app.component("player-speedrun-tabs", playerSpeedRunTabs);
        app.component("player-speedrun-charts", playerSpeedRunCharts);
        app.component("player-summary-chart-tabs", playerSummaryChartTabs);
        app.component("player-summary-charts", playerSummaryCharts);
        
        app.component("player-summary-donut-chart", playerSummaryDonutChart);         
        app.component("player-summary-bar-chart", playerSummaryBarChart);        
        app.component("player-summary-line-chart", playerSummaryLineChart);         
        app.component("player-speedrun-personalbest-chart", playerSpeedRunPersonalBestChart);
        app.component("player-speedrun-percentile-chart", playerSpeedRunPercentileChart);
        app.component("player-speedrun-top-chart", playerSpeedRunTopChart);
        app.config.globalProperties.getCsrfToken = () => { 
            return document.getElementsByName("__RequestVerificationToken")[0].value; 
        }

        app.mount('#vue-app');
        app.provide('app', 'Vue3');
    }
}




