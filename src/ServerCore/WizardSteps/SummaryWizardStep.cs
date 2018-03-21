using ServerCore.Models;

namespace ServerCore.WizardSteps
{
    public class SummaryWizardStep : IWizardStep
    {
        public WizardStepDefinition StepDefinition => new WizardStepDefinition("Summary", "SummaryWizardStep", "Summary", 300);

        public StepTransitionResult Next(Node node)
        {
            return StepTransitionResult.Success();
        }
    }
}