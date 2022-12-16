<template>
    <div v-for="variable in items" :key="variable.id">
        <div v-for="(variableValue, variableValueIndex) in variable.variableValues" :key="variableValue.id">
            <div v-if="variableValue.subVariables && variableValue.subVariables.length > 0">            
                <game-worldrecord-variable-grid-tab :items="variableValue.subVariables" :gameid="gameid" :categorytypeid="categorytypeid" :categoryids="categoryids" :levelids="levelids" :prevdata="(prevdata + ',' + variableValue.id).replace(/(^,)|(,$)/g, '')" :variableindex="variableindex + 1" :showmilliseconds="showmilliseconds" :variables="variables"></game-worldrecord-variable-grid-tab>
            </div>
            <div v-else-if="!isGenerated">
                <!-- {{ 'gameid: ' + gameid + ' categoryids: ' + categoryids + ' levelids: ' + (levelids ?? '') + ' variablevalues: ' + ((!prevdata ? prevdata + ',' + variableValue.id : prevdata).replace(/(^,)|(,$)/g, '') ?? '') }} -->
                <game-worldrecord-grid :gameid="gameid" :categorytypeid="categorytypeid" :categoryids="categoryids" :levelids="levelids" :variablevalues="(!prevdata ? prevdata + ',' + variableValue.id : prevdata).replace(/(^,)|(,$)/g, '')" :showmilliseconds="showmilliseconds" :variables="variables"></game-worldrecord-grid>
                {{ setIsGenerated(true) }}
            </div>
        </div>
        {{ setIsGenerated(false) }}
    </div>
</template>
<script>
    export default {
        name: "GameWorldRecordVariableGridTabOld",
        props: {
            items: Array,
            gameid: String,
            categorytypeid: String,
            categoryids: String,
            levelids: String,
            prevdata: String,
            variableindex: Number,
            showmilliseconds: Boolean,
            variables: Array
        },
        data() {
            return {
                isGenerated: false
            }
        },
        methods: {
            setIsGenerated: function (isGenerated) {
                this.isGenerated = isGenerated;
            }
        }                                    
    };
</script>







