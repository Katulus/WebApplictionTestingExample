using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Newtonsoft.Json;
using Server;
using Server.Models;
using Xunit;

namespace ServerTests.Controllers
{
    public class AddNodeWizardControllerHttpTest : HttpApiControllerTestBase
    {
        private readonly Mock<IWizardSession> _sessionMock;
        private readonly Mock<INodeService> _nodeServiceMock;
        private readonly Mock<IWizardStep> _step1Mock;
        private readonly Mock<IWizardStep> _step2Mock;

        public AddNodeWizardControllerHttpTest()
        {
            _sessionMock = new Mock<IWizardSession>();
            _nodeServiceMock = new Mock<INodeService>();

            // mock services through Dependency injection
            Initialize(s => 
            {
                s.Replace(new ServiceDescriptor(typeof(IWizardSession), _sessionMock.Object));
                s.Replace(new ServiceDescriptor(typeof(INodeService), _nodeServiceMock.Object));
            });

            _step1Mock = new Mock<IWizardStep>();
            _step1Mock.SetupGet(x => x.StepDefinition).Returns(new WizardStepDefinition("Step1", "Step1Control", "Step 1", 1));
            _step2Mock = new Mock<IWizardStep>();
            _step2Mock.SetupGet(x => x.StepDefinition).Returns(new WizardStepDefinition("Step2", "Step2Control", "Step 2", 2));
        }

        [Fact]
        public async Task GetSteps_ReturnsSteps()
        {
            _sessionMock.Setup(x => x.GetSteps()).Returns(new[] {_step1Mock.Object, _step2Mock.Object});

            HttpResponseMessage response = await HttpClient.GetAsync("/wizard/steps");

            response.EnsureSuccessStatusCode();

            var stepDefinitions = JsonConvert.DeserializeObject<IEnumerable<WizardStepDefinition>>(await response.Content.ReadAsStringAsync());

            stepDefinitions.Should().BeEquivalentTo(new object[]
            {
                new {Id = "Step1"},
                new {Id = "Step2"}
            });
        }

        [Fact]
        public async Task Next_MovesToNextInSession()
        {
            Node node = new Node { IpOrHostname = "test" };

            await HttpClient.PostAsync("/wizard/next", new ObjectContent<Node>(node, new JsonMediaTypeFormatter()));

            _sessionMock.Verify(x => x.Next(It.Is<Node>(y => y.IpOrHostname == "test")), Times.Once, "Session was not called to move to next step.");
        }

        [Fact]
        public async Task Next_ReturnsSessionResponse()
        {
            _sessionMock.Setup(x => x.Next(It.IsAny<Node>())).Returns(StepTransitionResult.Failure("test error"));
            Node node = new Node { IpOrHostname = "test" };

            HttpResponseMessage response = await HttpClient.PostAsync("/wizard/next", new ObjectContent<Node>(node, new JsonMediaTypeFormatter()));
            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<StepTransitionResult>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(new StepTransitionResult
            {
                CanTransition = false,
                ErrorMessage = "test error"
            });
        }

        [Fact]
        public async Task AddNode_CallsNodeService()
        {
            Node node = new Node { IpOrHostname = "test" };

            await HttpClient.PostAsync("/wizard/add", new ObjectContent<Node>(node, new JsonMediaTypeFormatter()));

            _nodeServiceMock.Verify(x => x.AddNode(It.Is<Node>(y => y.IpOrHostname == "test")), Times.Once, "NodeService was not called to add the node.");
        }

        // more tests for back, cancel etc.
    }
}
