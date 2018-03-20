using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using ServerCore.DAL;
using Swashbuckle.AspNetCore.Swagger;

namespace ServerCore
{
    public class Startup : IStartup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors(o => o.AddPolicy("EnableCors", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Server API", Version = "v1" });
            });

            AddDatabase(services);

            services.AddSingleton<INodeDAL, NodeDAL>();
            services.AddSingleton<INodePluginProvider, NodePluginProvider>();
            services.AddSingleton<IWizardSession, WizardSession>();
            services.AddSingleton<IWizardStepsProvider, WizardStepsProvider>();
            services.AddSingleton<INodeService, NodeService>();
            services.AddSingleton<IFilesContentProvider, FilesContentProvider>();
            services.AddSingleton<IConfigurationProvider, ConfigurationProvider>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton(typeof(ICache<>), typeof(Cache<>));

            return services.BuildServiceProvider();
        }

        

        private void AddDatabase(IServiceCollection services)
        {
            services.AddDbContext<ServerDbContext>(
                options =>
                {
                    options.ConfigureWarnings(
                        warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
                    options.UseInMemoryDatabase("TestDatabase");
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

            app.UseMvc();
            app.UseCors("EnableCors");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Server API V1");
            });
        }
    }
}
