using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Server;
using Server.Models;
using Xunit;

namespace EndToEndApiTests
{
    public class AddNodeWizardApiTest : ApiTestBase
    {
        public AddNodeWizardApiTest()
        {
            // reset the wizard
            Post("/wizard/cancel", string.Empty);
        }

        [Fact]
        public async Task GetSteps_ReturnsSteps()
        {
            HttpWebResponse response = Get("/wizard/steps");

            string responseData = await GetResponseData(response);
            var stepDefinitions = JsonConvert.DeserializeObject<IEnumerable<WizardStepDefinition>>(responseData);

            stepDefinitions.Should().BeEquivalentTo(new object[]
            {
                new {Id = "DefineNode"},
                new {Id = "Summary"}
            });
        }

        [Fact]
        public void AddNode_WithValidNode_AddsNode()
        {
            Node node = new Node() { IpOrHostname = "1.2.3.4", PollingMethod = "ICMP" };

            HttpWebResponse response = Post("/wizard/add", JsonConvert.SerializeObject(node));

            // Now check if the node was really added. It can be done for example via some other API call (which is not present in this sample).

            // For purpose of this sample we check just response code instead of node being really added.
            response.StatusCode.Should().Be(HttpStatusCode.OK, "Unexpected status code returned.");
        }

        [Fact]
        public async Task WalkThroughWizard_WithValidNode_AllowsNextOnFirstStep()
        {
            Node node = new Node() { IpOrHostname = "1.2.3.4", PollingMethod = "ICMP" };

            // first step
            HttpWebResponse response = Post("/wizard/next", JsonConvert.SerializeObject(node));
            string responseData = await GetResponseData(response);
            StepTransitionResult transitionResult = JsonConvert.DeserializeObject<StepTransitionResult>(responseData);

            transitionResult.CanTransition.Should().BeTrue("First step should allow Next() with valid node.");
        }

        [Fact]
        public async Task WalkThroughWizard_WithInvalidNode_DoesNodeAllowNextOnFirstStep()
        {
            Node node = new Node() { IpOrHostname = "", PollingMethod = "ICMP" };

            // first step
            HttpWebResponse response = Post("/wizard/next", JsonConvert.SerializeObject(node));
            string responseData = await GetResponseData(response);
            StepTransitionResult transitionResult = JsonConvert.DeserializeObject<StepTransitionResult>(responseData);

            transitionResult.Should().BeEquivalentTo(new StepTransitionResult
            {
                CanTransition = false,
                ErrorMessage = "Node address has to be specified."
            });
        }

        [Fact]
        public async Task WalkThroughWizard_WithValidNode_DoesNotAllowNextOnLastStep()
        {
            Node node = new Node() { IpOrHostname = "1.2.3.4", PollingMethod = "ICMP" };

            // first step
            Post("/wizard/next", JsonConvert.SerializeObject(node));
            // second step - the last
            HttpWebResponse response = Post("/wizard/next", JsonConvert.SerializeObject(node));

            string responseData = await GetResponseData(response);
            StepTransitionResult transitionResult = JsonConvert.DeserializeObject<StepTransitionResult>(responseData);

            transitionResult.Should().BeEquivalentTo(new StepTransitionResult
            {
                CanTransition = false,
                ErrorMessage = "This is the last step"
            });
        }

        // more tests for adding node with different node type, different invalid properties, navigating through wizard, ...
    }
}
