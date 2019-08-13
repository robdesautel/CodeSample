angular.module("PoolReserve.Item")
    .directive("manageItem", ["PoolReserveConstants",
        "ToastService", "ItemService", function (PoolReserveConstants, ToastService, ItemService) {
            return {
                restrict: 'E',
                templateUrl: PoolReserveConstants.Templates.Item.ManageItem,
                scope: {
                    theOptions: "=theOptions"
                },
                link: function ($scope, element, attr) {

                    $scope.IsModified = false;
                    $scope.IsAdd = false;

                    $scope.Invalidate = function () {
                        $scope.IsModified = true;
                    };

                    $scope.Clean = function () {
                        $scope.IsModified = false;
                    };

                    $scope.DetermineAction = function () {
                        var id = ($scope.theOptions || {}).ItemId;

                        if (id != null) {
                            $scope.LoadItem();
                        } else {
                            $scope.IsAdd = true;
                            $scope.LoadItemTypes();
                        }


                    };

                    $scope.LoadItemTypes = function () {
                        var id = ($scope.theOptions || {}).VenueId;

                        if (id == null) {
                            return;
                        }

                        ItemService.getItemTypesByVenueId(id).then(onItemTypesSuccess, onItemTypesFailure);
                    };

                    var onItemTypesSuccess = function (data) {

                        if (data.data == null || data.data.length == 0) {
                            ToastService.ShowSimpleToast("There are no item types available for this venue. Please refresh and try again!");
                            return;
                        }



                        $scope.ItemTypeList = data.data;

                        if ($scope.CurrentItem == null) {
                            $scope.CurrentItem = {};
                        }
                        
                        $scope.CurrentItem.Type = $scope.ItemTypeList[0];
                    };

                    var onItemTypesFailure = function (data) {
                        ToastService.ShowSimpleToast("Unable to load item types. Please refresh and try again!");
                    };

                    $scope.LoadItem = function () {
                        var id = ($scope.theOptions || {}).ItemId;

                        if (id == null) {
                            return;
                        }

                        ItemService.getItem(id).then(onLoadItemSuccess, onLoadItemError);

                    };

                    var onLoadItemSuccess = function (data) {
                        $scope.Clean();

                        var item = data.data;

                        if (item == null) {
                            return;
                        }

                        $scope.OriginalItem = item;

                        $scope.CloneOriginalItem();
                    };

                    var onLoadItemError = function () {
                        ToastService.ShowSimpleToast("Unable to load item.");
                    };


                    $scope.CloneOriginalItem = function () {
                        if ($scope.OriginalItem == null) {
                            return;
                        }

                        $scope.CurrentItem = JSON.parse(JSON.stringify($scope.OriginalItem));
                    };

                    $scope.AddItem = function () {
                        
                        var item = $scope.CurrentItem;

                        var venueId = $scope.theOptions.VenueId;

                        if (item == null || venueId == null) {
                            ToastService.ShowSimpleToast("Unable to add item. Please try again.");
                            return;
                        }

                        if (item.IconId == null && item.Icon == null && item.Icon.Id == null) {
                            ToastService.ShowSimpleToast("You must choose an icon before adding an item.");
                            return;
                        }

                        $scope.Clean();

                        ItemService.addItem(item.Name, item.Type.Id, venueId, item.NormalQuantity, item.Price, item.IconId || item.Icon.Id, item.IsHidden).then(onAddSuccess, onAddError);

                    };

                    var onAddSuccess = function (data) {
                        $scope.IsAdd = false;
                        ToastService.ShowSimpleToast("Item added successfully.");

                        var item = data.data;

                        if (item == null) {
                            return;
                        }

                        $scope.OriginalItem = item;
                        $scope.CloneOriginalItem();
                        $scope.RefreshPreviousScreen();
                    };

                    var onAddError = function () {
                        $scope.Invalidate();
                        ToastService.ShowSimpleToast("Unable to add item. Please try again.");
                    };


                    $scope.EditItem = function () {
                        var item = $scope.CurrentItem;

                        if (item == null) {
                            return;
                        }

                        if (item.IconId == null && item.Icon == null && item.Icon.Id == null) {
                            ToastService.ShowSimpleToast("You must choose an icon before adding an item.");
                            return;
                        }

                        $scope.Clean();
                        ItemService.editItem(item.Id, item.Name, item.Price, item.IconId || item.Icon.Id, item.IsHidden).then(onEditSuccess, onEditError);


                    };

                    var onEditSuccess = function (data) {
                        ToastService.ShowSimpleToast("Item updated successfully.");

                        var item = data.data;

                        if (item == null) {
                            return;
                        }

                        $scope.OriginalItem = item;
                        $scope.CloneOriginalItem();
                        $scope.RefreshPreviousScreen();
                    };

                    var onEditError = function () {
                        $scope.Invalidate();
                        ToastService.ShowSimpleToast("Unable to edit item. Please try again.");
                    };

                    $scope.DeleteItem = function () {
                        var item = $scope.CurrentItem;

                        if (item == null) {
                            return;
                        }


                    };

                    var onDeleteSuccess = function (data) {

                    };

                    var onDeleteError = function () {

                    };

                    $scope.CancelChanges = function () {
                        $scope.CloneOriginalItem();
                        $scope.Clean();
                    };

                    $scope.CancelAddItem = function () {
                        $scope.theOptions.DeleteWidget();
                    };

                    $scope.ManageQuantities = function () {
                        if (CurrentItem == null) {
                            return;
                        }

                        var id = CurrentItem.Id;
                        var name = CurrentItem.Name;

                        if (id == null) {
                            return;
                        }

                        $scope.theOptions.InvokeWidget(PoolReserveConstants.Widgets.Item.ManageItem.Id, { ItemId: id, ItemName: name });
                    };

                    $scope.ChooseIcon = function () {
                        $scope.theOptions.InvokeWidget(PoolReserveConstants.Widgets.Image.IconPicker.Id, null).then(function (data) {
                            if (data != null && $scope.CurrentItem != null) {
                                $scope.CurrentItem.Icon = data;
                                $scope.CurrentItem.IconId = data.Id;
                                $scope.Invalidate();
                            }
                        });
                    };

                    $scope.RefreshPreviousScreen = function () {
                        var theFunc = ($scope.theOptions || {}).RefreshPreviousScreen;

                        if (theFunc) {
                            try {
                                theFunc();
                            } catch (ex) {

                            }

                        }
                    };



                    $scope.DetermineAction();
                }
            };
        }]).run(["WidgetService", "PoolReserveConstants", function (WidgetService, PoolReserveConstants) {
            var widgetData = {
                Id: PoolReserveConstants.Widgets.Item.ManageItem.Id,
                Invoker: PoolReserveConstants.Widgets.Item.ManageItem.Invoker,
                ShowOptions: { IsPanel: false }
            };

            WidgetService.RegisterWidget(widgetData);
        }]);