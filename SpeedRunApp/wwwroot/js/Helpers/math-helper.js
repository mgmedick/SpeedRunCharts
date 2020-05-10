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

    mathHelper.prototype.round = function (num, decimals) {
        return parseFloat(num.toFixed(decimals));
    }
};

sra["mathHelper"] = new mathHelper();



 


