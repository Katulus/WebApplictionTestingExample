var AddNodeWizard;
(function (AddNodeWizard) {
    var SummaryWizardStepController = (function () {
        function SummaryWizardStepController() {
        }
        SummaryWizardStepController.prototype.setNode = function (node) {
            this.node = node;
        };
        SummaryWizardStepController.prototype.onNext = function () {
            return true;
        };
        return SummaryWizardStepController;
    }());
    AddNodeWizard.SummaryWizardStepController = SummaryWizardStepController;
})(AddNodeWizard || (AddNodeWizard = {}));
//# sourceMappingURL=SummaryWizardStepController.js.map