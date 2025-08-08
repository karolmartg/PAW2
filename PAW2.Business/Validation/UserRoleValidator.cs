using PAW2.Models;

namespace PAW2.Business.Validation
{
    public interface IUserRoleValidator
    {
        void ValidateForSave(UserRole ur);
        void ValidateForDelete(UserRole ur);
        void ValidateId(int id);
    }

    public class UserRoleValidator : IUserRoleValidator
    {
        public void ValidateForSave(UserRole ur)
        {
            if (ur is null) throw new ArgumentNullException(nameof(ur));
            if (ur.Id <= 0) throw new ArgumentException("RoleId must be > 0.", nameof(ur.Id));
            if (ur.UserId <= 0) throw new ArgumentException("UserId must be > 0.", nameof(ur.UserId));
        }

        public void ValidateForDelete(UserRole ur)
        {
            if (ur is null) throw new ArgumentNullException(nameof(ur));
            if (ur.Id <= 0) throw new ArgumentException("Id must be > 0.", nameof(ur.Id));
        }

        public void ValidateId(int id)
        {
            if (id <= 0) throw new ArgumentException("Id must be > 0.", nameof(id));
        }
    }
}
