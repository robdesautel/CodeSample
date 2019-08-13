angular.module("PoolReserve.Reservation")
    .directive("manageReservations", ["PoolReserveConstants",
        "ToastService", "ReservationService", function (PoolReserveConstants, ToastService, ReservationService) {
            return {
                restrict: 'E',
                templateUrl: PoolReserveConstants.Templates.Reservation.ManageReservations,
                scope: {
                    theOptions: "=theOptions"
                },
                link: function ($scope, element, attr) {
                    $scope.HotelMode = false;
                    $scope.UserMode = false;
                    $scope.AllMode = false;

                    $scope.DetermineAction = function () {
                        var hotelId = ($scope.theOptions || {}).HotelId;

                        if (hotelId != null) {
                            $scope.HotelMode = true;
                            return;
                        }

                        var userId = ($scope.theOptions || {}).UserId;

                        if (userId != null) {
                            $scope.UserMode = true;
                            return;
                        }

                        $scope.AllMode = true;
                    };

                    $scope.SearchReservations = function () {
                        if ($scope.SearchMode == null) {
                            $scope.InitializeSearchType();
                            return;
                        }
                        
                        if ($scope.SearchMode == null) {
                            ToastService.ShowSimpleToast("Please select a search type.");
                            return;
                        }

                        if ($scope.SearchMode == "HotelId" || $scope.SearchMode == "ReservationId") {
                            if ($scope.reservationSearchString != null) {
                                try{
                                    var result = parseInt($scope.reservationSearchString, 10);

                                    if (result == null || isNaN(result)) {
                                        throw null;
                                    }
                                } catch (ex) {
                                    ToastService.ShowSimpleToast("You must enter a valid number as the search string.");
                                    return;
                                }
                            }
                        }

                        if ($scope.StartDate == null || $scope.EndDate == null) {
                            $scope.InitializeDate();
                        }

                        if ($scope.StartDate == null || $scope.EndDate == null) {
                            return;
                        }

                        if ($scope.StartDate > $scope.EndDate) {
                            ToastService.ShowSimpleToast("The start date cannot be after the end date.");
                            return;
                        }


                        if ($scope.HotelMode) {
                            $scope.SearchHotels();
                        } else if ($scope.UserMode) {
                            $scope.SearchUsers();
                        } else if ($scope.AllMode) {
                            $scope.SearchAll();
                        }
                    };

                    $scope.SearchUsers = function () {

                        var userId = ($scope.theOptions || {}).UserId;

                        if(userId == null){
                            return;
                        }

                        ReservationService.SearchUsers(userId, $scope.reservationSearchString, $scope.SearchMode, $scope.StartDate, $scope.EndDate).then(onSearchSuccess, onSearchError);
                    };

                    $scope.SearchHotels = function () {
                        var hotelId = ($scope.theOptions || {}).HotelId;

                        if (hotelId == null) {
                            return;
                        }

                        ReservationService.SearchHotels(hotelId, $scope.reservationSearchString, $scope.SearchMode, $scope.StartDate, $scope.EndDate).then(onSearchSuccess, onSearchError);
                    };

                    $scope.SearchAll = function () {
                        ReservationService.SearchAll($scope.reservationSearchString, $scope.SearchMode, $scope.StartDate, $scope.EndDate).then(onSearchSuccess, onSearchError);
                    };

                    var onSearchSuccess = function (data) {

                        var results = data.data;

                        if (results == null) {
                            return;
                        }

                        $scope.ReservationsForList = results;

                        ToastService.ShowSimpleToast("Reservations loaded successfully");
                    };

                    var onSearchError = function (data) {
                        ToastService.ShowSimpleToast("Unable to load reservations");
                    };


                    $scope.InitializeDate = function () {
                        var numDaysBack = 10;

                        $scope.StartDate = new Date();
                        
                        $scope.StartDate.setDate($scope.StartDate.getDate() - numDaysBack);

                        $scope.EndDate = new Date();
                    };

                    $scope.InitializeSearchType = function () {
                        $scope.SearchMode = "ReservationId";
                    };

                    $scope.InitializeDate();
                    $scope.InitializeSearchType();
                    $scope.DetermineAction();
                   
                }
            };
        }]).run(["WidgetService", "PoolReserveConstants", function (WidgetService, PoolReserveConstants) {
            var widgetData = {
                Id: PoolReserveConstants.Widgets.Reservation.ManageReservations.Id,
                Invoker: PoolReserveConstants.Widgets.Reservation.ManageReservations.Invoker,
                ShowOptions: { IsPanel: false }
            };

            WidgetService.RegisterWidget(widgetData);
        }]);