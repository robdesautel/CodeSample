angular.module("PoolReserve.Item")
    .directive("manageItemQuantities", ["PoolReserveConstants",
        "ToastService", "ItemService", function (PoolReserveConstants, ToastService, ItemService) {
            return {
                restrict: 'E',
                templateUrl: PoolReserveConstants.Templates.Item.ManageItemQuantities,
                scope: {
                    theOptions: "=theOptions"
                },
                link: function ($scope, element, attr) {

                    $scope.LoadData = function () {
                        var id = ($scope.theOptions || {}).ItemId;

                        if (id == null) {
                            return;
                        }


                    };


                    $scope.LoadData();
                }
            };
        }]).run(["WidgetService", "PoolReserveConstants", function (WidgetService, PoolReserveConstants) {
            var widgetData = {
                Id: PoolReserveConstants.Widgets.Item.ManageItemQuantities.Id,
                Invoker: PoolReserveConstants.Widgets.Item.ManageItemQuantities.Invoker,
                ShowOptions: { IsPanel: false }
            };

            WidgetService.RegisterWidget(widgetData);
        }]);