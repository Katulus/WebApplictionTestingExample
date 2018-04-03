using System;
using FluentAssertions;
using Moq;
using Server;
using Server.DAL;
using Server.Models;
using Xunit;

namespace ServerTests
{
    public class NodeServiceTest
    {
        // Mocks for services required by NodeService - we are testing NodeService logic, not underlying classes.
        private readonly Mock<INodeDAL> _nodeDalMock;
        private readonly Mock<INodePluginProvider> _pluginProviderMock;
        private readonly NodeService _service;

        public NodeServiceTest()
        {
            _nodeDalMock = new Mock<INodeDAL>();
            _pluginProviderMock = new Mock<INodePluginProvider>();
            _service = new NodeService(_nodeDalMock.Object, _pluginProviderMock.Object);
        }

        [Fact]
        public void GetNodes_GetsFromDAL()
        {
            var testNodes = new[]
            {
                new Node {Id = 1, IpOrHostname = "1.1.1.1", PollingMethod = "SNMP"},
                new Node {Id = 2, IpOrHostname = "2.2.2.2", PollingMethod = "WMI"}
            };
            _nodeDalMock.Setup(x => x.GetNodes()).Returns(testNodes);

            var nodes = _service.GetNodes();

            nodes.Should().BeEquivalentTo(testNodes, "Noded from DAL were not returned");
        }

        [Fact]
        public void AddNode_AddsToDAL()
        {
            // use helper methods to create specific test data or setups
            Node node = GetValidNode();

            _service.AddNode(node);

            _nodeDalMock.Verify(x => x.AddNode(node), Times.Once(), "Node was not saved to DAL.");
        }

        [Fact]
        public void AddNode_CallsAllPluginsAfterNodeIsAdded()
        {
            // use helper methods to create test setup
            var plugin1Mock = GetPluginMock(true);
            var plugin2Mock = GetPluginMock(true);
            var plugin3Mock = GetPluginMock(true);

            _pluginProviderMock.Setup(x => x.GetPlugins()).Returns(
                new [] { plugin1Mock.Object, plugin2Mock.Object, plugin3Mock.Object });

            Node node = GetValidNode();

            _service.AddNode(node);

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

            _pluginProviderMock.Setup(x => x.GetPlugins()).Returns(new[] { plugin1Mock.Object, plugin2Mock.Object, plugin3Mock.Object });

            Node node = GetValidNode();

            _service.AddNode(node);

            // assert/verify all plugins - it's assertion of single state - that all plugins were called - so multiple assertions are acceptable in single test
            plugin1Mock.Verify(x => x.Validate(node), Times.Once(), "Plugin 1 was not called.");
            plugin2Mock.Verify(x => x.Validate(node), Times.Once(), "Plugin 2 was not called.");
            plugin3Mock.Verify(x => x.Validate(node), Times.Once(), "Plugin 3 was not called.");
        }

        [Fact]
        public void AddNode_ThrowsForInvalidNode()
        {
            Node node = GetInvalidNode();

            Assert.Throws<InvalidNodeException>(() => _service.AddNode(node));
        }

        [Fact]
        public void AddNode_DoesNotAddInvalidNode()
        {
            Node node = GetInvalidNode();

            try
            {
                _service.AddNode(node);
            }
            catch(Exception)
            { }

            _nodeDalMock.Verify(x => x.AddNode(node), Times.Never, "Node was saved to database but it should not be.");
        }

        [Fact]
        public void AddNode_FailedPluginValidation_Throws()
        {
            // this plugin mock says that node is not valid
            Mock<IAddNodePlugin> pluginMock = GetPluginMock(false);
            _pluginProviderMock.Setup(x => x.GetPlugins()).Returns(new[] { pluginMock.Object });

            Node node = GetValidNode();

            Assert.Throws<InvalidNodeException>(() => _service.AddNode(node));
        }

        [Fact]
        public void AddNode_FailedPluginValidation_DoesNotAddNode()
        {
            // this plugin mock says that node is not valid
            Mock<IAddNodePlugin> pluginMock = GetPluginMock(false);
            _pluginProviderMock.Setup(x => x.GetPlugins()).Returns(new[] { pluginMock.Object });

            Node node = GetValidNode();

            try
            {
                _service.AddNode(node);
            }
            catch (Exception)
            { }

            _nodeDalMock.Verify(x => x.AddNode(node), Times.Never, "Node was saved to database but it should not be.");
        }

        [Fact]
        public void DeleteAll_DeletesViaDAL()
        {
            _service.DeleteAll();

            _nodeDalMock.Verify(x => x.DeleteAll(), Times.Once(), "Nodes were not deleted.");
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