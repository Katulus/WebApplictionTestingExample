'use strict';
import IndexPage = require('./PageObjects/IndexPage');
import AddNodeStep = require('./PageObjects/AddNodeStep');

// to make tests progress visible
var sleepDuration = 500;

describe('Add Node Wizard end to end test', () => {
    it('should go to next step after clicking on Next button', () => {
        var addNodeStep = new AddNodeStep();
        addNodeStep.navigate();

        browser.sleep(sleepDuration);

        // wait until steps are loaded
        browser.wait(addNodeStep.isLoaded());
        browser.sleep(sleepDuration);

        // fill in IP address
        addNodeStep.setAddress('10.20.30.40');
        browser.sleep(sleepDuration);

        // click on Next button
        var summaryStep = addNodeStep.next();
        browser.sleep(sleepDuration);
        
        // check that second step is highlighted as active
        expect(summaryStep.isActive()).toBeTruthy('Second step is not active');
    });

    it('is able to go through whole wizard and add a node', () => {
        var indexPage = new IndexPage();
        indexPage.navigate();
        browser.sleep(sleepDuration);

        // make sure there is no node
        indexPage.deleteAll();

        // click on Add Node button
        var addNodeStep = indexPage.addNode();
        browser.sleep(sleepDuration);

        // wait until steps are loaded
        browser.wait(addNodeStep.isLoaded());
        browser.sleep(sleepDuration);

        // fill in IP address
        addNodeStep.setAddress('10.20.30.40');
        browser.sleep(sleepDuration);

        // fill select SNMP node
        addNodeStep.setSnmpMethod();
        browser.sleep(sleepDuration);

        // click on Next button to get to summary
        var summaryStep = addNodeStep.next();
        browser.sleep(sleepDuration);

        // click on Add Node button
        summaryStep.addNode();
        browser.sleep(sleepDuration);

        // check that there is new added node
        expect(indexPage.numberOfNodes()).toBe(1, 'Node was not added');
        expect(indexPage.getNodeAddress(0)).toBe('10.20.30.40', 'Node has wrong address');
    });
});