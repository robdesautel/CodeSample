angular.module("PoolReserve.Hotel")
    .directive("manageHotels", ["PoolReserveConstants",
        "ToastService", "HotelService", function (PoolReserveConstants, ToastService, HotelService) {
        return {
            restrict: 'E',
            templateUrl: PoolReserveConstants.Templates.Hotel.ManageHotels,
            scope: {
                theOptions: "=theOptions"
            },
            link: function ($scope, element, attr) {

                $scope.SearchHotels = function () {
                    var query = $scope.hotelSearchString;

                    HotelService.SearchHotels(query, null, null, true).then(onHotelsSearchSuccess, onHotelsSearchError);

                };

                var onHotelsSearchSuccess = function (data) {
                    var hotels = data.data;

                    $scope.HotelsForList = hotels;
                };

                var onHotelsSearchError = function () {

                };

                $scope.RefreshScreen = function () {
                    $scope.SearchHotels();
                };

                $scope.ShowManageHotel = function (hotel) {
                    if (hotel == null) {
                        return;
                    }

                    var id = hotel.Id;
                    var name = hotel.Name;
                    if (id == null) {
                        return;
                    }

                    $scope.theOptions.InvokeWidget(PoolReserveConstants.Widgets.Hotel.ManageHotel.Id, { HotelId: id, HotelName: name, RefreshPreviousScreen: $scope.RefreshScreen });
                };

                var UpdatePermissions = function () {

                };

                $scope.$on(PoolReserveConstants.Notifications.Authentication.LOGIN_STATUS_CHANGED, function () {

                });
            }
        };
        }]).run(["WidgetService", "PoolReserveConstants", function (WidgetService, PoolReserveConstants) {
            var widgetData = {
                Id: PoolReserveConstants.Widgets.Hotel.ManageHotels.Id,
                Invoker: PoolReserveConstants.Widgets.Hotel.ManageHotels.Invoker,
                ShowOptions: { IsPanel: false }
            };

            WidgetService.RegisterWidget(widgetData);
        }]);