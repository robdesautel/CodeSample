(function () {
    var app = angular.module("PoolReserve.Material");
    app.factory("ToastService", ['$mdToast',
        function ($mdToast) {

            var toastPosition = {
                bottom: false,
                top: true,
                left: false,
                right: true
            };

            var getToastPosition = function () {
                return Object.keys(toastPosition)
                  .filter(function (pos) { return toastPosition[pos]; })
                  .join(' ');
            };

            var ShowSimpleToast = function (text) {
                var toast = $mdToast.simple();
                toast.content(text)
                .hideDelay(4000)
                .position(getToastPosition());
                $mdToast.show(toast);
            };


            return {
                ShowSimpleToast: ShowSimpleToast
            };
        }
    ]);
}());