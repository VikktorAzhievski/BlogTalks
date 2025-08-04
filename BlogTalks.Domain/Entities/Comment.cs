using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogTalks.Domain.Entities
{
    public class Comment
    {

        public int Id { get; set; }

        public required string Text { get; set; }

        public DateTime Timestamp { get; set; }

        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }

        public int BlogPostId { get; set; }

    }
}
