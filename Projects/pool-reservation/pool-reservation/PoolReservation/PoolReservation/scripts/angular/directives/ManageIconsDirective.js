angular.module("PoolReserve.Image")
    .directive("manageIcons", ["PoolReserveConstants",
        "ToastService", "ImageService", "OutgoingFile", function (PoolReserveConstants, ToastService, ImageService, OutgoingFile) {
            return {
                restrict: 'E',
                templateUrl: PoolReserveConstants.Templates.Image.ManageIcons,
                scope: {
                    theOptions: "=theOptions"
                },
                link: function ($scope, element, attr) {
                    $scope.LoadIcons = function () {
                        ImageService.getAllIcons().then(onLoadIconsSuccess, onLoadIconsFailure);
                    };

                    var onLoadIconsSuccess = function (data) {
                        var results = data.data;

                        if (results == null) {
                            return;
                        }

                        $scope.IconsForList = results;
                    };

                    var onLoadIconsFailure = function (data) {
                        ToastService.ShowSimpleToast("Unable to load icons.");
                    };

                    var fileAdded = function (item) {
                        var file = item;

                        var reader = new FileReader();

                        var image = new Image();
                        var otherData = null;

                        reader.onload = (function () {
                            return function (e) {
                                image.src = e.target.result;
                                otherData = e.target.result;

                            };


                        })(file);

                        image.addEventListener("load", function () {
                            if (image.width > 150 || image.width < 150 || image.height > 150 || image.height < 150 ) {
                                ToastService.ShowSimpleToast("Image can only be 150 by 150 pixels.");
                                return;
                            }

                            $scope.PictureUrl = otherData;
                            var PictureFile = new OutgoingFile(file.name, file.size, $scope.PictureUrl);
                            $scope.AddIcon(PictureFile);
                            $scope.$apply();
                        });

                        if (file != null) {
                            reader.readAsDataURL(file);
                        }

            

                    };

                    $scope.ImageChanged = function (file) {
                        fileAdded(file);
                    };

                    $scope.AddIcon = function(picFile){
                        ImageService.uploadIcon(picFile).then(onUploadSuccess, onUploadFailure);
                    };

                    var onUploadSuccess = function (data) {
                        $scope.LoadIcons();
                        ToastService.ShowSimpleToast("Icon uploaded successfully!");
                    };

                    var onUploadFailure = function (data) {
                        $scope.LoadIcons();
                        ToastService.ShowSimpleToast("Icon failed to upload. An unknown error has occured. Please try again.");
                        
                    };

                    $scope.LoadIcons();
                }
            };
        }])
.run(["WidgetService", "PoolReserveConstants", function (WidgetService, PoolReserveConstants) {
    var widgetData = {
        Id: PoolReserveConstants.Widgets.Image.ManageIcons.Id,
        Invoker: PoolReserveConstants.Widgets.Image.ManageIcons.Invoker,
        ShowOptions: { IsPanel: false }
    };

    WidgetService.RegisterWidget(widgetData);
}]);