using BlazorApp.Api.Coordinators;
using BlazorApp.Api.DataAccess;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

//this is necessary to get the app to call our startup class
[assembly: FunctionsStartup(typeof(BlazorApp.Api.Startup))]

namespace BlazorApp.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton(s =>
            {
                var connectionString = Environment.GetEnvironmentVariable("CosmosDBConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException(
                        "Please specify a valid CosmosDBConnection.");
                }

                return new CosmosClientBuilder(connectionString)
                    .Build();
            });

            builder.Services.AddTransient<IArticleDataAccess, ArticleDataAccess>();
            builder.Services.AddTransient<IArticleCoordinator, ArticleCoordinator>();
        }
    }
}