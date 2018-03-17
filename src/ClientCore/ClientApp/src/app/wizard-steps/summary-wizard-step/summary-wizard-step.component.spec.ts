import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SummaryWizardStepComponent } from './summary-wizard-step.component';

describe('SummaryWizardStepComponent', () => {
  let component: SummaryWizardStepComponent;
  let fixture: ComponentFixture<SummaryWizardStepComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SummaryWizardStepComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SummaryWizardStepComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
