﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Client.Startup))]
namespace Client
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
