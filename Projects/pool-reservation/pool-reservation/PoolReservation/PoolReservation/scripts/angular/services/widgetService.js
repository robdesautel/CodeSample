(function () {
    var module = angular.module("PoolReserve");
    module.factory("WidgetService", [
        '$http',
        'PoolReserveConstants',

    function ($http, PoolReservationConstants) {

        var widgets = [];

        var RegisterWidget = function (widget) {
            if (widget == null) {
                return;
            }

            widgets.push(widget);
        };

        var GetWigetById = function (id) {
            for (var i = 0; i < widgets.length; i++) {
                var currentWidget = widgets[i];

                if (currentWidget == null) {
                    continue;
                }

                if (currentWidget.Id == id) {
                    return JSON.parse(JSON.stringify(currentWidget));
                }
            }
        };
        return {
            RegisterWidget: RegisterWidget,
            GetWigetById: GetWigetById
        };
    }]);
}());