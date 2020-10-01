using BlazorApp.Api.DataAccess;
using BlazorApp.Shared;
using System.Threading.Tasks;

namespace BlazorApp.Api.Coordinators
{
    public interface IArticleCoordinator
    {
        Task<Article> GetArticle(string id);
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
    }
}
