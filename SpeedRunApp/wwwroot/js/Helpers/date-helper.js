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
                result = date.seconds(value);
                break;
            case 'minutes':
                result = date.minutes(value);
                break;
            case 'hours':
                result = date.hours(value);
                break;
        }

        if (date.hours() > 0) {
            result = date.format("hh[h] mm[m] ss[s]")
        } else if (date.minutes() > 0) {
            result = date.format("mm[m] ss[s]")
        } else {
            result = date.format("ss[s]")
        }

        return result;
    }

    dateHelper.prototype.dateDiffList = function (datePart, startDate, endDate) {
        var dates = [];
        switch (datePart.toLowerCase()) {
            case "day":
                while (startDate <= endDate) {
                    dates.push(startDate);
                    startDate = moment(startDate).add(1, 'day').toDate();
                }
                break;
            case "month":
                var startMonthYear = moment([startDate.getFullYear(), startDate.getMonth(), 1]).toDate();
                var endMonthYear = moment([endDate.getFullYear(), endDate.getMonth(), 1]).toDate();
                while (startMonthYear <= endMonthYear) {
                    dates.push(startMonthYear);
                    startMonthYear = moment(startMonthYear).add(1, 'month').toDate();
                }
                break;
        }

        return dates;
    }

    dateHelper.prototype.add = function (date, value, datePart) {
        return moment(date).add(value, datePart).toDate();
    }

    dateHelper.prototype.maxDate = function (dates) {
        return moment(Math.max.apply(null, dates)).toDate();
    }

    dateHelper.prototype.minDate = function (dates) {
        return moment(Math.min.apply(null, dates)).toDate();
    }

    dateHelper.prototype.format = function (date, formatString) {
        return moment(date).format(formatString);
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



 


