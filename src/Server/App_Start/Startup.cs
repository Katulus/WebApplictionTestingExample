using System.Web.Http;
using Microsoft.Owin;
using Owin;
using Server;

[assembly: OwinStartup(typeof(Startup))]

namespace Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            NinjectConfig.Register(config);

            app.UseWebApi(config);
        }
    }
}