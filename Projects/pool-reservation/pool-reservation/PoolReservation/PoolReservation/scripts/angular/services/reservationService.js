(function () {
    var module = angular.module("PoolReserve.Reservation");
    module.factory("ReservationService", [
        '$http',
        'PoolReserveConstants',

    function ($http, PoolReservationConstants) {

        var SearchUsers = function (userId, query, searchType, startDate, endDate) {
            return $http.post('api/Reservation/User/Search', { UserId: userId, Query: query, StartDate: startDate, EndDate: endDate, SearchType: searchType, });
        };

        var SearchHotels = function (hotelId, query, searchType, startDate, endDate) {
            return $http.post('api/Reservation/Hotel/Search', { HotelId: hotelId, Query: query, StartDate: startDate, EndDate: endDate, SearchType: searchType, });
        };

        var SearchAll = function (query, searchType, startDate, endDate) {
            return $http.post('api/Reservation/Search', { Query: query, StartDate: startDate, EndDate: endDate, SearchType: searchType, });
        };

       

        return {
            SearchUsers: SearchUsers,
            SearchHotels: SearchHotels,
            SearchAll: SearchAll
        };

    }]);
}());