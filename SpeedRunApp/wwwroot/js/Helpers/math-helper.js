if (!sra)
    var sra = {};

function mathHelper () {
    mathHelper.prototype.getMedian = function (data) {
        var items = $(data).sort(function (a, b) {
            return a - b;
        });

        var middle = Math.floor((items.length - 1) / 2);
        if (items.length % 2) {
            return items[middle];
        } else {
            return (items[middle] + items[middle + 1]) / 2.0;
        }
    }

    mathHelper.prototype.getAverage = function (data) {
        var total = 0;
        for (var i = 0; i < data.length; i++) {
            total += data[i];
        }

        return this.round((total / data.length), 2);
    }

    mathHelper.prototype.getMin = function (data) {
        return Math.min.apply(null, data);
    }

    mathHelper.prototype.getMax = function (data) {
        return Math.max.apply(null, data);
    }

    mathHelper.prototype.round = function (num, decimals) {
        return parseFloat(num.toFixed(decimals));
    }

    mathHelper.prototype.getIntOrdinalString = function (value) {
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
};

sra["mathHelper"] = new mathHelper();



 


