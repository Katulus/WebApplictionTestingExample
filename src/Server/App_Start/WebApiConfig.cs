using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace Server
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var formatter = GlobalConfiguration.Configuration.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();
            if (formatter != null)
            {
                formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }

            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}