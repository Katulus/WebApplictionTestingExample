import { browser, by, element, promise } from 'protractor';
import { SummaryWizardStepComponentPage } from './summary-wizard-step.po';

export class DefineNodeWizardStepComponentPage {
  private _nextButton = element(by.css('.next'));
  private _addressField = element(by.name('ipOrHostname'));
  private _snmpRadio = element(by.id('snmpMethodRadio'));
  private _navigator = element(by.css('.sw-wizard-nav-item'));

  public navigate() {
    browser.get('/add');
  }

  public isLoaded(): promise.Promise<boolean> {
    return this._navigator.isPresent();
  }

  public next(): SummaryWizardStepComponentPage {
    this._nextButton.click();
    return new SummaryWizardStepComponentPage();
  }

  public setAddress(address: string) {
    this._addressField.sendKeys(address);
  }

  public setSnmpMethod() {
    this._snmpRadio.click();
  }

  // ... more methods to manipulate other fields, check errors etc.
}
