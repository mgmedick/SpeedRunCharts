<template>
    <div>
        <div>
            <div class="d-flex">                         
                <div class="ms-auto btn-group me-1">
                    <div class="dropdown">
                        <button class="btn btn-secondary btn-sm dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                            <i class="fas fa-filter"></i>
                        </button>
                        <ul class="dropdown-menu dropdown-menu-end dropdown-menu-lg-start">
                            <li>
                                <a class="dropdown-item" href="#/" :data-value="null" data-toggle="pill" draggable="false">All</a>
                            </li>
                            <li>
                                <a class="dropdown-item" href="#/" :data-value="null" data-toggle="pill" draggable="false">Full Game</a>
                            </li>
                            <li>
                                <a class="dropdown-item" href="#/" :data-value="null" data-toggle="pill" draggable="false">Level</a>
                            </li>                                                                
                        </ul>
                    </div>  
                </div>                                                                                                
            </div>            
            <!-- <div class="mb-2">
                <div class="btn-group" role="group">
                    <button type="button" class="btn btn-primary btn-sm categorytype" :class="{ 'active' : categoryTypeID == null }" @click="onCategoryTypeClick(null)">All</button>
                    <button type="button" class="btn btn-primary btn-sm categorytype" :class="{ 'active' : categoryTypeID == 0 }" @click="onCategoryTypeClick(0)">Full Game</button>
                    <button type="button" class="btn btn-primary btn-sm categorytype" :class="{ 'active' : categoryTypeID == 1 }" @click="onCategoryTypeClick(1)">Level</button>
                </div>                
            </div>                 -->
            <div class="row g-2 mb-lg-4 mb-3">
                <div v-for="(item, itemIndex) in indexvm.summaryLists" :key="item.id" class="col-auto">
                    <button type="button" class="btn btn-secondary btn-sm summary-list" :class="{ 'active' : summaryListID == item.id }" @click="onSummaryListClick(item.id)">{{ item.displayName }}</button>            
                </div>
            </div>
        </div>
        <div class="mx-sm-0 mx-n3">
            <summary-list :summarylistid="summaryListID" :categorytypeid="categoryTypeID" :defaulttopamt="indexvm.defaultTopAmount"></summary-list>
        </div>
    </div>  
</template>
<script>
    export default {
        name: 'Index',
        props: {
            indexvm: Object
        },
        data: function () {
            return {
                items: [],
                summaryListID: sessionStorage.getItem("summarylistid") ? parseInt(sessionStorage.getItem("summarylistid")) : this.indexvm.summaryLists[0]?.id,
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
                this.summaryListID = this.indexvm.summaryLists[0]?.id;
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





