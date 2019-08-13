angular.module("PoolReserve.Venue")
    .directive("manageVenue", ["PoolReserveConstants",
        "ToastService", "VenueService", function (PoolReserveConstants, ToastService, VenueService) {
            return {
                restrict: 'E',
                templateUrl: PoolReserveConstants.Templates.Venue.ManageVenue,
                scope: {
                    theOptions: "=theOptions"
                },
                link: function ($scope, element, attr) {

                    $scope.IsModified = false;
                    $scope.IsAdd = false;

                    $scope.Invalidate = function () {
                        $scope.IsModified = true;
                    };

                    $scope.Clean = function () {
                        $scope.IsModified = false;
                    };

                    $scope.DetermineAction = function () {
                        var id = ($scope.theOptions || {}).VenueId;

                        if (id != null) {
                            $scope.LoadVenue();
                        } else {
                            $scope.IsAdd = true;
                            $scope.LoadVenueTypes();
                        }


                    };

                    $scope.LoadVenueTypes = function () {
                        VenueService.getVenueTypes().then(onVenueTypesSuccess, onVenueTypesFailure);
                    };

                    var onVenueTypesSuccess = function (data) {

                        if (data.data == null || data.data.length == 0) {
                            ToastService.ShowSimpleToast("There are no Venue types available. Please refresh and try again!");
                            return;
                        }



                        $scope.VenueTypeList = data.data;

                        if ($scope.CurrentVenue == null) {
                            $scope.CurrentVenue = {};
                        }

                        $scope.CurrentVenue.Type = $scope.VenueTypeList[0];
                    };

                    var onVenueTypesFailure = function (data) {
                        ToastService.ShowSimpleToast("Unable to load Venue types. Please refresh and try again!");
                    };

                    $scope.LoadVenue = function () {
                        var id = ($scope.theOptions || {}).VenueId;

                        if (id == null) {
                            return;
                        }

                        VenueService.GetVenue(id).then(onLoadVenueSuccess, onLoadVenueError);

                    };

                    var onLoadVenueSuccess = function (data) {
                        $scope.Clean();

                        var Venue = data.data;

                        if (Venue == null) {
                            return;
                        }

                        $scope.OriginalVenue = Venue;

                        $scope.CloneOriginalVenue();
                    };

                    var onLoadVenueError = function () {
                        ToastService.ShowSimpleToast("Unable to load Venue.");
                    };


                    $scope.CloneOriginalVenue = function () {
                        if ($scope.OriginalVenue == null) {
                            return;
                        }

                        $scope.CurrentVenue = JSON.parse(JSON.stringify($scope.OriginalVenue));
                    };

                    $scope.AddVenue = function () {

                        var Venue = $scope.CurrentVenue;

                        var hotelId = $scope.theOptions.HotelId;

                        if (Venue == null || hotelId == null) {
                            ToastService.ShowSimpleToast("Unable to add Venue. Please try again.");
                            return;
                        }
                        $scope.Clean();

                        VenueService.AddVenue(Venue.Name, hotelId, Venue.Type.Id, Venue.IsHidden).then(onAddSuccess, onAddError);

                    };

                    var onAddSuccess = function (data) {
                        $scope.IsAdd = false;
                        ToastService.ShowSimpleToast("Venue added successfully.");

                        var Venue = data.data;

                        if (Venue == null) {
                            return;
                        }

                        $scope.OriginalVenue = Venue;
                        $scope.CloneOriginalVenue();
                        $scope.RefreshPreviousScreen();
                    };

                    var onAddError = function () {
                        $scope.Invalidate();
                        ToastService.ShowSimpleToast("Unable to add Venue. Please try again.");
                        $scope.RefreshPreviousScreen();
                    };


                    $scope.EditVenue = function () {
                        var Venue = $scope.CurrentVenue;

                        if (Venue == null) {
                            return;
                        }

                        $scope.Clean();
                        VenueService.EditVenue(Venue.Id, Venue.Name, Venue.IsHidden).then(onEditSuccess, onEditError);


                    };

                    var onEditSuccess = function (data) {
                        ToastService.ShowSimpleToast("Venue updated successfully.");

                        var Venue = data.data;

                        if (Venue == null) {
                            return;
                        }

                        $scope.OriginalVenue = Venue;
                        $scope.CloneOriginalVenue();
                        $scope.RefreshPreviousScreen();
                    };

                    var onEditError = function () {
                        $scope.Invalidate();
                        ToastService.ShowSimpleToast("Unable to edit Venue. Please try again.");
                        $scope.RefreshPreviousScreen();
                    };

                    $scope.DeleteVenue = function () {
                        var Venue = $scope.CurrentVenue;

                        if (Venue == null) {
                            return;
                        }


                    };

                    var onDeleteSuccess = function (data) {

                    };

                    var onDeleteError = function () {

                    };

                    $scope.CancelChanges = function () {
                        $scope.CloneOriginalVenue();
                        $scope.Clean();
                    };

                    $scope.CancelAddVenue = function () {
                        $scope.theOptions.DeleteWidget();
                    };


                    $scope.ShowItems = function () {
                        var venue = $scope.CurrentVenue;

                        if (venue == null) {
                            return null;
                        }

                        var id = venue.Id;
                        var name = venue.Name;

                        if (id == null || name == null) {
                            return;
                        }

                        $scope.theOptions.InvokeWidget(PoolReserveConstants.Widgets.Item.ManageItems.Id, { VenueId: id, VenueName: name });
                    };

                    $scope.ShowBlackouts = function () {
                        var venue = $scope.CurrentVenue;

                        if (venue == null) {
                            return null;
                        }

                        var id = venue.Id;
                        var name = venue.Name;

                        if (id == null || name == null) {
                            return;
                        }

                        $scope.theOptions.InvokeWidget(PoolReserveConstants.Widgets.Venue.ManageBlackouts.Id, { VenueId: id, VenueName: name });
                    };

                    $scope.RefreshPreviousScreen = function () {
                        var theFunc = ($scope.theOptions || {}).RefreshPreviousScreen;

                        if (theFunc) {
                            try{
                                theFunc();
                            } catch (ex) {

                            }
                            
                        }
                    };

                    $scope.DetermineAction();
                }
            };
        }]).run(["WidgetService", "PoolReserveConstants", function (WidgetService, PoolReserveConstants) {
            var widgetData = {
                Id: PoolReserveConstants.Widgets.Venue.ManageVenue.Id,
                Invoker: PoolReserveConstants.Widgets.Venue.ManageVenue.Invoker,
                ShowOptions: { IsPanel: false }
            };

            WidgetService.RegisterWidget(widgetData);
        }]);