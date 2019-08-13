angular.module("PoolReserve.Hotel")
    .directive("manageEmployeeDetails", ["PoolReserveConstants", "HotelService", "ToastService", function (PoolReserveConstants, HotelService, ToastService) {
        return {
            restrict: 'E',
            templateUrl: PoolReserveConstants.Templates.Hotel.ManageEmployeeDetails,
            scope: {
                theOptions: "=theOptions"
            },
            link: function ($scope, element, attr) {
                $scope.UsersForList = [];

                $scope.permissionsChanged = false;

                $scope.loadData = function () {
                    $scope.permissionsChanged = false;
                    var userId = ($scope.theOptions || {}).UserId;
                    var hotelId = ($scope.theOptions || {}).HotelId;

                    if (userId == null || hotelId == null) {
                        return;
                    }

                    HotelService.GetUserInHotel(hotelId, userId).then(onUsersSuccess, onUsersError);
                };

                var onUsersSuccess = function (data) {
                    var user = data.data;

                    if (user == null) {
                        return null;
                    }

                    $scope.CurrentUser = user;
                };

                var onUsersError = function (data) {
                    ToastService.ShowSimpleToast("An unknown error has occured.");
                };

                $scope.$watch("theOptions", function () {
                    $scope.loadData();
                });

                $scope.loadPermissionsList = function () {
                    $scope.permissionsChanged = false;
                    HotelService.GetHotelPermissionsList().then(onListSuccess, onListError);
                };

                var onListSuccess = function (data) {
                    var list = data.data;

                    if (list == null) {
                        return;
                    }

                    $scope.PermissionsList = list;
                };

                var onListError = function (data) {
                    ToastService.ShowSimpleToast("An unknown error has occured.");
                };

                $scope.getCurrentUsersPermissionsInHotel = function () {
                    $scope.permissionsChanged = false;
                    var hotelId = ($scope.theOptions || {}).HotelId;

                    if (hotelId == null) {
                        return;
                    }


                    HotelService.GetPermissionsOfUserInHotel(hotelId).then(onCurrentPermissionsSuccess, onCurrentPermissionsError);
                };

                var onCurrentPermissionsSuccess = function(data){
                    var permissions = data.data;

                    if (permissions == null) {
                        return;
                    }

                    $scope.LoggedInUsersPermissions = permissions;

                };

                var onCurrentPermissionsError = function (data) {
                    ToastService.ShowSimpleToast("An unknown error has occured.");
                };

                $scope.PermissionsChanged = function () {
                    $scope.permissionsChanged = true;
                };

                $scope.CancelChanges = function () {
                    $scope.permissionsChanged = false;
                    $scope.loadData();
                };

                $scope.ChangeUsersPermissionsInHotel = function () {
                    $scope.permissionsChanged = false;

                    var hotelId = ($scope.theOptions || {}).HotelId;
                    
                    if (hotelId == null) {
                        return;
                    }

                    if($scope.CurrentUser == null || $scope.CurrentUser.User == null || $scope.CurrentUser.User.Id == null || $scope.CurrentUser.Permissions == null || $scope.CurrentUser.Permissions.Id == null){
                        return;
                    }

                    var userId = $scope.CurrentUser.User.Id;
                    var permissionsId = $scope.CurrentUser.Permissions.Id;


                    HotelService.ChangeUsersPermissionsInHotel(hotelId, userId, permissionsId).then(onPermissionsChangeSuccess, onPermissionsChangeError);
                };

                var onPermissionsChangeSuccess = function (data) {
                    ToastService.ShowSimpleToast("Permissions changed successfully");

                    var user = data.data;

                    if (user == null) {
                        return;
                    }

                    $scope.CurrentUser = user;

                    refreshPreviousScreen();
                };

                var onPermissionsChangeError = function (data) {
                    ToastService.ShowSimpleToast("Unable to update permissions for user.");
                    $scope.loadData();
                    refreshPreviousScreen();
                };

                var refreshPreviousScreen = function () {
                    var theFunc = ($scope.theOptions || {}).RefreshPreviousScreen;

                    if (theFunc) {
                        try {
                            theFunc();
                        } catch (ex) {

                        }
                    }
                };

                $scope.loadPermissionsList();
                $scope.getCurrentUsersPermissionsInHotel();


            }
        };
    }]).run(["WidgetService", "PoolReserveConstants", function (WidgetService, PoolReserveConstants) {
        var widgetData = {
            Id: PoolReserveConstants.Widgets.Hotel.ManageEmployeeDetails.Id,
            Invoker: PoolReserveConstants.Widgets.Hotel.ManageEmployeeDetails.Invoker,
            ShowOptions: { IsPanel: false }
        };

        WidgetService.RegisterWidget(widgetData);
    }]);