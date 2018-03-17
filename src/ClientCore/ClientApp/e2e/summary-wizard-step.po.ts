import { browser, by, element, promise } from 'protractor';

export class SummaryWizardStepComponentPage {
  private _addButton = element(by.css('.add'));

  public addNode() {
    this._addButton.click();
  }

  public isActive(): promise.Promise<boolean> {
    const navElements = element.all(by.css('.sw-wizard-nav-item'));
    return navElements.get(1).getAttribute('class').then(value => {
      return /active/.test(value);
    });
  }
}
