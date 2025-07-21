using APW.Architecture;
using PAW.Architecture.Providers;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;

namespace PAW2.Services
{
    public interface INotificationService
    {
        Task<Notification> GetNotificationAsync(int id);
        Task<IEnumerable<Notification>> GetNotificationsAsync();
        Task<bool> DeleteNotificationAsync(int id);
        Task<bool> SaveNotificationsAsync(IEnumerable<Notification> notifications);
        Task<IEnumerable<NotificationViewModel>> FilterNotificationAsync(ConditionViewModel content);
    }

    public class NotificationService(IRestProvider restProvider) : INotificationService
    {
        public async Task<Notification> GetNotificationAsync(int id)
        {
            var result = await restProvider.GetAsync("https://localhost:7197/Notification/", "1");
            var notification = await JsonProvider.DeserializeAsync<Notification>(result);
            return notification;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsAsync()
        {
            var results = await restProvider.GetAsync("https://localhost:7197/Notification/", null);
            var notifications = await JsonProvider.DeserializeAsync<IEnumerable<Notification>>(results);
            return notifications;
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            var result = await restProvider.DeleteAsync("https://localhost:7197/Notification/", $"{id}");
            return true;
        }

        public async Task<bool> SaveNotificationsAsync(IEnumerable<Notification> notifications)
        {
            var content = JsonProvider.Serialize(notifications);
            var result = await restProvider.PostAsync("https://localhost:7197/Notification", content);
            return true;
        }

        public async Task<IEnumerable<NotificationViewModel>> FilterNotificationAsync(ConditionViewModel content)
        {
            var result = await restProvider.PostAsync("https://localhost:7197/Notification/filter", JsonProvider.Serialize(content));
            var notifications = await JsonProvider.DeserializeAsync<IEnumerable<NotificationViewModel>>(result);
            return notifications;
        }
    }
}
