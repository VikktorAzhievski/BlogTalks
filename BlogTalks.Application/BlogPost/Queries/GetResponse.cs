using BlogTalks.Application.BlogPost.Responses;
using BlogTalks.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.BlogPost.Commands
{
    public class GetResponse
    {
        public List<BlogPostModel> BlogPosts { get; set; } = new List<BlogPostModel>();
        public Metadata Metadata { get; set; } = new Metadata();
    }
}
