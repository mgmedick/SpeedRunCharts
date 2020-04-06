var dateHelper = (function () {
    function dateHelper() { };

    dateHelper.prototype.Today = function () {
        return moment().format('MM/DD/YYYY');
    };
 
    dateHelper.prototype.MonthsAgo = function (value) {
        if ((typeof value == 'undefined') || (isNaN(value)))
            return this.Today();
 
        return moment().subtract(value, 'months').format('MM/DD/YYYY');
    };
 
    dateHelper.prototype.BeginningOfYear = function (year) {
        if ((typeof year == 'undefined') || (isNaN(year)))
            return moment().startOf('year').format('MM/DD/YYYY');
 
        return moment(year, 'YYYY').startOf('year').format('MM/DD/YYYY');
    };
 
    dateHelper.prototype.EndOfYear = function (year) {
        if ((typeof year == 'undefined') || (isNaN(year)))
            return moment().endOf('year').format('MM/DD/YYYY');
 
        return moment(year, 'YYYY').endOf('year').format('MM/DD/YYYY');
    };
 
    return dateHelper;
 }());
 


