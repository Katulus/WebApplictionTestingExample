using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server
{
    public interface INodePluginProvider
    {
        IEnumerable<IAddNodePlugin> GetPlugins();
    }

    public class NodePluginProvider : INodePluginProvider
    {
        private const string PluginSearchPattern = "*.plugin";

        private readonly string _pluginsPath;
        private readonly IFilesContentProvider _filesContentProvider;
        private readonly ICache<List<IAddNodePlugin>> _cache;

        // Abstracting IFilesContentProvider allows easier unit testing and we can for example move 
        // plugins from filesystem to DB without touching this class
        public NodePluginProvider(IConfigurationProvider configuration, IFilesContentProvider filesContentProvider, ICache<List<IAddNodePlugin>> cache)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            _pluginsPath = configuration.PluginsPath;
            _filesContentProvider = filesContentProvider ?? throw new ArgumentNullException(nameof(filesContentProvider));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public IEnumerable<IAddNodePlugin> GetPlugins()
        {
            if (!_cache.TryGetData(out var plugins))
            {
                plugins = GetPluginsInternal();
                _cache.SetData(plugins);
            }

            return plugins;
        }

        private List<IAddNodePlugin> GetPluginsInternal()
        {
            List<IAddNodePlugin> plugins = new List<IAddNodePlugin>();
            IEnumerable<PluginDefinition> pluginDefinitions = LoadPluginDefinitions();
            foreach (PluginDefinition definition in pluginDefinitions.Distinct())
            {
                try
                {
                    Type pluginType = Type.GetType(definition.TypeName);
                    if (pluginType == null)
                    {
                        // log ...
                        continue;
                    }

                    if (!(Activator.CreateInstance(pluginType) is IAddNodePlugin plugin))
                    {
                        // log ...
                        continue;
                    }

                    plugins.Add(plugin);
                }
                catch (Exception)
                {
                    // log ...
                }
            }

            return plugins;
        }

        private IEnumerable<PluginDefinition> LoadPluginDefinitions()
        {
            foreach (var pluginXml in _filesContentProvider.GetFilesContent(_pluginsPath, PluginSearchPattern))
            {
                var plugin = ParsePluginDefinition(pluginXml);
                if (plugin != null)
                {
                    yield return plugin;
                }
            }
        }

        private PluginDefinition ParsePluginDefinition(string pluginXml)
        {
            try
            {
                var root = XElement.Parse(pluginXml);
                return new PluginDefinition(
                    (string) root.Element("id"),
                    (string) root.Element("typeName"));
            }
            catch (Exception)
            {
                // log
                return null;
            }
        }

        private class PluginDefinition
        {
            public PluginDefinition(string id, string typeName)
            {
                Id = id;
                TypeName = typeName;
            }

            public string Id { get; }

            public string TypeName { get; }

            private bool Equals(PluginDefinition other)
            {
                return string.Equals(Id, other.Id) && string.Equals(TypeName, other.TypeName);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((PluginDefinition) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Id?.GetHashCode() ?? 0)*397) ^ (TypeName?.GetHashCode() ?? 0);
                }
            }
        }
    }
}