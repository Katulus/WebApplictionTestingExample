using System.Collections.Generic;
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
    public class NodesControllerTest : ControllerTestBase
    {
        private Mock<INodeService> _nodeServiceMock;
        private NodesController _controller;

        [SetUp]
        public void SetUp()
        {
            _nodeServiceMock = new Mock<INodeService>();

            _controller = new NodesController(_nodeServiceMock.Object);
        }

        [Test]
        public void GetNodes_ReturnsNodesServiceResult()
        {
            _nodeServiceMock.Setup(x => x.GetNodes())
                .Returns(new[]
                {
                    new Node {Id = 1, IpOrHostname = "1.1.1.1", PollingMethod = "WMI"},
                    new Node {Id = 2, IpOrHostname = "2.2.2.2", PollingMethod = "SNMP"}
                });

            var actionResult = _controller.GetNodes();

            var response = AssertResponseOfType<OkNegotiatedContentResult<IEnumerable<Node>>>(actionResult);

            Assert.That(response.Content.Count(), Is.EqualTo(2), "Wrong number of nodes returned");
            Assert.That(response.Content.ElementAt(0).IpOrHostname, Is.EqualTo("1.1.1.1"), "Wrong first node returned");
            Assert.That(response.Content.ElementAt(1).IpOrHostname, Is.EqualTo("2.2.2.2"), "Wrong second node returned");
        }

        [Test]
        public void DeleteAll_CallsNodeService()
        {
            var actionResult = _controller.DeleteAll();

            AssertResponseOfType<OkResult>(actionResult);
            _nodeServiceMock.Verify(x => x.DeleteAll(), Times.Once, "Nodes were not deleted");
        }
    }
}
