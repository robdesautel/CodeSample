(function () {
    var module = angular.module("PoolReserve.Venue");
    module.factory("VenueService", [
        '$http',
        'PoolReserveConstants',

    function ($http, PoolReservationConstants) {
        var GetVenue = function (id) {
            return $http.get('api/Reservation/Venue?id=' + id);
        };
        var AddVenue = function (venueName, hotelId, venueTypeId, isHidden) {
            return $http.post('api/Venue/AddVenue', { Name: venueName, HotelId: hotelId, VenueTypeId: venueTypeId, IsHidden: isHidden});
        };

        var EditVenue = function (venueId, venueName, isHidden) {
            return $http.put('api/Venue/EditVenue', { Id: venueId, Name: venueName, IsHidden: isHidden });
        };

        var getVenueTypes = function () {
            return $http.get('api/Venue/VenueTypes');
        };
        var getVenueItemsTypes = function (id) {
            return $http.get('api/VenueItems/ItemTypes/ByVenueType?venueTypeId=' + id);
        };

        var getVenuesForHotel = function (id) {
            return $http.get('api/Venue/ByHotelId?hotelId=' + id + "&includeHidden=true");
        };

        var getBlackoutsForVenue = function (startDate, endDate, id) {
            return $http.post('api/Venue/GetBlackoutForVenue', { StartDate: startDate, EndDate: endDate, VenueId: id });
        };

        var deleteBlackout = function (blackoutId) {
            return $http.delete('api/Venue/Blackout?blackoutId=' + blackoutId);
        };

        var getBlackout = function (blackoutId) {
            return $http.get('api/Venue/Blackout?blackoutId=' + blackoutId);
        };

        var editBlackout = function (blackoutId, startDate, endDate, venueId) {
            return $http.patch('api/Venue/Blackout', { Id: blackoutId, StartDate: startDate, EndDate: endDate, VenueId: venueId });
        };

        var addBlackout = function (startDate, endDate, venueId) {
            return $http.post('api/Venue/Blackout', {StartDate: startDate, EndDate: endDate, VenueId: venueId });
        };

        return {
            GetVenue: GetVenue,
            AddVenue: AddVenue,
            EditVenue: EditVenue,
            getVenueTypes: getVenueTypes,
            getVenueItemsTypes: getVenueItemsTypes,
            getVenuesForHotel: getVenuesForHotel,
            getBlackoutsForVenue: getBlackoutsForVenue,
            deleteBlackout: deleteBlackout,
            getBlackout: getBlackout,
            editBlackout: editBlackout,
            addBlackout: addBlackout

        };

    }]);
}());