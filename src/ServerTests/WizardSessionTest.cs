using Moq;
using NUnit.Framework;
using Server;
using Server.Models;

namespace ServerTests
{
    [TestFixture]
    public class WizardSessionTest
    {
        private Mock<IWizardStepsProvider> _stepsProviderMock;
        private Mock<IWizardStep> _step1Mock;
        private Mock<IWizardStep> _step2Mock;
        private Mock<IWizardStep> _step3Mock;

        private WizardSession _session;

        [SetUp]
        public void SetUp()
        {
            _step1Mock = new Mock<IWizardStep>();
            _step1Mock.Setup(x => x.Next(It.IsAny<Node>())).Returns(StepTransitionResult.Success());
            _step2Mock = new Mock<IWizardStep>();
            _step2Mock.Setup(x => x.Next(It.IsAny<Node>())).Returns(StepTransitionResult.Success());
            _step3Mock = new Mock<IWizardStep>();
            _step3Mock.Setup(x => x.Next(It.IsAny<Node>())).Returns(StepTransitionResult.Success());

            _stepsProviderMock = new Mock<IWizardStepsProvider>();
            _stepsProviderMock.Setup(x => x.GetWizardSteps())
                .Returns(new[] { _step1Mock.Object, _step2Mock.Object, _step3Mock.Object });

            _session = new WizardSession(_stepsProviderMock.Object);
            // Session is using static fields to keep state for purpose of this example so we need to clear it.
            // Preduction code would use different way of keeping the state like real Session, special token etc.
            _session.Cancel(); 
        }

        [Test]
        public void WhenCreated_LoadsSteps()
        {
            _stepsProviderMock.Verify(x => x.GetWizardSteps(), Times.Once(), "Wizard steps were not loaded.");
        }

        [Test]
        public void StartsOnFirstStep()
        {
            Assert.That(_session.CurrentStep, Is.SameAs(_step1Mock.Object), "Should start on first step.");
        }

        [Test]
        public void Next_CallsNextOnStep()
        {
            Node node = new Node();
            _session.Next(node);

            _step1Mock.Verify(x => x.Next(node), Times.Once, "Next() was not called on the step.");
        }

        [Test]
        public void Next_DoesNotGoToNextStep_IfStepReturnsFailedTransition()
        {
            _step1Mock.Setup(x => x.Next(It.IsAny<Node>())).Returns(StepTransitionResult.Failure("test"));

            Node node = new Node();
            _session.Next(node);

            Assert.That(_session.CurrentStep, Is.SameAs(_step1Mock.Object), "Should be still on first step.");
        }

        [Test]
        public void Next_ReturnsStepTransitionResult()
        {
            _step1Mock.Setup(x => x.Next(It.IsAny<Node>())).Returns(StepTransitionResult.Failure("test"));

            Node node = new Node();
            StepTransitionResult result = _session.Next(node);

            Assert.That(result.CanTransition, Is.False, "Should return failed transition.");
            Assert.That(result.ErrorMessage, Is.EqualTo("test"), "Returned error is incorrect.");
        }

        [Test]
        public void Next_DoesNotAllowToGoForwardFromLastStep()
        {
            Node node = new Node();
            _session.Next(node);
            _session.Next(node);

            StepTransitionResult result = _session.Next(node);

            Assert.That(result.CanTransition, Is.False, "Can't go forward from last step.");
        }

        [Test]
        public void Next_GoesToNextStep()
        {
            Node node = new Node();
            _session.Next(node);

            Assert.That(_session.CurrentStep, Is.SameAs(_step2Mock.Object), "Should be on second step.");
        }

        [Test]
        public void Back_DoesNotAllowToGoBackFromFirstStep()
        {
            StepTransitionResult result = _session.Back();

            Assert.That(result.CanTransition, Is.False, "Can't go back from first step.");
        }

        [Test]
        public void Back_GoesToPrevStep()
        {
            Node node = new Node();
            _session.Next(node);
            _session.Next(node);
            _session.Back();

            Assert.That(_session.CurrentStep, Is.SameAs(_step2Mock.Object), "Should be on second step.");
        }

        [Test]
        public void Cancel_ResetsToFirstStep()
        {
            Node node = new Node();
            _session.Next(node);
            _session.Next(node);
            _session.Cancel();

            Assert.That(_session.CurrentStep, Is.SameAs(_step1Mock.Object), "Cancel should go to first step.");
        }
    }
}
