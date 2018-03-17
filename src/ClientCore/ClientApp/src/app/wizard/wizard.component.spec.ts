import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { WizardComponent } from './wizard.component';
import { WizardService } from '../wizard.service';
import { Node, WizardStepDefinition, StepTransitionResult } from '../models';
import { RouterTestingModule } from '@angular/router/testing';
import { BrowserDynamicTestingModule } from '@angular/platform-browser-dynamic/testing';
import { DefineNodeWizardStepComponent } from '../wizard-steps/define-node-wizard-step/define-node-wizard-step.component';
import { SummaryWizardStepComponent } from '../wizard-steps/summary-wizard-step/summary-wizard-step.component';
import { FormsModule } from '@angular/forms';
import { WizardStepDirective } from '../wizard-step.directive';


describe('WizardComponent', () => {
  let component: WizardComponent;
  let fixture: ComponentFixture<WizardComponent>;
  const wizardServiceMock: any = {
    loadSteps: jasmine.createSpy('loadSteps').and.callFake(
      (callback: (nodes: WizardStepDefinition[]) => void, failCallback) => {
        callback([
          { Id: 'Step1', ControlName: 'DefineNodeWizardStep', Title: 'Step 1' },
          { Id: 'Step2', ControlName: 'SummaryWizardStep', Title: 'Step 2' }
        ]);
      }),
    back: jasmine.createSpy('back'),
    next: jasmine.createSpy('next'),
    cancel: jasmine.createSpy('cancel').and.callFake((callback) => { callback(); }),
    addNode: jasmine.createSpy('addNode')
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        WizardComponent,
        WizardStepDirective,
        DefineNodeWizardStepComponent,
        SummaryWizardStepComponent],
      imports: [
        RouterTestingModule.withRoutes([]),
        FormsModule
      ],
      providers: [
        {
          provide: WizardService, useValue: wizardServiceMock
        }
      ]
    }).overrideModule(BrowserDynamicTestingModule,
      {
        set: {
          entryComponents: [DefineNodeWizardStepComponent, SummaryWizardStepComponent]
        }
      })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WizardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('loads steps from web service', () => {
    expect(component.stepDefinitions.length).toBe(2, 'Wrong number of steps returned.');
    expect(component.stepDefinitions[0].Id).toBe('Step1');
    expect(component.stepDefinitions[1].Id).toBe('Step2');
  });

  it('moves to next step on next() if server and step allows', () => {
    wizardServiceMock.next.and.callFake((node, callback: (result: StepTransitionResult) => void, failCallback) => {
      callback({ CanTransition: true, ErrorMessage: '' });
    });

    const testStep = {
      onNext: () => true,
      setNode: (node: Node) => { }
    };
    component.currentStep = testStep;

    component.next();

    expect(component.currentStepIndex).toBe(1, 'Controller hasn\'t moved to next step');
  });

  it('does not move to next step on next() if server denies', () => {
    wizardServiceMock.next.and.callFake((node, callback: (result: StepTransitionResult) => void, failCallback) => {
      callback({ CanTransition: false, ErrorMessage: 'test error' });
    });

    component.next();

    expect(component.currentStepIndex).toBe(0, 'Controller has moved to next step when it should not.');
  });

  it('does not move to next step on next() if current step denies', () => {
    wizardServiceMock.next.and.callFake((node, callback: (result: StepTransitionResult) => void, failCallback) => {
      callback({ CanTransition: true, ErrorMessage: '' });
    });

    const testStep = {
      onNext: () => false,
      setNode: (node: Node) => { }
    };
    component.currentStep = testStep;

    component.next();

    expect(component.currentStepIndex).toBe(0, 'Controller has moved to next step when it should not.');
  });

  it('sets error if server responds with error', () => {
    wizardServiceMock.next.and.callFake((node, callback: (result: StepTransitionResult) => void, failCallback) => {
      callback({ CanTransition: false, ErrorMessage: 'test error' });
    });

    component.next();

    expect(component.showError).toBe(true);
    expect(component.errorMessage).toBe('test error');
  });

  it('does not allow to move back from first step', () => {
    expect(component.cantGoBack).toBe(true);
  });

  it('does not allow to move forward from last step', () => {
    wizardServiceMock.next.and.callFake((node, callback: (result: StepTransitionResult) => void, failCallback) => {
      callback({ CanTransition: true, ErrorMessage: '' });
    });

    component.next();

    expect(component.cantGoForward).toBe(true);
  });

  it('adds node when addNode() is called', () => {
    component.addNode();

    expect(wizardServiceMock.addNode).toHaveBeenCalledWith(component.node, jasmine.any(Function), jasmine.any(Function));
  });

  it('sets error when addNode() fails', () => {
    wizardServiceMock.addNode.and.callFake((node, callback, failCallback: (error) => void) => {
      failCallback('test error');
    });

    component.addNode();

    expect(component.showError).toBe(true);
    expect(component.errorMessage).toBe('test error');
  });
});
