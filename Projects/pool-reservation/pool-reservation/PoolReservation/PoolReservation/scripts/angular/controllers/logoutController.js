(function () {
    var app = angular.module("PoolReserve.Login");
    app.controller("LogoutController", [
        '$scope',
        '$state',
        'ToastService',
        'LoginService',
        '$location',
        'PoolReserveConstants',
        'RedirectService', function (
        $scope,
        $state,
        ToastService,
        LoginService,
        $location,
        PoolReserveConstants,
        RedirectService) {
            $scope.Revalidate = function () {
                LoginService.Logout().then(onLogoutSuccess, onLogoutFailure);

            };

            var onLogoutSuccess = function (data) {
                $state.go(PoolReserveConstants.Pages.Authentication.Login.State);
            };

            var onLogoutFailure = function (data) {
                $state.go(PoolReserveConstants.Pages.Authentication.Login.State);
            };

            $scope.Revalidate();

        }]);
}());