using NUnit.Framework;
using Server;
using Server.Models;
using Server.WizardSteps;

namespace ServerTests.WizardSteps
{
    [TestFixture]
    public class DefineNodeWizardStepTest
    {
        [Test]
        public void DefineNodeWizardStep_WithInvalidAddress_BlocksTransition()
        {
            DefineNodeWizardStep step = new DefineNodeWizardStep();
            Node node = new Node {IpOrHostname = ""};

            StepTransitionResult result = step.Next(node);

            Assert.That(result.CanTransition, Is.False, "Transition should not be allowed.");
            Assert.That(result.ErrorMessage, Is.EqualTo("Node address has to be specified."), "Wrong error message was returned.");
        }

        [Test]
        public void DefineNodeWizardStep_WithValidAddress_AllowsTransition()
        {
            DefineNodeWizardStep step = new DefineNodeWizardStep();
            Node node = new Node { IpOrHostname = "address" };

            StepTransitionResult result = step.Next(node);

            Assert.That(result.CanTransition, Is.True, "Transition should be allowed.");
        }

        [Test]
        public void DefineNodeWizardStep_MakesAddressUppercase()
        {
            DefineNodeWizardStep step = new DefineNodeWizardStep();
            Node node = new Node { IpOrHostname = "lowerCaseAddress" };

            StepTransitionResult result = step.Next(node);

            Assert.That(node.IpOrHostname, Is.EqualTo("LOWERCASEADDRESS"), "Node address should be converted to uppercase.");
        }

        // more tests for more steps
    }
}
