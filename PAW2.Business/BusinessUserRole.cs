using PAW2.Data.Models;
using PAW2.Repositories;
using PAW2.Core;
using PAW2.Models;
using PAW2.Core.Extensions;
using PAW2.Models.ViewModels;
using System.Linq.Expressions;

namespace PAW2.Business;
public interface IBusinessUserRole
{
    Task<IEnumerable<UserRole>> GetAllUserRolesAsync();
    Task<bool> SaveUserRoleAsync(UserRole userRole);
    Task<bool> DeleteUserRoleAsync(UserRole userRole);
    Task<UserRole> GetUserRoleAsync(int id);
    Task<IEnumerable<UserRoleViewModel>> Filter(Expression<Func<UserRole, bool>> predicate);
}

public class BusinessUserRole(IRepositoryUserRole repositoryUserRole): IBusinessUserRole
{
    public async Task<IEnumerable<UserRole>> GetAllUserRolesAsync()
    {
        return await repositoryUserRole.ReadAsync();
    }

    public async Task<bool> SaveUserRoleAsync(UserRole userRole)
    { 
       var user = "";
        userRole.AddAudit(user);
        userRole.AddLogging(userRole.Id <= 0 ? Models.Enums.LoggingType.Create : Models.Enums.LoggingType.Update);

        return await repositoryUserRole.CheckBeforeSavingAsync(userRole);
    }

    public async Task<bool> DeleteUserRoleAsync(UserRole userRole)
    {
        return await repositoryUserRole.DeleteAsync(userRole);
    }

    public async Task<UserRole> GetUserRoleAsync(int id)
    {
        return await repositoryUserRole.FindAsync(id);
    }

    public async Task<IEnumerable<UserRoleViewModel>> Filter(Expression<Func<UserRole, bool>> predicate)
    {
        return await repositoryUserRole.FilterAsync(predicate);
    }
}