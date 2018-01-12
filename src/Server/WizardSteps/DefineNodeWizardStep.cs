using Server.Models;

namespace Server.WizardSteps
{
    public class DefineNodeWizardStep : IWizardStep
    {
        public WizardStepDefinition StepDefinition
        {
            get { return new WizardStepDefinition("DefineNode", "DefineNodeWizardStep", "Define node", 100); }
        }

        public StepTransitionResult Next(Node node)
        {
            // validates data, may do something more and returns transition result
            if (string.IsNullOrEmpty(node.IpOrHostname))
            {
                return StepTransitionResult.Failure("Node address has to be specified.");
            }

            node.IpOrHostname = NormalizeIpOrHostname(node.IpOrHostname);

            return StepTransitionResult.Success();
        }

        private string NormalizeIpOrHostname(string ipOrHostname)
        {
            // just for demonstration
            return ipOrHostname.ToUpperInvariant();
        }
    }
}