using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sindikat.Identity.API.Extensions;
using Sindikat.Identity.API.Middlewares;
using Sindikat.Identity.Application;
using Sindikat.Identity.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sindikat.Identity.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IContainer ApplicationContainer { get; private set; }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            services.AddWebServices(Configuration);

            builder.Populate(services);
            builder.RegisterModule(new PersistenceModule());
            builder.RegisterModule(new ApplicationModule());

            this.ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "User management");
            });

            app.UseRouting();

            var routesToCheckTokenBlacklist = new List<string> { "SignOut", "Claim/", "User/" };
            app.UseWhen(context => routesToCheckTokenBlacklist.Any(x => context.Request.Path.Value.Contains(x)), appBuilder =>
            {
                appBuilder.UseMiddleware<BlacklistedTokensMiddleware>();
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
