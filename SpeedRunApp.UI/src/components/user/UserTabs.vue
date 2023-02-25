<template>
    <div class="mt-4">
        <div class="row no-gutters pr-1 pt-1 pb-0">
            <div class="col tab-list">
                <ul class="nav nav-pills">
                    <li class="nav-item py-1 pr-1">
                        <a class="nav-link p-2" :class="{ 'active' : gridID == 0 }" href="#/" data-value="0" draggable="false" @click="onTabClick">{{ "Runs" }}</a>
                    </li>
                    <li class="nav-item py-1 pr-1">
                        <a class="nav-link p-2" :class="{ 'active' : gridID == 1 }" href="#/" data-value="1" draggable="false" @click="onTabClick">{{ "Personal Bests" }}</a>            
                    </li>
                </ul>
            </div>
        </div>
        <div v-if="gridID == 0">
            <user-speedrun-tabs :id="id" :speedrunid="speedrunid"></user-speedrun-tabs>
        </div>
        <div v-else>
            <personalbest-tabs :id="id"></personalbest-tabs>   
        </div>
    </div>
</template>
<script>
    export default {
        name: "UserTabs",
        props: {
            id: String,
            speedrunid: String           
        },
        data() {
            return {
                gridID: '0'
            }
        },
        methods: {
            onTabClick: function (event) {
                var value = event.target.getAttribute('data-value');
                this.gridID = value;              
            },
        }       
    };
</script>












