<template>
    <div v-for="variable in items" :key="variable.id">
        <div v-if="subcategoryvariablevalues[variable.name + variableindex]">
            <div class="variablerow row mb-2">
                <div class="col tab-list">
                    <ul class="nav nav-underline">
                        <li class="variableValue nav-item" v-for="(variableValue, variableValueIndex) in variable.variableValues.filter(va => (!hideempty || va.hasData))" :key="variableValue.id">
                            <a class="nav-link" :class="{ 'active' : subcategoryvariablevalues[variable.name + variableindex] == variableValue.name }" href="#/" data-type="variableValue" :data-variable="variable.name + variableindex" :data-value="variableValue.name" data-toggle="pill"  @click="$emit('ontabclick', $event)">{{ variableValue.name }}</a>
                        </li>
                        <li class="nav-item dropdown more" v-show="false">
                            <a class="nav-link dropdown-toggle" data-bs-toggle="dropdown" href="#" role="button" aria-expanded="false">More...</a>                        
                            <ul class="dropdown-menu dropdown-menu-end dropdown-menu-lg-start">
                                <li v-for="(variableValue, variableValueIndex) in variable.variableValues.filter(va => (!hideempty || va.hasData))" :key="variableValue.id" class="d-none">
                                    <a class="dropdown-item" :class="{ 'active' : subcategoryvariablevalues[variable.name + variableindex] == variableValue.name }" href="#/" data-type="variableValue" :data-variable="variable.name + variableindex" :data-value="variableValue.name" data-toggle="pill"  @click="$emit('ontabclick', $event)">{{ variableValue.name }}</a>
                                </li>
                            </ul>
                        </li>                          
                    </ul>
                </div>
            </div>
            <div v-for="(variableValue, variableValueIndex) in variable.variableValues" :key="variableValue.id">
                <div v-if="variableValue.subVariables && variableValue.subVariables.length > 0 && subcategoryvariablevalues[variable.name + variableindex] == variableValue.name">
                    <leaderboard-tabs-variable :items="variableValue.subVariables" :gameid="gameid" :categorytypeid="categorytypeid" :categoryid="categoryid" :levelid="levelid" :subcategoryvariablevalues="subcategoryvariablevalues" :speedruncode="speedruncode" :prevdata="(prevdata + ',' + variableValue.id).replace(/(^,)|(,$)/g, '')" :variableindex="variableindex + 1" :hideempty="hideempty" :showcharts="showcharts" :showalldata="showalldata" :showmilliseconds="showmilliseconds" :variables="variables" :exporttypes="exporttypes" :title="title" :istimerasc="istimerasc" @ontabclick="$emit('ontabclick', $event)" @onshowchartsclick2="$emit('onshowchartsclick2', $event)" @update:showalldata="$emit('update:showalldata', $event)"></leaderboard-tabs-variable>
                </div>
                <div v-else-if="subcategoryvariablevalues[variable.name + variableindex] == variableValue.name">
                    <leaderboard-charts :showcharts="showcharts" :showmilliseconds="showmilliseconds" :gameid="gameid" :categorytypeid="categorytypeid" :categoryid="categoryid" :levelid="levelid" :variablevalues="(prevdata + ',' + variableValue.id).replace(/(^,)|(,$)/g, '')" :title="title" :istimerasc="istimerasc" @onshowchartsclick="$emit('onshowchartsclick2', $event)"></leaderboard-charts>
                    <leaderboard-grid :gameid="gameid" :categorytypeid="categorytypeid" :categoryid="categoryid" :levelid="levelid" :variablevalues="(prevdata + ',' + variableValue.id).replace(/(^,)|(,$)/g, '')" :speedruncode="speedruncode" :showalldata="showalldata" :showmilliseconds="showmilliseconds" :variables="variables" :exporttypes="exporttypes" :title="title" :istimerasc="istimerasc" @update:showalldata="$emit('update:showalldata', $event)"></leaderboard-grid>
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







