using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BlogTalks.Domain.Entities;

namespace BlogTalks.Domain.Repositories
{
    public interface IBlogPostRepository : IRepository<BlogPost>
    {
        BlogPost? GetBlogPostByName(string name);
    }
}