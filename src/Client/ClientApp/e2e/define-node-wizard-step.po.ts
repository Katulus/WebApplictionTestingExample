import { browser, by, element, promise } from 'protractor';
import { SummaryWizardStepComponentPage } from './summary-wizard-step.po';

export class DefineNodeWizardStepComponentPage {
  private nextButton = element(by.css('.next'));
  private addressField = element(by.name('ipOrHostname'));
  private snmpRadio = element(by.id('snmpMethodRadio'));
  private snmpPort = element(by.name('snmpPort'));
  private snmpCommunityString = element(by.name('snmpCommunity'));
  private navigator = element(by.css('.sw-wizard-nav-item'));

  public async navigate() {
    await browser.get('/add');
  }

  public isLoaded(): promise.Promise<boolean> {
    return this.navigator.isPresent();
  }

  public async next(): Promise<SummaryWizardStepComponentPage> {
    await this.nextButton.click();
    return new SummaryWizardStepComponentPage();
  }

  public async setAddress(address: string) {
    await this.addressField.clear();
    await this.addressField.sendKeys(address);
  }

  public async setSnmpMethod() {
    await this.snmpRadio.click();
  }

  public async setSnmpPort(port: number) {
    await this.snmpPort.clear();
    await this.snmpPort.sendKeys(port);
  }

  public async setSnmpCommunityString(communityString: string) {
    await this.snmpCommunityString.clear();
    await this.snmpCommunityString.sendKeys(communityString);
  }

  // ... more methods to manipulate other fields, check errors etc.
}
