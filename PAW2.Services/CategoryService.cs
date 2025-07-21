using APW.Architecture;
using PAW.Architecture.Providers;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;


namespace PAW2.Services
{
    public interface ICategoryService
    {
        Task<Category> GetCategoryAsync(int id);
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<bool> DeleteCategoryAsync(int id);
        Task<bool> SaveCategoryAsync(IEnumerable<Category> categories);
        Task<IEnumerable<CategoryViewModel>> FilterCategoryAsync(ConditionViewModel content);
        Task<IEnumerable<CategoryViewModel>> FilterBusinessAsync(string filter);
    }

    public class CategoryService(IRestProvider restProvider) : ICategoryService
    {
        public async Task<Category> GetCategoryAsync(int id)
        {
            var result = await restProvider.GetAsync("https://localhost:7197/Category/", "");
            var category = await JsonProvider.DeserializeAsync<Category>(result);
            return category;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            var results = await restProvider.GetAsync("https://localhost:7197/Category/", null);
            var catalogs = await JsonProvider.DeserializeAsync<IEnumerable<Category>>(results);
            return catalogs;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var results = await restProvider.DeleteAsync("https://localhost:7197/Category/", $"{id}");
            return true;
        }

        public async Task<bool> SaveCategoryAsync(IEnumerable<Category> categories)
        {
            var content = JsonProvider.Serialize(categories);
            var result = await restProvider.PostAsync("https://localhost:7197/Category/", content);
            return true;
        }

        public async Task<IEnumerable<CategoryViewModel>> FilterCategoryAsync(ConditionViewModel content)
        {
            var result = await restProvider.PostAsync("https://localhost:7197/Category/filter", JsonProvider.Serialize(content));
            var categories = await JsonProvider.DeserializeAsync<IEnumerable<CategoryViewModel>>(result);
            return categories;
        }

        public async Task<IEnumerable<CategoryViewModel>> FilterBusinessAsync(string filter)
        {
            var result = await restProvider.GetAsync($"https://localhost:7197/Category/filter?filter={filter}", null);
            var categories = await JsonProvider.DeserializeAsync<IEnumerable<CategoryViewModel>>(result);
            return categories;
        }
    }
}
