<template>
    <div class="container p-0 mt-4">
        <div class="row no-gutters pr-1 pt-1 pb-0 pr-0">
            <div class="col tab-list">
                <ul class="nav nav-pills">
                    <li class="nav-item py-1 pr-1">
                        <a class="nav-link p-2" :class="{ 'active' : gridID == 0 }" href="#/" data-value="0" @click="onTabClick">{{ isgame ? "Leaderboards" : "Runs" }}</a>
                    </li>
                    <li class="nav-item py-1 pr-1">
                        <a class="nav-link p-2" :class="{ 'active' : gridID == 1 }" href="#/" data-value="1" @click="onTabClick">{{ isgame ? "World Records" : "Personal Bests" }}</a>            
                    </li>
                </ul>
            </div>
        </div>
        <div v-if="gridID == 0">
            <speedrun-grid-tab :isgame="isgame" :id="id" :speedrunid="speedrunid"></speedrun-grid-tab>
        </div>
        <div v-else>
            <worldrecord-grid-tab :isgame="isgame" :id="id"></worldrecord-grid-tab>   
        </div>
    </div>
</template>
<script>
    export default {
        name: "GridTabContainerVue",
        props: {
            isgame: Boolean,
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
<style scoped>
    :deep(.tab-list .nav) {
        /*overflow-y: hidden;*/
        flex-wrap: nowrap !important;
        white-space: nowrap !important;
    }

    :deep(.tab-list .nav-link) {
        background-color: #313131;
        font-size: 13px;
        font-weight: bold;
    }
    
    :deep(.btn) {
        font-size: 13px;
        font-weight: bold;
    }    

    :deep(.tab-list .nav-link.active) {
        background-color: var(--primary) !important;
    }

    :deep(.tab-list .dropdown .btn.dropdown-toggle) {
        background-color: #313131;
        font-size: 13px;
        font-weight: bold;
        border: none !important;
        padding: 0.5rem !important;
    }

    :deep(.tab-list .dropdown .btn.dropdown-toggle.active) {
        background-color: var(--primary) !important;
    }

    :deep(.tab-row-name) {
        font-size: 14px !important;
        line-height: 18px;
        font-weight: bold;
    }

    @media (max-width: 575px) {
        :deep(.tab-list .dropdown .dropdown-menu) {
            max-width: 200px;
        }
    }
</style>










