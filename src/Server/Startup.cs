﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Server.DAL;
using Swashbuckle.AspNetCore.Swagger;

namespace Server
{
    public class Startup : IStartup
    {
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("EnableCors", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));
            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            // Register the Swagger generator
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Server API", Version = "v1" });
            });

            AddDatabase(services);

            AddServices(services);

            return services.BuildServiceProvider();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddSingleton<INodeDAL, NodeDAL>();
            services.AddSingleton<INodePluginProvider, NodePluginProvider>();
            services.AddSingleton<IWizardSession, WizardSession>();
            services.AddSingleton<IWizardStepsProvider, WizardStepsProvider>();
            services.AddSingleton<INodeService, NodeService>();
            services.AddSingleton<IFilesContentProvider, FilesContentProvider>();
            services.AddSingleton<Configuration>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton(typeof(ICache<>), typeof(Cache<>));
        }

        private void AddDatabase(IServiceCollection services)
        {
            services.AddDbContext<ServerDbContext>(
                options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                    //options.UseSqlServer("... connection string ...");
                }, ServiceLifetime.Singleton);
            services.AddSingleton<IServerDbContext, ServerDbContext>();
        }

        public void Configure(IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IHostingEnvironment>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("EnableCors");
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Server API V1");
            });
        }
    }
}
