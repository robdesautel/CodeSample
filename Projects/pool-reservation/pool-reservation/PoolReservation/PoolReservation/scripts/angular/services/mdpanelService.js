(function () {
    var app = angular.module("PoolReserve.Material");
    app.factory("PanelService", ['$mdPanel','$controller',
        function ($mdPanel, $controller) {

            var ShowPanel = function (controllerName, templateUrl, scopedObjects ) {

                var panelPosition = $mdPanel.newPanelPosition()
                .absolute()
                .center();

                if (scopedObjects == null) {
                    scopedObjects = {
                        $panelReference: {}
                    };
                } else {
                    scopedObjects.$panelReference = {}
                }

                var config = {

                    attachTo: angular.element(document.body),
                    controller: controllerName,
                    controllerAs: "ctrl",
                    position: panelPosition,
                    templateUrl: templateUrl,
                    locals: scopedObjects,
                    disableParentScroll: true,
                    trapFocus: true,
                    zIndex: 150,
                    panelClass: 'demo-dialog-example',
                    hasBackdrop:true
                }

                var daPromise = $mdPanel.open(config);

                daPromise.then(function (result) {
                    config.locals.$panelReference.$reference = result;
                });

            };

            return {
                ShowPanel: ShowPanel
            };
        }
    ]);
}());