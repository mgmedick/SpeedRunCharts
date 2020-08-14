if (!sra) {
    var sra = {};
}

function apiSettings(maxPerPage, reqLimit, tLimitMS) {
    this.maxElementsPerPage = maxPerPage;
    this.requestLimit = reqLimit;
    this.timeLimitMS = tLimitMS;
}


function initializeClient(maxElementsPerPage, requestLimit, timeLimitMS) {
    initializeConstants(maxElementsPerPage, requestLimit, timeLimitMS);
    //initializeEvents();
}

function initializeConstants(maxElementsPerPage, requestLimit, timeLimitMS) {
    sra["apiSettings"] = new apiSettings(maxElementsPerPage, requestLimit, timeLimitMS);
}

//function initializeEvents() {
//    $('#txtGameUserSearch').autocomplete({
//        delay: 1000,
//        minlength: 3,
//        source: '../SpeedRun/SearchGamesAndUsers'
//    });
//}




