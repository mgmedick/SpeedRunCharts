<template>
    <div v-for="variable in items" :key="variable.id">
        <div v-if="subcategoryvariablevalueids[variable.name + variableindex]">
            <div class="variablerow row no-gutters pr-1 pt-1 pb-0 pr-0">
                <div class="col tab-list">
                    <ul class="nav nav-pills">
                        <li class="variableValue nav-item py-1 pr-1" v-for="(variableValue, variableValueIndex) in variable.variableValues" :key="variableValue.id">
                            <a class="nav-link p-2" :class="{ 'active' : subcategoryvariablevalueids[variable.name + variableindex] == variableValue.name }" href="#/" data-type="variableValue" :data-variable="variable.name + variableindex" :data-value="variableValue.name" data-toggle="pill" @click="$emit('ontabclick', $event)">{{ variableValue.name }}</a>
                        </li>
                        <button-dropdown v-show="false" class="more py-1 pr-1" :btnclasses="'btn-secondary'">
                            <template v-slot:text>
                                <span>More...</span>
                            </template>
                            <template v-slot:options>
                                <template v-for="(variableValue, variableValueIndex) in variable.variableValues" :key="variableValue.id">
                                    <a class="dropdown-item d-none" :class="{ 'active' : subcategoryvariablevalueids[variable.name + variableindex] == variableValue.name }" href="#/" data-type="variableValue" :data-variable="variable.name + variableindex" :data-value="variableValue.name" data-toggle="pill" @click="$emit('ontabclick', $event)">{{ variableValue.name }}</a>
                                </template>
                            </template>
                        </button-dropdown>                  
                    </ul>
                </div>
            </div>
            <div v-for="(variableValue, variableValueIndex) in variable.variableValues" :key="variableValue.id">
                <div v-if="variableValue.subVariables && variableValue.subVariables.length > 0 && subcategoryvariablevalueids[variable.name + variableindex] == variableValue.name">
                    <game-worldrecord-variable-grid-tab :items="variableValue.subVariables" :gameid="gameid" :categorytypeid="categorytypeid" :categoryid="categoryid" :levelid="levelid" :subcategoryvariablevalueids="subcategoryvariablevalueids" :speedrunid="speedrunid" :prevdata="(prevdata + ',' + variableValue.id).replace(/(^,)|(,$)/g, '')" :variableindex="variableindex + 1" :showmilliseconds="showmilliseconds" :variables="variables" @ontabclick="$emit('ontabclick', $event)"></game-worldrecord-variable-grid-tab>
                </div>
                <div v-else-if="subcategoryvariablevalueids[variable.name + variableindex] == variableValue.name">
                    <game-worldrecord-grid :gameid="gameid" :categorytypeid="categorytypeid" :categoryid="categoryid" :levelid="levelid" :variablevalues="(prevdata + ',' + variableValue.id).replace(/(^,)|(,$)/g, '')" :showmilliseconds="showmilliseconds" :variables="variables"></game-worldrecord-grid>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    export default {
        name: "GameWorldRecordVariableGridTab",
        emits: ["ontabclick"],
        props: {
            items: Array,
            gameid: String,
            categorytypeid: String,
            categoryid: String,
            levelid: String,
            subcategoryvariablevalueids: Object,
            prevdata: String,
            variableindex: Number,
            showmilliseconds: Boolean, 
            variables: Array
        }            
    };
</script>







