using APW.Architecture;
using PAW.Architecture.Providers;
using PAW2.Data.Models;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;

namespace PAW2.Services
{
    public interface IUserRoleService
    {
        Task<UserRole> GetUserRoleAsync(int id);
        Task<IEnumerable<UserRole>> GetUserRolesAsync();
        Task<bool> DeleteUserRoleAsync(int id);
        Task<bool> SaveUserRolesAsync(IEnumerable<UserRole> userRoles);
        Task<IEnumerable<UserRoleViewModel>> FilterUserRoleAsync(ConditionViewModel content);
    }

    public class UserRoleService(IRestProvider restProvider) : IUserRoleService
    {
        public async Task<UserRole> GetUserRoleAsync(int id)
        {
            var result = await restProvider.GetAsync("https://localhost:7197/UserRole/", "1");
            var userRole = await JsonProvider.DeserializeAsync<UserRole>(result);
            return userRole;
        }

        public async Task<IEnumerable<UserRole>> GetUserRolesAsync()
        {
            var results = await restProvider.GetAsync("https://localhost:7197/UserRole/", null);
            var userRoles = await JsonProvider.DeserializeAsync<IEnumerable<UserRole>>(results);
            return userRoles;
        }

        public async Task<bool> DeleteUserRoleAsync(int id)
        {
            var result = await restProvider.DeleteAsync("https://localhost:7197/UserRole/", $"{id}");
            return true;
        }

        public async Task<bool> SaveUserRolesAsync(IEnumerable<UserRole> userRoles)
        {
            var content = JsonProvider.Serialize(userRoles);
            var result = await restProvider.PostAsync("https://localhost:7197/UserRole", content);
            return true;
        }

        public async Task<IEnumerable<UserRoleViewModel>> FilterUserRoleAsync(ConditionViewModel content)
        {
            var result = await restProvider.PostAsync("https://localhost:7197/UserRole/filter", JsonProvider.Serialize(content));
            var userRoles = await JsonProvider.DeserializeAsync<IEnumerable<UserRoleViewModel>>(result);
            return userRoles;
        }
    }
}
