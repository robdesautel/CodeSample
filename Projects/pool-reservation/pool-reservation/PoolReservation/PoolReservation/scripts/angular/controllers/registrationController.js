(function () {
    var app = angular.module("PoolReserve.Login");
    app.controller("RegistrationController", [
        '$scope',
        '$state',
        'ToastService',
        'LoginService',
        'PoolReserveConstants',
        'OutgoingFile', function (
        $scope,
        $state,
        ToastService,
        LoginService,
        PoolReserveConstants,
        OutgoingFile) {

            var fileAdded = function (item) {
                var file = item;

                var reader = new FileReader();

                // Closure to capture the file information.
                reader.onload = (function (theFile) {
                    return function (e) {
                        $scope.HasUploaded = true;
                        $scope.PictureUrl = e.target.result;
                        $scope.PictureFile = new OutgoingFile(file.name, file.size, $scope.PictureUrl);
                        $scope.$apply();
                    };
                })(file);

                // Read in the image file as a data URL.
                reader.readAsDataURL(file);
            };

            $scope.ImageChanged = function (file) {
                fileAdded(file);
            };

            $scope.DeleteProfilePicture = function () {
                $scope.PictureUrl = null;
                $scope.PictureFile = null;
            };
                

            $scope.Register = function () {
                if ($scope.user == null || $scope.user.firstname == null || $scope.user.firstname === "") {
                    ToastService.ShowSimpleToast("You must enter your first name.");
                    return;
                }

                if ($scope.user.lastname == null || $scope.user.lastname === "") {
                    ToastService.ShowSimpleToast("You must enter your last name.");
                    return;
                }

                if ($scope.user.mail == null || $scope.user.mail === "") {
                    ToastService.ShowSimpleToast("You must enter a valid email address to register.");
                    return;
                }

                if ($scope.user.password == null || $scope.user.password === "") {
                    ToastService.ShowSimpleToast("You must enter a password.");
                    return;
                }

                if ($scope.user.password.length < 8) {
                    ToastService.ShowSimpleToast("Your password must be at least 8 characters long and contain uppercase, lowercase, and a digit.");
                    return;
                }

                if ($scope.user.confirmpassword == null || $scope.user.confirmpassword === "") {
                    ToastService.ShowSimpleToast("You must confirm your password.");
                    return;
                }

                if ($scope.user.password != $scope.user.confirmpassword) {
                    ToastService.ShowSimpleToast("Passwords do not match. Please reenter your password.");
                    return;
                }

                

                if ($scope.user.phonenumber == null || $scope.user.phonenumber === "") {
                    ToastService.ShowSimpleToast("You must enter your phone number.");
                    return;
                }

                $scope.user.phonenumber = $scope.user.phonenumber.replace(/\D/g, '');

                if ($scope.user.phonenumber.length < 6) {
                    ToastService.ShowSimpleToast("The phone number you entered is invalid.");
                }

                LoginService.Register($scope.user.mail, $scope.user.password, $scope.user.firstname, $scope.user.lastname, $scope.user.phonenumber, $scope.PictureFile).then(onRegisterSuccess, onRegisterFailure);

            };

            var onRegisterSuccess = function (data) {
                $state.go(PoolReserveConstants.Pages.Hotel.ManageHotels.State);
            };

            var onRegisterFailure = function (data) {
                var result = data.data;

                if (result == null) {
                    ToastService.ShowSimpleToast("An unknown error has occured. Please try again!");
                    return;
                }

                if (result.Action == "unknownFailure") {
                    ToastService.ShowSimpleToast("An unknown error has occured. Please try again!");
                    return;
                } else if (result.Action == "invalidPassword") {
                    ToastService.ShowSimpleToast("Your password must be at least 8 characters long and contain uppercase, lowercase, and a digit.");
                    return;
                } else {
                    ToastService.ShowSimpleToast("An unknown error has occured. Please try again!");
                    return;
                }
            };
        }]);
}());