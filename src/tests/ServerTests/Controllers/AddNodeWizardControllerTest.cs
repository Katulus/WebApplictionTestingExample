﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using Moq;
using NUnit.Framework;
using Server;
using Server.Controllers;
using Server.Models;

namespace ServerTests.Controllers
{
    [TestFixture]
    public class AddNodeWizardControllerTest : ControllerTestBase
    {
        private Mock<IWizardSession> _sessionMock;
        private Mock<INodeService> _nodeServiceMock;
        private Mock<IWizardStep> _step1Mock;
        private Mock<IWizardStep> _step2Mock;
        private Mock<IWizardStep> _step3Mock;
        private AddNodeWizardController _controller;

        [SetUp]
        public void SetUp()
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

        [Test]
        public void GetStepsReturnsStepsFromSession()
        {
            _sessionMock.Setup(x => x.GetSteps())
                .Returns(new[] {_step1Mock.Object, _step2Mock.Object, _step3Mock.Object});

            var actionResult = _controller.GetSteps();

            var response = AssertResponseOfType<OkNegotiatedContentResult<IEnumerable<WizardStepDefinition>>>(actionResult);

            Assert.That(response.Content.Count(), Is.EqualTo(3), "Wrong number of steps returned");
            Assert.That(response.Content.ElementAt(0).Id, Is.EqualTo("Step1"), "Wrong first step returned");
            Assert.That(response.Content.ElementAt(1).Id, Is.EqualTo("Step2"), "Wrong second step returned");
            Assert.That(response.Content.ElementAt(2).Id, Is.EqualTo("Step3"), "Wrong third step returned");
        }

        [Test]
        public void NextCallsNextOnWizardSession()
        {
            Node node = new Node();
            _controller.Next(node);

            _sessionMock.Verify(x => x.Next(node), Times.Once, "Session was not called.");
        }

        [Test]
        public void NextReturnsTransitionFromSession()
        {
            _sessionMock.Setup(x => x.Next(It.IsAny<Node>())).Returns(StepTransitionResult.Failure("test"));

            Node node = new Node();
            var actionResult = _controller.Next(node);

            var response = AssertResponseOfType<OkNegotiatedContentResult<StepTransitionResult>>(actionResult);
            Assert.That(response.Content.CanTransition, Is.False, "Wrong transition result returned.");
            Assert.That(response.Content.ErrorMessage, Is.EqualTo("test"), "Wrong transition error returned.");
        }

        [Test]
        public void PreviousCallsPreviousOnWizardSession()
        {
            _controller.Back();

            _sessionMock.Verify(x => x.Back(), Times.Once, "Session was not called.");
        }

        [Test]
        public void PreviousReturnsTransitionFromSession()
        {
            _sessionMock.Setup(x => x.Back()).Returns(StepTransitionResult.Failure("test"));

            var actionResult = _controller.Back();

            var response = AssertResponseOfType<OkNegotiatedContentResult<StepTransitionResult>>(actionResult);
            Assert.That(response.Content.CanTransition, Is.False, "Wrong transition result returned.");
            Assert.That(response.Content.ErrorMessage, Is.EqualTo("test"), "Wrong transition error returned.");
        }

        [Test]
        public void CancelCallsCancelOnWizardSession()
        {
            _controller.Cancel();

            _sessionMock.Verify(x => x.Cancel(), Times.Once, "Session was not called.");
        }

        [Test]
        public void AddNodeCallsNodeService()
        {
            Node node = new Node();
            _controller.AddNode(node);

            _nodeServiceMock.Verify(x => x.AddNode(node), Times.Once, "Node service was not called.");
        }
    }
}