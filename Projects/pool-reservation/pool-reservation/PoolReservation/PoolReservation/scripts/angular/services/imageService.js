(function () {
    var module = angular.module("PoolReserve.Image");
    module.factory("ImageService", [
        '$http',
        'PoolReserveConstants',

    function ($http, PoolReservationConstants) {
        var getAllIcons = function () {
            return $http.get('api/Admin/Icons');
        };

        var uploadIcon = function (file) {
            return $http.post('api/Admin/Icons', file);
        };

        return {
            getAllIcons: getAllIcons,
            uploadIcon: uploadIcon
        };

    }]);
}());