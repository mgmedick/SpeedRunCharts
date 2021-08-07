
const getFormData = object => Object.keys(object).reduce((formData, key) => {
    formData.append(key, object[key]);
    return formData;
}, new FormData());

//function getFormData(object) {
//    const formData = new FormData();
//    Object.keys(object).forEach(key => formData.append(key, object[key]));
//    return formData;
//}

export { getFormData }

























