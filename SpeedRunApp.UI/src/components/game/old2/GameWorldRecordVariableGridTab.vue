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
                <div v-if="subcategoryvariablevalueids[variable.name + variableindex] == variableValue.name">
                    <div v-if="variableValue.subVariables?.filter(v1 => (v1.scopeTypeID == '0' || v1.scopeTypeID == '2')).length > 0"> 
                        {{ 'test6' }}           
                        <game-worldrecord-variable-grid-tab :items="variableValue.subVariables?.filter(v1 => (v1.scopeTypeID == '0' || v1.scopeTypeID == '2'))" :gameid="gameid" :categorytypeid="categorytypeid" :categoryids="categoryids" :levelids="levelids" :singlelevelids="singlelevelids" :alllevelvariables="alllevelvariables" :subcategoryvariablevalueids="subcategoryvariablevalueids" :prevdata="(prevdata + ',' + variableValue.id).replace(/(^,)|(,$)/g, '')" :prevdatanames="(prevdatanames + ',' + variableValue.name).replace(/(^,)|(,$)/g, '')" :variableindex="variableindex + 1" :showmilliseconds="showmilliseconds" :variables="variables" @ontabclick="$emit('ontabclick', $event)"></game-worldrecord-variable-grid-tab>
                    </div>
                    <div v-else>
                        <game-worldrecord-grid :gameid="gameid" :categorytypeid="categorytypeid" :categoryids="categoryids" :levelids="levelids" :variablevalues="(prevdata + ',' + variableValue.id).replace(/(^,)|(,$)/g, '')" :showmilliseconds="showmilliseconds" :variables="variables"></game-worldrecord-grid>                          
                        <div v-for="(levelID, levelIndex) in singlelevelids" :key="levelID">
                            <div v-if="alllevelvariables?.filter(v1 => v1.categoryID == categoryids && v1.levelID == levelID).length > 0"> 
                                {{ 'test7' }}           
                                <game-worldrecord-variable-grid :items="alllevelvariables?.filter(v1 => v1.categoryID == categoryids && v1.levelID == levelID)" :gameid="gameid" :categorytypeid="categorytypeid" :categoryids="categoryids" :levelids="levelID.toString()" :subcategoryvariablevalueids="subcategoryvariablevalueids" :prevdata="(prevdata + ',' + variableValue.id).replace(/(^,)|(,$)/g, '')" :currdata="''" :variableindex="variableindex + 1" :showmilliseconds="showmilliseconds" :variables="variables"></game-worldrecord-variable-grid>                        
                            </div>
                            <div v-else>
                                {{ 'test8' }}           
                                <game-worldrecord-grid :gameid="gameid" :categorytypeid="categorytypeid" :categoryids="categoryids" :levelids="levelID.toString()" :variablevalues="getLevelVariableValueIDs(categoryids, levelID, variableValue.name)" :showmilliseconds="showmilliseconds" :variables="variables"></game-worldrecord-grid>                                  
                            </div>   
                        </div>                      
                    </div>                                                         
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
            categoryids: String,
            levelids: String,
            singlelevelids: Array,
            alllevelvariables: Array,
            subcategoryvariablevalueids: Object,
            prevdata: String,
            prevdatanames: String,
            variableindex: Number,
            showmilliseconds: Boolean,
            variables: Array
        },
        methods: {
            getLevelVariableValueIDs: function(categoryID, levelID, currVariableValueName){
                var subCategoryVariablesTabs = this.alllevelvariables?.filter(v1 => v1.categoryID == categoryID && v1.levelID == levelID);
                var variableValueNames = (this.prevdatanames + ',' + currVariableValueName)?.split(',')
                var variableValueIDs = this.getVariableValueIDs(subCategoryVariablesTabs, variableValueNames);
                var variableValues = variableValueIDs.join(',');

                return variableValues;
            },
            getVariableValueIDs: function(subCategoryVariablesTabs, variableValueNames, variableValueIDs) {
                if(!variableValueIDs){
                    variableValueIDs = [];
                }

                subCategoryVariablesTabs?.forEach((variable, variableIndex) => {
                    variable.variableValues?.forEach(variableValue => { 
                        if (variableValueNames.indexOf(variableValue.name) > -1 && variableValueIDs.indexOf(variableValue.id) == -1){
                            variableValueIDs.push(variableValue.id);
                        }

                        if (variableValue.subVariables && variableValue.subVariables.length > 0) {
                            this.getVariableValueIDs(variableValue.subVariables, variableValueNames, variableValueIDs);
                        }           
                    });
                });

                return variableValueIDs;
            },               
        }                                    
    };
</script>







