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
        IQueryable<BlogPost> Query();
        Task<(int count, List<BlogPost> list)> GetPagedAsync(int pageNumber, int pageSize, string? searchWord, string? tag);
    }
}