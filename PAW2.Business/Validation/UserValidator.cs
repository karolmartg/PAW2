using PAW2.Models;

namespace PAW2.Business.Validation
{
    public interface IUserValidator
    {
        void ValidateForSave(User u);
        void ValidateForDelete(User u);
        void ValidateId(int id);
    }

    public class UserValidator : IUserValidator
    {
        public void ValidateForSave(User u)
        {
            if (u is null) throw new ArgumentNullException(nameof(u));
            if (string.IsNullOrWhiteSpace(u.Username) || u.Username.Length > 255)
                throw new ArgumentException("Username is required (max 255).", nameof(u.Username));
        }

        public void ValidateForDelete(User u)
        {
            if (u is null) throw new ArgumentNullException(nameof(u));
            if (u.UserId <= 0)
                throw new ArgumentException("UserId must be > 0.", nameof(u.UserId));
        }

        public void ValidateId(int id)
        {
            if (id <= 0) throw new ArgumentException("Id must be > 0.", nameof(id));
        }
    }
}
