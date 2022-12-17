<template>
    <div v-for="variable in items" :key="variable.id">
        <div v-for="(variableValue, variableValueIndex) in variable.variableValues" :key="variableValue.id">
            <div v-if="variableValue.subVariables && variableValue.subVariables.length > 0">            
                <game-worldrecord-variable-grid :items="variableValue.subVariables" :gameid="gameid" :categorytypeid="categorytypeid" :categoryids="categoryids" :levelids="levelids" :prevdata="prevdata" :currdata="(currdata + ',' + variableValue.id).replace(/(^,)|(,$)/g, '')" :variableindex="variableindex + 1" :showmilliseconds="showmilliseconds" :variables="variables"></game-worldrecord-variable-grid>
            </div>
            <div v-else-if="!isGenerated && (currdata + ',' + variableValue.id).replace(/(^,)|(,$)/g, '').startsWith(prevdata)">
                <!-- {{ 'gameid: ' + gameid + ' categoryids: ' + categoryids + ' levelids: ' + (levelids ?? '') + ' variablevalues: ' + (currdata + ',' + variableValue.id).replace(/(^,)|(,$)/g, '') }} -->
                <game-worldrecord-grid :gameid="gameid" :categorytypeid="categorytypeid" :categoryids="categoryids" :levelids="levelids" :variablevalues="(currdata ? currdata : currdata + ',' + variableValue.id).replace(/(^,)|(,$)/g, '')" :showmilliseconds="showmilliseconds" :variables="variables"></game-worldrecord-grid>                
                {{ setIsGenerated(true) }}
            </div>
        </div>
        {{ setIsGenerated(false) }}
    </div>
</template>
<script>
    export default {
        name: "GameWorldRecordVariableGrid",
        props: {
            items: Array,
            gameid: String,
            categorytypeid: String,
            categoryids: String,
            levelids: String,
            prevdata: String,
            currdata: String,            
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







