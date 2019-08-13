(function () {
    var app = angular.module("PoolReserve.Login");
    app.controller("SendForgotPasswordController", [
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
            $scope.SendForgotPasswordEmail = function () {
                var email = $scope.email;

                if (email == null || email == "") {
                    ToastService.ShowSimpleToast("Please enter your email");
                    return;
                }
                $scope.CurrentDisplayState = $scope.DisplayStates.LOADING;
                LoginService.SendResetPasswordEmail(email).then(onSendSuccess, onSendFailure);

            };

            var onSendSuccess = function (data) {
                $scope.CurrentDisplayState = $scope.DisplayStates.SUCCESS;
            };

            var onSendFailure = function (data) {
                ToastService.ShowSimpleToast("An unknown error has occured. Please try again.");
            };

            $scope.DisplayStates = {
                ENTER_INFORMATION: "ENTER_INFORMATION",
                LOADING: "LOADING",
                SUCCESS: "SUCCESS"
            };

            $scope.CurrentDisplayState = $scope.DisplayStates.ENTER_INFORMATION;

        }]);
}());