using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogTalks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BlogTalks.Application.Abstractions;

namespace BlogTalks.Infrastructure.Data.DataContext
{
    public sealed class ApplicationDbContext (DbContextOptions<ApplicationDbContext>options) : DbContext(options),IApplicationDbContext
    {
        public DbSet<BlogPost> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogPost>().HasKey(p => p.Id);
            modelBuilder.Entity<BlogPost>()
                .HasMany(p => p.Comments)
                .WithOne(p => p.Blog)
                .HasForeignKey(p => p.BlogPostId)
                .IsRequired();

            modelBuilder.Entity<Comment>().HasKey(p => p.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
