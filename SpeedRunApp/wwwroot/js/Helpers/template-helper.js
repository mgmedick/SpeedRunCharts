if (!sra)
    var sra = {};

function templateStorage (storage) {
    storage = (typeof storage == 'undefined') ? {} : storage;

    templateStorage.prototype.get = function (identifier) {
        return storage[identifier];
    };

    templateStorage.prototype.set = function (identifier, value) {
        storage[identifier] = value;
    }
};

function templateHelper (ajax, templateStorageHelper) {
   var $ajax = ajax;
   var _templateStorageHelper = templateStorageHelper;

    templateHelper.prototype.getTemplateFromHtml = function (html, data) {
       var myTemplate = _.template(html);

       return myTemplate({
           items: data
       });
   };

    templateHelper.prototype.getUnderscoreTemplateContents = function (templateID, data) {
       var that = this;

       var template = $('#' + templateID).html();

       return that.getTemplateFromHtml(template, data);
   };

    templateHelper.prototype.getTemplateContentsAsJQuery = function (templateID, data) {
       var that = this;

       var html = that.getUnderscoreTemplateContents(templateID, data);

       return $(html);
   };

    templateHelper.prototype.getTemplateFromUrl = function (url, data, callback, failCallback) {
       var that = this;
       var _url = url;
       var _data = data;
       var _callback = callback;
       var _failCallback = failCallback;

       var onSuccess = function (html) {
           _templateStorageHelper.set(_url, html);

           that.getTemplateFromUrl(_url, _data, _callback, _failCallback);
       };

       if (typeof _templateStorageHelper.get(url) !== 'undefined') { // window.templates[url]
           var html = that.getTemplateFromHtml(_templateStorageHelper.get(url), _data);
           _callback(html);
       }
       else
       {
            $ajax.get(url, {}, onSuccess, _failCallback);
        }
   };
};

sra["templates"] = (typeof sra["templates"] !== 'undefined') ? sra["templates"] : {};
sra["templateHelperStorage"] = new templateStorage(sra["templates"]);
sra["templateHelper"] = new templateHelper(sra.ajaxHelper, sra.templateHelperStorage);



