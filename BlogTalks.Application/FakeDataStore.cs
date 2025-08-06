using BlogTalks.Domain.Entities;

public class FakeDataStore
{
    private static List<Comment> _comments;
    private static List<BlogPost> _blogPosts;

    public FakeDataStore()
    {
        if (_comments == null)
        {
            _comments = new List<Comment>
            {
                new Comment { Id = 1, Text = "This is the first comment", CreatedAt = DateTime.Now, CreatedBy = 1},
                new Comment { Id = 2, Text = "This is the second comment", CreatedAt = DateTime.Now, CreatedBy = 2 }
            };
        }

        if (_blogPosts == null)
        {
            _blogPosts = new List<BlogPost>
            {
                new BlogPost
                {
                    Id = 1,
                    Title = "First Post",
                    Text = "This is the first post",
                    CreatedBy = 1,
                    Tags = new List<string> { "tag1", "tag2" },
                    Comments = _comments.Where(c => c.BlogPostId == 1).ToList()
                },
                new BlogPost
                {
                    Id = 2,
                    Title = "Second Post",
                    Text = "This is the second post",
                    CreatedBy = 2,
                    Tags = new List<string> { "tag3" },
                    Comments = _comments.Where(c => c.BlogPostId == 2).ToList()
                }
            };
        }
    }
    public async Task AddComment(Comment comment)
    {
        _comments.Add(comment);
        await Task.CompletedTask;
    }
    public async Task<bool> DeleteCommentAsync(int id)
    {
        var comment = _comments.FirstOrDefault(c => c.Id == id);
        if (comment == null)
            return false;

        _comments.Remove(comment);
        return true;
    }
    public async Task SaveAsync(Comment comment)
    {
        
    }

    public Task<List<Comment>> GetAllComments()
    {
        return Task.FromResult(_comments);
    }

    public Task<List<BlogPost>> GetAllBlogs()
    {
        return Task.FromResult(_blogPosts);
    }
    public async Task<Comment> GetCommentById(int id) => await Task.FromResult(_comments.Single(p => p.Id == id));

    public async Task<IEnumerable<Comment>> GetCommentsByBlogPostId(int blogPostId)
    {
        return await Task.FromResult(_comments.Where(c => c.BlogPostId == blogPostId));
        
    }

}
