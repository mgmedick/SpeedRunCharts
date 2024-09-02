<template>
    <div v-for="variable in items" :key="variable.id">
        <div v-if="subcategoryvariablevalues[variable.name + variableindex]">
            <div class="variablerow row no-gutters pe-1">
                <div class="col tab-list">
                    <ul class="nav nav-pills">
                        <li class="variableValue nav-item py-1 pe-1" v-for="(variableValue, variableValueIndex) in variable.variableValues.filter(va => (!hideempty || va.hasData))" :key="variableValue.id">
                            <a class="nav-link p-2" :class="{ 'active' : subcategoryvariablevalues[variable.name + variableindex] == variableValue.name }" href="#/" data-type="variableValue" :data-variable="variable.name + variableindex" :data-value="variableValue.name" data-toggle="pill" draggable="false" @click="$emit('ontabclick', $event)">{{ variableValue.name }}</a>
                        </li>
                        <div class="dropdown more py-1 pe-1" v-show="false">
                            <button class="btn btn-secondary btn-sm dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                <span>More...</span>
                            </button>
                            <ul class="dropdown-menu dropdown-menu-end dropdown-menu-lg-start">
                                <li v-for="(variableValue, variableValueIndex) in variable.variableValues.filter(va => (!hideempty || va.hasData))" :key="variableValue.id" class="d-none">
                                    <a class="dropdown-item" :class="{ 'active' : subcategoryvariablevalues[variable.name + variableindex] == variableValue.name }" href="#/" data-type="variableValue" :data-variable="variable.name + variableindex" :data-value="variableValue.name" data-toggle="pill" draggable="false" @click="$emit('ontabclick', $event)">{{ variableValue.name }}</a>
                                </li>
                            </ul>
                        </div>                          
                    </ul>
                </div>
            </div>
            <div v-for="(variableValue, variableValueIndex) in variable.variableValues" :key="variableValue.id">
                <div v-if="variableValue.subVariables && variableValue.subVariables.length > 0 && subcategoryvariablevalues[variable.name + variableindex] == variableValue.name">
                    <leaderboard-tabs-variable :items="variableValue.subVariables" :gameid="gameid" :categorytypeid="categorytypeid" :categoryid="categoryid" :levelid="levelid" :subcategoryvariablevalues="subcategoryvariablevalues" :speedruncode="speedruncode" :prevdata="(prevdata + ',' + variableValue.id).replace(/(^,)|(,$)/g, '')" :variableindex="variableindex + 1" :hideempty="hideempty" :showcharts="showcharts" :showalldata="showalldata" :showmilliseconds="showmilliseconds" :variables="variables" :exporttypes="exporttypes" :title="title" :istimerasc="istimerasc" @ontabclick="$emit('ontabclick', $event)" @onshowchartsclick2="$emit('onshowchartsclick2', $event)" @update:showalldata="$emit('update:showalldata', $event)"></leaderboard-tabs-variable>
                </div>
                <div v-else-if="subcategoryvariablevalues[variable.name + variableindex] == variableValue.name">
                    <leaderboard-grid :gameid="gameid" :categorytypeid="categorytypeid" :categoryid="categoryid" :levelid="levelid" :variablevalues="(prevdata + ',' + variableValue.id).replace(/(^,)|(,$)/g, '')" :speedruncode="speedruncode" :showcharts="showcharts" :showalldata="showalldata" :showmilliseconds="showmilliseconds" :variables="variables" :exporttypes="exporttypes" :title="title" :istimerasc="istimerasc" @onshowchartsclick1="$emit('onshowchartsclick2', $event)" @update:showalldata="$emit('update:showalldata', $event)"></leaderboard-grid>
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
            subcategoryvariablevalues: Object,
            speedruncode: String,
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







