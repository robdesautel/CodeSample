angular.module("PoolReserve.Image")
    .directive("userSelector", ["PoolReserveConstants", "ToastService", "UserService", function (PoolReserveConstants, ToastService, UserService) {
        return {
            restrict: 'E',
            templateUrl: PoolReserveConstants.Templates.User.UserSelector,
            scope: {
                theOptions: "=theOptions"
            },
            link: function ($scope, element, attr) {

                $scope.SearchUsers = function () {

                    if ($scope.query == null || $scope.query == "" || $scope.query.length <= 3) {
                        ToastService.ShowSimpleToast("You must enter at least 4 characters to search.");
                        return;
                    }

                    UserService.SearchMinimalUsers($scope.query).then(onUsersSuccess, onUsersFailure);
                };

                var onUsersSuccess = function (data) {
                    var results = data.data;

                    if (results == null) {
                        return;
                    }

                    $scope.UsersForList = results;
                };

                var onUsersFailure = function (data) {
                    ToastService.ShowSimpleToast("Unable to load users.");
                };

                $scope.CloseWidget = function (result) {
                    $scope.theOptions.DeleteWidget(result);
                };

                $scope.ChooseUser = function (user) {
                    $scope.CloseWidget(user);
                };
            }
        };
    }]).run(["WidgetService", "PoolReserveConstants", function (WidgetService, PoolReserveConstants) {
        var widgetData = {
            Id: PoolReserveConstants.Widgets.User.UserSelector.Id,
            Invoker: PoolReserveConstants.Widgets.User.UserSelector.Invoker,
            ShowOptions: { IsPanel: true }
        };

        WidgetService.RegisterWidget(widgetData);
    }]);