import { browser } from 'protractor';
import { HomeComponentPage } from './home.po';

// to make tests progress visible
const sleepDuration = 1000;

describe('App', () => {
  it('is able to go through whole wizard and add a node', async () => {
    const indexPage = new HomeComponentPage();
    await indexPage.navigate();
    await browser.sleep(sleepDuration);

    // make sure there is no node
    await indexPage.deleteAll();

    // click on Add Node button
    const addNodeStep = await indexPage.addNode();
    await browser.sleep(sleepDuration);

    // wait until steps are loaded
    await browser.wait(addNodeStep.isLoaded());
    await browser.sleep(sleepDuration);

    // fill in IP address
    await addNodeStep.setAddress('10.20.30.40');
    await browser.sleep(sleepDuration);

    // select SNMP node
    await addNodeStep.setSnmpMethod();
    await browser.sleep(sleepDuration);

    // select SNMP node
    await addNodeStep.setSnmpMethod();
    await browser.sleep(sleepDuration);

    // change port
    await addNodeStep.setSnmpPort(123);
    await browser.sleep(sleepDuration);

    // change community string
    await addNodeStep.setSnmpCommunityString('testCommunityString');
    await browser.sleep(sleepDuration);

    // click on Next button to get to summary
    const summaryStep = await addNodeStep.next();
    await browser.sleep(sleepDuration);

    // check that second step is highlighted as active
    expect(summaryStep.isActive()).toBeTruthy('Second step is not active');

    let nodeAddress = await summaryStep.getNodeAddress();
    let pollingMethod = await summaryStep.getNodePollingMethod();
    let snmpPort = await summaryStep.getSnmpPort();
    let communityString = await summaryStep.getSnmpCommunityString();

    // check that summary step shows proper data
    expect(nodeAddress).toBe('10.20.30.40', 'Node address is not correct');
    expect(pollingMethod).toBe('SNMP', 'Node polling method is not correct');
    expect(snmpPort).toBe(123, 'Node SNMP port is not correct');
    expect(communityString).toBe('testCommunityString', 'Node SNMP community string is not correct');

    // click on Add Node button
    summaryStep.addNode();
    browser.sleep(sleepDuration);

    // check that there is new added node
    expect(await indexPage.numberOfNodes()).toBe(1, 'Node was not added');
    expect(await indexPage.getNodeAddress(0)).toBe('10.20.30.40', 'Node has wrong address');
  });
});
