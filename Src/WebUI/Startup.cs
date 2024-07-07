using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Northwind.Infrastructure;
using Northwind.Persistence;
using Northwind.Application;
using Northwind.Application.Common.Interfaces;
using Northwind.WebUI.Common;
using Northwind.WebUI.Services;

namespace Northwind.WebUI;

public class Startup
{
    private IServiceCollection _services;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        Configuration = configuration;
        Environment = environment;
    }

    public IConfiguration Configuration { get; }
    public IWebHostEnvironment Environment { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddInfrastructure(Configuration, Environment);
        services.AddPersistence(Configuration);
        services.AddApplication();

        services.AddHealthChecks()
            .AddDbContextCheck<NorthwindDbContext>();

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();

        services
            .AddControllersWithViews()
            .AddNewtonsoftJson()
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<INorthwindDbContext>());

        services.AddRazorPages();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        // In production, the Angular files will be served from this directory
        services.AddSpaStaticFiles(configuration =>
        {
            configuration.RootPath = "ClientApp/dist/browser";
        });
            
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Northwind Traders API", Version = "v1" });
        });

        _services = services;
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app)
    {
        if (Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            RegisteredServicesPage(app);
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseCustomExceptionHandler();
        app.UseHealthChecks("/health");
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseSpaStaticFiles();

        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        // specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Northwind Traders API V1");
        });

        app.UseRouting();

        app.UseAuthentication();
        app.UseIdentityServer();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");
            endpoints.MapControllers();
            endpoints.MapRazorPages();
        });

        app.UseSpa(spa =>
        {
            // To learn more about options for serving an Angular SPA from ASP.NET Core,
            // see https://go.microsoft.com/fwlink/?linkid=864501

            spa.Options.SourcePath = "ClientApp";
        });
    }

    private void RegisteredServicesPage(IApplicationBuilder app)
    {
        app.Map("/services", builder => builder.Run(async context =>
        {
            StringBuilder sb = new();
            sb.Append("<h1>Registered Services</h1>");
            sb.Append("<table><thead>");
            sb.Append("<tr><th>Type</th><th>Lifetime</th><th>Instance</th></tr>");
            sb.Append("</thead><tbody>");
            foreach (ServiceDescriptor svc in _services)
            {
                sb.Append("<tr>");
                sb.Append($"<td>{svc.ServiceType.FullName}</td>");
                sb.Append($"<td>{svc.Lifetime}</td>");
                sb.Append($"<td>{svc.ImplementationType?.FullName}</td>");
                sb.Append("</tr>");
            }
            sb.Append("</tbody></table>");
            await context.Response.WriteAsync(sb.ToString());
        }));
    }
}