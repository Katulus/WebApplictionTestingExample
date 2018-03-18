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
    var WizardService = /** @class */ (function (_super) {
        __extends(WizardService, _super);
        function WizardService($http) {
            var _this = _super.call(this, $http) || this;
            _this._wizardServiceAddress = '/server/wizard';
            return _this;
        }
        WizardService.Create = function ($http) {
            return new WizardService($http);
        };
        WizardService.prototype.loadSteps = function (callback, errorCallback) {
            this.get(this._wizardServiceAddress + '/steps', callback, errorCallback);
        };
        WizardService.prototype.back = function (callback, errorCallback) {
            this.post(this._wizardServiceAddress + '/back', null, callback, errorCallback);
        };
        WizardService.prototype.next = function (node, callback, errorCallback) {
            this.post(this._wizardServiceAddress + '/next', node, callback, errorCallback);
        };
        WizardService.prototype.cancel = function (callback, errorCallback) {
            this.post(this._wizardServiceAddress + '/cancel', null, callback, errorCallback);
        };
        WizardService.prototype.addNode = function (node, callback, errorCallback) {
            this.post(this._wizardServiceAddress + '/add', node, callback, errorCallback);
        };
        return WizardService;
    }(AddNodeWizard.HttpServiceBase));
    AddNodeWizard.WizardService = WizardService;
})(AddNodeWizard || (AddNodeWizard = {}));
//# sourceMappingURL=WizardService.js.map