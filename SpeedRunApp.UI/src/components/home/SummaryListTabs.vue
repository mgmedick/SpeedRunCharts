<template>
    <div>
        <div>
            <div style="margin-bottom:10px;">
                <div class="btn-group btn-group-toggle">
                    <label class="btn btn-primary btn-sm categorytype" :class="{ 'active' : !categorytypeid }">
                        <input type="radio" autocomplete="off" value="" v-model="categorytypeid" @change="onCategoryTypeChange">All
                    </label>
                    <label class="btn btn-primary btn-sm categorytype" :class="{ 'active' : categorytypeid == 0 }">
                        <input type="radio" autocomplete="off" value="0" v-model="categorytypeid" @change="onCategoryTypeChange">Full Game
                    </label>
                    <label class="btn btn-primary btn-sm categorytype" :class="{ 'active' : categorytypeid == 1 }">
                        <input type="radio" autocomplete="off" value="1" v-model="categorytypeid" @change="onCategoryTypeChange">Level
                    </label>                                                
                </div>
            </div>                
            <div style="margin-bottom:20px;">
                <div class="btn-group btn-group-toggle" style="display: block">
                    <label v-for="(item, itemIndex) in summarylists" class="btn btn-primary btn-sm summary-list" :class="{ 'active' : summarylistid == item.id }" v-tippy="item.description">
                        <input type="radio" autocomplete="off" :value="item.id" v-model="summarylistid" @change="onSummaryListChange"><i :class="getIconClass(item.id)"></i>&nbsp;{{ item.displayName.replace(/ /g, '\u00a0') }}
                    </label>
                </div>
            </div>
        </div>
        <div>
            <summary-list :summarylistid="summarylistid" :defaulttopamt="defaulttopamt" :categorytypeid="categorytypeid"></summary-list>
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
                summarylistid: sessionStorage.getItem("summarylistid") ?? this.summarylists[0]?.id,
                categorytypeid: sessionStorage.getItem("categorytypeid") ?? null
            }
        },
        created() {
            var isPageReloaded = ((window.performance.navigation && window.performance.navigation.type === 1) ||
                        window.performance.getEntriesByType('navigation').map((nav) => nav.type).includes('reload'));

            if (isPageReloaded) {
                this.resetParams();                   
            }

            sessionStorage.setItem("summarylistid", this.summarylistid);            
        },
        methods: {
            resetParams: function() {
                this.summarylistid = null;
                sessionStorage.removeItem("summarylistid");
                this.categorytypeid = null;
                sessionStorage.removeItem("categorytypeid");                
            },            
            getIconClass: function (id) {
                var iconClass = '';

                switch (id) {
                    case 0:
                        iconClass = 'fa fa-certificate';
                        break;
                    case 1:
                        iconClass = 'fa fa-percentage';
                        break;
                    case 2:
                        iconClass = 'fa fa-award';
                        break;
                    case 3:
                        iconClass = 'fa fa-cubes';
                        break;
                    case 4:
                        iconClass = 'fa fa-star';
                        break;
                    case 5:
                        iconClass = 'fa fa-fire';
                        break;
                    case 7:
                        iconClass = 'fa fa-gamepad';
                        break;       
                    case 8:
                        iconClass = 'fa fa-lightbulb';
                        break;    
                    case 9:
                        iconClass = 'fa fa-chart-line';
                        break;                                                                                               
                }

                iconClass += " fa-sm";

                return iconClass;
            },                    
            onSummaryListChange: function (event) {
                Array.from(document.querySelectorAll('.summary-list.active')).forEach((el) => el.classList.remove('active'));
                event.target.parentElement.classList.add("active");
                sessionStorage.setItem("summarylistid", this.summarylistid); 
            },                            
            onCategoryTypeChange: function (event) {
                Array.from(document.querySelectorAll('.categorytype.active')).forEach((el) => el.classList.remove('active'));
                event.target.parentElement.classList.add("active");
                sessionStorage.setItem("categorytypeid", this.categorytypeid); 
            }                          
        }
    };
</script>





