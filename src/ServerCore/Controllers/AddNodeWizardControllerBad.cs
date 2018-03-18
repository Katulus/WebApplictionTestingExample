using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerCore.DAL;
using ServerCore.Models;
using ServerCore.WizardSteps;

namespace ServerCore.Controllers
{
    [Route("wizardBad")]
    public class AddNodeWizardControllerBad : Controller
    { 
        // controller should not care about steps, what they are and how to get them
        private readonly List<IWizardStep> _steps = new List<IWizardStep>
        {
            new DefineNodeWizardStep(),
            new SummaryWizardStep()
        };
        private static int _currentIndex;
        // Controller is created for each request, we need static cache to keep the data
        private static List<IAddNodePlugin> _cache;
        private static DateTime _cacheCreationTime;

        [HttpGet]
        [Route("steps")]
        [ProducesResponseType(typeof(IEnumerable<WizardStepDefinition>), StatusCodes.Status200OK)]
        public IActionResult GetSteps()
        {
            return Ok(_steps.Select(x => x.StepDefinition));
        }

        [HttpPost]
        [Route("next")]
        [ProducesResponseType(typeof(StepTransitionResult), StatusCodes.Status200OK)]
        public IActionResult Next(Node node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (!CanGoForward)
            {
                return Ok(StepTransitionResult.Failure("This is the last step"));
            }

            StepTransitionResult result = _steps[_currentIndex].Next(node);
            if (result.CanTransition)
            {
                _currentIndex++;
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("back")]
        [ProducesResponseType(typeof(StepTransitionResult), StatusCodes.Status200OK)]
        public IActionResult Back()
        {
            if (!CanGoBack)
                return Ok(StepTransitionResult.Failure("This is the first step"));

            _currentIndex--;
            return Ok(StepTransitionResult.Success());
        }

        [HttpPost]
        [Route("cancel")]
        public IActionResult Cancel()
        {
            _currentIndex = 0;
            return Ok();
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddNode(Node node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            try
            {
                if (!IsNodeValid(node))
                    throw new InvalidNodeException();

                // This can't be unit tested, there is no way how to not use database
                NodeDAL dal = new NodeDAL(new ServerDbContext(null));
                dal.AddNode(node);
                foreach (IAddNodePlugin plugin in GetPlugins())
                {
                    plugin.AfterNodeAdded(node);
                }

                _currentIndex = 0;
                return Ok();
            }
            catch (Exception ex)
            {
                return  StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        private bool IsNodeValid(Node node)
        {
            bool valid = Validate(node);
            foreach (IAddNodePlugin plugin in GetPlugins())
            {
                valid = valid && plugin.Validate(node);
            }
            return valid;
        }

        private IEnumerable<IAddNodePlugin> GetPlugins()
        {
            // Caching? Is it responsibility of controller?
            // And how it can be tested? It works with real date and time. Should we wait 5 minutes in the test? Probably not.
            if (_cache == null || DateTime.UtcNow - _cacheCreationTime > TimeSpan.FromMinutes(5))
            {
                _cache = GetPluginsInternal();
                _cacheCreationTime = DateTime.UtcNow;
            }

            return _cache;
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
                    // Creating plugins instances? Why should controller know anything about that?
                    IAddNodePlugin plugin = Activator.CreateInstance(pluginType) as IAddNodePlugin;
                    if (plugin == null)
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
            if (!Directory.Exists("Plugins"))
                yield break;

            string[] pluginPaths = Directory.GetFiles("Plugins", "*.plugin");
            foreach (string path in pluginPaths)
            {
                // Reading from files and parsing XML? Not something that controller should do.
                var pluginXml = System.IO.File.ReadAllText(path);
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
                    (string)root.Element("id"),
                    (string)root.Element("typeName"));
            }
            catch (Exception)
            {
                // log
                return null;
            }
        }

        private bool Validate(Node node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            // Just test validation, all nodes with Id != 0 are considered invalid.
            return node.Id == 0;
        }

        private bool CanGoForward
        {
            get { return _currentIndex < _steps.Count - 1; }
        }

        private bool CanGoBack
        {
            get { return _currentIndex > 0; }
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
                return Equals((PluginDefinition)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Id?.GetHashCode() ?? 0) * 397) ^ (TypeName?.GetHashCode() ?? 0);
                }
            }
        }
    }
}