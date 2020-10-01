using System;
using System.Text.Json.Serialization;

namespace BlazorApp.Api.DataAccess.Model
{
    public class ArticleDB
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
        [JsonPropertyName("createdDate")]
        public DateTimeOffset CreatedDate { get; set; }
    }
}
