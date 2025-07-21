using APW.Architecture;
using PAW.Architecture.Providers;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;

namespace PAW2.Services
{
    public interface ICatalogService
    {
        Task<Catalog> GetCatalogAsync(int od);
        Task<IEnumerable<Catalog>> GetCatalogsAsync();
        Task<bool> DeleteCatalogAsync(int id);
        Task<bool> SaveCatalogsAsync(IEnumerable<Catalog> catalogs);
        Task<IEnumerable<CatalogViewModel>> FilterCatalogAsync(ConditionViewModel content);

    }

    public class CatalogService(IRestProvider restProvider) : ICatalogService
    {
        // Get a catalog by id
        public async Task<Catalog> GetCatalogAsync(int id)
        {
            var result = await restProvider.GetAsync("https://localhost:7197/Catalog/", "1");
            var catalog = await JsonProvider.DeserializeAsync<Catalog>(result);
            return catalog;
        }

        // Get All Catalogs
        public async Task<IEnumerable<Catalog>> GetCatalogsAsync()
        {
            var results = await restProvider.GetAsync("https://localhost:7197/Catalog/", null);
            var catalogs = await JsonProvider.DeserializeAsync<IEnumerable<Catalog>>(results);
            return catalogs;
        }

        // Delete a Catalog by Id
        public async Task<bool> DeleteCatalogAsync(int id)
        {
            var result = await restProvider.DeleteAsync("https://localhost:7197/Catalog/", $"{id}");
            return true;
        }

        // Save Catalogs
        public async Task<bool> SaveCatalogsAsync(IEnumerable<Catalog> catalogs)
        {
            var content = JsonProvider.Serialize(catalogs);
            var result = await restProvider.PostAsync("https://localhost:7197/Catalog", content);
            return true;
        }

        // Filter Catalogs
        public async Task<IEnumerable<CatalogViewModel>> FilterCatalogAsync(ConditionViewModel content)
        {
            var result = await restProvider.PostAsync("https://localhost:7197/Catalog/filter", JsonProvider.Serialize(content));
            var catalogs = await JsonProvider.DeserializeAsync<IEnumerable<CatalogViewModel>>(result);
            return catalogs;
        }
    }
}
