<template>
    <div v-for="variable in items" :key="variable.id">
        <div v-if="subcategoryvariablevalueids[variable.name + variableindex]">
            <div class="variablerow row no-gutters pr-1 pt-1 pb-0 pr-0">
                <div class="col tab-list">
                    <ul class="nav nav-pills">
                        <li class="variableValue nav-item py-1 pr-1" v-for="(variableValue, variableValueIndex) in variable.variableValues.filter(va => (!hideempty || va.hasData))" :key="variableValue.id">
                            <a class="nav-link p-2" :class="{ 'active' : subcategoryvariablevalueids[variable.name + variableindex] == variableValue.name }" href="#/" data-type="variableValue" :data-variable="variable.name + variableindex" :data-value="variableValue.name" data-toggle="pill" @click="$emit('ontabclick', $event)">{{ variableValue.name }}</a>
                        </li>
                        <button-dropdown v-show="false" class="more py-1 pr-1" :btnclasses="'btn-secondary'">
                            <template v-slot:text>
                                <span>More...</span>
                            </template>
                            <template v-slot:options>
                                <template v-for="(variableValue, variableValueIndex) in variable.variableValues.filter(va => (!hideempty || va.hasData))" :key="variableValue.id">
                                    <a class="dropdown-item d-none" :class="{ 'active' : subcategoryvariablevalueids[variable.name + variableindex] == variableValue.name }" href="#/" data-type="variableValue" :data-variable="variable.name + variableindex" :data-value="variableValue.name" data-toggle="pill" @click="$emit('ontabclick', $event)">{{ variableValue.name }}</a>
                                </template>
                            </template>
                        </button-dropdown>                  
                    </ul>
                </div>
            </div>
            <div v-for="(variableValue, variableValueIndex) in variable.variableValues" :key="variableValue.id">
                <div v-if="variableValue.subVariables.length > 0 && subcategoryvariablevalueids[variable.name + variableindex] == variableValue.name">
                    <speed-run-grid-tab-variable :items="variableValue.subVariables" :gameid="gameid" :categorytypeid="categorytypeid" :categoryid="categoryid" :levelid="levelid" :subcategoryvariablevalueids="subcategoryvariablevalueids" :userid="userid" :prevdata="(prevdata + ',' + variableValue.id).replace(/(^,)|(,$)/g, '')" :variableindex="variableindex + 1" :hideempty="hideempty" :showalldata="showalldata" @ontabclick="$emit('ontabclick', $event)" @onhideemptyclick="$emit('onhideemptyclick', $event)" @onshowalldataclick="$emit('onshowalldataclick', $event)" :showcharts="showcharts" :variables="variables" @onshowchartsclick2="$emit('onshowchartsclick2', $event)"></speed-run-grid-tab-variable>
                </div>
                <div v-else-if="subcategoryvariablevalueids[variable.name + variableindex] == variableValue.name">
                    <div v-if="!userid">
                        <div class="row no-gutters pr-1 pt-1">
                            <div class="col-auto pr-2">
                                <label class="tab-row-name">Hide Empty:</label>
                            </div>
                            <div class="col align-self-center">
                                <div class="custom-control custom-switch">
                                    <input id="chkHideEmpty" type="checkbox" class="custom-control-input" data-toggle="toggle" v-model="hideempty" @click="$emit('onhideemptyclick', $event)">
                                    <label class="custom-control-label" for="chkHideEmpty"></label>
                                </div>
                            </div>
                        </div>
                        <div class="row no-gutters pr-1 pt-0">
                            <div class="col-auto pr-2">
                                <label class="tab-row-name">Show All Runs:</label>
                            </div>
                            <div class="col align-self-center">
                                <div class="custom-control custom-switch">
                                    <input id="chkShowAllData" type="checkbox" class="custom-control-input" data-toggle="toggle" v-model="showalldata" @click="$emit('onshowalldataclick', $event)">
                                    <label class="custom-control-label" for="chkShowAllData"></label>
                                </div>
                            </div>
                        </div>
                    </div>                   
                    <speedrun-grid :gameid="gameid" :categorytypeid="categorytypeid" :categoryid="categoryid" :levelid="levelid" :variablevalues="(prevdata + ',' + variableValue.id).replace(/(^,)|(,$)/g, '')" :userid="userid" :showalldata="showalldata" :showcharts="showcharts" :variables="variables" @onshowchartsclick1="$emit('onshowchartsclick2', $event)"></speedrun-grid>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    export default {
        name: "speed-run-grid-tab-variable",
        emits: ["ontabclick", "onhideemptyclick", "onshowalldataclick", "onshowchartsclick2"],
        props: {
            items: Array,
            gameid: String,
            categorytypeid: String,
            categoryid: String,
            levelid: String,
            subcategoryvariablevalueids: Object,
            userid: String,
            prevdata: String,
            variableindex: Number,
            hideempty: Boolean,
            showalldata: Boolean,
            showcharts: Boolean,
            variables: Array
        }
    };
</script>







