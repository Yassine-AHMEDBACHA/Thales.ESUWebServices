using ESU.DashbordWS.Core;
using ESU.DashbordWS.Infrastructures;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;

namespace ESU.DashbordWS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var logFilePath = configuration.GetValue("LogFilePath", "C://Logs//CollectWS-{Date}.txt");
            var logEventLevel = configuration.GetValue("LogEventLevel", LogEventLevel.Information);
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logFilePath)
                .MinimumLevel.Is(logEventLevel)
                .CreateLogger();

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHealthChecks();
            services.AddDbContext<ESUContext>();
            services.AddScoped<HostService>();
            services.AddScoped<StatProvider>();
            services.AddMvc().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ESU.DashbordWS", Version = "v1" });
            });

            //services.AddAuthentication().AddIdentityServerJwt();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ESU.DashbordWS v1"));
            }

            app.UseHealthChecks("/api/ishealthy");

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthentication();

            //app.UseIdentityServer();

            app.UseCors(app =>
            {
                app.AllowAnyOrigin();
               // app.AllowAnyMethod();
                app.AllowAnyHeader();
            });

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
