namespace Christmas2016.Service
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Repositories;
    using System.Net;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            ServicePointManager.UseNagleAlgorithm = false;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"AppSettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseStaticFiles();
            app.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IBlobRepository, BlobRepository>();
            services.AddMvc();
        }
    }
}