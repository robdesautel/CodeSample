(function () {
    var module = angular.module("PoolReserve.Item");
    module.factory("ItemService", [
        '$http',
        'PoolReserveConstants',

    function ($http, PoolReservationConstants) {
        var getItemsForVenue = function (id) {
            return $http.get('api/VenueItems/ByVenueId?venueId=' + id + "&includeHidden=true");
        };

        var getItem = function (id) {
            return $http.get('api/VenueItems/?id=' + id);
        };

        var getItemTypesByVenueId = function (venueId) {
            return $http.get('api/VenueItems/ItemTypes?venueId=' + venueId);
        };

        var addItem = function (name, typeId, venueId, quantity, price, iconId, isHidden) {
            return $http.post('api/VenueItems/AddVenueItem', { ItemTypeId: typeId, Name: name, VenueId: venueId, NormalQuantity: quantity, Price: price, IconId : iconId, IsHidden: isHidden });
        };

        var editItem = function (id, name, price, iconId, isHidden) {
            return $http.patch('api/VenueItems/EditVenueItem', { Id: id, Name: name, Price: price, IconId: iconId, IsHidden: isHidden });
        };

        return {
            getItemsForVenue: getItemsForVenue,
            getItem: getItem,
            addItem: addItem,
            editItem: editItem,
            getItemTypesByVenueId: getItemTypesByVenueId
        };

    }]);
}());