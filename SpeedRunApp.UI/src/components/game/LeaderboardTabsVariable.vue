﻿<template>
    <div v-for="variable in items" :key="variable.id">
        <div v-if="subcategoryvariablevalueids[variable.name + variableindex]">
            <div class="variablerow row no-gutters pr-1">
                <div class="col tab-list">
                    <ul class="nav nav-pills">
                        <li class="variableValue nav-item py-1 pr-1" v-for="(variableValue, variableValueIndex) in variable.variableValues.filter(va => (!hideempty || va.hasData))" :key="variableValue.id">
                            <a class="nav-link p-2" :class="{ 'active' : subcategoryvariablevalueids[variable.name + variableindex] == variableValue.name }" href="#/" data-type="variableValue" :data-variable="variable.name + variableindex" :data-value="variableValue.name" data-toggle="pill" draggable="false" @click="$emit('ontabclick', $event)">{{ variableValue.name }}</a>
                        </li>
                        <button-dropdown v-show="false" class="more py-1 pr-1" :btnclasses="'btn-secondary'">
                            <template v-slot:text>
                                <span>More...</span>
                            </template>
                            <template v-slot:options>
                                <template v-for="(variableValue, variableValueIndex) in variable.variableValues.filter(va => (!hideempty || va.hasData))" :key="variableValue.id">
                                    <a class="dropdown-item d-none" :class="{ 'active' : subcategoryvariablevalueids[variable.name + variableindex] == variableValue.name }" href="#/" data-type="variableValue" :data-variable="variable.name + variableindex" :data-value="variableValue.name" data-toggle="pill" draggable="false" @click="$emit('ontabclick', $event)">{{ variableValue.name }}</a>
                                </template>
                            </template>
                        </button-dropdown>                  
                    </ul>
                </div>
            </div>
            <div v-for="(variableValue, variableValueIndex) in variable.variableValues" :key="variableValue.id">
                <div v-if="variableValue.subVariables && variableValue.subVariables.length > 0 && subcategoryvariablevalueids[variable.name + variableindex] == variableValue.name">
                    <leaderboard-tabs-variable :items="variableValue.subVariables" :gameid="gameid" :categorytypeid="categorytypeid" :categoryid="categoryid" :levelid="levelid" :subcategoryvariablevalueids="subcategoryvariablevalueids" :speedrunid="speedrunid" :prevdata="(prevdata + ',' + variableValue.id).replace(/(^,)|(,$)/g, '')" :variableindex="variableindex + 1" :hideempty="hideempty" :showcharts="showcharts" :showalldata="showalldata" :showmilliseconds="showmilliseconds" :variables="variables" :exporttypes="exporttypes" :title="title" :istimerasc="istimerasc" @ontabclick="$emit('ontabclick', $event)" @onshowchartsclick2="$emit('onshowchartsclick2', $event)" @update:showalldata="$emit('update:showalldata', $event)"></leaderboard-tabs-variable>
                </div>
                <div v-else-if="subcategoryvariablevalueids[variable.name + variableindex] == variableValue.name">
                    <leaderboard-grid :gameid="gameid" :categorytypeid="categorytypeid" :categoryid="categoryid" :levelid="levelid" :variablevalues="(prevdata + ',' + variableValue.id).replace(/(^,)|(,$)/g, '')" :speedrunid="speedrunid" :showcharts="showcharts" :showalldata="showalldata" :showmilliseconds="showmilliseconds" :variables="variables" :exporttypes="exporttypes" :title="title" :istimerasc="istimerasc" @onshowchartsclick1="$emit('onshowchartsclick2', $event)" @update:showalldata="$emit('update:showalldata', $event)"></leaderboard-grid>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    export default {
        name: "LeaderboardTabsVariable",
        emits: ["ontabclick", "update:showalldata", "onshowchartsclick2"],
        props: {
            items: Array,
            gameid: String,
            categorytypeid: String,
            categoryid: String,
            levelid: String,
            subcategoryvariablevalueids: Object,
            speedrunid: String,
            prevdata: String,
            variableindex: Number,
            hideempty: Boolean,
            showalldata: Boolean,          
            showcharts: Boolean,
            showmilliseconds: Boolean, 
            variables: Array,
            exporttypes: Array,
            title: String,
            istimerasc: Boolean
        },
        data: function () {
            return {
                showAllData: this.showalldata
            }
        },        
    };
</script>







