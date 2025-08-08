using PAW2.Business.Validation;
using PAW2.Models;
using PAW2.Models.ViewModels;
using System.Linq.Expressions;

namespace PAW2.Business
{
    public class UserRoleValidationDecorator : IBusinessUserRole
    {
        private readonly IBusinessUserRole _inner;
        private readonly IUserRoleValidator _validator;

        public UserRoleValidationDecorator(IBusinessUserRole inner, IUserRoleValidator validator)
        {
            _inner = inner;
            _validator = validator;
        }

        public Task<IEnumerable<UserRole>> GetAllUserRolesAsync()
            => _inner.GetAllUserRolesAsync();

        public async Task<UserRole> GetUserRoleAsync(int id)
        {
            _validator.ValidateId(id);
            return await _inner.GetUserRoleAsync(id);
        }

        public async Task<bool> SaveUserRoleAsync(UserRole userRole)
        {
            _validator.ValidateForSave(userRole);
            return await _inner.SaveUserRoleAsync(userRole);
        }

        public async Task<bool> DeleteUserRoleAsync(UserRole userRole)
        {
            _validator.ValidateForDelete(userRole);
            return await _inner.DeleteUserRoleAsync(userRole);
        }

        public Task<IEnumerable<UserRoleViewModel>> Filter(Expression<Func<UserRole, bool>> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            return _inner.Filter(predicate);
        }
    }
}
