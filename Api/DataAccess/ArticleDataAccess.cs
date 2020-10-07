using BlazorApp.Api.DataAccess.Model;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Api.DataAccess
{
    public interface IArticleDataAccess
    {
        Task<ArticleDB> GetArticle(string id);
        Task<List<ArticleThumbnailDB>> GetArticles();
    }

    public class ArticleDataAccess : IArticleDataAccess
    {
        private readonly Container _container;

        public ArticleDataAccess(CosmosClient dbClient)
        {
            _container = dbClient.GetContainer(Environment.GetEnvironmentVariable("SparksBlogDatabaseName"), Environment.GetEnvironmentVariable("ArticleContainerName"));
        }

        public async Task<ArticleDB> GetArticle(string id)
        {
            var articles = new List<ArticleDB>();
            var queryDef = new QueryDefinition("SELECT * FROM Articles a WHERE a.id = @id")
                .WithParameter("@id", id);

            var options = new QueryRequestOptions() { MaxBufferedItemCount = 100 };
            options.MaxConcurrency = 1; //max parallel tasks
            using (var query = _container.GetItemQueryIterator<ArticleDB>(
                queryDef,
                requestOptions: options))
            {
                while (query.HasMoreResults)
                {
                    foreach (var article in await query.ReadNextAsync())
                    {
                        articles.Add(article);
                    }
                }
            }

            return articles.FirstOrDefault();
        }

        public async Task<List<ArticleThumbnailDB>> GetArticles()
        {
            var articles = new List<ArticleThumbnailDB>();
            var queryDef = new QueryDefinition("SELECT a.id, a.title, a.createdDate FROM Articles a");

            var options = new QueryRequestOptions() { MaxBufferedItemCount = 100 };
            options.MaxConcurrency = 1; //max parallel tasks
            using (var query = _container.GetItemQueryIterator<ArticleThumbnailDB>(
                queryDef,
                requestOptions: options))
            {
                while (query.HasMoreResults)
                {
                    foreach (var article in await query.ReadNextAsync())
                    {
                        articles.Add(article);
                    }
                }
            }

            return articles;
        }
    }
}
