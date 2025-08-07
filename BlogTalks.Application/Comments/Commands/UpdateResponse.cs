using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comments.Commands
{
    public record UpdateResponse
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int BlogPostId { get; set; }

    }
}
