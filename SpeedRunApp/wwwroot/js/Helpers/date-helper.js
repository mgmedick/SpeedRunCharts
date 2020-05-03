if (!speedRun)
    var speedRun = {};

function dateHelper () {
    var today = function () {
        return moment().format('MM/DD/YYYY');
    };

    var monthsAgo = function (value) {
        if ((typeof value == 'undefined') || (isNaN(value))) {
            return this.today();
        }

        return moment().subtract(value, 'months').format('MM/DD/YYYY');
    };

    var beginningOfYear = function (year) {
        if ((typeof year == 'undefined') || (isNaN(year)))
            return moment().startOf('year').format('MM/DD/YYYY');

        return moment(year, 'YYYY').startOf('year').format('MM/DD/YYYY');
    };

    var endOfYear = function (year) {
        if ((typeof year == 'undefined') || (isNaN(year)))
            return moment().endOf('year').format('MM/DD/YYYY');

        return moment(year, 'YYYY').endOf('year').format('MM/DD/YYYY');
    };

    return {
        today: today,
        monthsAgo: monthsAgo,
        beginningOfYear: beginningOfYear,
        endOfYear: endOfYear
    };
};

speedRun["dateHelper"] = dateHelper();



 


