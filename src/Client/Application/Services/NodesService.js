'use strict';
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var AddNodeWizard;
(function (AddNodeWizard) {
    var NodesService = (function (_super) {
        __extends(NodesService, _super);
        function NodesService($http) {
            return _super.call(this, $http) || this;
        }
        NodesService.Create = function ($http) {
            return new NodesService($http);
        };
        NodesService.prototype.loadNodes = function (callback, errorCallback) {
            this.get('/server/nodes', callback, errorCallback);
        };
        NodesService.prototype.deleteAll = function (callback, errorCallback) {
            this.post('/server/deleteAll', null, callback, errorCallback);
        };
        return NodesService;
    }(AddNodeWizard.HttpServiceBase));
    AddNodeWizard.NodesService = NodesService;
})(AddNodeWizard || (AddNodeWizard = {}));
//# sourceMappingURL=NodesService.js.map