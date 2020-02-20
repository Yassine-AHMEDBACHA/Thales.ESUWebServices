using ESU.ConfirmationWS.Core;
using ESU.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;

namespace ESU.ConfirmationWS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var logFilePath = configuration.GetValue("LogFilePath", "C://Logs//CollectWS-{Date}.txt");
            var logEventLevel = configuration.GetValue("LogEventLevel", LogEventLevel.Information);
            Log.Logger = new LoggerConfiguration()
                .WriteTo.RollingFile(logFilePath)
                .MinimumLevel.Is(logEventLevel)
                .CreateLogger();

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            services.AddDbContext<ESUContext>(ServiceLifetime.Singleton);
            services.AddSingleton<IConfirmationProvider, ConfirmationProvider>();
            services.AddSingleton<ILicenseActivator, LicenseActivator>();
            services.AddControllers();
            services.AddMvc().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/api/ishealthy");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
