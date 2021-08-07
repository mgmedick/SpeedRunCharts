<template>
    <div>
        <div>
            <div class="mx-auto p-2" style="max-width:598px; margin-bottom:20px;">
                <div class="btn-group btn-group-toggle pr-2">
                    <label class="btn btn-primary btn-sm category active">
                        <input type="radio" id="btnCategory0" name="btnCategory" autocomplete="off" value="0" v-model="categoryid" v-on:click="onCategoryChange"><i class="fa fa-certificate fa-lg"></i>&nbsp;New
                    </label>
                    <label class="btn btn-primary btn-sm category">
                        <input type="radio" id="btnCategory1" name="btnCategory" autocomplete="off" value="1" v-model="categoryid" v-on:click="onCategoryChange"><i class="fa fa-percentage fa-lg"></i>&nbsp;First
                    </label>
                    <label class="btn btn-primary btn-sm category">
                        <input type="radio" id="btnCategory2" name="btnCategory" autocomplete="off" value="2" v-model="categoryid" v-on:click="onCategoryChange"><i class="fa fa-award fa-lg"></i>&nbsp;Top 5%
                    </label>
                    <label class="btn btn-primary btn-sm category">
                        <input type="radio" id="btnCategory3" name="btnCategory" autocomplete="off" value="3" v-model="categoryid" v-on:click="onCategoryChange"><i class="fa fa-fire fa-lg"></i>&nbsp;Top 3
                    </label>
                    <label class="btn btn-primary btn-sm category">
                        <input type="radio" id="btnCategory4" name="btnCategory" autocomplete="off" value="4" v-model="categoryid" v-on:click="onCategoryChange"><i class="fa fa-star fa-lg"></i>&nbsp;PBs
                    </label>
                    <i class="fa fa-info-circle pl-1" data-toggle="tooltip" data-placement="bottom" data-html="true" title="<b>New:</b> Newly verified runs.<br/><br/><b>Top 5%:</b> Runs with rank in top 5% of category.<br/><br/><b>First:</b> Runs with 1st place in category.<br/><br/><b>Top 3:</b> Runs in top 3 with 20 or more runs.<br/><br/><b>PBs:</b> Personal Bests for that category."></i>
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


