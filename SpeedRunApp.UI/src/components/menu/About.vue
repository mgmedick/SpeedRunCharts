<template>
    <div class="mx-auto">
        <h2 class="text-center mb-3">About</h2>
        <div class="mx-auto" style="max-width:400px;">   
            <p class="lead text-center">An open source site that displays data from <a href="https://www.speedrun.com/">speedrun.com</a>.</p>
            <div class="mb-3"><span>The <strong>Games</strong>, <strong>Players</strong> and <strong>Runs</strong> are imported from the <a href="https://github.com/speedruncomorg/api">speedrun.com api</a>. The site imports verified runs every 15 mins and updates game definitions nightly.</span></div>
            <div class="mb-3"><span><strong>Note:</strong> Some games may have incorrect leaderboard data because the <a href="https://github.com/speedruncomorg/api">speedrun.com api</a> can erroneously include deleted variables. This only affects a small percentage of games (ex. Sonic Adventures). Generally if our leaderboard structure matches <a href="https://www.speedrun.com/">speedrun.com</a> the rankings should be correct.</span></div>
            <div class="mb-3 text-center"><span>This is a side project, so it might not be updated frequently. If you have any questions reach out on <a href="https://github.com/mgmedick/SpeedRunApp" class="link-underline-info fw-bold">github</a>.</span></div>
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


