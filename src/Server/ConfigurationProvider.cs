using System;

namespace Server
{
    public class Configuration
    {
        public TimeSpan CacheLifetime { get; set; } = TimeSpan.FromMinutes(5);
        public string PluginsPath { get; set; } = "Plugins";
    }
}