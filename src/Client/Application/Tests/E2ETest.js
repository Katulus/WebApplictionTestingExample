'use strict';
exports.__esModule = true;
var IndexPage = require("./PageObjects/IndexPage");
var AddNodeStep = require("./PageObjects/AddNodeStep");
var sleepDuration = 500;
describe('Add Node Wizard end to end test', function () {
    it('should go to next step after clicking on Next button', function () {
        var addNodeStep = new AddNodeStep();
        addNodeStep.navigate();
        browser.sleep(sleepDuration);
        browser.wait(addNodeStep.isLoaded());
        browser.sleep(sleepDuration);
        addNodeStep.setAddress('10.20.30.40');
        browser.sleep(sleepDuration);
        var summaryStep = addNodeStep.next();
        browser.sleep(sleepDuration);
        expect(summaryStep.isActive()).toBeTruthy('Second step is not active');
    });
    it('is able to go through whole wizard and add a node', function () {
        var indexPage = new IndexPage();
        indexPage.navigate();
        browser.sleep(sleepDuration);
        indexPage.deleteAll();
        var addNodeStep = indexPage.addNode();
        browser.sleep(sleepDuration);
        browser.wait(addNodeStep.isLoaded());
        browser.sleep(sleepDuration);
        addNodeStep.setAddress('10.20.30.40');
        browser.sleep(sleepDuration);
        addNodeStep.setSnmpMethod();
        browser.sleep(sleepDuration);
        var summaryStep = addNodeStep.next();
        browser.sleep(sleepDuration);
        summaryStep.addNode();
        browser.sleep(sleepDuration);
        expect(indexPage.numberOfNodes()).toBe(1, 'Node was not added');
        expect(indexPage.getNodeAddress(0)).toBe('10.20.30.40', 'Node has wrong address');
    });
});
//# sourceMappingURL=E2ETest.js.map