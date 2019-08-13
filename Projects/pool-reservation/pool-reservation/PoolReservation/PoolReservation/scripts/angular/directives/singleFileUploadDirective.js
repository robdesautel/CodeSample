angular.module("PoolReserve.Image")
    .directive("singleFileUpload", function () {
        return {
            restrict: 'A',
            scope: {
                onChange: "=onChange"
            },
            link: function (scope, element, attr) {

                element.bind('change', function () {
                    scope.onChange(element[0].files[0]);
                });
            }
        };
    });