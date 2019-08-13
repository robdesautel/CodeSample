(function () {
    var app = angular.module("PoolReserve.Login");
    app.controller("LoginController", [
        '$scope',
        '$state',
        '$location',
        'ToastService',
        'LoginService',
        'PoolReserveConstants',
        'RedirectService', function (
        $scope,
        $state,
        $location,
        ToastService,
        LoginService,
        PoolReserveConstants,
        RedirectService) {
            $scope.LoginUser = function () {

                if ($scope.user == null || $scope.user.mail == null || $scope.user.mail === "") {
                    ToastService.ShowSimpleToast("You must enter a valid email address to login.");
                    return;
                }

                if ($scope.user.password == null || $scope.user.password === "") {
                    ToastService.ShowSimpleToast("You must enter a password to login.");
                    return;
                }

                LoginService.Login($scope.user.mail, $scope.user.password, $scope.user.rememberme || false).then(onLoginSuccess, onLoginFailure);

            };

            var onLoginSuccess = function (data) {
                var specialUrl = RedirectService.GetSpecialUrl();

                if (specialUrl == null) {
                    $state.go(PoolReserveConstants.Pages.Hotel.ManageHotels.State);
                }

                $location.url(specialUrl);
            };

            var onLoginFailure = function (data) {
                var result = data.data;

                if (result == null) {
                    ToastService.ShowSimpleToast("An unknown error has occured. Please try again!");
                    return;
                }

                if (result.Action == "invalidCredentials") {
                    ToastService.ShowSimpleToast("Invalid email/password!");
                    return;
                } else if (result.Action == "verificationRequired") {
                    ToastService.ShowSimpleToast("Your email must be verified before logging in.");
                    return;
                } else if (result.Action == "lockedOut") {
                    ToastService.ShowSimpleToast("Your account has been locked out due to too many failed login attempts. Please wait a while and try again later.");
                    return;
                } else {
                    ToastService.ShowSimpleToast("An unknown error has occured. Please try again!");
                    return;
                }
            };

        }]);
}());