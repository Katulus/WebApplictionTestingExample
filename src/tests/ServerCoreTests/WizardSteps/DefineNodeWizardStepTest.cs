using FluentAssertions;
using ServerCore;
using ServerCore.Models;
using ServerCore.WizardSteps;
using Xunit;

namespace ServerCoreTests.WizardSteps
{
    public class DefineNodeWizardStepTest
    {
        [Fact]
        public void DefineNodeWizardStep_WithInvalidAddress_BlocksTransition()
        {
            DefineNodeWizardStep step = new DefineNodeWizardStep();
            Node node = new Node {IpOrHostname = ""};

            StepTransitionResult result = step.Next(node);

            result.Should().BeEquivalentTo(new StepTransitionResult
            {
                CanTransition = false,
                ErrorMessage = "Node address has to be specified."
            });
        }

        [Fact]
        public void DefineNodeWizardStep_WithValidAddress_AllowsTransition()
        {
            DefineNodeWizardStep step = new DefineNodeWizardStep();
            Node node = new Node { IpOrHostname = "address" };

            StepTransitionResult result = step.Next(node);

            result.CanTransition.Should().BeTrue("Transition should be allowed.");
        }

        [Fact]
        public void DefineNodeWizardStep_MakesAddressUppercase()
        {
            DefineNodeWizardStep step = new DefineNodeWizardStep();
            Node node = new Node { IpOrHostname = "lowerCaseAddress" };

            step.Next(node);

            node.IpOrHostname.Should().Be("LOWERCASEADDRESS", "Node address should be converted to uppercase.");
        }

        // more tests for more steps
    }
}
