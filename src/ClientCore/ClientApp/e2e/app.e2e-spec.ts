import { AppPage } from './app.po';
import { browser } from 'protractor';
import { DefineNodeWizardStepComponentPage } from './define-node-wizard-step.po';
import { HomeComponentPage } from './home.po';


// to make tests progress visible
const sleepDuration = 1000;

describe('App', () => {
  let page: AppPage;

  beforeEach(() => {
    page = new AppPage();
  });

  it('is able to go through whole wizard and add a node', () => {
    const indexPage = new HomeComponentPage();
    indexPage.navigate();
    browser.sleep(sleepDuration);

    // make sure there is no node
    indexPage.deleteAll();

    // click on Add Node button
    const addNodeStep = indexPage.addNode();
    browser.sleep(sleepDuration);

    // wait until steps are loaded
    browser.wait(addNodeStep.isLoaded());
    browser.sleep(sleepDuration);

    // fill in IP address
    addNodeStep.setAddress('10.20.30.40');
    browser.sleep(sleepDuration);

    // select SNMP node
    addNodeStep.setSnmpMethod();
    browser.sleep(sleepDuration);

    // select SNMP node
    addNodeStep.setSnmpMethod();
    browser.sleep(sleepDuration);

    // change port
    addNodeStep.setSnmpPort(123);
    browser.sleep(sleepDuration);

    // change community string
    addNodeStep.setSnmpCommunityString('testCommunityString');
    browser.sleep(sleepDuration);

    // click on Next button to get to summary
    const summaryStep = addNodeStep.next();
    browser.sleep(sleepDuration);

    // check that second step is highlighted as active
    expect(summaryStep.isActive()).toBeTruthy('Second step is not active');

    // check that summary step shows proper data
    expect(summaryStep.getNodeAddress()).toBe('10.20.30.40', 'Node address is not correct');
    expect(summaryStep.getNodePollingMethod()).toBe('SNMP', 'Node polling method is not correct');
    expect(summaryStep.getSnmpPort()).toBe(123, 'Node SNMP port is not correct');
    expect(summaryStep.getSnmpCommunityString()).toBe('testCommunityString', 'Node SNMP community string is not correct');

    // click on Add Node button
    summaryStep.addNode();
    browser.sleep(sleepDuration);

    // check that there is new added node
    expect(indexPage.numberOfNodes()).toBe(1, 'Node was not added');
    expect(indexPage.getNodeAddress(0)).toBe('10.20.30.40', 'Node has wrong address');
  });
});
