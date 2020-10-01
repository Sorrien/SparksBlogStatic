namespace BlazorApp.Api.DataAccess.Model
{
    public class CosmosDbClientOptions
    {
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }
        public string AccountEndpoint { get; set; }
        public string AuthKey { get; set; }
        public string PartitionKeyPath { get; set; }
    }
}
