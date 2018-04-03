import { Component } from '@angular/core';
import { Node, IWizardStep } from '../../models';

@Component({
  selector: 'app-summary-wizard-step',
  templateUrl: './summary-wizard-step.component.html',
  styleUrls: ['./summary-wizard-step.component.css']
})
export class SummaryWizardStepComponent implements IWizardStep {
  private node: Node = new Node();

  public setNode(node: Node): void {
    this.node = node;
  }

  public onNext(): boolean {
    return true;
  }
}
