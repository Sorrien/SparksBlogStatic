using System;

namespace BlazorApp.Shared
{

    public class Article
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
