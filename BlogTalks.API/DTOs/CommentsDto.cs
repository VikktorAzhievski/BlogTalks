namespace BlogTalks.API.DTOs
{
    public class CommentsDto
    {
        public int Id { get; set; }

        public required string Text { get; set; }

        public DateTime Timestamp { get; set; }

        public DateTime CreatedAt { get;  set; }
        public int CreatedBy { get;  set; }

        public int BlogPostId { get; set; }
    }
}
