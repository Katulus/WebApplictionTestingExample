import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Node, WizardStepDefinition, StepTransitionResult } from './models';
import { HttpServiceBase } from './http-service-base.service';

export interface IWizardService {
  loadSteps(callback: (result: WizardStepDefinition[]) => void, errorCallback: (error: string) => void): void;
  back(callback: (result: StepTransitionResult) => void, errorCallback: (error: string) => void): void;
  next(node: Node, callback: (result: StepTransitionResult) => void, errorCallback: (error: string) => void): void;
  cancel(callback: () => void, errorCallback: (error: string) => void): void;
  addNode(node: Node, callback: () => void, errorCallback: (error: string) => void): void;
}

@Injectable()
export class WizardService extends HttpServiceBase implements IWizardService {

  private _wizardServiceAddress = '/wizard';

  static Create(http: HttpClient): IWizardService {
    return new WizardService(http);
  }

  constructor(http: HttpClient) {
    super(http);
  }

  public loadSteps(callback: (result: WizardStepDefinition[]) => void, errorCallback: (error: string) => void): void {
    this.get(this._wizardServiceAddress + '/steps', callback, errorCallback);
  }

  public back(callback: (result: StepTransitionResult) => void, errorCallback: (error: string) => void): void {
    this.post(this._wizardServiceAddress + '/back', null, callback, errorCallback);
  }

  public next(node: Node, callback: (result: StepTransitionResult) => void, errorCallback: (error: string) => void): void {
    this.post(this._wizardServiceAddress + '/next', node, callback, errorCallback);
  }

  public cancel(callback: () => void, errorCallback: (error: string) => void): void {
    this.post(this._wizardServiceAddress + '/cancel', null, callback, errorCallback);
  }

  public addNode(node: Node, callback: () => void, errorCallback: (error: string) => void): void {
    this.post(this._wizardServiceAddress + '/add', node, callback, errorCallback);
  }

}
