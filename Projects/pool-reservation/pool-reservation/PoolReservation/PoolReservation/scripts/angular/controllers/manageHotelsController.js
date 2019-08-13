(function () {
    var app = angular.module("PoolReserve.Hotel");
    app.controller("ManageHotelsController", [
        '$scope',
        '$location',
        'ToastService',
        'HotelService',
        'PoolReserveConstants',
        "WidgetService",
        "LoginService",
        "PanelService",
        "$q",
        "$timeout",
        "$window", function (
        $scope,
        $location,
        ToastService,
        HotelService,
        PoolReserveConstants,
        WidgetService,
        LoginService,
        PanelService,
        $q, 
        $timeout,
        $window) {
            $scope.Widgets = [];
            $scope.IsFullScreen = false;

            var randomId = 0;

            $scope.LoadHotelWidget = function () {

                $scope.InvokeWidget(PoolReserveConstants.Widgets.Hotel.ManageHotels.Id, null);
            };

            $scope.LoadCreateHotelWidget = function () {
                $scope.InvokeWidget(PoolReserveConstants.Widgets.Hotel.ManageHotel.Id, null);
            };

            $scope.LoadManageIcons = function () {
                $scope.InvokeWidget(PoolReserveConstants.Widgets.Image.ManageIcons.Id, null);
            };

            $scope.LoadManageReservations = function () {
                $scope.InvokeWidget(PoolReserveConstants.Widgets.Reservation.ManageReservations.Id, null);
            };

            $scope.InvokeWidget = function (id, options) {
                var theWidget = WidgetService.GetWigetById(id);

                theWidget.Options = options || {};

                theWidget.Options.InvokeWidget = $scope.InvokeWidget;

                if (theWidget == null) {
                    return;
                }

                theWidget.trackById = randomId++;

                var deferred = $q.defer();

                var deleteThisWidget = function (result) {
                    $scope.DeleteWidget(theWidget);
                    deferred.resolve(result);
                };

                theWidget.Options.DeleteWidget = deleteThisWidget;

                theWidget.Options.SitePermissions = $scope.CurrentSitePermissions || {};

                if (theWidget.ShowOptions != null && theWidget.ShowOptions.IsPanel == true) {
                    PanelService.ShowPanel("ShowWidgetAsPanelController", "views/pages/templates/ShowWidgetAsPanel.html", { widget: theWidget, deferredPromise: deferred });
                } else {
                    $scope.Widgets.push(theWidget);

                    if ($scope.IsFullScreen == false) {
                        $timeout(function () {
                            scrollToBottom();
                        }, 15);
                    }
                    
                   
                }

                return deferred.promise;
            };

            var scrollToBottom = function () {
                try {
                    $('html, body').animate({
                        scrollTop: $(document).height()
                    }, 'slow');
                }
                catch (ex) {

                }
            };

            $scope.DeleteWidget = function (widget) {
                var index = $scope.Widgets.indexOf(widget);

                $scope.CloseWidget(widget, index);
            };

            $scope.CloseWidget = function (widget, index) {
                $scope.Widgets.splice(index, 1);
            };

            $scope.ClearPermissions = function(){
                $scope.CurrentSitePermissions = null;
            };

            $scope.UpdatePermissions = function () {
                var currentUser = LoginService.GetCurrentUser();

                if(currentUser == null){
                    $scope.ClearPermissions();
                    return;
                }

                var currentPermissions = currentUser.SiteWidePermissions;

                if(currentPermissions == null){
                    $scope.ClearPermissions();
                    return;
                }

                $scope.CurrentSitePermissions = JSON.parse(JSON.stringify(currentPermissions));
            };

            $scope.$on(PoolReserveConstants.Notifications.Authentication.LOGIN_STATUS_CHANGED, function () {
                $scope.UpdatePermissions();
            });

            $scope.ChangeFullScreen = function () {
                $scope.IsFullScreen = !$scope.IsFullScreen;

                if ($scope.IsFullScreen == false) {
                    $timeout(function () {
                        scrollToBottom();
                    }, 30);
                }
            };

            $scope.DetermineScreenMode = function () {
                if ($window.innerWidth < 768) {
                    $scope.IsFullScreen = true;
                }
            };
            
            $scope.DetermineScreenMode();
            $scope.UpdatePermissions();
            $scope.LoadHotelWidget();
          
        }]);
}());