import { browser, by, element, promise } from 'protractor';
import { SummaryWizardStepComponentPage } from './summary-wizard-step.po';

export class DefineNodeWizardStepComponentPage {
  private nextButton = element(by.css('.next'));
  private addressField = element(by.name('ipOrHostname'));
  private snmpRadio = element(by.id('snmpMethodRadio'));
  private snmpPort = element(by.name('snmpPort'));
  private snmpCommunityString = element(by.name('snmpCommunity'));
  private navigator = element(by.css('.sw-wizard-nav-item'));

  public navigate() {
    browser.get('/add');
  }

  public isLoaded(): promise.Promise<boolean> {
    return this.navigator.isPresent();
  }

  public next(): SummaryWizardStepComponentPage {
    this.nextButton.click();
    return new SummaryWizardStepComponentPage();
  }

  public setAddress(address: string) {
    this.addressField.clear();
    this.addressField.sendKeys(address);
  }

  public setSnmpMethod() {
    this.snmpRadio.click();
  }

  public setSnmpPort(port: number) {
    this.snmpPort.clear();
    this.snmpPort.sendKeys(port);
  }

  public setSnmpCommunityString(communityString: string) {
    this.snmpCommunityString.clear();
    this.snmpCommunityString.sendKeys(communityString);
  }

  // ... more methods to manipulate other fields, check errors etc.
}
