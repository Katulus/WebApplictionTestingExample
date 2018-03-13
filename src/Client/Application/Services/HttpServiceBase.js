'use strict';
var AddNodeWizard;
(function (AddNodeWizard) {
    var HttpServiceBase = (function () {
        function HttpServiceBase($http) {
            this.$http = $http;
            this._baseAddress = 'http://localhost:63598';
        }
        HttpServiceBase.prototype.get = function (path, callback, errorCallback) {
            this.$http.get(this._baseAddress + path).success(function (result) {
                callback(result);
            })
                .error(function (error) {
                if (errorCallback) {
                    errorCallback(error);
                }
            });
        };
        HttpServiceBase.prototype.post = function (path, data, callback, errorCallback) {
            this.$http.post(this._baseAddress + path, data).success(function (result) {
                callback(result);
            })
                .error(function (error) {
                if (errorCallback) {
                    errorCallback(error);
                }
            });
        };
        return HttpServiceBase;
    }());
    AddNodeWizard.HttpServiceBase = HttpServiceBase;
})(AddNodeWizard || (AddNodeWizard = {}));
//# sourceMappingURL=HttpServiceBase.js.map