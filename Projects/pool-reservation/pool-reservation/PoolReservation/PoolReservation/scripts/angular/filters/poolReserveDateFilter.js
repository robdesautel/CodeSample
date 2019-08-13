angular.module('PoolReserve').filter('poolReserveDate', ['$filter', function ($filter) {
    return function (possibleDateObject, dateFormatString) {

        var bad = "Invalid Date";

        if (possibleDateObject == null) {
            return bad;
        }

        if (dateFormatString == null) {
            return bad;
        }

        var dateObj = null;

        if (Object.prototype.toString.call(possibleDateObject) === '[object Date]') {
            dateObj = possibleDateObject;
        } else {
            try{
                dateObj = new Date(possibleDateObject);
            } catch (ex) {
                return bad;
            }
        }
        try {
            return $filter('date')(dateObj, dateFormatString);
        } catch (ex) {
            return bad;
        }

    }
}]);