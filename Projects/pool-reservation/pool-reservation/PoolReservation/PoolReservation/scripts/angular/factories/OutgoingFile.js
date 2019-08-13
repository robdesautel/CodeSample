angular.module("PoolReserve.File")
.factory("OutgoingFile", [function () {
    var OutgoingFile = function (filename, size, data) {
        this.FileName = filename;
        this.Size = size;
        this.Data = data;
    };

    return OutgoingFile;
}]);