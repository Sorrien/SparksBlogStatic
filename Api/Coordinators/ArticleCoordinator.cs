using BlazorApp.Api.DataAccess;
using BlazorApp.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Api.Coordinators
{
    public interface IArticleCoordinator
    {
        Task<Article> GetArticle(string id);
        Task<List<ArticleThumbnail>> GetArticles();
    }

    public class ArticleCoordinator : IArticleCoordinator
    {
        private readonly IArticleDataAccess _articleDataAccess;
        public ArticleCoordinator(IArticleDataAccess articleDataAccess)
        {
            _articleDataAccess = articleDataAccess;
        }

        public async Task<Article> GetArticle(string id)
        {
            var articleItem = await _articleDataAccess.GetArticle(id);

            var article = articleItem == null ? null : new Article()
            {
                Id = articleItem.Id,
                Title = articleItem.Title,
                Content = articleItem.Content,
                CreatedDate = articleItem.CreatedDate
            };
            return article;
        }

        public async Task<List<ArticleThumbnail>> GetArticles()
        {
            var articleItems = await _articleDataAccess.GetArticles();
            var articles = articleItems.Select(x => new ArticleThumbnail() { Id = x.Id, Title = x.Title, CreatedDate = x.CreatedDate }).ToList();
            return articles;
        }
    }
}
