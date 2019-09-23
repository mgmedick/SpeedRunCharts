function GetUniqueValues(data) {
    var uniqueTexts = [];
    var textsMap = {};

    for (var i = 0; i < data.length; i++) {
        var text = data[i];
        if (text !== undefined && textsMap[text] === undefined) {
            textsMap[text] = true;
            uniqueTexts.push(text);
        }
    }

    return uniqueTexts;
}

