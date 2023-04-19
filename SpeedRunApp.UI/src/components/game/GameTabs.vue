<template>
    <div class="mt-3">        
        <div v-if="gridID == 0 || gridID == 1" class="row no-gutters pr-1">
            <div class="col-auto">
                <label class="tab-row-name pr-2">Show Misc:</label>
            </div>
            <div class="col align-self-center">
                <div class="custom-control custom-switch">
                    <input id="chkShowMisc" type="checkbox" class="custom-control-input" data-toggle="toggle" v-model="showMisc">
                    <label class="custom-control-label" for="chkShowMisc"></label>
                </div>
            </div>
        </div> 
        <div v-if="gridID == 0" class="row no-gutters pr-1">
            <div class="col-auto">
                <label class="tab-row-name pr-2">Hide Empty:</label>
            </div>
            <div class="col align-self-center">
                <div class="custom-control custom-switch">
                    <input id="chkHideEmpty" type="checkbox" class="custom-control-input" data-toggle="toggle" v-model="hideEmpty">
                    <label class="custom-control-label" for="chkHideEmpty"></label>
                </div>
            </div>
        </div>                
        <div id="divGameTabContainer" class="row no-gutters pr-1">
            <div class="col tab-list">                
                <ul class="nav nav-pills">
                    <li class="nav-item py-1 pr-1">
                        <a class="nav-link p-2" :class="{ 'active' : gridID == 0 }" href="#/" data-value="0" draggable="false" @click="onTabClick">{{ "Leaderboards" }}</a>
                    </li>
                    <li class="nav-item py-1 pr-1">
                        <a class="nav-link p-2" :class="{ 'active' : gridID == 1 }" href="#/" data-value="1" draggable="false" @click="onTabClick">{{ "World Recs" }}</a>            
                    </li>
                    <li class="nav-item py-1 pr-1">
                        <a class="nav-link p-2" :class="{ 'active' : gridID == 2 }" href="#/" data-value="2" draggable="false" @click="onTabClick">{{ "Summary Charts" }}</a>            
                    </li>
                    <button-dropdown v-show="false" class="more py-1 pr-1" :btnclasses="'btn-secondary'">
                        <template v-slot:text>
                            <span>More...</span>
                        </template>
                        <template v-slot:options>
                            <a class="dropdown-item d-none" :class="{ 'active' : gridID == 0 }" href="#/" data-value="0" draggable="false" @click="onTabClick">{{ "Leaderboards" }}</a>            
                            <a class="dropdown-item d-none" :class="{ 'active' : gridID == 1 }" href="#/" data-value="1" draggable="false" @click="onTabClick">{{ "World Recs" }}</a>            
                            <a class="dropdown-item d-none" :class="{ 'active' : gridID == 2 }" href="#/" data-value="2" draggable="false" @click="onTabClick">{{ "Summary Charts" }}</a>                          
                        </template>
                    </button-dropdown>                                          
                </ul>
            </div>
        </div>
        <div v-if="gridID == 0">
            <leaderboard-tabs :id="id" :speedrunid="speedrunid" :hideempty="hideEmpty" :showmisc="showMisc"></leaderboard-tabs>
        </div>
        <div v-else-if="gridID == 1">
            <worldrecord-tabs :id="id" :showmisc="showMisc"></worldrecord-tabs>   
        </div>
        <div v-else>
            <game-chart-tabs :id="id"></game-chart-tabs>
        </div>        
    </div>
</template>
<script>
    export default {
        name: "GameTabs",
        props: {
            id: String,
            speedrunid: String           
        },
        data() {
            return {
                gridID: '0',
                hideEmpty: true,
                showMisc: false
            }
        },
        mounted: function () {
            this.resizeGMTabs();
            window.addEventListener('resize', this.resizeGMTabs);
        },
        updated: function () {
            this.resizeGMTabs();
        },                 
        methods: {
            onTabClick: function (event) {
                var value = event.target.getAttribute('data-value');
                this.gridID = value;              
            },
            resizeGMTabs: function () {
                var rows = document.querySelectorAll('#divGameTabContainer .tab-list');

                for (var g = 0; g < rows.length; g++) {
                    var totalWidth = 0;
                    var tabitems = rows[g].querySelectorAll('li:not(.dropdown-item)');
                    var morediv =  rows[g].querySelector('.more');
                    var morebtn =  morediv.querySelector('.btn');
                    var moreItems = morediv.querySelectorAll('a.dropdown-item');
                    var moredrp = morediv.querySelector('.dropdown-menu');

                    for (var i = 0; i < tabitems.length; i++) {
                        tabitems[i].style.left = "-10000px";
                        tabitems[i].classList.remove('d-none');
                        totalWidth += tabitems[i].offsetWidth;
                        if (totalWidth > ((rows[g].offsetWidth - tabitems[i].offsetWidth) - 30)) {
                            tabitems[i].classList.add('d-none');
                            moreItems[i].classList.remove('d-none');
                        } else {
                            tabitems[i].classList.remove('d-none');
                            moreItems[i].classList.add('d-none');                     
                        }
                    }

                    var items = Array.from(morediv.querySelectorAll('a:not(.d-none)'));
                    if (items.length > 0) {
                        var item = items.find(i=>i.classList.contains('active'));
                        if (item) {
                            morebtn.innerHTML = item.innerHTML;
                            morebtn.classList.add('active');
                        } else {
                            morebtn.innerHTML = 'More...';
                            morebtn.classList.remove('active');
                        }
                        
                        morediv.style.display = 'block';
                    } else {
                        morediv.style.display = 'none';                       
                    }

                    var ww = document.documentElement.clientWidth;
                    var pos = this.getPosition(morediv);
                    if (pos.x > (ww / 2)) {
                        moredrp.classList.remove('dropdown-menu-left');
                        moredrp.classList.add('dropdown-menu-right');
                    } else {
                        moredrp.classList.remove('dropdown-menu-right');
                        moredrp.classList.add('dropdown-menu-left');
                    }
               }
            },
            getPosition: function (element) {
                var xPosition = 0;
                var yPosition = 0;

                while (element) {
                    xPosition += (element.offsetLeft - element.scrollLeft + element.clientLeft);
                    yPosition += (element.offsetTop - element.scrollTop + element.clientTop);
                    element = element.offsetParent;
                }
                return {
                    x: xPosition,
                    y: yPosition
                };
            }                        
        }       
    };
</script>












