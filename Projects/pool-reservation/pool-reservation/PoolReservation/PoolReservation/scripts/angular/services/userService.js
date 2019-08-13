(function () {
    var module = angular.module("PoolReserve.User");
    module.factory("UserService", [
        '$http',
        'PoolReserveConstants',
    function ($http, PoolReservationConstants) {

        var SearchMinimalUsers = function (query) {
            return $http.get('api/User/Search/Minimal?query=' + query);
        };



        return {
            SearchMinimalUsers: SearchMinimalUsers
        };

    }]);
}());