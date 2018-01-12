
module AddNodeWizard {
    export class SummaryWizardStepController implements IWizardStep {
        private node: Node;

        public setNode(node: Node): void {
            this.node = node;
        }

        public onNext(): boolean {
            return true;
        }
    }
}