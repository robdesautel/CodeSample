angular.module("PoolReserve.Hotel")
    .directive("manageItems", ["PoolReserveConstants", "ToastService", "ItemService", function (PoolReserveConstants, ToastService, ItemService) {
            return {
                restrict: 'E',
                templateUrl: PoolReserveConstants.Templates.Item.ManageItems,
                scope: {
                    theOptions: "=theOptions"
                },
                link: function ($scope, element, attr) {

                    $scope.LoadItems = function () {
                        var venueId = ($scope.theOptions || {}).VenueId;

                        if (venueId == null) {
                            ToastService.ShowSimpleToast("An unknown error has occured loading items.");
                            return;
                        }

                        ItemService.getItemsForVenue(venueId).then(onItemsSuccess, onItemsError);

                    };

                    var onItemsSuccess = function (data) {
                        var items = data.data;

                        if (items == null) {
                            return;
                        }

                        $scope.ItemsForList = items;
                    };

                    var onItemsError = function () {
                        ToastService.ShowSimpleToast("An unknown error has occured loading items.");
                    };

                    $scope.ShowItemDetails = function (item) {
                        if (item == null) {
                            return;
                        }

                        var id = item.Id;

                        if (id == null) {
                            return;
                        }

                        $scope.theOptions.InvokeWidget(PoolReserveConstants.Widgets.Item.ManageItem.Id, { ItemId: id, VenueName: $scope.theOptions.VenueName, RefreshPreviousScreen: $scope.RefreshScreen });
                    };

                    $scope.AddItem = function () {
                        $scope.theOptions.InvokeWidget(PoolReserveConstants.Widgets.Item.ManageItem.Id, { VenueId: $scope.theOptions.VenueId, VenueName: $scope.theOptions.VenueName, RefreshPreviousScreen: $scope.RefreshScreen });
                    };

                    $scope.RefreshScreen = function () {
                        $scope.LoadItems();
                    };

                    $scope.LoadItems();
                }
            };
        }]).run(["WidgetService", "PoolReserveConstants", function (WidgetService, PoolReserveConstants) {
            var widgetData = {
                Id: PoolReserveConstants.Widgets.Item.ManageItems.Id,
                Invoker: PoolReserveConstants.Widgets.Item.ManageItems.Invoker,
                ShowOptions: { IsPanel: false }
            };

            WidgetService.RegisterWidget(widgetData);
        }]);