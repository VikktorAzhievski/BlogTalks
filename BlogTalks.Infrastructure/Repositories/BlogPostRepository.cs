using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using BlogTalks.Infrastructure.Data.DataContext;
using Microsoft.EntityFrameworkCore;

namespace BlogTalks.Infrastructure.Repositories
{
    public class BlogPostRepository : GenericRepository<BlogPost>, IBlogPostRepository
    {
        public BlogPostRepository(ApplicationDbContext context) : base(context) { }

        public BlogPost? GetBlogPostByName(string name)
        {
            return _dbSet.FirstOrDefault(p => p.Title.Equals(name));
        }

        public IQueryable<BlogPost> Query() => _context.Blogs.AsQueryable();

        public async Task<(int count, List<BlogPost> list)> GetPagedAsync(int pageNumber, int pageSize, string? searchWord, string? tag)
        {
            var query = _context.Blogs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchWord))
                query = query.Where(bp => bp.Title.Contains(searchWord) || bp.Text.Contains(searchWord));

            if (!string.IsNullOrWhiteSpace(tag))
                query = query.Where(bp => bp.Tags.Contains(tag));

            var count = query.Count();
            var list = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (count, list);
        }

    }
}