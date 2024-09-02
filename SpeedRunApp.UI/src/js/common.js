import { Toast } from 'bootstrap';
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

const getDateLocalString = (value) => {
    var result = dayjs(value).format("YYYY-MM-DD");
    
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

const successToast = (successMsg) => {
    var el = document.getElementById('successtoast').cloneNode(true);
    el.querySelector('.msg-text').innerHTML = successMsg;
    document.getElementById('toastcontainer').appendChild(el);
    new Toast(el).show(); 
}

const errorToast = (errorMsg) => {
    var el = document.getElementById('errortoast').cloneNode(true);
    el.querySelector('.msg-text').innerHTML = errorMsg;
    document.getElementById('toastcontainer').appendChild(el);
    new Toast(el).show();
}

const resizeTabs = () => {
    var rows = document.querySelectorAll('.tab-list');

    for (var g = 0; g < rows.length; g++) {
        var totalWidth = 0;
        var tabitems = rows[g].querySelectorAll('li.nav-item');
        var morediv =  rows[g].querySelector('.more');
        var morebtn =  morediv.querySelector('.btn');
        var moreItems = morediv.querySelectorAll('li');

        for (var i = 0; i < tabitems.length; i++) {
            tabitems[i].style.left = "-10000px";
            tabitems[i].classList.remove('d-none');
            totalWidth += tabitems[i].offsetWidth;
            if (totalWidth > ((rows[g].offsetWidth - tabitems[i].offsetWidth) - 30)) {
                tabitems[i].classList.add('d-none');
                moreItems[i].classList.remove('d-none');
            } else {
                tabitems[i].classList.remove('d-none');
                moreItems[i].classList.add('d-none');                     
            }
        }

        var items = Array.from(morediv.querySelectorAll('li:not(.d-none) a'));
        if (items.length > 0) {
            var item = items.find(i=>i.classList.contains('active'));
            if (item) {
                morebtn.innerHTML = item.innerHTML;
                morebtn.classList.add('active');
            } else {
                morebtn.innerHTML = 'More...';
                morebtn.classList.remove('active');
            }
            
            morediv.style.display = 'block';
        } else {
            morediv.style.display = 'none';                       
        }
    }
}

export { getFormData, getIntOrdinalString, getDateDiffList, formatTime, getDateTimeLocalString, getDateLocalString, isValidDate, escapeHtml, formatFileName, setCookie, getCookie, successToast, errorToast, resizeTabs }




























