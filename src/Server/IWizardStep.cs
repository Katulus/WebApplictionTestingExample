using Server.Models;

namespace Server
{
    public interface IWizardStep
    {
        WizardStepDefinition StepDefinition { get; }
        StepTransitionResult Next(Node node);
    }
}