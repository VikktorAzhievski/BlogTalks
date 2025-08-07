using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using BlogTalks.Infrastructure.Data.DataContext;


namespace BlogTalks.Infrastructure.Repositories
{
    public class BlogPostRepository : GenericRepository<BlogPost>, IBlogPostRepository
    {
        public BlogPostRepository(ApplicationDbContext context) : base(context) { }

        public BlogPost? GetBlogPostByName(string name)
        {
            return _dbSet.FirstOrDefault(p => p.Title.Equals(name));
        }


    }
}