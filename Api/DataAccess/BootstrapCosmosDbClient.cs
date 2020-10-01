using BlazorApp.Api.DataAccess.Model;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BlazorApp.Api.DataAccess
{
    public static class BootstrapCosmosDbClient
    {
        private static event EventHandler initializeDatabase = delegate { };

        public static IServiceCollection AddCosmosDbService<TInterface, TImplement>(this IServiceCollection services, CosmosDbClientOptions options) where TImplement : CosmosDataAccess, TInterface, new()
            where TInterface : class
        {
            Func<IServiceProvider, TInterface> factory = (sp) =>
            {
                //resolve configuration
                //var configuration = sp.GetService<IConfigurationHelper>();
                //and get the configured settings(Microsoft.Extensions.Configuration.Binder.dll)
                //string databaseName = configuration.CosmosDbDatabaseName;
                //string containerName = configuration.CosmosDbCollectionName;
                //string account = configuration.CosmosDbAccount;
                //string key = configuration.CosmosDbKey;
                var databaseName = options.DatabaseName;
                var containerName = options.ContainerName;
                var account = options.AccountEndpoint;
                var key = options.AuthKey;

                var clientBuilder = new CosmosClientBuilder(account, key);
                var client = clientBuilder.WithConnectionModeDirect().Build();
                var cosmosDbService = new TImplement();
                cosmosDbService.Configure(client, databaseName, containerName);

                //async event handler
                EventHandler handler = null;
                handler = async (sender, args) =>
                {
                    initializeDatabase -= handler; //unsubscribe
                    DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
                    await database.Database.CreateContainerIfNotExistsAsync(containerName, options.PartitionKeyPath);
                };
                initializeDatabase += handler; //subscribe
                initializeDatabase(null, EventArgs.Empty); //raise the event to initialize db

                return cosmosDbService;
            };
            services.AddSingleton(factory);
            return services;
        }
    }
}
