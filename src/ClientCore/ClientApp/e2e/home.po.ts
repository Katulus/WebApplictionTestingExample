import { browser, by, element, promise } from 'protractor';
import { DefineNodeWizardStepComponentPage } from './define-node-wizard-step.po';

export class HomeComponentPage {
  private _addNodeButton = element(by.id('addNodeButton'));
  private _deleteAllButton = element(by.id('deleteAllButton'));

  public navigate() {
    browser.get('/');
  }

  public addNode(): DefineNodeWizardStepComponentPage {
    this._addNodeButton.click();
    return new DefineNodeWizardStepComponentPage();
  }

  public deleteAll() {
    this._deleteAllButton.click();
  }

  public numberOfNodes(): promise.Promise<number> {
    return element.all(by.css('.node')).count();
  }

  public getNodeAddress(index: number): promise.Promise<string> {
    return element.all(by.css('.node')).get(index).all(by.tagName('td')).get(1).getText();
  }
}
