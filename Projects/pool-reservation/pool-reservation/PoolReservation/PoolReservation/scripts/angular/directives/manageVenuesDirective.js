angular.module("PoolReserve.Venue")
    .directive("manageVenues", ["PoolReserveConstants",
        "ToastService", "VenueService", '$mdPanel', function (PoolReserveConstants, ToastService, VenueService, $mdPanel) {
            return {
                restrict: 'E',
                templateUrl: PoolReserveConstants.Templates.Venue.ManageVenues,
                scope: {
                    theOptions: "=theOptions"
                },
                link: function ($scope, element, attr) {

                    $scope.LoadVenues = function () {
                        var hotelId = ($scope.theOptions || {}).HotelId;

                        if (hotelId == null) {
                            ToastService.ShowSimpleToast("An unknown error has occured loading venues.");
                            return;
                        }

                        VenueService.getVenuesForHotel(hotelId).then(onVenuesSuccess, onVenuesError);

                    };

                    var onVenuesSuccess = function (data) {
                        var venues = data.data;

                        if (venues == null) {
                            return;
                        }

                        $scope.VenuesForList = venues;
                    };

                    var onVenuesError = function () {
                        ToastService.ShowSimpleToast("An unknown error has occured loading venues.");
                    };




                    $scope.ShowManageVenue = function (venue) {
                        if (venue == null) {
                            return;
                        }

                        var id = venue.Id;
                        var name = venue.Name;
                        var hotelName = $scope.theOptions.HotelName;
                        var hotelId = $scope.theOptions.HotelId;

                        if (id == null || name == null) {
                            return;
                        }

                        $scope.theOptions.InvokeWidget(PoolReserveConstants.Widgets.Venue.ManageVenue.Id, { VenueId: id, VenueName: name, HotelName: hotelName, HotelId: hotelId, RefreshPreviousScreen: $scope.RefreshScreen });
                    };

                    $scope.AddVenue = function () {
                        var hotelName = $scope.theOptions.HotelName;
                        var hotelId = $scope.theOptions.HotelId;
                        $scope.theOptions.InvokeWidget(PoolReserveConstants.Widgets.Venue.ManageVenue.Id, { HotelName: hotelName, HotelId: hotelId, RefreshPreviousScreen: $scope.RefreshScreen });
                    };

                    $scope.RefreshScreen = function () {
                        $scope.LoadVenues();
                    };


                    $scope.ShowSpecialMessage = function (venue) {

                        //var venueMessagePopUpCtrl = function($scope) {


                        //}

                        var daTroller = function ($scope, $sce, SpecialMessageService) {
                            $scope.UpdateDaPage = function () {
                                $scope.safeHtmlData = $sce.trustAsHtml($scope.HtmlData);
                            };

                            $scope.CloseDaWinnow = function () {
                                if ($scope.ctrl.mdPanelRef) {
                                    $scope.ctrl.mdPanelRef.close();
                                }
                            };

                            $scope.Submit = function () {
                                if ($scope.message == null || $scope.message.title == null || $scope.message.title === "") {
                                    ToastService.ShowSimpleToast("You must enter a name for the message.");
                                    return;
                                }

                                if ($scope.HtmlData == null || $scope.HtmlData === "") {
                                    ToastService.ShowSimpleToast("You must enter a message.");
                                    return;
                                }

                                if (venue == null) {
                                    ToastService.ShowSimpleToast("An unknown error has occured. Please copy your changes, refresh the page and try again.");
                                    return;
                                }


                                var venueId = venue.Id;

                                if (venueId == null) {
                                    ToastService.ShowSimpleToast("An unknown error has occured. Please copy your changes, refresh the page and try again.");
                                    return;
                                }

                                SpecialMessageService.NewMessage($scope.message.title, $scope.HtmlData, venueId).then(onCreateSuccess, onCreateFailure);

                            }

                            var onCreateSuccess = function (data) {
                                $scope.CloseDaWinnow(ToastService.ShowSimpleToast("Messaged Saved."));
                                return;


                            };

                            var onCreateFailure = function (data) {
                                var result = data.data;

                                if (result == null) {
                                    ToastService.ShowSimpleToast("An unkown error has occured. Please try again.");
                                    return;
                                }

                                if (result.Action == "unknownFailure") {
                                    ToastService.ShowSimpleToast("An unknown error has occured. Please try again.")
                                    return;
                                }

                                else {
                                    ToastService.ShowSimpleToast("An unknown error has occured. Please try again.")
                                }
                            }

                            $scope.LoadData = function () {
                                var id = (venue || {}).Id;

                                if (id == null) {
                                    return;
                                }



                                SpecialMessageService.ExisitingMessage(id).then(onGetSpecialSuccess, onGetSpecialFailure);



                            };

                            var onGetSpecialSuccess = function (data) {
                                var message = data.data;

                                if (message == null) {
                                    return;
                                }

                                $scope.message = { title: message.PageName };
                                $scope.HtmlData = message.PageData;
                            };

                            var onGetSpecialFailure = function () {
                                ToastService.ShowSimpleToast("An unknown error has occured loading venues.");

                            };

                            //Initialize - KEEP AT BOTTOM
                            $scope.LoadData();

                        };



                        //angular.module("PoolReserve.Venue")
                        //.controller('venueMessagePopUpCtrl', ['$scope', '$sce', function ($scope, $sce) {
                        //    $scope.UpdateDaPage = function () {
                        //        $scope.safeHtmlData = $sce.trustAsHtml($scope.HtmlData);
                        //    };

                        //    $scope.CloseDaWinnow = function () {
                        //        if ($scope.ctrl.mdPanelRef) {
                        //            $scope.ctrl.mdPanelRef.close();
                        //        }
                        //    };


                        //}]);

                        var panelPosition = $mdPanel.newPanelPosition()
			            .absolute()
			            .top('25%')
			            .left('25%');

                        var config = {

                            attachTo: angular.element(document.body),
                            controller: daTroller,
                            controllerAs: 'ctrl',
                            position: panelPosition,
                            templateUrl: 'views/pages/templates/ManageSpecialMessage.html',
                            panelClass: 'textarea',
                            locals: { mdPanelRef: null },
                            disableParentScroll: true,
                            trapFocus: true,
                            zIndex: 150,
                        }


                        var daPromise = $mdPanel.open(config);

                        daPromise.then(function (result) {
                            config.locals.mdPanelRef = result;
                        });


                    };

                    $scope.LoadVenues();
                }
            };
        }]).run(["WidgetService", "PoolReserveConstants", function (WidgetService, PoolReserveConstants) {
            var widgetData = {
                Id: PoolReserveConstants.Widgets.Venue.ManageVenues.Id,
                Invoker: PoolReserveConstants.Widgets.Venue.ManageVenues.Invoker,
                ShowOptions: { IsPanel: false }
            };

            WidgetService.RegisterWidget(widgetData);
        }]);