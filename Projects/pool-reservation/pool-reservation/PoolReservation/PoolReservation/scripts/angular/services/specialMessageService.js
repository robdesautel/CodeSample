(function () {
    var module = angular.module("PoolReserve.Venue");
    module.factory("SpecialMessageService", [
        '$http',
        'PoolReserveConstants',

        function ($http, PoolReservationConstants) {
            var NewMessage = function (messageTitle, messageBody, venueId) {
                return $http.post('api/Miscellaneous/AddNewMessage', { PageName: messageTitle, PageData: messageBody, VenueId: venueId });
            };

            var ExisitingMessage = function (venueId) {
                return $http.get('api/Miscellaneous/GetSpecialMessageByVenueId?venueId=' + venueId);
            };

            return {
                NewMessage: NewMessage,
                ExisitingMessage: ExisitingMessage
            };
        }]);
}());