using APW.Architecture;
using PAW.Architecture.Providers;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;

namespace PAW2.Services
{
    public interface IUserService
    {
        Task<User> GetUserAsync(int id);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<bool> DeleteUserAsync(int id);
        Task<bool> SaveUsersAsync(IEnumerable<User> catalogs);
        Task<IEnumerable<UserViewModel>> FilterUserAsync(ConditionViewModel content);
    }

    public class UserService(IRestProvider restProvider) : IUserService
    {
        public async Task<User> GetUserAsync(int id)
        {
            var result = await restProvider.GetAsync("https://localhost:7197/User/", "1");
            var user = await JsonProvider.DeserializeAsync<User>(result);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var results = await restProvider.GetAsync("https://localhost:7197/User/", null);
            var users = await JsonProvider.DeserializeAsync<IEnumerable<User>>(results);
            return users;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var result = await restProvider.DeleteAsync("https://localhost:7197/User/", $"{id}");
            return true;
        }

        public async Task<bool> SaveUsersAsync(IEnumerable<User> catalogs)
        {
            var content = JsonProvider.Serialize(catalogs);
            var result = await restProvider.PostAsync("https://localhost:7197/User", content);
            return true;
        }

        public async Task<IEnumerable<UserViewModel>> FilterUserAsync(ConditionViewModel content)
        {
            var result = await restProvider.PostAsync("https://localhost:7197/User/filter", JsonProvider.Serialize(content));
            var users = await JsonProvider.DeserializeAsync<IEnumerable<UserViewModel>>(result);
            return users;
        }
    }
}
