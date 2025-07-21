using Microsoft.EntityFrameworkCore;
using PAW2.Models;
using PAW2.Models.ViewModels;
using System.Linq.Expressions;

namespace PAW2.Repositories
{
    public interface IRepositoryUser
    {
        Task<bool> UpsertAsync(User entity, bool isUpdating);
        Task<bool> CreateAsync(User entity);
        Task<bool> DeleteAsync(User entity);
        Task<IEnumerable<User>> ReadAsync();
        Task<User> FindAsync(int id);
        Task<bool> UpdateAsync(User entity);
        Task<bool> UpdateManyAsync(IEnumerable<User> entities);
        Task<bool> ExistsAsync(User entity);
        Task<bool> CheckBeforeSavingAsync(User entity);
        Task<IEnumerable<UserViewModel>> FilterAsync(Expression<Func<User, bool>> predicate);
    }

    public class RepositoryUser : RepositoryBase<User>, IRepositoryUser
    {
        public async Task<bool> CheckBeforeSavingAsync(User entity)
        {
            var exists = await ExistsAsync(entity);
            if (exists) { };
            
            return await UpsertAsync(entity, exists);
        }
    
        public async Task<IEnumerable<UserViewModel>> FilterAsync(Expression<Func<User, bool>> predicate)
        {
            var random = new Random();

            return await DbContext.Users.Where(predicate)
                .Select(x => new UserViewModel
                {
                    UserId = x.UserId,
                    Username = x.Username,
                    Email = x.Email,
                    PasswordHash = x.PasswordHash,
                    CreatedAt = x.CreatedAt,
                    IsActive = x.IsActive,
                    LastModified = x.LastModified,
                    ModifiedBy = x.ModifiedBy,
                    TempID = random.Next(1, 1000) // Simulating a temporary ID for the view model
                }).ToListAsync();
        }
    
        public async new Task<bool> ExistsAsync (User entity)
        {
            return await DbContext.Users.AnyAsync(x => x.UserId == entity.UserId);
        }
    }
}
