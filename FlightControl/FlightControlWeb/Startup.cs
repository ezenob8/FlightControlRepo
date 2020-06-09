using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using FlightControlWeb.Model;
using System.Linq;

namespace FlightControlWeb
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "AllowOrigin";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                 builder =>
                                 {
                                     builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                                 });
            });


            //FlightPlanDBContext flightPlanDBContext = new FlightPlanDBContext();

            //var arrayOrigins = (from server in flightPlanDBContext.Servers.ToList()
            //                   select server.ServerURL.Substring(0, server.ServerURL.Length - 1)).ToArray();
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(name: MyAllowSpecificOrigins,
            //                     builder =>
            //                     {
            //                         builder.WithOrigins(arrayOrigins).AllowAnyHeader().AllowAnyMethod();
            //                     });
            //});


            services.AddEntityFrameworkSqlServer().AddDbContext<FlightPlanDBContext>();
            services.AddControllersWithViews().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //FlightPlanDBContext flightPlanDBContext = new FlightPlanDBContext();

            //var arrayOrigins = (from server in flightPlanDBContext.Servers.ToList()
            //                    select server.ServerURL.Substring(0, server.ServerURL.Length - 1)).ToArray();
            app.UseCors(options =>
                          options.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();
            //app.UseCors(options =>
            //options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());


            //app.UseCors(MyAllowSpecificOrigins);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            }).UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                    //spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });


        }
    }
}
