using APW.Architecture;
using PAW.Architecture.Providers;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;

namespace PAW2.Services
{
    public interface IInventoryService
    {
        Task<Inventory> GetInventoryAsync(int id);
        Task<IEnumerable<Inventory>> GetInventoriesAsync();
        Task<bool> DeleteInventoryAsync(int id);
        Task<bool> SaveInventoriesAsync(IEnumerable<Inventory> inventories);
        Task<IEnumerable<InventoryViewModel>> FilterInventoryAsync(ConditionViewModel content);
    }

    public class InventoryService(IRestProvider restProvider) : IInventoryService
    {
        public async Task<Inventory> GetInventoryAsync(int id)
        {
            var result = await restProvider.GetAsync("https://localhost:7197/Inventory/", "1");
            var inventory = await JsonProvider.DeserializeAsync<Inventory>(result);
            return inventory;
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesAsync()
        {
            var results = await restProvider.GetAsync("https://localhost:7197/Inventory/", null);
            var inventories = await JsonProvider.DeserializeAsync<IEnumerable<Inventory>>(results);
            return inventories;
        }

        public async Task<bool> DeleteInventoryAsync(int id)
        {
            var result = await restProvider.DeleteAsync("https://localhost:7197/Inventory/", $"{id}");
            return true;
        }

        public async Task<bool> SaveInventoriesAsync(IEnumerable<Inventory> inventories)
        {
            var content = JsonProvider.Serialize(inventories);
            var result = await restProvider.PostAsync("https://localhost:7197/Inventory", content);
            return true;
        }

        public async Task<IEnumerable<InventoryViewModel>> FilterInventoryAsync(ConditionViewModel content)
        {
            var result = await restProvider.PostAsync("https://localhost:7197/Inventory/filter", JsonProvider.Serialize(content));
            var inventories = await JsonProvider.DeserializeAsync<IEnumerable<InventoryViewModel>>(result);
            return inventories;
        }
    }
}
