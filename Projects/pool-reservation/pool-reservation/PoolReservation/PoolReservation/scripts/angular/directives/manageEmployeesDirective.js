angular.module("PoolReserve.Hotel")
    .directive("manageEmployees",
    ["PoolReserveConstants",
        "HotelService",
        "ToastService",


        function (PoolReserveConstants, HotelService, ToastService) {
        return {
            restrict: 'E',
            templateUrl: PoolReserveConstants.Templates.Hotel.ManageEmployees,
            scope: {
                theOptions: "=theOptions"
            },
            link: function ($scope, element, attr) {
                $scope.UsersForList = [];



                $scope.loadData = function () {
                    var id = ($scope.theOptions || {}).HotelId;

                    if (id == null) {
                        return;
                    }

                    HotelService.GetUsersInHotel(id, 0, 100).then(onUsersSuccess, onUsersError);
                };

                $scope.SearchUsers = function () {
                    var id = ($scope.theOptions || {}).HotelId;

                    if (id == null) {
                        return;
                    }

                    var query = $scope.userSearchString;

                    if (query == null || query == "") {
                        $scope.loadData();
                        return;
                    }

                    HotelService.SearchUsersInHotel(id, query, 0, 100).then(onUsersSuccess, onUsersError);
                };

                var onUsersSuccess = function (data) {
                    var users = data.data;

                    if (users == null) {
                        return null;
                    }

                    $scope.UsersForList = users;
                };

                var onUsersError = function (data) {
                    ToastService.ShowSimpleToast("An unknown error has occured.");
                };

                $scope.ShowDetails = function (user) {
                    if (user == null || user.User == null || $scope.theOptions == null) {
                        return;
                    }

                    var userId = user.User.Id;
                    var hotelId = $scope.theOptions.HotelId;

                    if (userId == null || hotelId == null) {
                        return null;
                    }

                    $scope.theOptions.InvokeWidget(PoolReserveConstants.Widgets.Hotel.ManageEmployeeDetails.Id, { HotelId: hotelId, UserId: userId, HotelName: $scope.theOptions.HotelName, RefreshPreviousScreen: $scope.RefreshScreen });
                };

                $scope.ShowAddUser = function () {
                    $scope.theOptions.InvokeWidget(PoolReserveConstants.Widgets.User.UserSelector.Id).then(function (data) {
                        if (data == null) {
                            return;
                        }

                        $scope.InitialAddUser(data);

                    });
                };

                $scope.InitialAddUser = function (user) {
                    if (user == null || user.Id == null) {
                        return;
                    }

                    var hotelId = ($scope.theOptions || {}).HotelId;
                    
                    if (hotelId == null) {
                        return;
                    }

                    HotelService.AddUsersPermissionsInHotel(hotelId, user.Id, 5).then(onInviteUserSuccess, onInviteUserError);
                };

                var onInviteUserSuccess = function (data) {
                    ToastService.ShowSimpleToast("User invited successfully!");

                    $scope.loadData();
                };

                var onInviteUserError = function (data) {
                    ToastService.ShowSimpleToast("Unable to invite users. Please ensure that the user that you are trying to add is not already inside the hotel, and that you have permission to invite users.");
                };

                $scope.getCurrentUsersPermissionsInHotel = function () {
                    $scope.permissionsChanged = false;
                    var hotelId = ($scope.theOptions || {}).HotelId;

                    if (hotelId == null) {
                        return;
                    }


                    HotelService.GetPermissionsOfUserInHotel(hotelId).then(onCurrentPermissionsSuccess, onCurrentPermissionsError);
                };

                var onCurrentPermissionsSuccess = function (data) {
                    var permissions = data.data;

                    if (permissions == null) {
                        return;
                    }

                    $scope.LoggedInUsersPermissions = permissions;

                };

                var onCurrentPermissionsError = function (data) {
                    ToastService.ShowSimpleToast("An unknown error has occured.");
                };

                $scope.RefreshScreen = function () {
                    $scope.loadData();
                    $scope.getCurrentUsersPermissionsInHotel();
                };

                $scope.loadData();
                $scope.getCurrentUsersPermissionsInHotel();

            }
        };
    }]).run(["WidgetService", "PoolReserveConstants", function (WidgetService, PoolReserveConstants) {
        var widgetData = {
            Id: PoolReserveConstants.Widgets.Hotel.ManageEmployees.Id,
            Invoker: PoolReserveConstants.Widgets.Hotel.ManageEmployees.Invoker,
            ShowOptions: { IsPanel: false }
        };

        WidgetService.RegisterWidget(widgetData);
    }]);