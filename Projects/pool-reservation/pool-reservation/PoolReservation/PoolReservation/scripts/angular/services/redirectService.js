(function () {
    var app = angular.module("PoolReserve");
    app.factory("RedirectService", ['$location',
        function ($location) {

            var visitedUrls = [];
            var specialUrl = null;

            var SetSpecialUrl = function (url) {
                specialUrl = url ? JSON.parse(JSON.stringify(url)) : null;
            };

            var GetSpecialUrl = function (url) {
                return specialUrl;
            };

            var AddUrl = function (url) {
                if (url == null) {
                    return null;
                }

                visitedUrls.push(url);

                return visitedUrls.length - 1;
            };

            var GetUrl = function (index) {
                if (visitedUrls.length >= index) {
                    return null;
                }

                if (index < 0) {
                    return null;
                }



                return visitedUrls[index];
            };

            var GetPreviousUrl = function () {
                return GetUrl(visitedUrls.length - 2);
            };

            var ClearUrls = function () {
                visitedUrls = [];
            };


            return {
                AddUrl: AddUrl,
                GetUrl: GetUrl,
                GetPreviousUrl: GetPreviousUrl,
                ClearUrls: ClearUrls,
                SetSpecialUrl: SetSpecialUrl,
                GetSpecialUrl: GetSpecialUrl
            };
        }
    ]);
}());