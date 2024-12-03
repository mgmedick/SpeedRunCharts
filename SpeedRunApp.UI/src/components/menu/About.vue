<template>
    <div class="mx-auto">
        <h2 class="text-center mb-3">About</h2>
        <div class="mx-auto text-center" style="max-width:400px;">   
            <p class="lead">An open source site that displays data from <a href="https://www.speedrun.com/">speedrun.com</a>.</p>
            <div class="mb-3"><span>The <strong>Games</strong>, <strong>Players</strong> and <strong>Runs</strong> are imported from the <a href="https://github.com/speedruncomorg/api">speedrun.com api</a>. The site imports verified Runs every 15 mins and updates Game definitions nightly.</span></div>
            <div class="mb-3"><span>If you have any questions reach out on our <a href="https://github.com/mgmedick/SpeedRunApp" class="link-underline-info fw-bold">github</a>.</span></div>
        </div>        
    </div>    
</template>
<script>    
    export default {
        name: "About",
        props: {
        },
        data() {
            return {
            }
        },
        computed: {
        },
        mounted: function () {
        },
        methods: {                      
        }
    };
</script>


