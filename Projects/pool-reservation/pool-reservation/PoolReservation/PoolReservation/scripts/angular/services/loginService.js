(function () {
    var module = angular.module("PoolReserve.Login");
    module.factory("LoginService",
                    ['$http',
                     '$q',
                     '$rootScope',
                     'PoolReserveConstants',

        function ($http, $q, $rootScope, PoolReserveConstants) {

            var CurrentUser = null;

            var Login = function (email, password, rememberme) {
                var deferred = $q.defer();

                $http.post('api/Authentication/Login', { Email: email, Password: password, RememberMe: rememberme }).then(
                    function (data) {
                        onLoginSuccess(data, deferred);
                    },
                    function (data) {
                        onLoginFailure(data, deferred);
                    }
                );

                return deferred.promise;
            };

            var Register = function (email, password, firstname, lastname, phonenumber, image) {
                var deferred = $q.defer();

                $http.post('api/Authentication/Register', { Email: email, Password: password, FirstName: firstname, LastName: lastname, PhoneNumber: phonenumber, Image: image }).then(
                    function (data) {
                        onRegistrationSuccess(data, deferred);
                    },
                    function (data) {
                        onRegistrationFailure(data, deferred);
                    }
                );

                return deferred.promise;
            };

            var Revalidate = function () {
                var deferred = $q.defer();

                $http.get('api/Authentication/Revalidate').then(
                    function (data) {
                        onRevalidateSuccess(data, deferred);
                    },
                    function (data) {
                        onRevalidateFailure(data, deferred);
                    }
                );

                return deferred.promise;
            };

            var Logout = function () {
                var deferred = $q.defer();

                $http.get('api/Authentication/Logout').then(
                    function (data) {
                        onLogoutSuccess(data, deferred);
                    },
                    function (data) {
                        onLogoutFailure(data, deferred);
                    }
                );

                return deferred.promise;
            };


            var GetCurrentUser = function () {
                if (CurrentUser != null) {
                    return JSON.parse(JSON.stringify(CurrentUser));
                } else {
                    return null;
                }


            };

            var IsUserLoggedIn = function () {
                return CurrentUser != null;
            };

            var ResetPassword = function (token, email, password) {
                return $http.post('api/Authentication/ForgotPasswordWithToken', { Token: token, Email: email, Password: password });
            };

            var SendResetPasswordEmail = function (email) {
                return $http.get('api/Authentication/ForgotPassword?email=' + email);
            };


            //Private methods

            var onLogoutSuccess = function (data, deferred) {
                ClearAllExistingUserData();
                deferred.resolve(data);
                notifyLoginChange();
            };

            var onLogoutFailure = function (data, deferred) {
                ClearAllExistingUserData();
                deferred.reject(data);
                notifyLoginChange();
            };


            var onRevalidateSuccess = function (data, deferred) {
                ClearAllExistingUserData();

                var result = data.data;

                if (result == null) {
                    deferred.reject(data);
                    notifyLoginChange();
                    return;
                }

                CurrentUser = result;

                deferred.resolve(data);
                notifyLoginChange();
            };

            var onRevalidateFailure = function (data, deferred) {
                if (data == null) {
                    return;
                }

                if (data.status == 401 || data.status == 403) {
                    ClearAllExistingUserData();
                    deferred.reject(data);
                    notifyLoginChange();
                    return;
                }

                deferred.reject(data);
            };

            var onLoginSuccess = function (data, deferred) {
                ClearAllExistingUserData();

                var result = data.data;

                if (result == null) {
                    deferred.reject(data);
                    notifyLoginChange();
                    return;
                }

                CurrentUser = result;

                deferred.resolve(data);
                notifyLoginChange();
            };

            var onLoginFailure = function (data, deferred) {
                ClearAllExistingUserData();
                deferred.reject(data);
                notifyLoginChange();
            };

            var onRegistrationSuccess = function (data, deferred) {
                ClearAllExistingUserData();

                var result = data.data;

                if (result == null) {
                    deferred.reject(data);
                    notifyLoginChange();
                    return;
                }

                CurrentUser = result;

                deferred.resolve(data);
                notifyLoginChange();
            };

            var onRegistrationFailure = function (data, deferred) {
                ClearAllExistingUserData();
                deferred.reject(data);
                notifyLoginChange();
            };

            var ClearAllExistingUserData = function () {
                CurrentUser = null;
            };

            var notifyLoginChange = function () {
                $rootScope.$broadcast(PoolReserveConstants.Notifications.Authentication.LOGIN_STATUS_CHANGED);
            };

            return {
                Login: Login,
                Register: Register,
                GetCurrentUser: GetCurrentUser,
                IsUserLoggedIn: IsUserLoggedIn,
                Revalidate: Revalidate,
                Logout: Logout,
                ResetPassword: ResetPassword,
                SendResetPasswordEmail: SendResetPasswordEmail
            };

        }]);
}());