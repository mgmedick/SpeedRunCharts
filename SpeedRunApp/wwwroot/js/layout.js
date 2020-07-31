if (!sra) {
    var sra = {};
}

function initializeGlobalConstants(maxElementsPerPage, requestLimit, timeLimitMS) {
    sra["apiSettings"] = new apiSettings(maxElementsPerPage, requestLimit, timeLimitMS);
}

function apiSettings(maxPerPage, reqLimit, tLimitMS) {
    this.maxElementsPerPage = maxPerPage;
    this.requestLimit = reqLimit;
    this.timeLimitMS = tLimitMS;
}

