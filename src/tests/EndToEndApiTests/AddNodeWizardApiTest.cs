using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using Server;
using Server.Models;

namespace ServerApiTests
{
    public class AddNodeWizardApiTest : ApiTestBase
    {
        // Set to URL of running application server instance
        private const string BaseUrl = "http://localhost:63598/server";

        [SetUp]
        public void Setup()
        {
            // reset the wizard
            Post(BaseUrl + "/wizard/cancel", string.Empty);
        }

        [Test]
        public async Task GetSteps_ReturnsSteps()
        {
            HttpWebResponse response = Get(BaseUrl + "/wizard/steps");

            string responseData = await GetResponseData(response);
            var stepDefinitions = JsonConvert.DeserializeObject<IEnumerable<WizardStepDefinition>>(responseData);

            Assert.That(stepDefinitions.Count(), Is.EqualTo(2), "Wrong number of steps returned.");
            Assert.That(stepDefinitions.ElementAt(0).Id, Is.EqualTo("DefineNode"), "Wrong first step returned.");
            Assert.That(stepDefinitions.ElementAt(1).Id, Is.EqualTo("Summary"), "Wrong third step returned.");
        }

        [Test]
        public void AddNode_WithValidNode_AddsNode()
        {
            Node node = new Node() { IpOrHostname = "1.2.3.4", PollingMethod = "ICMP" };

            HttpWebResponse response = Post(BaseUrl + "/wizard/add", JsonConvert.SerializeObject(node));

            // Now check if the node was really added. It can be done for example via some other API call (which is not present in this sample).

            // For purpose of this sample we check just response code instead of node being really added.
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code returned.");
        }

        [Test]
        public async Task WalkThroughWizard_WithValidNode_AllowsNextOnFirstStep()
        {
            Node node = new Node() { IpOrHostname = "1.2.3.4", PollingMethod = "ICMP" };

            // first step
            HttpWebResponse response = Post(BaseUrl + "/wizard/next", JsonConvert.SerializeObject(node));
            string responseData = await GetResponseData(response);
            StepTransitionResult transitionResult = JsonConvert.DeserializeObject<StepTransitionResult>(responseData);

            Assert.That(transitionResult.CanTransition, Is.True, "First step should allow Next() with valid node.");
        }

        [Test]
        public async Task WalkThroughWizard_WithInvalidNode_DoesNodeAllowNextOnFirstStep()
        {
            Node node = new Node() { IpOrHostname = "", PollingMethod = "ICMP" };

            // first step
            HttpWebResponse response = Post(BaseUrl + "/wizard/next", JsonConvert.SerializeObject(node));
            string responseData = await GetResponseData(response);
            StepTransitionResult transitionResult = JsonConvert.DeserializeObject<StepTransitionResult>(responseData);

            Assert.That(transitionResult.CanTransition, Is.False, "First step should not allow Next() with invalid node.");
            Assert.That(transitionResult.ErrorMessage, Is.EqualTo("Node address has to be specified."), "First step should return proper error on Next().");
        }

        [Test]
        public async Task WalkThroughWizard_WithValidNode_DoesNotAllowNextOnLastStep()
        {
            Node node = new Node() { IpOrHostname = "1.2.3.4", PollingMethod = "ICMP" };

            // first step
            Post(BaseUrl + "/wizard/next", JsonConvert.SerializeObject(node));
            // second step - the last
            HttpWebResponse response = Post(BaseUrl + "/wizard/next", JsonConvert.SerializeObject(node));

            string responseData = await GetResponseData(response);
            StepTransitionResult transitionResult = JsonConvert.DeserializeObject<StepTransitionResult>(responseData);

            Assert.That(transitionResult.CanTransition, Is.False, "Last step should not allow Next().");
            Assert.That(transitionResult.ErrorMessage, Is.EqualTo("This is the last step"), "Last step should return proper error on Next().");
        }

        // more tests for adding node with different node type, different invalid properties, navigating through wizard, ...
    }
}
