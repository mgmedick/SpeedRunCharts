
const getFormData = object => Object.keys(object).reduce((formData, key) => {
    formData.append(key, object[key]);
    return formData;
}, new FormData());

const getIntOrdinalString = value => {
    var result = '';
    var num = parseInt(value);

    if (num <= 0) {
        result = num.toString();
    }

    switch (num % 100) {
        case 11:
        case 12:
        case 13:
            result = num + "th";
            break;
    }

    switch (num % 10) {
        case 1:
            result = num + "st";
            break;
        case 2:
            result = num + "nd";
            break;
        case 3:
            result = num + "rd";
            break;
        default:
            result = num + "th";
            break;
    }

    return result;
}

const getDateDiffList = (datePart, startDate, endDate) => {
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

const formatTime = (timepart, value) => {
    var result;
    var date = moment().startOf('day');

    switch (timepart) {
        case 'milliseconds':
            result = date.milliseconds(value);
            break;
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
    } else if (date.seconds() > 0) {
        result = date.format("ss[s]")
    } else {
        result = date.format("SSS[ms]")
    }

    return result;
}

export { getFormData, getIntOrdinalString, getDateDiffList, formatTime }




























