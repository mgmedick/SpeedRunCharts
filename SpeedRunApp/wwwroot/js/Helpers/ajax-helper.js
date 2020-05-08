if (!sra)
    var sra = {};
 
function ajaxHelper() {
    ajaxHelper.prototype.postJSON = function (_url, _parameters, _successCallback, _failCallback, _completeCallback, _antiforgeryToken) {
        post(_url, _parameters, _successCallback, _failCallback, _completeCallback, "json", "application/json; charset=utf-8", _antiforgeryToken);
    };
 
    ajaxHelper.prototype.getJSON = function (_url, _parameters, _successCallback, _failCallback, _completeCallback) {
        get(_url, _parameters, _successCallback, _failCallback, _completeCallback, "json", "application/json; charset=utf-8");
    };
 
    ajaxHelper.prototype.get = function (_url, _parameters, _successCallback, _failCallback, _completeCallback, _dataType, _contentType) {
        $.ajax({
            type: "GET",
            cache: false,
            url: _url,
            data: _parameters,
            dataType: (typeof _dataType !== "undefined") ? _dataType : undefined,
            contentType: (typeof _contentType !== "undefined") ? _contentType : "application/x-www-form-urlencoded; charset=UTF-8",
            success: _successCallback,
            error: _failCallback,
            complete: _completeCallback,
        });
    };
 
    ajaxHelper.prototype.post = function (_url, _parameters, _successCallback, _failCallback, _completeCallback, _dataType, _contentType, _antiforgeryToken) {
        $.ajax({
            type: "POST",
            cache: false,
            url: _url,
            data: _parameters,
            dataType: (typeof _dataType !== "undefined") ? _dataType : undefined,
            contentType: (typeof _contentType !== "undefined") ? _contentType : "application/x-www-form-urlencoded; charset=UTF-8",
            success: _successCallback,
            error: _failCallback,
            complete: _completeCallback,
            headers: (typeof _antiforgeryToken !== "undefined") ? { "__RequestVerificationToken": _antiforgeryToken } : {},
        });
    };
 
    //Ajax post to the server within a Promise
    ajaxHelper.prototype.getWithPromise = function (Promise, _url, _parameters, _dataType, _contentType) {
        var onSuccess = function (data) {
            Promise.resolve(data);
        };
 
        var onFailure = function () {
            Promise.reject();
        };
 
        $.ajax({
            type: "GET",
            cache: false,
            url: _url,
            data: _parameters,
            dataType: (typeof dataType !== "undefined") ? _dataType : undefined,
            contentType: (typeof _contentType !== "undefined") ? _contentType : "application/x-www-form-urlencoded; charset=UTF-8",
            success: onSuccess,
            error: onFailure,
            //complete: _completeCallback,
        });
 
        return Promise.promise();
    };
 
    ajaxHelper.prototype.postWithPromise = function (Promise, _url, _parameters, _dataType, _contentType) {
        var onSuccess = function (data) {
            Promise.resolve(data);
        };
 
        var onFailure = function () {
            Promise.reject();
        };
 
        $.ajax({
            type: "POST",
            cache: false,
            url: _url,
            data: _parameters,
            dataType: (typeof dataType !== "undefined") ? _dataType : undefined,
            contentType: (typeof _contentType !== "undefined") ? _contentType : "application/x-www-form-urlencoded; charset=UTF-8",
            success: onSuccess,
            error: onFailure,
            //complete: _completeCallback,
        });
 
        return Promise.promise();
    };
 };
 
sra["ajaxHelper"] = new ajaxHelper();



 




