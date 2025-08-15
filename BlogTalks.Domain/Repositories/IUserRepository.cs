using BlogTalks.Domain.Entities;
using System.Threading.Tasks;

namespace BlogTalks.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> RegisterAsync(string username, string password, string email);
        Task<User?> FindByEmail(string email);
        public IEnumerable<User> GetUsersByIds(IEnumerable<int> ids);

    }
}
