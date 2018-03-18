using ServerCore.Models;

namespace ServerCore
{
    public interface IWizardStep
    {
        WizardStepDefinition StepDefinition { get; }
        StepTransitionResult Next(Node node);
    }
}