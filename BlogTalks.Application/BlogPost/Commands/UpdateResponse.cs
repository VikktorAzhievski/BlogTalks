﻿using BlogTalks.Application.Comments.Queries;

namespace BlogTalks.Application.BlogPost.Commands
{
    public class UpdateResponse
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string Text { get; set; } = string.Empty;

        public int CreatedBy { get; set; }
        public DateTime Timestamp { get; set; }
        public List<string> Tags { get; set; } = new();

    }
}
