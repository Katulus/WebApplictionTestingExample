import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { IWizardStep, Node, WizardStepDefinition, StepTransitionResult } from '../models';
import { IWizardService, WizardService } from '../wizard.service';

@Component({
  selector: 'app-wizard',
  templateUrl: './wizard.component.html',
  styleUrls: ['./wizard.component.css']
})
export class WizardComponent implements OnInit {
  public title: string;
  public wizardServiceAddress: string;
  public stepDefinitions: WizardStepDefinition[] = [];
  public currentStepIndex: number;

  public cantGoBack: boolean;
  public cantGoForward: boolean;

  public errorMessage: string;
  public showError: boolean;

  public node: Node;
  private currentStep: IWizardStep;
  private currentStepControlName: string;

  constructor(
    private wizardService: WizardService,
    private location: Location) { }

  ngOnInit() {
  }

  private loadSteps() {
    this.wizardService.loadSteps((result: WizardStepDefinition[]) => {
      this.stepDefinitions = result;
      this.reset();
    },
      (error) => {
        this.setError(error);
      });
  }

  public back(): void {
    this.setError('');

    if (this.cantGoBack) {
      return;
    }

    this.wizardService.back((result: StepTransitionResult) => {
      if (result.CanTransition) {
        this.currentStepIndex--;
        this.refresh();
      } else {
        this.setError(result.ErrorMessage);
      }
    },
      (error) => {
        this.setError(error);
      });
  }

  public next(): void {
    this.setError('');

    if (this.cantGoForward) {
      return;
    }

    if (this.currentStep && !this.currentStep.onNext()) {
      // can't go next because step blocks it, inform user
      return;
    }

    this.wizardService.next(this.node,
      (result: StepTransitionResult) => {
        if (result.CanTransition) {
          this.currentStepIndex++;
          this.refresh();
        } else {
          this.setError(result.ErrorMessage);
        }
      },
      (error) => {
        this.setError(error);
      });
  }

  public addNode(): void {
    this.wizardService.addNode(this.node,
      () => {
        this.cancel();
      }, (error) => {
        this.setError(error);
      });
  }

  public cancel(): void {
    this.setError('');
    this.createNewNode();

    this.reset();
    this.location.go('/');
  }

  private refresh(): void {
    this.cantGoBack = this.currentStepIndex === 0;
    this.cantGoForward = this.currentStepIndex === this.stepDefinitions.length - 1;

    if (this.stepDefinitions.length > 0) {
      this.currentStepControlName = this.stepDefinitions[this.currentStepIndex].ControlName;
    }
  }

  private setError(error: string) {
    this.errorMessage = error;
    this.showError = error !== '';
  }

  private reset(): void {
    this.wizardService.cancel(() => {
      this.currentStepIndex = 0;
      this.refresh();

      if (this.currentStep) {
        this.currentStep.setNode(this.node);
      }
    },
      (error) => {
        this.setError(error);
      });
  }

  public registerStep(step: IWizardStep): void {
    this.currentStep = step;
    this.currentStep.setNode(this.node);
  }

  private createNewNode(): void {
    this.node = new Node();
    this.node.PollingMethod = 'ICMP';
  }

}
