(function () {
    var app = angular.module("PoolReserve.Login");
    app.controller("RevalidateController", [
        '$scope',
        '$state',
        'ToastService',
        'LoginService',
        '$location',
        'PoolReserveConstants',
        'RedirectService',
        '$timeout', function (
        $scope,
        $state,
        ToastService,
        LoginService,
        $location,
        PoolReserveConstants,
        RedirectService,
        $timeout) {
            $scope.Revalidate = function () {
                LoginService.Revalidate().then(onRevalSuccess, onRevalFailure);

            };

            $scope.stop = true;

            var onRevalSuccess = function (data) {
                var specialUrl = RedirectService.GetSpecialUrl();

                if (specialUrl == null) {
                    $state.go(PoolReserveConstants.Pages.Hotel.ManageHotels.State);
                    return;
                }

                $location.url(specialUrl);
            };

            var onRevalFailure = function (data) {
                $state.go(PoolReserveConstants.Pages.Authentication.Login.State);

            };

            $scope.Revalidate();

        }]);
}());