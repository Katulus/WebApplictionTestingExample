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
    public class NodesControllerTest
    {
        private readonly Mock<INodeService> _nodeServiceMock;
        private readonly NodesController _controller;

        public NodesControllerTest()
        {
            _nodeServiceMock = new Mock<INodeService>();

            _controller = new NodesController(_nodeServiceMock.Object);
        }

        [Fact]
        public void GetNodes_ReturnsNodesServiceResult()
        {
            _nodeServiceMock.Setup(x => x.GetNodes())
                .Returns(new[]
                {
                    new Node {Id = 1, IpOrHostname = "1.1.1.1", PollingMethod = "WMI"},
                    new Node {Id = 2, IpOrHostname = "2.2.2.2", PollingMethod = "SNMP"}
                });

            var actionResult = _controller.GetNodes();

            actionResult.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<IEnumerable<Node>>()
                .Which.Should().BeEquivalentTo(new[]
                    {
                        new {IpOrHostname = "1.1.1.1"},
                        new {IpOrHostname = "2.2.2.2"}
                    },
                    options => options.ExcludingMissingMembers().WithStrictOrdering());
        }

        [Fact]
        public void DeleteAll_CallsNodeService()
        {
            var actionResult = _controller.DeleteAll();

            actionResult.Should().BeOfType<OkResult>();
            _nodeServiceMock.Verify(x => x.DeleteAll(), Times.Once, "Nodes were not deleted");
        }
    }
}
