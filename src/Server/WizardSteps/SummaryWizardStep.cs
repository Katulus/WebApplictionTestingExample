using Server.Models;

namespace Server.WizardSteps
{
    public class SummaryWizardStep : IWizardStep
    {
        public WizardStepDefinition StepDefinition
        {
            get { return new WizardStepDefinition("Summary", "SummaryWizardStep", "Summary", 300); }
        }

        public StepTransitionResult Next(Node node)
        {
            return StepTransitionResult.Success();
        }
    }
}