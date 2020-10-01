using System;

namespace BlazorApp.Shared
{
    public class ArticleThumbnail
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
