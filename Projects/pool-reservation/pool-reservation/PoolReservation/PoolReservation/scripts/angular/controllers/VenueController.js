(function () {
    var app = angular.module("PoolReserve.Venue");
    app.controller("VenueController", [
        '$scope',
        '$state',
        'ToastService',
        'VenueService',
        'PoolReserveConstants', function (
        $scope,
        $state,
        ToastService,
        VenueService,
        PoolReserveConstants) {

            $scope.ItemsInVenue = [];


            var loadvenuetypes = function () {
                VenueService.getVenueTypes().then(onSuccess, onFail);
            };

            var onSuccess = function (data) {
                var venueTypes = data.data;

                if (venueTypes == null) {
                    return;
                }

                $scope.VenuesTypes = venueTypes;
            };

            var onFail = function (data) {
                var result = data.data;
                if (result == null) {
                    ToastService.ShowSimpleToast("An unknown error has occured. Please try again!");
                    return;
                }

                if (result.Action == "unknownFailure") {
                    ToastService.ShowSimpleToast("An unknown error has occured. Please try again!");
                    return;
                }
                else {
                    ToastService.ShowSimpleToast("An unknown error has occured. Please try again!");
                    return;
                }
            };

            $scope.LoadVenueItemTypes = function () {
                var selectedVenue = $scope.SelectedVenueType;

                if (selectedVenue == null) {
                    return;
                }


                var id = $scope.VenuesTypes[parseInt(selectedVenue)].Id;

                if (id == null) {
                    return;
                }

                VenueService.getVenueItemsTypes(id).then(onitemtypesSuccess, onitemtypesFail);

            };


            var onitemtypesSuccess = function (data) {
                var itemTypes = data.data;

                if (itemTypes == null) {
                    return;
                }

                $scope.ItemTypes = itemTypes;
            };

            var onitemtypesFail = function (data) {
                var result = data.data;
                if (result == null) {
                    ToastService.ShowSimpleToast("An unknown error has occured. Please try again!");
                    return;
                }

                if (result.Action == "unknownFailure") {
                    ToastService.ShowSimpleToast("An unknown error has occured. Please try again!");
                    return;
                }
                else {
                    ToastService.ShowSimpleToast("An unknown error has occured. Please try again!");
                    return;
                }
            };

            $scope.LoadItemTypes = function () {
                var selectedVenueItem = $scope.SelectedVenueItemType;

                if (selectedVenueItem == null) {
                    return;
                }


                var id = VenuesItemTypes[parseInt(selectedVenueItem)].Id;

                if (id == null) {
                    return;
                }


            };

            $scope.$watch("selectedVenueItem", function () {
                if ($scope.ItemTypes == null || $scope.selectedVenueItem == null) {
                    return;
                }

                $scope.completeSelectedVenueItem = $scope.ItemTypes[$scope.selectedVenueItem];
            });

            $scope.Venues = function () {

                if ($scope.venue == null || $scope.venue.name == null || $scope.venue.name === "") {
                    ToastService.ShowSimpleToast("You must enter a venue name.");
                    return;
                }
                if ($scope.venue.item == null || $scope.venue.item.name == null || $scope.venue.item.name === "") {
                    ToastService.ShowSimpleToast("You must enter an item name tax.");
                    return;
                }

                if ($scope.venue.item.price == null || $scope.venue.item.price === "") {
                    ToastService.ShowSimpleToast("You must set a price for the item.");
                    return;
                }


                VenueService.Venue($scope.venue.name, $scope.venue.item.name, $scope.venue.item.price).then(onCreateSuccess, onCreateFailure);

            };

            $scope.AddItemsToList = function () {
                $scope.ItemsInVenue.push({});
                return;

            };

            var onCreateSuccess = function (data) {
                $state.go(PoolReserveConstants.Pages.Base.Home.State);
            };

            var onCreateFailure = function (data) {
                var result = data.data;

                if (result == null) {
                    ToastService.ShowSimpleToast("An unknown error has occured. Please try again!");
                    return;
                }

                if (result.Action == "unknownFailure") {
                    ToastService.ShowSimpleToast("An unknown error has occured. Please try again!");
                    return;
                }
                else {
                    ToastService.ShowSimpleToast("An unknown error has occured. Please try again!");
                    return;
                }
            };
            loadvenuetypes();
            $scope.ItemsInVenue.push({});
            
        }]);
}());