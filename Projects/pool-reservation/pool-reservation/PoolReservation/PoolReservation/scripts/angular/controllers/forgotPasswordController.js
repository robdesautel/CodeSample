(function () {
    var app = angular.module("PoolReserve.Login");
    app.controller("ForgotPasswordController", [
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
            $scope.ResetPassword = function () {
                var email = $scope.email;

                var password = $scope.password;

                var confirmpassword = $scope.confirmpassword;

                if ($scope.token == null || $scope.token == "") {
                    showMissingCode();
                    return;
                }

                if (email == null || email == "") {
                    ToastService.ShowSimpleToast("Please enter your email");
                    return;
                }

                if (password == null || password == "") {
                    ToastService.ShowSimpleToast("Please enter a new password");
                    return;
                }

                if (confirmpassword == null || confirmpassword == "") {
                    ToastService.ShowSimpleToast("Please confirm your new password");
                    return;
                }

                if (confirmpassword !== password) {
                    ToastService.ShowSimpleToast("Your password's do not match. Please re-enter your passwords.");
                    return;
                }

                $scope.CurrentDisplayState = $scope.DisplayStates.LOADING;
                LoginService.ResetPassword($scope.token, email, password).then(onResetSuccess, onResetFailure);

            };

            var onResetSuccess = function (data) {
                $scope.CurrentDisplayState = $scope.DisplayStates.SUCCESS;
            };

            var onResetFailure = function (data) {
                if (data) {
                    if (data.data) {
                        if (data.data.Action == "invalidPassword") {
                            $scope.CurrentDisplayState = $scope.DisplayStates.ENTER_INFORMATION;
                            ToastService.ShowSimpleToast("Your password must be at least 8 characters long and contain uppercase, lowercase, and a digit.");
                            return;
                        }
                    }
                }

                $scope.CurrentDisplayState = $scope.DisplayStates.ENTER_INFORMATION;
                ToastService.ShowSimpleToast("Unable to change password. Please check your email and try again. If that does not work, please re-click the link in the email.");
            };

            var showMissingCode = function () {
                $scope.ErrMessage = "This link for resetting your password is invalid. If you are an app user, please open the app up and hit \"Forgot Password\" again. If you are a CMS user, please go to the login screen and hit \"Forgot Password\" again.";
                $scope.CurrentDisplayState = $scope.DisplayStates.MISSING_CODE;
            };

            $scope.DisplayStates = {
                ENTER_INFORMATION: "ENTER_INFORMATION",
                LOADING: "LOADING",
                MISSING_CODE: "MISSING_CODE",
                SUCCESS: "SUCCESS"
            };

            $scope.token = $location.search().token;
            $scope.ErrMessage = "";

            if ($scope.token == null || $scope.token == "") {
                showMissingCode();
                return;
            }

            $scope.CurrentDisplayState = $scope.DisplayStates.ENTER_INFORMATION;

        }]);
}());