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

    dateHelper.prototype.yearsAgo = function (value) {
        if ((typeof value == 'undefined') || (isNaN(value))) {
            return this.today();
        }

        return moment().subtract(value, 'years').format('MM/DD/YYYY');
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

    dateHelper.prototype.formatTime = function (timepart, value, format) {
        var result;
        var date = moment().startOf('day');

        switch (timepart) {
            case 'seconds':
                result = date.seconds(value).format(format);
                break;
            case 'minutes':
                result = date.minutes(value).format(format);
                break;
            case 'hours':
                result = date.hours(value).format(format);
                break;
        }

        return result;
    }

    /*
    dateHelper.prototype.convertToHHMMSS = function (value) {
        var result;
        var date = moment().startOf('day').seconds(value);

        if (date.hours() > 0) {
            result = date.format("HH [hr] mm [min] ss [sec]")
        } else if (date.minutes() > 0) {
            result = date.format("mm [min] ss [sec]")
        } else {
            result = date.format("ss [sec]")
        }

        return result;
    };
    */
};

sra["dateHelper"] = new dateHelper();



 


