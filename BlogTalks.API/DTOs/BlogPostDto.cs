namespace BlogTalks.API.DTOs
{
    public class BlogPostDto
    {
        public long Id { get; set; }

        public required string Title { get; set; }

        public string Text { get; set; } = string.Empty;

        public List<string> Tags { get; set; } = new List<string>();

        public DateTime Timestamp { get; set; }

        public int CreatedBy { get; set; }

        public List<CommentsDto> comments { get; set; } = new List<CommentsDto>();
        public List<CommentsDto> Comments { get; internal set; }
    }
}
