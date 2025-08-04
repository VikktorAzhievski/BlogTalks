using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogTalks.Domain.Entities
{
    public class Blog
    {
        public Blog()
        {
        }
        public int id { get; set; }

        public required string Title { get; set; }

        public string Text { get; set; } = string.Empty;

        public List<string> Tags { get; set; } = new List<string>();

        public DateTime Timestamp { get; set; }

        public int CreatedBy { get; set; }

        public List<Comment> Comments { get; set; }


    }


}
