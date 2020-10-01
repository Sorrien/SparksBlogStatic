using Microsoft.Azure.Cosmos;


namespace BlazorApp.Api.DataAccess
{
    public abstract class CosmosDataAccess
    {
        private Container _container;

        public virtual void Configure(CosmosClient dbClient, string databaseName, string containerName)
        {
            _container = dbClient.GetContainer(databaseName, containerName);
        }
    }
}
