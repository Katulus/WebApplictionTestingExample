"use strict";
var AddNodeStep = require("./AddNodeStep");
var IndexPage = (function () {
    function IndexPage() {
        this._addNodeButton = element(by.id('addNodeButton'));
        this._deleteAllButton = element(by.id('deleteAllButton'));
    }
    IndexPage.prototype.navigate = function () {
        browser.get('http://localhost:8081/');
    };
    IndexPage.prototype.addNode = function () {
        this._addNodeButton.click();
        return new AddNodeStep();
    };
    IndexPage.prototype.deleteAll = function () {
        this._deleteAllButton.click();
    };
    IndexPage.prototype.numberOfNodes = function () {
        return element.all(by.repeater('node in vm.nodes')).count();
    };
    IndexPage.prototype.getNodeAddress = function (index) {
        return element.all(by.repeater('node in vm.nodes')).get(index).all(by.tagName('td')).get(1).getText();
    };
    return IndexPage;
}());
module.exports = IndexPage;
//# sourceMappingURL=IndexPage.js.map