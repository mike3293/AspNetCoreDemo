using AspNetCoreDemo.Dependencies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace AspNetCoreDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DefaultUserOptions>(
                Configuration.GetSection(DefaultUserOptions.DefaultUser)
                );

            //services.AddTransient<ITestDependency, TestDependency>();
            //services.AddScoped<ITestDependency, TestDependency>();
            services.AddSingleton<ITestDependency, TestDependency>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureDevelopment(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.MapWhen(context => context.Request.Query.ContainsKey("mapWhenProp"), HandleBranch);

            app.Use(next => context =>
            {
                var endpoint = context.GetEndpoint();
                if (endpoint is null)
                {
                    return next(context);
                }

                Console.WriteLine($"Endpoint: {endpoint.DisplayName}");

                return next(context);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapGet("/", async context =>
                {
                    var defaultUserOptions = new DefaultUserOptions();
                    // Also implemented via dependency injection
                    Configuration.GetSection(DefaultUserOptions.DefaultUser).Bind(defaultUserOptions);

                    await context.Response.WriteAsync($"SomeVariable: {Configuration["SomeVariable"]}\nDefault user: {defaultUserOptions.Name} - {defaultUserOptions.Title}");
                });
            });
        }

        private static void HandleBranch(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var mapWhenProp = context.Request.Query["mapWhenProp"];
                await context.Response.WriteAsync($"{mapWhenProp}");
            });
        }
    }
}
