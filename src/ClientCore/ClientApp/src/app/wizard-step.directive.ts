import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[appWizardStep]'
})
export class WizardStepDirective {

  constructor(public viewContainerRef?: ViewContainerRef) { }

}
