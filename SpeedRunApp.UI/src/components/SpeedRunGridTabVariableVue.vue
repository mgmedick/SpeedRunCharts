<template>
    <div v-for="(variable, variableIndex) in items" :key="variable.id">
        <div v-if="variablevalueids[variable.id]">
            <div class="row no-gutters pl-1 pt-1 pb-0 pr-0">
                <div class="col-sm-1 align-self-top pt-1">
                    <label class="tab-row-name">{{ variable.name }}:</label>
                </div>
                <div class="col pl-2">
                    <ul class="nav nav-pills">
                        <li class="nav-item p-1" v-for="(variableValue, variableValueIndex) in variable.variableValues" :key="variableValue.id">
                            <a class="variableValue nav-link p-2" :class="{ 'active' : variablevalueids[variable.id] == variableValue.id }" href="#/" :data-variable="variable.id" :data-value="variableValue.id" data-toggle="pill" @click="$emit('variablevalueclick', $event)">{{ variableValue.name }}</a>
                        </li>
                    </ul>
                </div>
            </div>
            <div v-for="(variableValue, variableValueIndex) in variable.variableValues" :key="variableValue.id">
                <div v-if="variableValue.subVariables.length > 0 && variablevalueids[variable.id] == variableValue.id">
                    <speed-run-grid-tab-variable :items="variableValue.subVariables" :gameid="gameid" :categorytypeid="categorytypeid" :categoryid="categoryid" :levelid="levelid" :variablevalueids="variablevalueids" :prevdata="(prevdata + ',' + variableValue.id).replace(/(^,)|(,$)/g, '')" @variablevalueclick="$emit('variablevalueclick', $event)"></speed-run-grid-tab-variable>
                </div>
                <div v-else>
                    <div class="grid" :gameid="gameid" :categorytypeid="categorytypeid" :categoryid="categoryid" :levelid="levelid" :variablevalues="(prevdata + ',' + variableValue.id).replace(/(^,)|(,$)/g, '')"></div>
                    <!--<speed-run-grid :gameid="gameid" :categorytypeid="categorytypeid" :categoryid="categoryid" :levelid="levelid" :variablevalues="(prevdata + ',' + variableValue.id).replace(/(^,)|(,$)/g, '')"></speed-run-grid>-->
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    export default {
        name: "speed-run-grid-tab-variable",
        emits: ["variablevalueclick"],
        props: {
            items: Array,
            gameid: String,
            categorytypeid: String,
            categoryid: String,
            levelid: String,
            variablevalueids: Object,
            prevdata: String
        }
    };
</script>







