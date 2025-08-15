using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.BlogPost.Responses
{
    public class BlogPostModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
        public string CreatorName { get; set; } = string.Empty;
    }
}
