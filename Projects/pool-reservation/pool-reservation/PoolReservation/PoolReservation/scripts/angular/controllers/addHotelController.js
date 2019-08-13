(function () {
    var app = angular.module("PoolReserve.Hotel");
    app.controller("HotelController", [
        '$scope',
        '$state',
        'ToastService',
        'HotelService',
        'PoolReserveConstants', function (
        $scope,
        $state,
        ToastService,
        HotelService,
        PoolReserveConstants) {

            $scope.Hotel = function () {
              
                if ($scope.hotel == null || $scope.hotel.name == null || $scope.hotel.name === "") {
                    ToastService.ShowSimpleToast("You must enter a hotel name.");
                    return;
                }
                var taxRate = 0;
                if ($scope.hotel.taxRate == null || $scope.hotel.taxRate === "") {
                    ToastService.ShowSimpleToast("You must enter the hotel tax.");
                    return;
                }
                else {
                    taxRate = $scope.hotel.taxRate / 100;
                    if (taxRate < 0 || taxRate > 1) {
                        ToastService.ShowSimpleToast("tax rate must be between 0 and 100.");
                        return;
                    }
                }


                if ($scope.hotel.address == null || $scope.hotel.address === "") {
                    ToastService.ShowSimpleToast("You must enter a hotel address.");
                    return;
                }


                HotelService.AddHotel($scope.hotel.name, $scope.hotel.address, taxRate).then(onCreateSuccess, onCreateFailure);

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
        }]);
}());