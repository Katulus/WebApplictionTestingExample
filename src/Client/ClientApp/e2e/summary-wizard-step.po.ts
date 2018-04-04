import { browser, by, element, promise } from 'protractor';

export class SummaryWizardStepComponentPage {
  private addButton = element(by.css('.add'));
  private nodeAddressElement = element(by.id('nodeAddress'));
  private nodePollingMethodElement = element(by.id('nodePollingMethod'));
  private snmpPortElement = element(by.id('snmpPort'));
  private snmpCommunityStringElement = element(by.id('snmpCommunityString'));

  public async addNode() {
    await this.addButton.click();
  }

  public getNodeAddress(): promise.Promise<string> {
    return this.nodeAddressElement.getText();
  }

  public getNodePollingMethod(): promise.Promise<string> {
    return this.nodePollingMethodElement.getText();
  }

  public getSnmpPort(): promise.Promise<number> {
    return this.snmpPortElement.getText().then(x => Number.parseInt(x));
  }

  public getSnmpCommunityString(): promise.Promise<string> {
    return this.snmpCommunityStringElement.getText();
  }

  public isActive(): promise.Promise<boolean> {
    const navElements = element.all(by.css('.sw-wizard-nav-item'));
    return navElements.get(1).getAttribute('class').then(value => {
      return /active/.test(value);
    });
  }
}
