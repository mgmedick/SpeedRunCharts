<template>
    <div>
        <div>
            <div style="margin-bottom:10px;">
                <div class="btn-group" role="group">
                    <button type="button" class="btn btn-primary btn-sm categorytype" :class="{ 'active' : categoryTypeID == null }" @click="onCategoryTypeClick(null)">All</button>
                    <button type="button" class="btn btn-primary btn-sm categorytype" :class="{ 'active' : categoryTypeID == 0 }" @click="onCategoryTypeClick(0)">Full Game</button>
                    <button type="button" class="btn btn-primary btn-sm categorytype" :class="{ 'active' : categoryTypeID == 1 }" @click="onCategoryTypeClick(1)">Level</button>
                </div>                
            </div>                
            <div style="margin-bottom:20px;">
                <div class="btn-group" role="group">
                    <template v-for="(item, itemIndex) in summarylists" class="btn btn-primary btn-sm summary-list" :class="{ 'active' : summaryListID == item.id }">
                        <button type="button" class="btn btn-primary btn-sm summary-list" :class="{ 'active' : summaryListID == item.id }" @click="onSummaryListClick(item.id)"><i :class="getIconClass(item.id)"></i>&nbsp;{{ item.displayName.replace(/ /g, '\u00a0') }}</button>
                    </template>
                </div>                  
            </div>
        </div>
        <div>
            <summary-list :summarylistid="summaryListID" :categorytypeid="categoryTypeID" :defaulttopamt="defaulttopamt"></summary-list>
        </div>
    </div>  
</template>
<script>
    export default {
        name: 'SummaryListTabs',
        props: {
            summarylists: Array,
            defaulttopamt: Number
        },
        data: function () {
            return {
                items: [],
                summaryListID: sessionStorage.getItem("summarylistid") ? parseInt(sessionStorage.getItem("summarylistid")) : this.summarylists[0]?.id,
                categoryTypeID: sessionStorage.getItem("categorytypeid") ? parseInt(sessionStorage.getItem("categorytypeid")) : null
            }
        },
        created() {
            var isPageReloaded = ((window.performance.navigation && window.performance.navigation.type === 1) ||
                        window.performance.getEntriesByType('navigation').map((nav) => nav.type).includes('reload'));

            if (isPageReloaded) {
                this.resetParams();                   
            }

            sessionStorage.setItem("summarylistid", this.summaryListID);            
        },
        methods: {
            resetParams: function() {
                this.summaryListID = this.summarylists[0]?.id;
                sessionStorage.removeItem("summarylistid");
                this.categoryTypeID = null;
                sessionStorage.removeItem("categorytypeid");                
            },            
            getIconClass: function (id) {
                var iconClass = '';

                switch (id) {
                    case 0:
                        iconClass = 'fa fa-certificate';
                        break;
                    case 1:
                        iconClass = 'fa fa-award';
                        break;
                    case 2:
                        iconClass = 'fa fa-cubes';
                        break;                                                                                            
                }

                iconClass += " fa-sm";

                return iconClass;
            },                    
            onSummaryListClick: function (summaryListID) {
                // Array.from(document.querySelectorAll('.summary-list.active')).forEach((el) => el.classList.remove('active'));
                // event.target.parentElement.classList.add("active");
                this.summaryListID = summaryListID;
                sessionStorage.setItem("summarylistid", this.summaryListID); 
            },                            
            onCategoryTypeClick: function (categoryTypeID) {
                // Array.from(document.querySelectorAll('.categorytype.active')).forEach((el) => el.classList.remove('active'));
                // event.target.parentElement.classList.add("active");
                this.categoryTypeID = categoryTypeID;
                sessionStorage.setItem("categorytypeid", this.categoryTypeID); 
            }                          
        }
    };
</script>





