angular.module("PoolReserve.Hotel")
    .directive("manageHotel", ["PoolReserveConstants",
        "ToastService", "HotelService", "OutgoingFile", function (PoolReserveConstants, ToastService, HotelService, OutgoingFile) {
            return {
                restrict: 'E',
                templateUrl: PoolReserveConstants.Templates.Hotel.ManageHotel,
                scope: {
                    theOptions: "=theOptions"
                },
                link: function ($scope, element, attr) {
                    $scope.IsModified = false;
                    $scope.IsAdd = false;
                    $scope.AddressNeedsLatLon = true;

                    $scope.Invalidate = function () {
                        $scope.IsModified = true;
                    };

                    $scope.InvalidateAddress = function () {
                        $scope.Invalidate();
                        $scope.AddressNeedsLatLon = true;
                    };

                    var fileAdded = function (item) {
                        var file = item;

                        var reader = new FileReader();
                        var image = new Image();

                        // Closure to capture the file information.
                        reader.onload = (function () {
                            return function (e) {
                                image.src = e.target.result;
                                otherData = e.target.result;
                                $scope.HasUploaded = true;
                                $scope.Invalidate();
                            };
                        })(file);

                        image.addEventListener("load", function () {
                            if (image.width < 250 || image.width > 250 || image.height < 250 || image.height > 250) {
                                ToastService.ShowSimpleToast("Image can only be 250 by 250 pixels.");
                                return;
                            }

                            $scope.PictureUrl = otherData;
                            var PictureFile = new OutgoingFile(file.name, file.size, $scope.PictureUrl);
                            $scope.$apply();


                        });

                        // Read in the image file as a data URL.
                        if (file != null) {
                            reader.readAsDataURL(file);
                        }
                    };

                    $scope.ImageChanged = function (file) {
                        fileAdded(file);
                    };

                    $scope.DeleteHotelPicture = function () {
                        $scope.Invalidate();
                        $scope.PictureUrl = null;
                        $scope.PictureFile = null;
                    };

                    $scope.Clean = function () {
                        $scope.AddressNeedsLatLon = false;
                        $scope.IsModified = false;
                    };

                    $scope.DetermineAction = function () {
                        var id = ($scope.theOptions || {}).HotelId;

                        if (id != null) {
                            $scope.LoadItem();
                        } else {
                            $scope.IsAdd = true;
                        }


                    };

                    $scope.LoadItem = function () {
                        var id = ($scope.theOptions || {}).HotelId;

                        if (id == null) {
                            return;
                        }

                        HotelService.GetHotel(id).then(onLoadItemSuccess, onLoadItemError);



                    };

                    var onLoadItemSuccess = function (data) {
                        $scope.Clean();

                        var item = data.data;

                        if (item == null) {
                            return;
                        }

                        $scope.OriginalHotel = item;

                        

                        $scope.CloneOriginalItem();
                    };

                    var onLoadItemError = function () {
                        ToastService.ShowSimpleToast("Unable to load hotel.");
                    };


                    $scope.CloneOriginalItem = function () {
                        if ($scope.OriginalHotel == null) {
                            return;
                        }

                        $scope.CurrentHotel = JSON.parse(JSON.stringify($scope.OriginalHotel));

                        $scope.CurrentHotel.TaxRate = $scope.CurrentHotel.TaxRate * 100;

                        if ($scope.CurrentHotel != null && $scope.CurrentHotel.Picture != null && $scope.CurrentHotel.Picture.Resolutions != null && $scope.CurrentHotel.Picture.Resolutions.length != 0) {
                            $scope.PictureUrl = $scope.CurrentHotel.Picture.Resolutions[0].BlobUrl;
                        }
 
                    };

                    $scope.AddItem = function () {

                        var hotel = $scope.CurrentHotel;

                        if (hotel == null) {
                            ToastService.ShowSimpleToast("Unable to add hotel. Please try again.");
                            return;
                        }
                       

                        if (hotel == null || hotel.Name == null || hotel.Name === "") {
                            ToastService.ShowSimpleToast("You must enter a hotel name.");
                            return;
                        }
                        var taxRate = 0;
                        if (hotel.TaxRate == null || hotel.TaxRate === "") {
                            ToastService.ShowSimpleToast("You must enter the hotel tax.");
                            return;
                        }
                        else {
                            taxRate = hotel.TaxRate / 100;
                            if (taxRate < 0 || taxRate > 1) {
                                ToastService.ShowSimpleToast("tax rate must be between 0 and 100.");
                                return;
                            }
                        }


                        if (hotel.Address == null || hotel.Address === "") {
                            ToastService.ShowSimpleToast("You must enter a hotel address.");
                            return;
                        }

                        if (hotel.AddressNeedsLatLon == true || hotel.Latitude == null || hotel.Longitude == null) {
                            ToastService.ShowSimpleToast("You must enter a valid hotel address which has a latitude and longitude.");
                            return;
                        }

                        $scope.Clean();
                        HotelService.AddHotel(hotel.Name, hotel.Address, taxRate, hotel.Latitude || 0, hotel.Longitude || 0, $scope.PictureFile, hotel.IsHidden).then(onAddSuccess, onAddError);

                    };

                    var onAddSuccess = function (data) {
                        $scope.IsAdd = false;
                        ToastService.ShowSimpleToast("Hotel added successfully.");

                        var item = data.data;

                        if (item == null) {
                            return;
                        }

                        $scope.OriginalHotel = item;
                        $scope.CloneOriginalItem();
                        refreshPreviousScreen();
                    };

                    var onAddError = function () {
                        $scope.Invalidate();
                        ToastService.ShowSimpleToast("Unable to add hotel. Please try again.");
                        refreshPreviousScreen();
                    };


                    $scope.EditItem = function () {
                        var hotel = $scope.CurrentHotel;

                        if (hotel == null) {
                            ToastService.ShowSimpleToast("Unable to add hotel. Please try again.");
                            return;
                        }
                       

                        if (hotel == null || hotel.Name == null || hotel.Name === "") {
                            ToastService.ShowSimpleToast("You must enter a hotel name.");
                            return;
                        }
                        var taxRate = 0;
                        if (hotel.TaxRate == null || hotel.TaxRate === "") {
                            ToastService.ShowSimpleToast("You must enter the hotel tax.");
                            return;
                        }
                        else {
                            taxRate = hotel.TaxRate / 100;
                            if (taxRate < 0 || taxRate > 1) {
                                ToastService.ShowSimpleToast("tax rate must be between 0 and 99.9.");
                                return;
                            }
                        }


                        if (hotel.Address == null || hotel.Address === "") {
                            ToastService.ShowSimpleToast("You must enter a hotel address.");
                            return;
                        }

                        if (hotel.AddressNeedsLatLon == true || hotel.Latitude == null || hotel.Longitude == null) {
                            ToastService.ShowSimpleToast("You must enter a valid hotel address which has a latitude and longitude.");
                            return;
                        }

                        $scope.Clean();
                        HotelService.EditHotel(hotel.Id, hotel.Name, hotel.Address, taxRate, hotel.Latitude || 0, hotel.Longitude || 0, $scope.PictureFile, hotel.IsHidden).then(onEditSuccess, onEditError);
                    };

                    var onEditSuccess = function (data) {
                        ToastService.ShowSimpleToast("Hotel updated successfully.");

                        var response = data.data;

                        if(response == null){
                            return;
                        }

                        $scope.OriginalHotel = response;
                        $scope.CloneOriginalItem();
                        refreshPreviousScreen();
                    };

                    var onEditError = function () {
                        
                        $scope.Invalidate();
                        ToastService.ShowSimpleToast("Unable to edit hotel. Please try again.");
                        refreshPreviousScreen();
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

                    $scope.getLatLonLookup = function () {

                        if ($scope.CurrentHotel == null) {
                            return;
                        }

                        var address = $scope.CurrentHotel.Address;

                        if (address == null || address == "") {
                            return;
                        }

                        var daCoda = new google.maps.Geocoder();
                        daCoda.geocode({ address: address }, function (results, status) {
                            if (status != "OK") {
                                ToastService.ShowSimpleToast("Unable to find the Latitude and Longitude of your address. Please enter a new address.");
                                $scope.AddressNeedsLatLon = true;
                                return;
                            }

                            var lat = results[0].geometry.location.lat();
                            var lon = results[0].geometry.location.lng();

                            

                            if (lat == null || lon == null) {
                                ToastService.ShowSimpleToast("Unable to find the Latitude and Longitude of your address. Please enter a new address.");
                                $scope.AddressNeedsLatLon = true;
                                return;
                                
                            }

                            if ($scope.CurrentHotel == null) {
                                ToastService.ShowSimpleToast("Unable to find the Latitude and Longitude of your address. Please enter a new address.");
                                $scope.AddressNeedsLatLon = true;
                                return;
                            }

                            $scope.CurrentHotel.Latitude = lat;
                            $scope.CurrentHotel.Longitude = lon;
                            $scope.AddressNeedsLatLon = false;
                            $scope.$apply();
                            return;
                        });
                    };

                    $scope.ShowEmployees = function () {
                        var hotel = $scope.CurrentHotel;

                        if (hotel == null) {
                            return;
                        }

                        var id = hotel.Id;
                        var name = hotel.Name;
                        if (id == null) {
                            return;
                        }

                        $scope.theOptions.InvokeWidget(PoolReserveConstants.Widgets.Hotel.ManageEmployees.Id, { HotelId: id, HotelName: name });
                    };

                    $scope.ShowVenues = function () {
                        var hotel = $scope.CurrentHotel;

                        if (hotel == null) {
                            return;
                        }

                        var id = hotel.Id;
                        var name = hotel.Name;
                        if (id == null) {
                            return;
                        }

                        $scope.theOptions.InvokeWidget(PoolReserveConstants.Widgets.Venue.ManageVenues.Id, { HotelId: id, HotelName: name });
                    };

                    $scope.ShowReservations = function () {
                        var hotel = $scope.CurrentHotel;

                        if (hotel == null) {
                            return;
                        }

                        var id = hotel.Id;
                        var name = hotel.Name;
                        if (id == null) {
                            return;
                        }

                        $scope.theOptions.InvokeWidget(PoolReserveConstants.Widgets.Reservation.ManageReservations.Id, { HotelId: id, HotelName: name });
                    };

                    var refreshPreviousScreen = function () {
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
        }])
.run(["WidgetService", "PoolReserveConstants", function (WidgetService, PoolReserveConstants) {
    var widgetData = {
        Id: PoolReserveConstants.Widgets.Hotel.ManageHotel.Id,
        Invoker: PoolReserveConstants.Widgets.Hotel.ManageHotel.Invoker,
        ShowOptions : {IsPanel:false}
    };

    WidgetService.RegisterWidget(widgetData);
}]);