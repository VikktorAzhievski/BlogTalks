using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlogTalks.Domain.Shared;

namespace BlogTalks.Domain.Entities
{
    public class Comment : IEntity
    {

        public int Id { get; set; }

        public  string Text { get; set; }  = string.Empty;

        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }

        // Navigation
        public int BlogPostId { get; set; }
        public BlogPost Blog { get; set; } = new BlogPost();

    }
}
