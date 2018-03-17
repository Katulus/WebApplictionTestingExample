import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[wizard-step]'
})
export class WizardStepDirective {

  constructor(public viewContainerRef: ViewContainerRef) { }

}
