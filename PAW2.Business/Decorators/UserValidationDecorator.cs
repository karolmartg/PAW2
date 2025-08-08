using PAW2.Business.Validation;
using PAW2.Models;
using PAW2.Models.ViewModels;
using System.Linq.Expressions;

namespace PAW2.Business
{
    public class UserValidationDecorator : IBusinessUser
    {
        private readonly IBusinessUser _inner;
        private readonly IUserValidator _validator;

        public UserValidationDecorator(IBusinessUser inner, IUserValidator validator)
        {
            _inner = inner;
            _validator = validator;
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
            => _inner.GetAllUsersAsync();

        public async Task<User> GetUserAsync(int id)
        {
            _validator.ValidateId(id);
            return await _inner.GetUserAsync(id);
        }

        public async Task<bool> SaveUserAsync(User user)
        {
            _validator.ValidateForSave(user);
            return await _inner.SaveUserAsync(user);
        }

        public async Task<bool> DeleteUserAsync(User user)
        {
            _validator.ValidateForDelete(user);
            return await _inner.DeleteUserAsync(user);
        }

        public Task<IEnumerable<UserViewModel>> Filter(Expression<Func<User, bool>> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            return _inner.Filter(predicate);
        }
    }
}
