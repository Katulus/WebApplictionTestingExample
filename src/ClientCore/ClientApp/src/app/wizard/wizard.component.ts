import { Component, OnInit, ViewChild, ComponentFactoryResolver } from '@angular/core';
import { IWizardStep, Node, WizardStepDefinition, StepTransitionResult } from '../models';
import { IWizardService, WizardService } from '../wizard.service';
import { WizardStepDirective } from '../wizard-step.directive';
import { DefineNodeWizardStepComponent } from '../wizard-steps/define-node-wizard-step/define-node-wizard-step.component';
import { SummaryWizardStepComponent } from '../wizard-steps/summary-wizard-step/summary-wizard-step.component'
import { Router } from '@angular/router';

@Component({
  selector: 'app-wizard',
  templateUrl: './wizard.component.html',
  styleUrls: ['./wizard.component.css']
})
export class WizardComponent implements OnInit {
  public wizardServiceAddress: string;
  public stepDefinitions: WizardStepDefinition[] = [];
  public currentStepIndex: number;

  public cantGoBack: boolean;
  public cantGoForward: boolean;

  public errorMessage: string;
  public showError: boolean;

  public node: Node;
  public currentStep: IWizardStep;
  private currentStepControlName: string;

  private components = {
    "DefineNodeWizardStep": DefineNodeWizardStepComponent,
    "SummaryWizardStep": SummaryWizardStepComponent,
  };

  @ViewChild(WizardStepDirective) wizardStepHost: WizardStepDirective;

  constructor(
    private wizardService: WizardService,
    private router: Router,
    private componentFactoryResolver: ComponentFactoryResolver) { }

  ngOnInit() {
    this.createNewNode();
    this.reset();
    this.loadSteps();
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
    this.router.navigateByUrl('/');
  }

  private refresh(): void {
    this.cantGoBack = this.currentStepIndex === 0;
    this.cantGoForward = this.currentStepIndex === this.stepDefinitions.length - 1;

    if (this.stepDefinitions.length > 0) {
      this.currentStepControlName = this.stepDefinitions[this.currentStepIndex].ControlName;
      var component = this.components[this.currentStepControlName];

      let componentFactory = this.componentFactoryResolver.resolveComponentFactory(component);

      let viewContainerRef = this.wizardStepHost.viewContainerRef;
      viewContainerRef.clear();
      this.currentStep = <IWizardStep>viewContainerRef.createComponent(componentFactory).instance;
      this.currentStep.setNode(this.node);
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

  private createNewNode(): void {
    this.node = new Node();
    this.node.IpOrHostname = "test";
    this.node.PollingMethod = 'ICMP';
  }

}
