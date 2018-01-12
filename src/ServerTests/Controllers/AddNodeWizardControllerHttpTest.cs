using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Server;
using Server.Models;

namespace ServerTests.Controllers
{
    [TestFixture]
    public class AddNodeWizardControllerHttpTest : HttpApiControllerTestBase
    {
        private Mock<IWizardSession> _sessionMock;
        private Mock<INodeService> _nodeServiceMock;
        private Mock<IWizardStep> _step1Mock;
        private Mock<IWizardStep> _step2Mock;

        [SetUp]
        public void SetUp()
        {
            _sessionMock = new Mock<IWizardSession>();
            _nodeServiceMock = new Mock<INodeService>();

            // mock services through Dependency injection
            NinjectConfig.Kernel.Unbind<IWizardSession>();
            NinjectConfig.Kernel.Bind<IWizardSession>().ToConstant(_sessionMock.Object);
            NinjectConfig.Kernel.Unbind<INodeService>();
            NinjectConfig.Kernel.Bind<INodeService>().ToConstant(_nodeServiceMock.Object);

            _step1Mock = new Mock<IWizardStep>();
            _step1Mock.SetupGet(x => x.StepDefinition).Returns(new WizardStepDefinition("Step1", "Step1Control", "Step 1", 1));
            _step2Mock = new Mock<IWizardStep>();
            _step2Mock.SetupGet(x => x.StepDefinition).Returns(new WizardStepDefinition("Step2", "Step2Control", "Step 2", 2));
        }

        [Test]
        public async void GetSteps_ReturnsSteps()
        {
            _sessionMock.Setup(x => x.GetSteps()).Returns(new[] {_step1Mock.Object, _step2Mock.Object});

            HttpResponseMessage response = await Server.HttpClient.GetAsync("/wizard/steps");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code.");

            var stepDefinitions = JsonConvert.DeserializeObject<IEnumerable<WizardStepDefinition>>(await response.Content.ReadAsStringAsync());

            Assert.That(stepDefinitions.Count(), Is.EqualTo(2), "Wrong number of steps returned.");
            Assert.That(stepDefinitions.ElementAt(0).Id, Is.EqualTo("Step1"), "Wrong first step returned.");
            Assert.That(stepDefinitions.ElementAt(1).Id, Is.EqualTo("Step2"), "Wrong second step returned.");
        }

        [Test]
        public async void Next_MovesToNextInSession()
        {
            Node node = new Node { IpOrHostname = "test" };

            await Server.HttpClient.PostAsync("/wizard/next", new ObjectContent<Node>(node, new JsonMediaTypeFormatter()));

            _sessionMock.Verify(x => x.Next(It.Is<Node>(y => y.IpOrHostname == "test")), Times.Once, "Session was not called to move to next step.");
        }

        [Test]
        public async void Next_ReturnsSessionResponse()
        {
            _sessionMock.Setup(x => x.Next(It.IsAny<Node>())).Returns(StepTransitionResult.Failure("test error"));
            Node node = new Node { IpOrHostname = "test" };

            HttpResponseMessage response = await Server.HttpClient.PostAsync("/wizard/next", new ObjectContent<Node>(node, new JsonMediaTypeFormatter()));

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code.");
            var result = JsonConvert.DeserializeObject<StepTransitionResult>(await response.Content.ReadAsStringAsync());
            Assert.That(result.CanTransition, Is.False, "Wrong transition result returned");
            Assert.That(result.ErrorMessage, Is.EqualTo("test error"), "Wrong transition error returned");
        }

        [Test]
        public async void AddNode_CallsNodeService()
        {
            Node node = new Node { IpOrHostname = "test" };

            await Server.HttpClient.PostAsync("/wizard/add", new ObjectContent<Node>(node, new JsonMediaTypeFormatter()));

            _nodeServiceMock.Verify(x => x.AddNode(It.Is<Node>(y => y.IpOrHostname == "test")), Times.Once, "NodeService was not called to add the node.");
        }

        // more tests for back, cancel etc.
    }
}
