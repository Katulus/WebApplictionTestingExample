using FluentAssertions;
using Moq;
using Server;
using Server.Models;
using Xunit;

namespace ServerTests
{
    public class WizardSessionTest
    {
        private readonly Mock<IWizardStepsProvider> _stepsProviderMock;
        private readonly Mock<IWizardStep> _step1Mock;
        private readonly Mock<IWizardStep> _step2Mock;

        private readonly WizardSession _session;

        public WizardSessionTest()
        {
            _step1Mock = new Mock<IWizardStep>();
            _step1Mock.Setup(x => x.Next(It.IsAny<Node>())).Returns(StepTransitionResult.Success());
            _step2Mock = new Mock<IWizardStep>();
            _step2Mock.Setup(x => x.Next(It.IsAny<Node>())).Returns(StepTransitionResult.Success());
            var step3Mock = new Mock<IWizardStep>();
            step3Mock.Setup(x => x.Next(It.IsAny<Node>())).Returns(StepTransitionResult.Success());

            _stepsProviderMock = new Mock<IWizardStepsProvider>();
            _stepsProviderMock.Setup(x => x.GetWizardSteps())
                .Returns(new[] { _step1Mock.Object, _step2Mock.Object, step3Mock.Object });

            _session = new WizardSession(_stepsProviderMock.Object);
            // Session is using static fields to keep state for purpose of this example so we need to clear it.
            // Production code would use different way of keeping the state like real Session, special token etc.
            _session.Cancel(); 
        }

        [Fact]
        public void WhenCreated_LoadsSteps()
        {
            _stepsProviderMock.Verify(x => x.GetWizardSteps(), Times.Once(), "Wizard steps were not loaded.");
        }

        [Fact]
        public void StartsOnFirstStep()
        {
            _session.CurrentStep.Should().BeSameAs(_step1Mock.Object, "Should start on first step.");
        }

        [Fact]
        public void Next_CallsNextOnStep()
        {
            Node node = new Node();
            _session.Next(node);

            _step1Mock.Verify(x => x.Next(node), Times.Once, "Next() was not called on the step.");
        }

        [Fact]
        public void Next_DoesNotGoToNextStep_IfStepReturnsFailedTransition()
        {
            _step1Mock.Setup(x => x.Next(It.IsAny<Node>())).Returns(StepTransitionResult.Failure("test"));

            Node node = new Node();
            _session.Next(node);

            _session.CurrentStep.Should().BeSameAs(_step1Mock.Object, "Should be still on first step.");
        }

        [Fact]
        public void Next_ReturnsStepTransitionResult()
        {
            _step1Mock.Setup(x => x.Next(It.IsAny<Node>())).Returns(StepTransitionResult.Failure("test"));

            Node node = new Node();
            StepTransitionResult result = _session.Next(node);

            result.Should().BeEquivalentTo(new
            {
                CanTransition = false,
                ErrorMessage = "test"
            });
        }

        [Fact]
        public void Next_DoesNotAllowToGoForwardFromLastStep()
        {
            Node node = new Node();
            _session.Next(node);
            _session.Next(node);

            StepTransitionResult result = _session.Next(node);

            result.CanTransition.Should().BeFalse("Can't go forward from last step.");
        }

        [Fact]
        public void Next_GoesToNextStep()
        {
            Node node = new Node();
            _session.Next(node);

            _session.CurrentStep.Should().BeSameAs(_step2Mock.Object, "Should be on second step.");
        }

        [Fact]
        public void Back_DoesNotAllowToGoBackFromFirstStep()
        {
            StepTransitionResult result = _session.Back();

            result.CanTransition.Should().BeFalse("Can't go back from first step.");
        }

        [Fact]
        public void Back_GoesToPrevStep()
        {
            Node node = new Node();
            _session.Next(node);
            _session.Next(node);
            _session.Back();

            _session.CurrentStep.Should().BeSameAs(_step2Mock.Object, "Should be on second step.");
        }

        [Fact]
        public void Cancel_ResetsToFirstStep()
        {
            Node node = new Node();
            _session.Next(node);
            _session.Next(node);
            _session.Cancel();

            _session.CurrentStep.Should().BeSameAs(_step1Mock.Object, "Cancel should go to first step.");
        }
    }
}
