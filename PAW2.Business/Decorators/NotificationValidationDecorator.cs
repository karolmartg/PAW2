using PAW2.Business.Validation;
using PAW2.Models;
using PAW2.Models.ViewModels;
using System.Linq.Expressions;

namespace PAW2.Business
{
    public class NotificationValidationDecorator : IBusinessNotification
    {
        private readonly IBusinessNotification _inner;
        private readonly INotificationValidator _validator;

        public NotificationValidationDecorator(IBusinessNotification inner, INotificationValidator validator)
        {
            _inner = inner;
            _validator = validator;
        }

        public Task<IEnumerable<Notification>> GetAllNotificationsAsync()
            => _inner.GetAllNotificationsAsync();

        public async Task<Notification> GetNotificationAsync(int id)
        {
            _validator.ValidateId(id);
            return await _inner.GetNotificationAsync(id);
        }

        public async Task<bool> SaveNotificationAsync(Notification notification)
        {
            _validator.ValidateForSave(notification);
            return await _inner.SaveNotificationAsync(notification);
        }

        public async Task<bool> DeleteNotificationAsync(Notification notification)
        {
            _validator.ValidateForDelete(notification);
            return await _inner.DeleteNotificationAsync(notification);
        }

        public Task<IEnumerable<NotificationViewModel>> Filter(Expression<Func<Notification, bool>> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            return _inner.Filter(predicate);
        }
    }
}
