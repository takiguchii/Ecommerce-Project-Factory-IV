using Ecommerce.Data.Context;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EcommerceDbContext _dbContext;

        public UserRepository(EcommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(User user)
        {
            _dbContext.Users.Add(user);
        }

        public async Task<User?> GetByAppUserIdAsync(string appUserId)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.AppUserId == appUserId);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public void Update(User user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
        }
    }
}