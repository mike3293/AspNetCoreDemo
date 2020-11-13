using AspNetCoreDemo.Dependencies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

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
    }
}
