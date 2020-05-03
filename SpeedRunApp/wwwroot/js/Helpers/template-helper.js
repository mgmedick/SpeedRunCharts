if (!speedRun)
   var speedRun = {};

function templateStorage (storage) {
   storage = (typeof storage == 'undefined') ? {} : storage;

   var get = function (identifier) {
       return storage[identifier];
   };

   var set = function (identifier, value) {
       storage[identifier] = value;
   }

   return {
       get: get,
       set: set,
   };
};

function templateHelper (ajax, templateStorageHelper) {
   var $ajax = ajax;
   var _templateStorageHelper = templateStorageHelper;

   var renderTemplate = function (html, data) {
       var myTemplate = _.template(html);

       return myTemplate({
           items: data
       });
   };

   var getUnderscoreTemplateContents = function (templateID, data) {
       var that = this;

       var template = $('#' + templateID).html();

       return that.getTemplateFromHtml(template, data);
   };

   var getTemplateContentsAsJQuery = function (templateID, data) {
       var that = this;

       var html = that.getUnderscoreTemplateContents(templateID, data);

       return $(html);
   };

   var getTemplate = function (url, data, callback, failCallback) {
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

   return {
       getUnderscoreTemplateContents: getUnderscoreTemplateContents,
       getTemplateContentsAsJQuery: getTemplateContentsAsJQuery,
       getTemplateFromUrl: getTemplate,
       getTemplateFromHtml: renderTemplate,
   }
};

speedRun["templates"] = (typeof speedRun["templates"] !== 'undefined') ? speedRun["templates"] : {};
speedRun["templateHelperStorage"] = templateStorage(speedRun["templates"]);
speedRun["templateHelper"] = templateHelper(speedRun.ajaxHelper, speedRun.templateHelperStorage);



