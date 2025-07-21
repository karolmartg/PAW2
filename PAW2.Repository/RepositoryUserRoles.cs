using Microsoft.EntityFrameworkCore;
using PAW2.Models;
using PAW2.Models.ViewModels;
using System.Linq.Expressions;

namespace PAW2.Repositories
{
    public interface IRepositoryUserRole
    {
        Task<bool> UpsertAsync(UserRole entity, bool isUpdating);
        Task<bool> CreateAsync(UserRole entity);
        Task<bool> DeleteAsync(UserRole entity);
        Task<IEnumerable<UserRole>> ReadAsync();
        Task<UserRole> FindAsync(int id);
        Task<bool> UpdateAsync(UserRole entity);
        Task<bool> UpdateManyAsync(IEnumerable<UserRole> entities);
        Task<bool> ExistsAsync(UserRole entity);
        Task<bool> CheckBeforeSavingAsync(UserRole entity);
        Task<IEnumerable<UserRoleViewModel>> FilterAsync(Expression<Func<UserRole, bool>> predicate);
    }

    public class RepositoryUserRole : RepositoryBase<UserRole>, IRepositoryUserRole
    {
        public async Task<bool> CheckBeforeSavingAsync(UserRole entity)
        {
            var exists = await ExistsAsync(entity);
            if (exists) { };
            
            return await UpsertAsync(entity, exists);
        }
    
        public async Task<IEnumerable<UserRoleViewModel>> FilterAsync(Expression<Func<UserRole, bool>> predicate)
        {
            var random = new Random();

            return await DbContext.UserRoles.Where(predicate)
                .Select(x => new UserRoleViewModel
                {
                    Id = x.Id,
                    RoldId = x.RoldId,
                    UserId = x.UserId,
                    TempID = random.Next(1, 1000) // Simulating a temporary ID for the view model
                }).ToListAsync();
        }
    
        public async new Task<bool> ExistsAsync (UserRole entity)
        {
            return await DbContext.UserRoles.AnyAsync(x => x.Id == entity.Id);
        }
    }
}
