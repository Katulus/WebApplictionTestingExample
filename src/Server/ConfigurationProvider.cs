using System;

namespace Server
{
    public interface IConfigurationProvider
    {
        TimeSpan CacheLifetime { get; }
        string PluginsPath { get; }
    }

    public class ConfigurationProvider : IConfigurationProvider
    {
        public TimeSpan CacheLifetime => TimeSpan.FromMinutes(5);
        public string PluginsPath => "Plugins";
    }
}