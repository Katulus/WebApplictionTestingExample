using System;
using FluentAssertions;
using Moq;
using ServerCore;
using ServerCore.DAL;
using ServerCore.Models;
using Xunit;

namespace ServerCoreTests
{
    public class NodeServiceTest
    {
        // Mocks for services required by NodeService - we are testing NodeService logic, not underlying classes.
        private Mock<INodeDAL> nodeDalMock;
        private Mock<INodePluginProvider> pluginProviderMock;

        public NodeServiceTest()
        {
            nodeDalMock = new Mock<INodeDAL>();
            pluginProviderMock = new Mock<INodePluginProvider>();
        }

        [Fact]
        public void GetNodes_GetsFromDAL()
        {
            NodeService service = new NodeService(nodeDalMock.Object, pluginProviderMock.Object);
            var testNodes = new Node[]
            {
                new Node {Id = 1, IpOrHostname = "1.1.1.1", PollingMethod = "SNMP"},
                new Node {Id = 2, IpOrHostname = "2.2.2.2", PollingMethod = "WMI"}
            };
            nodeDalMock.Setup(x => x.GetNodes()).Returns(testNodes);

            var nodes = service.GetNodes();

            nodes.Should().BeEquivalentTo(testNodes, "Noded from DAL were not returned");
        }

        [Fact]
        public void AddNode_AddsToDAL()
        {
            NodeService service = new NodeService(nodeDalMock.Object, pluginProviderMock.Object);
            // use helper methods to create specific test data or setups
            Node node = GetValidNode();

            service.AddNode(node);

            nodeDalMock.Verify(x => x.AddNode(node), Times.Once(), "Node was not saved to DAL.");
        }

        [Fact]
        public void AddNode_CallsAllPluginsAfterNodeIsAdded()
        {
            // use helper methods to create test setup
            var plugin1Mock = GetPluginMock(true);
            var plugin2Mock = GetPluginMock(true);
            var plugin3Mock = GetPluginMock(true);

            pluginProviderMock.Setup(x => x.GetPlugins()).Returns(
                new [] { plugin1Mock.Object, plugin2Mock.Object, plugin3Mock.Object });

            NodeService service = new NodeService(nodeDalMock.Object, pluginProviderMock.Object);
            Node node = GetValidNode();

            service.AddNode(node);

            // assert/verify all plugins - it's assertion of single state - that all plugins were called - so multiple assertions are acceptable in single test
            plugin1Mock.Verify(x => x.AfterNodeAdded(node), Times.Once(), "Plugin 1 was not called.");
            plugin2Mock.Verify(x => x.AfterNodeAdded(node), Times.Once(), "Plugin 2 was not called.");
            plugin3Mock.Verify(x => x.AfterNodeAdded(node), Times.Once(), "Plugin 3 was not called.");
        }

        [Fact]
        public void AddNode_CallsAllPluginsToValidateNode()
        {
            // use helper methods to create test setup
            var plugin1Mock = GetPluginMock(true);
            var plugin2Mock = GetPluginMock(true);
            var plugin3Mock = GetPluginMock(true);

            pluginProviderMock.Setup(x => x.GetPlugins()).Returns(new[] { plugin1Mock.Object, plugin2Mock.Object, plugin3Mock.Object });

            NodeService service = new NodeService(nodeDalMock.Object, pluginProviderMock.Object);
            Node node = GetValidNode();

            service.AddNode(node);

            // assert/verify all plugins - it's assertion of single state - that all plugins were called - so multiple assertions are acceptable in single test
            plugin1Mock.Verify(x => x.Validate(node), Times.Once(), "Plugin 1 was not called.");
            plugin2Mock.Verify(x => x.Validate(node), Times.Once(), "Plugin 2 was not called.");
            plugin3Mock.Verify(x => x.Validate(node), Times.Once(), "Plugin 3 was not called.");
        }

        [Fact]
        public void AddNode_ThrowsForInvalidNode()
        {
            NodeService service = new NodeService(nodeDalMock.Object, pluginProviderMock.Object);
            Node node = GetInvalidNode();

            Assert.Throws<InvalidNodeException>(() => service.AddNode(node));
        }

        [Fact]
        public void AddNode_DoesNotAddInvalidNode()
        {
            NodeService service = new NodeService(nodeDalMock.Object, pluginProviderMock.Object);
            Node node = GetInvalidNode();

            try
            {
                service.AddNode(node);
            }
            catch(Exception)
            { }

            nodeDalMock.Verify(x => x.AddNode(node), Times.Never, "Node was saved to database but it should not be.");
        }

        [Fact]
        public void AddNode_FailedPluginValidation_Throws()
        {
            // this plugin mock says that node is not valid
            Mock<IAddNodePlugin> pluginMock = GetPluginMock(false);
            pluginProviderMock.Setup(x => x.GetPlugins()).Returns(new[] { pluginMock.Object });

            NodeService service = new NodeService(nodeDalMock.Object, pluginProviderMock.Object);
            Node node = GetValidNode();

            Assert.Throws<InvalidNodeException>(() => service.AddNode(node));
        }

        [Fact]
        public void AddNode_FailedPluginValidation_DoesNotAddNode()
        {
            // this plugin mock says that node is not valid
            Mock<IAddNodePlugin> pluginMock = GetPluginMock(false);
            pluginProviderMock.Setup(x => x.GetPlugins()).Returns(new[] { pluginMock.Object });

            NodeService service = new NodeService(nodeDalMock.Object, pluginProviderMock.Object);
            Node node = GetValidNode();

            try
            {
                service.AddNode(node);
            }
            catch (Exception)
            { }

            nodeDalMock.Verify(x => x.AddNode(node), Times.Never, "Node was saved to database but it should not be.");
        }

        [Fact]
        public void DeleteAll_DeletesViaDAL()
        {
            NodeService service = new NodeService(nodeDalMock.Object, pluginProviderMock.Object);

            service.DeleteAll();

            nodeDalMock.Verify(x => x.DeleteAll(), Times.Once(), "Nodes were not deleted.");
        }

        /// <summary>
        /// Get mock representing AddNodePlugin which accepts or rejects node during validation, based on passed parameter.
        /// </summary>
        /// <param name="isNodeValid">True if mock should say that node is valid, false if it should be invalid.</param>
        /// <returns>Mock representing IAddNodePlugin</returns>
        private static Mock<IAddNodePlugin> GetPluginMock(bool isNodeValid)
        {
            Mock<IAddNodePlugin> plugin1Mock = new Mock<IAddNodePlugin>();
            plugin1Mock.Setup(x => x.Validate(It.IsAny<Node>())).Returns(isNodeValid);
            return plugin1Mock;
        }

        private Node GetInvalidNode()
        {
            // AddNode accepts only node with empty ID so node with ID != 0 is considered as invalid.
            return new Node() { Id = 123 };
        }

        private Node GetValidNode()
        {
            // AddNode accepts only node with ID == 0 so this node is considered valid.
            return new Node() { Id = 0 };
        }
    }
}