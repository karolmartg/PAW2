using PAW2.Models;

namespace PAW2.Business.Validation
{
    public interface INotificationValidator
    {
        void ValidateForSave(Notification notification);
        void ValidateForDelete(Notification notification);
        void ValidateId(int id);
    }

    public class NotificationValidator : INotificationValidator
    {
        public void ValidateForSave(Notification n)
        {
            if (n is null) throw new ArgumentNullException(nameof(n));
            if (n.UserId <= 0) throw new ArgumentException("UserId must be > 0.", nameof(n.UserId));
            if (string.IsNullOrWhiteSpace(n.Message) || n.Message.Length > 255)
                throw new ArgumentException("Message is required (max 255).", nameof(n.Message));
        }

        public void ValidateForDelete(Notification n)
        {
            if (n is null) throw new ArgumentNullException(nameof(n));
            if (n.Id <= 0) throw new ArgumentException("Id must be > 0.", nameof(n.Id));
        }

        public void ValidateId(int id)
        {
            if (id <= 0) throw new ArgumentException("Id must be > 0.", nameof(id));
        }
    }
}
