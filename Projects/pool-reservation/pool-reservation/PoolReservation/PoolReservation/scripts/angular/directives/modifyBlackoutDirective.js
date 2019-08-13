angular.module("PoolReserve.Venue")
    .directive("modifyBlackout", ["PoolReserveConstants", "ToastService", "VenueService",
        function (PoolReserveConstants, ToastService, VenueService) {
            return {
                restrict: 'E',
                templateUrl: PoolReserveConstants.Templates.Venue.ModifyBlackout,
                scope: {
                    theOptions: "=theOptions"
                },
                link: function ($scope, element, attr) {

                    $scope.IsModified = false;
                    $scope.IsAdd = false;

                    $scope.DetermineAction = function () {
                        var blackoutId = ($scope.theOptions || {}).BlackoutId;

                        if (blackoutId) {
                            $scope.LoadBlackout();
                        } else {
                            $scope.IsAdd = true;
                            $scope.CurrentBlackout = {};
                        }
                    };

                    $scope.Invalidate = function () {
                        $scope.IsModified = true;
                    };

                    $scope.Clean = function () {
                        $scope.IsModified = false;
                    };

                    $scope.LoadBlackout = function () {
                        

                        var blackoutId = ($scope.theOptions || {}).BlackoutId;

                        if (blackoutId == null) {
                            ToastService.ShowSimpleToast("An unknown error has occured loading blackout.");
                            return;
                        }
                        $scope.Clean();
                        VenueService.getBlackout(blackoutId).then(onBlackoutSuccess, onBlackoutError);

                    };

                    var onBlackoutSuccess = function (data) {
                        var blackout = data.data;

                        if (blackout == null) {
                            return;
                        }

                       


                        
                        $scope.OriginalBlackout = blackout;

                        $scope.CloneFromOriginal();
                    };

                    $scope.CloneFromOriginal = function () {
                        if ($scope.OriginalBlackout == null) {
                            return;
                        }
                        $scope.CurrentBlackout = JSON.parse(JSON.stringify($scope.OriginalBlackout));

                        $scope.CurrentBlackout.StartDate = new Date($scope.CurrentBlackout.StartDate);
                        $scope.CurrentBlackout.EndDate = new Date($scope.CurrentBlackout.EndDate);
                    };

                    var onBlackoutError = function () {
                        ToastService.ShowSimpleToast("An unknown error has occured loading blackout.");
                    };

                    $scope.CancelChanges = function () {
                        $scope.CloneFromOriginal();
                        $scope.Clean();
                    };

                    $scope.SaveEdits = function () {
                        
                        var blackoutId = ($scope.CurrentBlackout || {}).Id;

                        if (blackoutId == null) {
                            ToastService.ShowSimpleToast("Unable to edit blackout. Please try again.");
                            return;
                        }
                        $scope.Clean();
                        VenueService.editBlackout(blackoutId, $scope.CurrentBlackout.StartDate, $scope.CurrentBlackout.EndDate, $scope.CurrentBlackout.VenueId).then(onBlackoutEditSuccess, onBlackoutEditError);

                    };

                    var onBlackoutEditSuccess = function (data) {
                        ToastService.ShowSimpleToast("Blackout edited successfully!");

                        var blackout = data.data;

                        if (blackout == null) {
                            $scope.LoadBlackout();
                            return;
                        }

                        $scope.OriginalBlackout = blackout;

                        $scope.CloneFromOriginal();
                    };

                    var onBlackoutEditError = function () {
                        $scope.Invalidate();
                        ToastService.ShowSimpleToast("Unable to edit blackout. Please try again.");
                    };

                    $scope.AddBlackout = function () {
                        
                        var venueId = ($scope.theOptions || {}).VenueId;

                        if (venueId == null) {
                            ToastService.ShowSimpleToast("Unable to add blackout. Please try again.");
                            return;
                        }
                        $scope.Clean();
                        VenueService.addBlackout($scope.CurrentBlackout.StartDate, $scope.CurrentBlackout.EndDate, venueId).then(onBlackoutAddSuccess, onBlackoutAddError);

                    };

                    var onBlackoutAddSuccess = function (data) {
                        ToastService.ShowSimpleToast("Blackout added successfully!");

                        var realBlackout = data.data;

                        if (realBlackout == null) {
                            return;
                        }

                        $scope.OriginalBlackout = realBlackout;
                        $scope.CloneFromOriginal();

                        $scope.IsAdd = false;
                    };

                    var onBlackoutAddError = function () {
                        $scope.IsAdd = true;
                        $scope.Invalidate();
                        ToastService.ShowSimpleToast("Unable to add blackout. Please try again.");
                    };

                    $scope.DeleteBlackout = function () {
                        var blackoutId = ($scope.CurrentBlackout || {}).Id;

                        if (blackoutId == null) {
                            ToastService.ShowSimpleToast("Unable to delete blackout. Please try again.");
                            $scope.LoadBlackouts();
                            return;
                        }

                        $scope.Clean();
                        VenueService.deleteBlackout(blackoutId).then(onBlackoutDeleteSuccess, onBlackoutDeleteError);

                    };

                    var onBlackoutDeleteSuccess = function (data) {
                        ToastService.ShowSimpleToast("Blackout deleted successfully!");
                        $scope.theOptions.DeleteWidget();
                    };

                    var onBlackoutDeleteError = function () {
                        ToastService.ShowSimpleToast("Unable to delete blackout. Please try again.");
                        $scope.LoadBlackout();
                    };

                    

                    $scope.DetermineAction();
                }
            };
        }]).run(["WidgetService", "PoolReserveConstants", function (WidgetService, PoolReserveConstants) {
            var widgetData = {
                Id: PoolReserveConstants.Widgets.Venue.ModifyBlackout.Id,
                Invoker: PoolReserveConstants.Widgets.Venue.ModifyBlackout.Invoker,
                ShowOptions: { IsPanel: false }
            };

            WidgetService.RegisterWidget(widgetData);
        }]);