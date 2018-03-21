using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Server;
using Server.Models;

namespace ServerTests
{
    [TestFixture]
    class NodePluginProviderTest
    {
        private const string PluginsPath = @"c:\plugins";
        private Mock<IConfigurationProvider> _configurationProviderMock;
        private Mock<IFilesContentProvider> _filesContentProviderMock;
        private Mock<ICache<List<IAddNodePlugin>>> _cacheMock;
        private NodePluginProvider _provider;

        [SetUp]
        public void SetUp()
        {
            _filesContentProviderMock = new Mock<IFilesContentProvider>();
            _cacheMock = new Mock<ICache<List<IAddNodePlugin>>>();

            _configurationProviderMock = new Mock<IConfigurationProvider>();
            _configurationProviderMock.SetupGet(x => x.PluginsPath).Returns(PluginsPath);

            // setup empty cache
            List<IAddNodePlugin> tmp;
            _cacheMock.Setup(x => x.TryGetData(out tmp)).Returns(false);

            _provider = new NodePluginProvider(_configurationProviderMock.Object, _filesContentProviderMock.Object, _cacheMock.Object);
        }

        [Test]
        public void GetPlugins_NoPlugins_ReturnsEmptyCollection()
        {
            _filesContentProviderMock.Setup(x => x.GetFilesContent(It.IsAny<string>(), It.IsAny<string>())).Returns(Enumerable.Empty<string>());

            CollectionAssert.IsEmpty(_provider.GetPlugins(), "No plugins should have been returned");
        }

        [Test]
        public void GetPlugins_ReturnsValidPluginsFromFiles()
        {
            _filesContentProviderMock.Setup(x => x.GetFilesContent(It.IsAny<string>(), It.IsAny<string>())).Returns(
                new[]
                {
                    string.Format(@"<plugin><id>TestNodePlugin1</id><typeName>{0}</typeName></plugin>", typeof(TestNodePlugin1).AssemblyQualifiedName),
                    string.Format(@"<plugin><id>TestNodePlugin2</id><typeName>{0}</typeName></plugin>", typeof(TestNodePlugin2).AssemblyQualifiedName)
                });

            IEnumerable<IAddNodePlugin> plugins = _provider.GetPlugins();

            // multiple assertions are fine here - they test one thing, that returned plugins are correct
            Assert.That(plugins.Count(), Is.EqualTo(2), "Wrong number of plugins returned");
            Assert.That(plugins.Any(x => x.GetType() == typeof(TestNodePlugin1)), Is.True, "TestNodePlugin1 was not returned");
            Assert.That(plugins.Any(x => x.GetType() == typeof(TestNodePlugin2)), Is.True, "TestNodePlugin2 was not returned");
        }

        [Test]
        public void GetPlugins_ReturnsEachPluginJustOnce()
        {
            _filesContentProviderMock.Setup(x => x.GetFilesContent(It.IsAny<string>(), It.IsAny<string>())).Returns(
                new[]
                {
                    string.Format(@"<plugin><id>TestNodePlugin1</id><typeName>{0}</typeName></plugin>", typeof(TestNodePlugin1).AssemblyQualifiedName),
                    string.Format(@"<plugin><id>TestNodePlugin1</id><typeName>{0}</typeName></plugin>", typeof(TestNodePlugin1).AssemblyQualifiedName)
                });

            IEnumerable<IAddNodePlugin> plugins = _provider.GetPlugins();

            Assert.That(plugins.Count(), Is.EqualTo(1), "Only one plugin should have been returned, other one is duplicate.");
        }

        [Test]
        public void GetPlugins_SkipsInvalidPluginDefinitions()
        {
            _filesContentProviderMock.Setup(x => x.GetFilesContent(It.IsAny<string>(), It.IsAny<string>())).Returns(
                new[]
                {
                    "Invalid XML",
                    string.Format(@"<plugin><id>TestNodePlugin1</id><typeName>{0}</typeName></plugin>", typeof(TestNodePlugin1).AssemblyQualifiedName)
                });

            IEnumerable<IAddNodePlugin> plugins = _provider.GetPlugins();

            Assert.That(plugins.Count(), Is.EqualTo(1), "Only one plugin should have been returned, other one is invalid definition.");
            Assert.That(plugins.Any(x => x.GetType() == typeof(TestNodePlugin1)), Is.True, "TestNodePlugin1 was not returned");
        }

        [Test]
        public void GetPlugins_ReturnsPluginsFromCache_IfCacheIsValid()
        {
            // setup populated cache
            List<IAddNodePlugin> cachedPlugins = new List<IAddNodePlugin> { new TestNodePlugin1(), new TestNodePlugin2() };
            _cacheMock.Setup(x => x.TryGetData(out cachedPlugins)).Returns(true);

            IEnumerable<IAddNodePlugin> plugins = _provider.GetPlugins();

            // multiple assertions are fine here - they test one thing, that returned plugins are correct
            Assert.That(plugins.Count(), Is.EqualTo(2), "Wrong number of plugins returned");
            Assert.That(plugins.Any(x => x.GetType() == typeof(TestNodePlugin1)), Is.True, "TestNodePlugin1 was not returned");
            Assert.That(plugins.Any(x => x.GetType() == typeof(TestNodePlugin2)), Is.True, "TestNodePlugin2 was not returned");
        }

        [Test]
        public void GetPlugins_DoesNotReadFiles_IfCacheIsValid()
        {
            // setup populated cache
            List<IAddNodePlugin> cachedPlugins = new List<IAddNodePlugin> { new TestNodePlugin1(), new TestNodePlugin2() };
            _cacheMock.Setup(x => x.TryGetData(out cachedPlugins)).Returns(true);

            _provider.GetPlugins();

            _filesContentProviderMock.Verify(x => x.GetFilesContent(It.IsAny<string>(), It.IsAny<string>()), 
                Times.Never, "FilesContentProvider should not be called when cache is used.");
        }

        // Test classes for test node plugins.
        // These classes can't be mocked because NodePluginProvider needs to be able to create their instance
        // from plugin definition by calling Activator.CreateInstance(...)
        public class TestNodePlugin1 : IAddNodePlugin
        {
            public void AfterNodeAdded(Node node)
            {
            }

            public bool Validate(Node node)
            {
                return true;
            }
        }

        public class TestNodePlugin2 : IAddNodePlugin
        {
            public void AfterNodeAdded(Node node)
            {
            }

            public bool Validate(Node node)
            {
                return true;
            }
        }
    }
}
