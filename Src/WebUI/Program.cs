using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Northwind.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Northwind.Application.System.Commands.SeedSampleData;
using Northwind.Infrastructure;
using Northwind.WebUI;

IWebHost host = CreateWebHostBuilder(args).Build();

using (IServiceScope scope = host.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;

    try
    {
        NorthwindDbContext northwindContext = services.GetRequiredService<NorthwindDbContext>();
        await northwindContext.Database.MigrateAsync();

        ApplicationDbContext identityContext = services.GetRequiredService<ApplicationDbContext>();
        await identityContext.Database.MigrateAsync();

        IMediator mediator = services.GetRequiredService<IMediator>();
        await mediator.Send(new SeedSampleDataCommand(), CancellationToken.None);
    }
    catch (Exception ex)
    {
        ILogger<Program> logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or initializing the database.");
    }
}

await host.RunAsync();
return;

static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            IWebHostEnvironment env = hostingContext.HostingEnvironment;

            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

            if (env.IsDevelopment())
            {
                Assembly appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                config.AddUserSecrets(appAssembly, optional: true);
            }

            config.AddSystemsManager("/Northwind");
            config.AddEnvironmentVariables();

            config.AddCommandLine(args);
        })
        .UseStartup<Startup>();
