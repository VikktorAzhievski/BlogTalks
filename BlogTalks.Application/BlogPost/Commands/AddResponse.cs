using BlogTalks.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.BlogPost.Commands
{
    public class AddResponse
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string Text { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
        public DateTime Timestamp { get; set; }
        public int CreatedBy { get; set; }
    }
}

