(function () {
    var app = angular.module("PoolReserve.Hotel");
    app.controller("ShowWidgetAsPanelController", [
        '$scope', "widget", "$panelReference", "deferredPromise", function (
        $scope, widget, $panelReference, deferredPromise) {
            $scope.widget = widget;
            $scope.$mdPanelRef = $panelReference;

            $scope.DeletePanel = function (result) {
                if ($scope.$mdPanelRef == null || $scope.$mdPanelRef.$reference == null) {
                    return;
                }

                $scope.$mdPanelRef.$reference.close();

                if (deferredPromise != null) {
                    deferredPromise.resolve(result);
                }
            };

            if ($scope.widget != null && $scope.widget.Options != null) {
                $scope.widget.Options.DeleteWidget = $scope.DeletePanel;
            }

        }]);
}());