using ESU.Data;
using ESU.Monitoring.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;

namespace ESU.Monitoring
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var logFilePath = configuration.GetValue("LogFilePath", "C://Logs//MonitoringWS-{Date}.txt");
            var logEventLevel = configuration.GetValue("LogEventLevel", LogEventLevel.Information);
            Log.Logger = new LoggerConfiguration()
                .WriteTo.RollingFile(logFilePath)
                .MinimumLevel.Is(logEventLevel)
                .CreateLogger();

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            services.AddControllers();
            services.AddScoped<StatisticsProvider>();
            services.AddScoped<HostProvider>();
            services.AddSingleton<HostAnalyzer>();
            services.AddSingleton<ServiceHealthyChecker>();
            services.AddDbContext<ESUContext>();
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
