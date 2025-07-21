using PAW2.Data.Models;
using PAW2.Repositories;
using PAW2.Core;
using PAW2.Models;
using PAW2.Core.Extensions;
using PAW2.Models.ViewModels;
using System.Linq.Expressions;

namespace PAW2.Business;
public interface IBusinessInventory
{
    Task<IEnumerable<Inventory>> GetAllInventoriesAsync();
    Task<bool> SaveInventoryAsync(Inventory inventory);
    Task<bool> DeleteInventoryAsync(Inventory inventory);
    Task<Inventory> GetInventoryAsync(int id);
    Task<IEnumerable<InventoryViewModel>> Filter(Expression<Func<Inventory, bool>> predicate);
}

public class BusinessInventory(IRepositoryInventory repositoryInventory) : IBusinessInventory
{
    public async Task<IEnumerable<Inventory>> GetAllInventoriesAsync()
    {
        return await repositoryInventory.ReadAsync();
    }

    public async Task<bool> SaveInventoryAsync(Inventory inventory)
    { 
       var user = "";
       inventory.AddAudit(user);
       inventory.AddLogging(inventory.InventoryId <= 0 ? Models.Enums.LoggingType.Create : Models.Enums.LoggingType.Update);

        return await repositoryInventory.CheckBeforeSavingAsync(inventory);
    }

    public async Task<bool> DeleteInventoryAsync (Inventory inventory)
    {
        return await repositoryInventory.DeleteAsync(inventory);
    }

    public async Task<Inventory> GetInventoryAsync(int id)
    {
        return await repositoryInventory.FindAsync(id);
    }

    public async Task<IEnumerable<InventoryViewModel>> Filter(Expression<Func<Inventory, bool>> predicate)
    {
        return await repositoryInventory.FilterAsync(predicate);
    }
}