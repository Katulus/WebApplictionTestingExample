import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DefineNodeWizardStepComponent } from './define-node-wizard-step.component';

describe('DefineNodeWizardStepComponent', () => {
  let component: DefineNodeWizardStepComponent;
  let fixture: ComponentFixture<DefineNodeWizardStepComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DefineNodeWizardStepComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DefineNodeWizardStepComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
