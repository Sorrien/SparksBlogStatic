using BlazorApp.Api.Coordinators;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorApp.Api.Functions
{
    public class ArticleFunction
    {
        private readonly IArticleCoordinator _articleCoordinator;
        private readonly ILogger<ArticleFunction> _logger;

        public ArticleFunction(IArticleCoordinator articleCoordinator, ILogger<ArticleFunction> logger)
        {
            _articleCoordinator = articleCoordinator;
            _logger = logger;
        }

        [FunctionName("GetArticle")]
        [Display(Name = "Get Article", Description = "Gets an article")]
        [ProducesResponseType(typeof(HttpResponseMessage), (int)HttpStatusCode.OK)]
        public async Task<HttpResponseMessage> GetArticle([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Article/{id}")] GetArticleRequest request)
        {
            HttpStatusCode statusCode;
            Article article;
            try
            {
                article = await _articleCoordinator.GetArticle(request.Id);
                if (article == null)
                {
                    statusCode = HttpStatusCode.NotFound;
                }
                else
                {
                    statusCode = HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get article.");
                statusCode = HttpStatusCode.InternalServerError;
                article = null;
            }

            return new HttpResponseMessage()
            {
                StatusCode = statusCode,
                Content = new StringContent(JsonSerializer.Serialize(article), Encoding.UTF8, MediaTypeNames.Application.Json)
            };
        }

        [FunctionName("GetArticles")]
        [Display(Name = "Get Articles", Description = "Get articles")]
        [ProducesResponseType(typeof(HttpResponseMessage), (int)HttpStatusCode.OK)]
        public async Task<HttpResponseMessage> GetArticles([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Article")] HttpRequestMessage request)
        {
            HttpStatusCode statusCode;
            List<ArticleThumbnail> articles;
            var exceptionMessage = "";
            try
            {
                articles = await _articleCoordinator.GetArticles();
                statusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get article.");
                statusCode = HttpStatusCode.InternalServerError;
                articles = null;
                exceptionMessage = $"{{ \"Error\": \"Message: {ex.Message} StackTrace: {ex.StackTrace} InnerMessage: {ex.InnerException?.Message}\" }}";
            }

            return new HttpResponseMessage()
            {
                StatusCode = statusCode,
                Content = new StringContent(articles == null ? exceptionMessage : JsonSerializer.Serialize(articles), Encoding.UTF8, MediaTypeNames.Application.Json)
            };
        }
    }
}
