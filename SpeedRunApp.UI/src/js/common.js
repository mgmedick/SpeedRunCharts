
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

export { getFormData, getIntOrdinalString }




























