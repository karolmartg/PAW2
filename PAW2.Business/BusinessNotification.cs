using PAW2.Data.Models;
using PAW2.Repositories;
using PAW2.Core;
using PAW2.Models;
using PAW2.Core.Extensions;
using PAW2.Models.ViewModels;
using System.Linq.Expressions;

namespace PAW2.Business;
public interface IBusinessNotification
{
    Task<IEnumerable<Notification>> GetAllNotificationsAsync();
    Task<bool> SaveNotificationAsync(Notification catalog);
    Task<bool> DeleteNotificationAsync(Notification catalog);
    Task<Notification> GetNotificationAsync(int id);
    Task<IEnumerable<NotificationViewModel>> Filter(Expression<Func<Notification, bool>> predicate);
}

public class BusinessNotification(IRepositoryNotification repositoryNotification) : IBusinessNotification
{
    public async Task<IEnumerable<Notification>> GetAllNotificationsAsync()
    {
        return await repositoryNotification.ReadAsync();
    }

    public async Task<bool> SaveNotificationAsync(Notification notification)
    { 
       var user = "";
       notification.AddAudit(user);
       notification.AddLogging(notification.Id <= 0 ? Models.Enums.LoggingType.Create : Models.Enums.LoggingType.Update);

        return await repositoryNotification.CheckBeforeSavingAsync(notification);
    }

    public async Task<bool> DeleteNotificationAsync(Notification notification)
    {
        return await repositoryNotification.DeleteAsync(notification);
    }

    public async Task<Notification> GetNotificationAsync(int id)
    {
        return await repositoryNotification.FindAsync(id);
    }

    public async Task<IEnumerable<NotificationViewModel>> Filter(Expression<Func<Notification, bool>> predicate)
    {
        return await repositoryNotification.FilterAsync(predicate);
    }
}