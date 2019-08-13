angular.module("PoolReserve.Venue")
    .directive("manageBlackouts", ["PoolReserveConstants", "ToastService", "VenueService",
        function (PoolReserveConstants, ToastService, VenueService) {
            return {
                restrict: 'E',
                templateUrl: PoolReserveConstants.Templates.Venue.ManageBlackouts,
                scope: {
                    theOptions: "=theOptions"
                },
                link: function ($scope, element, attr) {

                    $scope.LoadBlackouts = function () {
                        var venueId = ($scope.theOptions || {}).VenueId;

                        if (venueId == null) {
                            ToastService.ShowSimpleToast("An unknown error has occured loading blackouts.");
                            return;
                        }

                        var startDate = new Date();
                        startDate.setUTCHours(0, 0, 0, 0);

                        var endDate = new Date(new Date().setFullYear(startDate.getFullYear() + 1))

                        VenueService.getBlackoutsForVenue(startDate, endDate, venueId).then(onBlackoutsSuccess, onBlackoutsError);

                    };

                    var onBlackoutsSuccess = function (data) {
                        var blackouts = data.data;

                        if (blackouts == null) {
                            return;
                        }

                        $scope.BlackoutsForList = blackouts;
                    };

                    var onBlackoutsError = function () {
                        ToastService.ShowSimpleToast("An unknown error has occured loading blackouts.");
                    };

                    $scope.EditBlackout = function (blackout) {
                        if (blackout == null) {
                            return;
                        }

                        var venueId = ($scope.theOptions || {}).VenueId;

                        if (venueId == null) {
                            return;
                        }

                        var id = blackout.Id;

                        if (id == null) {
                            return;
                        }

                        $scope.theOptions.InvokeWidget(PoolReserveConstants.Widgets.Venue.ModifyBlackout.Id, { BlackoutId: id, VenueId: venueId, VenueName: $scope.theOptions.VenueName });
                    };

                    $scope.AddBlackout = function () {
                        var venueId = ($scope.theOptions || {}).VenueId;

                        if (venueId == null) {
                            return;
                        }

                        $scope.theOptions.InvokeWidget(PoolReserveConstants.Widgets.Venue.ModifyBlackout.Id, { VenueId: venueId, VenueName: $scope.theOptions.VenueName });
                    };


                    $scope.DeleteBlackout = function (blackout) {
                        var blackoutId = (blackout || {}).Id;

                        if (blackoutId == null) {
                            ToastService.ShowSimpleToast("Unable to delete blackout. Please try again.");
                            $scope.LoadBlackouts();
                            return;
                        }

                        VenueService.deleteBlackout(blackoutId).then(onBlackoutDeleteSuccess, onBlackoutDeleteError);

                    };

                    var onBlackoutDeleteSuccess = function (data) {
                        ToastService.ShowSimpleToast("Blackout deleted successfully!");
                        $scope.LoadBlackouts();
                    };

                    var onBlackoutDeleteError = function () {
                        ToastService.ShowSimpleToast("Unable to delete blackout. Please try again.");
                        $scope.LoadBlackouts();
                    };



                    $scope.LoadBlackouts();
                }
            };
        }]).run(["WidgetService", "PoolReserveConstants", function (WidgetService, PoolReserveConstants) {
            var widgetData = {
                Id: PoolReserveConstants.Widgets.Venue.ManageBlackouts.Id,
                Invoker: PoolReserveConstants.Widgets.Venue.ManageBlackouts.Invoker,
                ShowOptions: { IsPanel: false }
            };

            WidgetService.RegisterWidget(widgetData);
        }]);