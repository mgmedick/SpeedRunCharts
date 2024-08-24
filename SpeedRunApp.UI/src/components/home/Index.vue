<template>
    <div class="container-lg mx-auto p-0" style="max-width:598px;">
        <summary-list-tabs :summarylists="indexvm.summaryLists" :defaulttopamt="indexvm.defaultTopAmount"></summary-list-tabs>
    </div>
</template>
<script>
    export default {
        name: "Index",
        props: {
            indexvm: Object
        },       
        computed: { 
        },                 
        watch: {
        },           
        created: function () {
        },
        mounted: function () {
        },  
        destroyed() {
        },                  
        methods: {              
        },
    };
</script>






