using AutoMapper;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using LoopStats.Models.Entities;
using LoopStats.Repository;
using LoopStats.Services;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;


[assembly: FunctionsStartup(typeof(LoopStats.Startup))]

namespace LoopStats;

public class Startup : FunctionsStartup
{
    private IConfiguration _configuration;

    public override void Configure(IFunctionsHostBuilder builder)
    {
        _configuration = builder.GetContext().Configuration;
        ConfigureServices(builder.Services);
    }

    private void ConfigureServices(IServiceCollection services)
    {
        var uri = _configuration.GetSection("graphQlUrl").Value;
        services.AddScoped<IGraphQLClient>(s => new GraphQLHttpClient(uri, new NewtonsoftJsonSerializer()));
        services.AddScoped<ILoopringGraphConsumer, LoopringGraphConsumer>();
        services.AddCloudTable(_configuration);
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}

static class CustomExtensionMethods
{
    public static void AddCloudTable(this IServiceCollection services, IConfiguration configuration)
    {
        var storageAccount = CloudStorageAccount.Parse(configuration.GetSection("connectionString").Value);

        var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

        var statsTable = tableClient.GetTableReference(configuration.GetSection("tableName").Value);

        if (!statsTable.Exists())
        {
            statsTable.Create();
            var sequence = new LoopringStatsEntity
            {
                PartitionKey = DateTime.UtcNow.ToString(),
                RowKey = "Test"
            };

            statsTable.Execute(TableOperation.Insert(sequence));
        }
        services.AddSingleton(statsTable);
        services.AddScoped<IStatsRepository<LoopringStatsEntity>, StatsRepository<LoopringStatsEntity>>();
    }
}
