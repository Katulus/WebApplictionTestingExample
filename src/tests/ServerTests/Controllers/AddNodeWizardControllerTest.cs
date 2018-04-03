using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Server;
using Server.Controllers;
using Server.Models;
using Xunit;

namespace ServerTests.Controllers
{
    public class AddNodeWizardControllerTest
    {
        private readonly Mock<IWizardSession> _sessionMock;
        private readonly Mock<INodeService> _nodeServiceMock;
        private readonly AddNodeWizardController _controller;

        public AddNodeWizardControllerTest()
        {
            _sessionMock = new Mock<IWizardSession>();
            _nodeServiceMock = new Mock<INodeService>();

            _controller = new AddNodeWizardController(_sessionMock.Object, _nodeServiceMock.Object);
        }

        [Fact]
        public void GetSteps_ReturnsStepsFromSession()
        {
            _sessionMock.Setup(x => x.GetSteps())
                .Returns(new[]
                {
                    new TestStep("Step1"),
                    new TestStep("Step2"),
                    new TestStep("Step3")
                });

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
        }

        [Fact]
        public void Next_CallsNextOnWizardSession()
        {
            Node node = new Node();
            _controller.Next(node);

            _sessionMock.Verify(x => x.Next(node), Times.Once, "Session was not called.");
        }

        [Fact]
        public void Next_ReturnsTransitionFromSession()
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
        public void Previous_CallsPreviousOnWizardSession()
        {
            _controller.Back();

            _sessionMock.Verify(x => x.Back(), Times.Once, "Session was not called.");
        }

        [Fact]
        public void Previous_ReturnsTransitionFromSession()
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
        public void Cancel_CallsCancelOnWizardSession()
        {
            _controller.Cancel();

            _sessionMock.Verify(x => x.Cancel(), Times.Once, "Session was not called.");
        }

        [Fact]
        public void AddNode_CallsNodeService()
        {
            Node node = new Node();
            _controller.AddNode(node);

            _nodeServiceMock.Verify(x => x.AddNode(node), Times.Once, "Node service was not called.");
        }

        private class TestStep : IWizardStep
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

        //var okResult = actionResult as OkObjectResult;
        //Assert.NotNull(okResult);
        //var steps = okResult.Value as IEnumerable<WizardStepDefinition>;
        //Assert.NotNull(steps);
        //Assert.Equal("Step1", steps.ElementAt(0).Id);
        //Assert.Equal("Step2", steps.ElementAt(1).Id);
        //Assert.Equal("Step3", steps.ElementAt(2).Id);
    }
}
