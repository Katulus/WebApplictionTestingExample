using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Server;
using Server.Controllers;
using Server.Models;
using Xunit;

namespace ServerTests.Controllers
{
    public class AddNodeWizardControllerTest : ControllerTestBase
    {
        private readonly Mock<IWizardSession> _sessionMock;
        private readonly Mock<INodeService> _nodeServiceMock;
        private readonly Mock<IWizardStep> _step1Mock;
        private readonly Mock<IWizardStep> _step2Mock;
        private readonly Mock<IWizardStep> _step3Mock;
        private readonly AddNodeWizardController _controller;

        public AddNodeWizardControllerTest()
        {
            _sessionMock = new Mock<IWizardSession>();
            _nodeServiceMock = new Mock<INodeService>();

            _step1Mock = new Mock<IWizardStep>();
            _step1Mock.SetupGet(x => x.StepDefinition).Returns(new WizardStepDefinition("Step1", "Step1Control", "Step 1", 1));
            _step2Mock = new Mock<IWizardStep>();                                     
            _step2Mock.SetupGet(x => x.StepDefinition).Returns(new WizardStepDefinition("Step2", "Step2Control", "Step 2", 2));
            _step3Mock = new Mock<IWizardStep>();                                     
            _step3Mock.SetupGet(x => x.StepDefinition).Returns(new WizardStepDefinition("Step3", "Step3Control", "Step 3", 3));

            _controller = new AddNodeWizardController(_sessionMock.Object, _nodeServiceMock.Object);
        }

        [Fact]
        public void GetStepsReturnsStepsFromSession()
        {
            _sessionMock.Setup(x => x.GetSteps())
                .Returns(new[] {_step1Mock.Object, _step2Mock.Object, _step3Mock.Object});

            var actionResult = _controller.GetSteps();

            actionResult.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<IEnumerable<WizardStepDefinition>>()
                .Which.Should().BeEquivalentTo(new[]
                    {
                        new {Id = "Step1"},
                        new {Id = "Step2"},
                        new {Id = "Step3"}
                    },
                    options => options.ExcludingMissingMembers().WithStrictOrdering());

            // TODO: Show
            //var response = AssertResponseOfType<OkNegotiatedContentResult<IEnumerable<WizardStepDefinition>>>(actionResult);

            //Assert.That(response.Content.Count(), Is.EqualTo(3), "Wrong number of steps returned");
            //Assert.That(response.Content.ElementAt(0).Id, Is.EqualTo("Step1"), "Wrong first step returned");
            //Assert.That(response.Content.ElementAt(1).Id, Is.EqualTo("Step2"), "Wrong second step returned");
            //Assert.That(response.Content.ElementAt(2).Id, Is.EqualTo("Step3"), "Wrong third step returned");
        }

        [Fact]
        public void NextCallsNextOnWizardSession()
        {
            Node node = new Node();
            _controller.Next(node);

            _sessionMock.Verify(x => x.Next(node), Times.Once, "Session was not called.");
        }

        [Fact]
        public void NextReturnsTransitionFromSession()
        {
            _sessionMock.Setup(x => x.Next(It.IsAny<Node>())).Returns(StepTransitionResult.Failure("test"));

            Node node = new Node();
            var actionResult = _controller.Next(node);

            actionResult.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<StepTransitionResult>()
                .Which.Should().BeEquivalentTo(new StepTransitionResult
                {
                    CanTransition = false,
                    ErrorMessage = "test"
                });
        }

        [Fact]
        public void PreviousCallsPreviousOnWizardSession()
        {
            _controller.Back();

            _sessionMock.Verify(x => x.Back(), Times.Once, "Session was not called.");
        }

        [Fact]
        public void PreviousReturnsTransitionFromSession()
        {
            _sessionMock.Setup(x => x.Back()).Returns(StepTransitionResult.Failure("test"));

            var actionResult = _controller.Back();

            actionResult.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<StepTransitionResult>()
                .Which.Should().BeEquivalentTo(new StepTransitionResult
                {
                    CanTransition = false,
                    ErrorMessage = "test"
                });
        }

        [Fact]
        public void CancelCallsCancelOnWizardSession()
        {
            _controller.Cancel();

            _sessionMock.Verify(x => x.Cancel(), Times.Once, "Session was not called.");
        }

        [Fact]
        public void AddNodeCallsNodeService()
        {
            Node node = new Node();
            _controller.AddNode(node);

            _nodeServiceMock.Verify(x => x.AddNode(node), Times.Once, "Node service was not called.");
        }
    }
}
