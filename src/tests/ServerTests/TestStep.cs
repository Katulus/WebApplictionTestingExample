using Server;
using Server.Models;

namespace ServerTests
{
    internal class TestStep : IWizardStep
    {
        public TestStep(string id)
        {
            StepDefinition = new WizardStepDefinition(id, id + "Control", id, 1);
        }

        public WizardStepDefinition StepDefinition { get; }

        public StepTransitionResult Next(Node node)
        {
            return StepTransitionResult.Success();
        }
    }
}