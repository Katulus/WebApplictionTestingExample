"use strict";
var SummaryStep = require("./SummaryStep");
var AddNodeStep = (function () {
    function AddNodeStep() {
        this._nextButton = element(by.css('.next'));
        this._addressField = element(by.model('vm.node.IpOrHostname'));
        this._snmpRadio = element(by.id('snmpMethodRadio'));
        this._navigator = element(by.css('.sw-wizard-nav-item'));
    }
    AddNodeStep.prototype.navigate = function () {
        browser.get('http://localhost:8081/#/add');
    };
    AddNodeStep.prototype.isLoaded = function () {
        return this._navigator.isPresent();
    };
    AddNodeStep.prototype.next = function () {
        this._nextButton.click();
        return new SummaryStep();
    };
    AddNodeStep.prototype.setAddress = function (address) {
        this._addressField.sendKeys(address);
    };
    AddNodeStep.prototype.setSnmpMethod = function () {
        this._snmpRadio.click();
    };
    return AddNodeStep;
}());
module.exports = AddNodeStep;
//# sourceMappingURL=AddNodeStep.js.map