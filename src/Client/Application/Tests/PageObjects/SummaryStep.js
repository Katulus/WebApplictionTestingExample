"use strict";
var SummaryStep = (function () {
    function SummaryStep() {
        this._addButton = element(by.css('.add'));
    }
    SummaryStep.prototype.addNode = function () {
        this._addButton.click();
    };
    SummaryStep.prototype.isActive = function () {
        var navElements = element.all(by.css('.sw-wizard-nav-item'));
        return navElements.get(1).getAttribute('class').then(function (value) {
            return /active/.test(value);
        });
    };
    return SummaryStep;
}());
module.exports = SummaryStep;
//# sourceMappingURL=SummaryStep.js.map