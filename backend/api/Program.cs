using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddSingleton<CosmosClient>(serviceProvider =>
        {
            var connectionString = Environment.GetEnvironmentVariable("AzureResumeConnectionString");
            return new CosmosClient(connectionString);
        });
    })
    .Build();

await host.RunAsync();