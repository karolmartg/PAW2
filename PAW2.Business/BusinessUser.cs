using PAW2.Data.Models;
using PAW2.Repositories;
using PAW2.Core;
using PAW2.Models;
using PAW2.Core.Extensions;
using PAW2.Models.ViewModels;
using System.Linq.Expressions;

namespace PAW2.Business;
public interface IBusinessUser
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<bool> SaveUserAsync(User user);
    Task<bool> DeleteUserAsync(User user);
    Task<User> GetUserAsync(int id);
    Task<IEnumerable<UserViewModel>> Filter(Expression<Func<User, bool>> predicate);
}

public class BusinessUser(IRepositoryUser repositoryUser) : IBusinessUser
{
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await repositoryUser.ReadAsync();
    }

    public async Task<bool> SaveUserAsync(User user)
    { 
       var _user = "";
       user.AddAudit(_user);
       user.AddLogging(user.UserId <= 0 ? Models.Enums.LoggingType.Create : Models.Enums.LoggingType.Update);

        return await repositoryUser.CheckBeforeSavingAsync(user);
    }

    public async Task<bool> DeleteUserAsync(User user)
    {
        return await repositoryUser.DeleteAsync(user);
    }

    public async Task<User> GetUserAsync(int id)
    {
        return await repositoryUser.FindAsync(id);
    }

    public async Task<IEnumerable<UserViewModel>> Filter(Expression<Func<User, bool>> predicate)
    {
        return await repositoryUser.FilterAsync(predicate);
    }
}