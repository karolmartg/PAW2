using APW.Architecture;
using PAW.Architecture.Providers;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;

namespace PAW2.Services
{
    public interface ISupplierService
    {
        Task<Supplier> GetSupplierAsync(int id);
        Task<IEnumerable<Supplier>> GetSuppliersAsync();
        Task<bool> DeleteSupplierAsync(int id);
        Task<bool> SaveSuppliersAsync(IEnumerable<Supplier> suppliers);
        Task<IEnumerable<SupplierViewModel>> FilterSupplierAsync(ConditionViewModel content);
    }

    public class SupplierService(IRestProvider restProvider) : ISupplierService
    {
        // Get a catalog by id
        public async Task<Supplier> GetSupplierAsync(int id)
        {
            var result = await restProvider.GetAsync("https://localhost:7197/Supplier/", "1");
            var supplier = await JsonProvider.DeserializeAsync<Supplier>(result);
            return supplier;
        }

        // Get All Catalogs
        public async Task<IEnumerable<Supplier>> GetSuppliersAsync()
        {
            var results = await restProvider.GetAsync("https://localhost:7197/Supplier/", null);
            var suppliers = await JsonProvider.DeserializeAsync<IEnumerable<Supplier>>(results);
            return suppliers;
        }

        // Delete a Catalog by Id
        public async Task<bool> DeleteSupplierAsync(int id)
        {
            var result = await restProvider.DeleteAsync("https://localhost:7197/Supplier/", $"{id}");
            return true;
        }

        // Save Catalogs
        public async Task<bool> SaveSuppliersAsync(IEnumerable<Supplier> suppliers)
        {
            var content = JsonProvider.Serialize(suppliers);
            var result = await restProvider.PostAsync("https://localhost:7197/Supplier", content);
            return true;
        }

        // Filter Catalogs
        public async Task<IEnumerable<SupplierViewModel>> FilterSupplierAsync(ConditionViewModel content)
        {
            var result = await restProvider.PostAsync("https://localhost:7197/Supplier/filter", JsonProvider.Serialize(content));
            var suppliers = await JsonProvider.DeserializeAsync<IEnumerable<SupplierViewModel>>(result);
            return suppliers;
        }
    }
}
