<template>
    <div>
        <div>
            <div class="mx-auto p-2" style="max-width:598px; margin-bottom:20px;">
                <div class="btn-group btn-group-toggle pr-2">
                    <label class="btn btn-primary btn-sm category" :class="{ 'active' : categoryid == 0 }">
                        <input type="radio" id="btnCategory0" name="btnCategory" autocomplete="off" value="0" v-model="categoryid" @click="onCategoryChange"><i class="fa fa-certificate fa-lg"></i>&nbsp;New
                    </label>
                    <label class="btn btn-primary btn-sm category" :class="{ 'active' : categoryid == 1 }">
                        <input type="radio" id="btnCategory1" name="btnCategory" autocomplete="off" value="1" v-model="categoryid" @click="onCategoryChange"><i class="fa fa-percentage fa-lg"></i>&nbsp;Top 5%
                    </label>
                    <label class="btn btn-primary btn-sm category" :class="{ 'active' : categoryid == 2 }">
                        <input type="radio" id="btnCategory2" name="btnCategory" autocomplete="off" value="2" v-model="categoryid" @click="onCategoryChange"><i class="fa fa-award fa-lg"></i>&nbsp;First
                    </label>
                    <label class="btn btn-primary btn-sm category" :class="{ 'active' : categoryid == 3 }">
                        <input type="radio" id="btnCategory3" name="btnCategory" autocomplete="off" value="3" v-model="categoryid" @click="onCategoryChange"><i class="fa fa-fire fa-lg"></i>&nbsp;Top 3
                    </label>
                    <label class="btn btn-primary btn-sm category" :class="{ 'active' : categoryid == 4 }">
                        <input type="radio" id="btnCategory4" name="btnCategory" autocomplete="off" value="4" v-model="categoryid" @click="onCategoryChange"><i class="fa fa-star fa-lg"></i>&nbsp;PBs
                    </label>
                    <div v-tippy="'<b>New:</b><br/>Newly verified runs<br/><br/><b>Top 5%:</b><br/>Runs with rank in top 5% of category with pre-existing runs<br/><br/><b>First:</b><br/>Runs with 1st place in category with pre-existing runs<br/><br/><b>Top 3:</b><br/>Runs in top 3 in category with pre-existing runs<br/><br/><b>PBs:</b><br/>PB for category where user has pre-existing runs'"><i class="fa fa-info-circle pl-1"></i></div>
                </div>
            </div>
        </div>
        <div>
            <speedrun-list :categoryid="categoryid"></speedrun-list>
        </div>
    </div>
</template>
<script>
    export default {
        name: 'SpeedRunListCategoryVue',
        data: function () {
            return {
                categoryid: "0"
            }
        },
        methods: {
            onCategoryChange: function (event) {
                Array.from(document.querySelectorAll('.category.active')).forEach((el) => el.classList.remove('active'));
                event.target.parentElement.classList.add("active");
            }
        }
    };
</script>


