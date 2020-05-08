if (!sra)
    var sra = {};

function dateHelper () {
    dateHelper.prototype.today = function () {
        return moment().format('MM/DD/YYYY');
    };

    dateHelper.prototype.monthsAgo = function (value) {
        if ((typeof value == 'undefined') || (isNaN(value))) {
            return this.today();
        }

        return moment().subtract(value, 'months').format('MM/DD/YYYY');
    };

    dateHelper.prototype.beginningOfYear = function (year) {
        if ((typeof year == 'undefined') || (isNaN(year)))
            return moment().startOf('year').format('MM/DD/YYYY');

        return moment(year, 'YYYY').startOf('year').format('MM/DD/YYYY');
    };

    dateHelper.prototype.endOfYear = function (year) {
        if ((typeof year == 'undefined') || (isNaN(year)))
            return moment().endOf('year').format('MM/DD/YYYY');

        return moment(year, 'YYYY').endOf('year').format('MM/DD/YYYY');
    };
};

sra["dateHelper"] = new dateHelper();



 


