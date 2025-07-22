using System;
using System.Net.Http;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using XRoadTestApp;
using XRoadTestApp.Models;
using XRoadTestApp.Services;
using XRoadTestApp.XRoad.Services.TorService;
using XRoadTestApp.XRoad.Services.XRoadService;

[assembly: FunctionsStartup(typeof(Startup))]

namespace XRoadTestApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            Console.WriteLine("[STARTUP] Startup.Configure called");

            // Add configuration - only client and connection details
            builder.Services.AddOptions<XRoadConfig>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    Console.WriteLine("[STARTUP] Configuring XRoadConfig");

                    settings.SecurityServerUrl = configuration["XRoad:SecurityServerUrl"];
                    settings.ClientInstance = configuration["XRoad:ClientInstance"];
                    settings.ClientMemberClass = configuration["XRoad:ClientMemberClass"];
                    settings.ClientMemberCode = configuration["XRoad:ClientMemberCode"];
                    settings.ClientSubsystemCode = configuration["XRoad:ClientSubsystemCode"];
                    settings.ProtocolVersion = configuration["XRoad:ProtocolVersion"];

                    // Parse SSL settings more carefully
                    var ignoreSslErrorsStr = configuration["XRoad:IgnoreSslErrors"];
                    settings.IgnoreSslErrors = !string.IsNullOrEmpty(ignoreSslErrorsStr) &&
                                               (ignoreSslErrorsStr.ToLower() == "true" || ignoreSslErrorsStr == "1");

                    settings.ClientCertificatePath = configuration["XRoad:ClientCertificatePath"];
                    settings.ClientCertificatePassword = configuration["XRoad:ClientCertificatePassword"];

                    if (int.TryParse(configuration["XRoad:Timeout"], out int timeout))
                        settings.Timeout = timeout;
                    else
                        settings.Timeout = 30;

                    Console.WriteLine($"[STARTUP] Configuration loaded - IgnoreSslErrors: {settings.IgnoreSslErrors}");
                });

            // ALTERNATIVE APPROACH: Use a factory pattern instead of HttpClient injection
            builder.Services.AddSingleton<IHttpClientFactory>(serviceProvider =>
            {
                Console.WriteLine("[STARTUP] Creating custom HttpClientFactory");

                var config = serviceProvider.GetRequiredService<IOptions<XRoadConfig>>().Value;

                var services = new ServiceCollection();
                services.AddHttpClient("XRoadClient", client =>
                    {
                        client.Timeout = TimeSpan.FromSeconds(config.Timeout);
                        Console.WriteLine($"[STARTUP] HttpClient configured with timeout: {config.Timeout}s");
                    })
                    .ConfigurePrimaryHttpMessageHandler(() =>
                    {
                        Console.WriteLine("[STARTUP] ConfigurePrimaryHttpMessageHandler called");
                        Console.WriteLine($"[STARTUP] IgnoreSslErrors: {config.IgnoreSslErrors}");

                        var handler = new HttpClientHandler();

                        if (config.IgnoreSslErrors)
                        {
                            Console.WriteLine("[STARTUP] Setting up SSL certificate validation callback");

                            handler.ServerCertificateCustomValidationCallback =
                                (sender, certificate, chain, sslPolicyErrors) =>
                                {
                                    Console.WriteLine($"[SSL CALLBACK] Certificate validation callback called!");
                                    Console.WriteLine($"[SSL CALLBACK] SSL Policy Errors: {sslPolicyErrors}");
                                    Console.WriteLine($"[SSL CALLBACK] Certificate Subject: {certificate?.Subject}");
                                    Console.WriteLine($"[SSL CALLBACK] Returning true to ignore SSL errors");
                                    return true;
                                };
                        }

                        Console.WriteLine("[STARTUP] HttpClientHandler configured");
                        return handler;
                    });

                var serviceProvider2 = services.BuildServiceProvider();
                return serviceProvider2.GetRequiredService<IHttpClientFactory>();
            });

            // Add services
            builder.Services.AddScoped<IXRoadService, XRoadService>();
            builder.Services.AddScoped<ITorService, TorService>();

            Console.WriteLine("[STARTUP] All services registered");
        }
    }
}