angular.module("PoolReserve.Image")
    .directive("iconPicker", ["PoolReserveConstants", "ToastService", "ImageService", function (PoolReserveConstants, ToastService, ImageService) {
        return {
            restrict: 'E',
            templateUrl: PoolReserveConstants.Templates.Image.IconPicker,
            scope: {
                theOptions: "=theOptions"
            },
            link: function ($scope, element, attr) {

                $scope.LoadIcons = function () {
                    ImageService.getAllIcons().then(onLoadIconsSuccess, onLoadIconsFailure);
                };

                var onLoadIconsSuccess = function (data) {
                    var results = data.data;

                    if (results == null) {
                        return;
                    }

                    $scope.IconsForList = results;
                };

                var onLoadIconsFailure = function (data) {
                    ToastService.ShowSimpleToast("Unable to load icons.");
                };

                $scope.CloseWidget = function (result) {
                    $scope.theOptions.DeleteWidget(result);
                };

                $scope.ChooseIcon = function (icon) {
                    $scope.CloseWidget(icon);
                };

                $scope.LoadIcons();

                
            }
        };
    }]).run(["WidgetService", "PoolReserveConstants", function (WidgetService, PoolReserveConstants) {
        var widgetData = {
            Id: PoolReserveConstants.Widgets.Image.IconPicker.Id,
            Invoker: PoolReserveConstants.Widgets.Image.IconPicker.Invoker,
            ShowOptions: { IsPanel: true }
        };

        WidgetService.RegisterWidget(widgetData);
    }]);