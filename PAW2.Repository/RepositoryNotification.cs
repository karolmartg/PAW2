using Microsoft.EntityFrameworkCore;
using PAW2.Models;
using PAW2.Models.ViewModels;
using System.Linq.Expressions;

namespace PAW2.Repositories
{
    public interface IRepositoryNotification
    {
        Task<bool> UpsertAsync(Notification entity, bool isUpdating);
        Task<bool> CreateAsync(Notification entity);
        Task<bool> DeleteAsync(Notification entity);
        Task<IEnumerable<Notification>> ReadAsync();
        Task<Notification> FindAsync(int id);
        Task<bool> UpdateAsync(Notification entity);
        Task<bool> UpdateManyAsync(IEnumerable<Notification> entities);
        Task<bool> ExistsAsync(Notification entity);
        Task<bool> CheckBeforeSavingAsync(Notification entity);
        Task<IEnumerable<NotificationViewModel>> FilterAsync(Expression<Func<Notification, bool>> predicate);
    }

    public class RepositoryNotification : RepositoryBase<Notification>, IRepositoryNotification
    {
        public async Task<bool> CheckBeforeSavingAsync(Notification entity)
        {
            var exists = await ExistsAsync(entity);
            if (exists) { };
            
            return await UpsertAsync(entity, exists);
        }
    
        public async Task<IEnumerable<NotificationViewModel>> FilterAsync(Expression<Func<Notification, bool>> predicate)
        {
            var random = new Random();

            return await DbContext.Notifications.Where(predicate)
                .Select(x => new NotificationViewModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Message = x.Message,
                    IsRead  = x.IsRead,
                    CreatedAt  = x.CreatedAt,
                    TempID = random.Next(1, 1000) // Simulating a temporary ID for the view model
                }).ToListAsync();
        }
    
        public async new Task<bool> ExistsAsync (Notification entity)
        {
            return await DbContext.Notifications.AnyAsync(x => x.Id == entity.Id);
        }
    }
}
