<template>
    <div>
        <div class="sticky-top bg-body d-flex pb-3 px-sm-0 px-2" style="top: 67px; z-index: 1019;">
            <div class="btn-group me-2">
                <div class="dropdown" style="overflow: initial;">
                    <button class="btn btn-secondary btn-sm dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                        <i class="fas fa-filter"></i>
                    </button>
                    <ul class="dropdown-menu">
                        <li>
                            <a class="dropdown-item" :class="{ 'active' : categoryTypeID == null }"  href="#/" :data-value="null" data-toggle="pill" @click="onCategoryTypeClick(null)">All</a>
                        </li>
                        <li>
                            <a class="dropdown-item" :class="{ 'active' : categoryTypeID == 0 }" href="#/" :data-value="null" data-toggle="pill" @click="onCategoryTypeClick(0)">Full Game</a>
                        </li>
                        <li>
                            <a class="dropdown-item" :class="{ 'active' : categoryTypeID == 1 }" href="#/" :data-value="null" data-toggle="pill" @click="onCategoryTypeClick(1)">Level</a>
                        </li>                                                                
                    </ul>
                </div>  
            </div>
            <div class="d-flex" style="overflow: auto;">
                <div v-for="(item, itemIndex) in indexvm.summaryLists" :key="item.id" class="me-2">
                    <button type="button" class="btn btn-secondary btn-sm summary-list nowrap-elipsis" :class="{ 'active' : summaryListID == item.id }" @click="onSummaryListClick(item.id)">{{ item.displayName }}</button>            
                </div>
            </div>
        </div>
        <summary-list :summarylistid="summaryListID" :categorytypeid="categoryTypeID" :defaulttopamt="indexvm.defaultTopAmount"></summary-list>
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

                if (this.summaryListID) {
                    sessionStorage.setItem("summarylistid", this.summaryListID); 
                } else {
                    sessionStorage.removeItem("summarylistid");               
                }                
            },                            
            onCategoryTypeClick: function (categoryTypeID) {
                // Array.from(document.querySelectorAll('.categorytype.active')).forEach((el) => el.classList.remove('active'));
                // event.target.parentElement.classList.add("active");
                this.categoryTypeID = categoryTypeID;
                
                if (this.categoryTypeID) {
                    sessionStorage.setItem("categorytypeid", this.categoryTypeID);
                } else {
                    sessionStorage.removeItem("categorytypeid");               
                }
            }                          
        }
    };
</script>





