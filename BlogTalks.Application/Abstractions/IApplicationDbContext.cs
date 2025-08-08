using Microsoft.EntityFrameworkCore;
using BlogTalks.Domain.Entities;


namespace BlogTalks.Application.Abstractions
{
    public interface IApplicationDbContext
    {
        public DbSet<Domain.Entities.BlogPost> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Domain.Entities.User> Users { get; set; }

    }
}
