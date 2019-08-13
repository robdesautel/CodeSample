(function () {
    var module = angular.module("PoolReserve.Hotel");
    module.factory("HotelService", [
        '$http',
        'PoolReserveConstants',

    function ($http, PoolReservationConstants) {

        var AddHotel = function (hotelName, hotelAddress, hotelTax, lat, lon, image, isHidden) {
            return $http.post('api/Hotel/AddHotel', { Name: hotelName, Address: hotelAddress, TaxRate: hotelTax, Latitude: lat, Longitude: lon, Image: image, IsHidden: isHidden });
        };

        var SearchHotels = function (query, startingIndex, numberToGet, IncludeHidden) {
            return $http.post('api/Hotel/Search', { query: query, startingIndex: startingIndex, numberToGet: numberToGet, IncludeHidden: IncludeHidden });
        };

        var GetUsersInHotel = function (hotelId, startingIndex, numberToGet) {
            return $http.post('api/Hotel/Users', { HotelId: hotelId, StartsWith: startingIndex, NumberToGet: numberToGet });
        };

        var SearchUsersInHotel = function (hotelId, query, startingIndex, numberToGet) {
            return $http.post('api/Hotel/Users/Search', { HotelId: hotelId, Query: query, StartsWith: startingIndex, NumberToGet: numberToGet });
        };

        var GetUserInHotel = function (hotelId, userId) {
            return $http.get('api/Hotel/' + hotelId + '/ByUserId/' + userId);
        };

        var GetHotelPermissionsList = function () {
            return $http.get('api/Hotel/Permissions');
        };

        var GetPermissionsOfUserInHotel = function (hotelId) {
            return $http.get('api/Hotel/User/Permissions/' + hotelId);
        };

        var ChangeUsersPermissionsInHotel = function (hotelId, userId, permissionsId) {
            return $http.patch('api/Hotel/User/Permissions/', { HotelId: hotelId, UserIdToChange: userId, PermissionsId: permissionsId });
        };

        var AddUsersPermissionsInHotel = function (hotelId, userId, permissionsId) {
            return $http.post('api/Hotel/User/Permissions/', { HotelId: hotelId, UserIdToChange: userId, PermissionsId: permissionsId });
        };

        var EditHotel = function (id, name, address, taxrate, latitude, longitude, image, isHidden) {
            return $http.post('api/Hotel/EditHotel', { Id: id, Name: name, Address: address, TaxRate: taxrate, Latitude: latitude, Longitude: longitude, Image: image, IsHidden: isHidden });
        };

        var GetHotel = function (id) {
            return $http.get('api/Hotel?id=' + id);
        };


        return {
            AddHotel: AddHotel,
            EditHotel: EditHotel,
            GetHotel: GetHotel,
            SearchHotels: SearchHotels,
            GetUsersInHotel: GetUsersInHotel,
            SearchUsersInHotel: SearchUsersInHotel,
            GetUserInHotel: GetUserInHotel,
            GetHotelPermissionsList: GetHotelPermissionsList,
            GetPermissionsOfUserInHotel: GetPermissionsOfUserInHotel,
            ChangeUsersPermissionsInHotel: ChangeUsersPermissionsInHotel,
            AddUsersPermissionsInHotel: AddUsersPermissionsInHotel
        };
    }]);
}());