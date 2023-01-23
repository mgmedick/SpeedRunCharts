const dayjs = require('dayjs');

const getFormData = object => Object.keys(object).reduce((formData, key) => {
    if (Array.isArray(object[key])) {
        for (var i = 0; i < object[key].length; i++) {
            formData.append(key, object[key][i]);
        }
    } else {
        formData.append(key, object[key]);
    }
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
                startDate = dayjs(startDate).add(1, 'day').toDate();
            }
            break;
        case "month":
            var startMonthYear = new Date(startDate.getFullYear(), startDate.getMonth(), 1);
            var endMonthYear = new Date(endDate.getFullYear(), endDate.getMonth(), 1);
            while (startMonthYear <= endMonthYear) {
                dates.push(startMonthYear);
                startMonthYear = dayjs(startMonthYear).add(1, 'month').toDate();
            }
            break;
    }

    return dates;
}

const formatTime = (timepart, value) => {
    var result;
    var date = dayjs().startOf('day').add(value, timepart);

    if (date.hour() > 0) {
        result = date.format("hh[h] mm[m] ss[s]")
    } else if (date.minute() > 0) {
        result = date.format("mm[m] ss[s]")
    } else if (date.second() > 0) {
        result = date.format("ss[s]")
    }

    if (date.millisecond() > 0) {
        result = result + " " + date.millisecond() + "ms";
    }    

    return result;
}

const getDateTimeLocalString = (value) => {
    var result = dayjs(value).format("YYYY-MM-DDTHH:mm:ss");
    
    return result;
}

const isValidDate = (date, format) => {
    return dayjs(date, format).format(format) === date;
}

const escapeHtml = (value) => {
    return value.replace("&", "&amp;")
                .replace("<", "&lt;")
                .replace(">", "&gt;")
                .replace("\"", "&quot;")
                .replace("\'", "&#039;");
    // let div = document.createElement('div');
    // div.innerText = value;
    // return div.innerHTML;            
}

const formatFileName = (value) => {
    return value.replaceAll("[\\\\/:*?\"<>|]", "");           
}

const setCookie = (key, value, days) => {
    var expires = new Date();
    var cookieString = key + '=' + value;
    if (days) {
        expires.setTime(expires.getTime() + (days * 24 * 60 * 60 * 1000));
        cookieString += ';expires=' + expires.toUTCString();
    }
    cookieString += ';path=/';
    
    document.cookie = cookieString;
}

const getCookie = (key) => {
    var keyValue = document.cookie.match('(^|;) ?' + key + '=([^;]*)(;|$)');
    return keyValue ? keyValue[2] : null;
}

export { getFormData, getIntOrdinalString, getDateDiffList, formatTime, getDateTimeLocalString, isValidDate, escapeHtml, formatFileName, setCookie, getCookie }




























