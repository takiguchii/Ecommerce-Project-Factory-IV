using Ecommerce.Entity;

namespace Ecommerce.Interfaces.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);
        void Update(User user);
        Task<User?> GetByAppUserIdAsync(string appUserId);
        void SaveChanges();
        Task<User?> FindByAppUserIdAsync(string appUserId);
    }
}