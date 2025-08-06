using BlogTalks.Domain.Entities;

namespace BlogTalks.Infrastructure
{
    public class FakeDataStoreBlog
    {
        private readonly List<BlogPost> _blogPosts = new()
        {
            new BlogPost
            {
                Id = 1,
                Title = "First Post",
                Text = "Welcome to BlogTalks!",
                Tags = new List<string> { "intro", "welcome" },
                CreatedBy = 1,
                Comments = new List<Comment>
                {
                    new Comment
                    {
                        Id = 1,
                        Text = "Nice post!",
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = 2,
                        BlogPostId = 1
                    }
                }
            },
            new BlogPost
            {
                Id = 2,
                Title = "Second Post",
                Text = "Another blog entry...",
                Tags = new List<string> { "update" },
                CreatedBy = 2,
               
                Comments = new List<Comment>()
            }
        };

        public async Task<List<BlogPost>> GetAllBlogPostsAsync()
        {
            return await Task.FromResult(_blogPosts);
        }

        public async Task<BlogPost?> GetBlogPostByIdAsync(int id)
        {
            var blog = _blogPosts.FirstOrDefault(p => p.Id == id);
            return await Task.FromResult(blog);
        }

        public async Task AddBlogPostAsync(BlogPost blog)
        {
            _blogPosts.Add(blog);
            await Task.CompletedTask;
        }

        public int GetNextBlogPostId()
        {
            return _blogPosts.Any() ? _blogPosts.Max(b => b.Id) + 1 : 1;
        }

        public async Task<bool> DeleteBlogPostAsync(int id)
        {
            var blog = _blogPosts.FirstOrDefault(b => b.Id == id);
            if (blog == null)
                return false;

            _blogPosts.Remove(blog);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateBlogPostAsync(BlogPost updatedBlog)
        {
            var existing = _blogPosts.FirstOrDefault(b => b.Id == updatedBlog.Id);
            if (existing == null)
                return false;

            existing.Title = updatedBlog.Title;
            existing.Text = updatedBlog.Text;
            existing.Tags = updatedBlog.Tags;
            existing.CreatedBy = updatedBlog.CreatedBy;
            existing.Comments = updatedBlog.Comments;

            return await Task.FromResult(true);
        }
    }
}
