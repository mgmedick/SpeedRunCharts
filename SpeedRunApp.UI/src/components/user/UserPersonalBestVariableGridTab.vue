<template>
    <div v-for="variable in items" :key="variable.id">
        <div v-if="variable.variableValues.filter(va => va.subVariables && va.subVariables.length > 0).length > 0">
            <div v-for="(variableValue, variableValueIndex) in variable.filter(va => va.subVariables && va.subVariables.length > 0)" :key="variableValue.id">
                <user-personalbest-variable-grid-tab :items="variableValue.subVariables" :gameid="gameid" :categorytypeid="categorytypeid" :categoryid="categoryid" :levelid="levelid" :userid="userid" :prevdata="(prevdata + ',' + variableValue.id).replace(/(^,)|(,$)/g, '')" :variableindex="variableindex + 1" :variables="variables"></user-personalbest-variable-grid-tab>
            </div>
        </div>
        <div v-else>
            <user-personalbest-grid :gameid="gameid" :categorytypeid="categorytypeid" :categoryid="categoryid" :levelid="levelid" :userid="userID" :showmilliseconds="showmilliseconds" :variables="variables"></user-personalbest-grid>
        </div>    
    </div>
</template>
<script>
    export default {
        name: "UserPersonalBestVariableGridTab",
        props: {
            items: Array,
            gameid: String,
            categorytypeid: String,
            categoryid: String,
            levelid: String,
            userid: String,
            prevdata: String,
            variableindex: Number,
            showmilliseconds: Boolean,
            variables: Array
        },
        data: function () {
            return {
                hideEmpty: this.hideempty,
                showAllData: this.showalldata
            }
        },        
    };
</script>







