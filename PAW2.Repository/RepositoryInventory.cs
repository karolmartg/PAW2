using Microsoft.EntityFrameworkCore;
using PAW2.Models;
using PAW2.Models.ViewModels;
using System.Linq.Expressions;

namespace PAW2.Repositories
{
    public interface IRepositoryInventory
    {
        Task<bool> UpsertAsync(Inventory entity, bool isUpdating);
        Task<bool> CreateAsync(Inventory entity);
        Task<bool> DeleteAsync(Inventory entity);
        Task<IEnumerable<Inventory>> ReadAsync();
        Task<Inventory> FindAsync(int id);
        Task<bool> UpdateAsync(Inventory entity);
        Task<bool> UpdateManyAsync(IEnumerable<Inventory> entities);
        Task<bool> ExistsAsync(Inventory entity);
        Task<bool> CheckBeforeSavingAsync(Inventory entity);
        Task<IEnumerable<InventoryViewModel>> FilterAsync(Expression<Func<Inventory, bool>> predicate);
    }

    public class RepositoryInventory : RepositoryBase<Inventory>, IRepositoryInventory
    {
        public async Task<bool> CheckBeforeSavingAsync(Inventory entity)
        {
            var exists = await ExistsAsync(entity);
            if (exists) { };
            
            return await UpsertAsync(entity, exists);
        }
    
        public async Task<IEnumerable<InventoryViewModel>> FilterAsync(Expression<Func<Inventory, bool>> predicate)
        {
            var random = new Random();

            return await DbContext.Inventories.Where(predicate)
                .Select(x => new InventoryViewModel
                {
                    InventoryId = x.InventoryId,
                    UnitPrice = x.UnitPrice,
                    UnitsInStock = x.UnitsInStock,
                    LastUpdated = x.LastUpdated,
                    DateAdded = x.DateAdded,
                    ModifiedBy = x.ModifiedBy,
                    TempID = random.Next(1, 1000) // Simulating a temporary ID for the view model
                }).ToListAsync();
        }
    
        public async new Task<bool> ExistsAsync (Inventory entity)
        {
            return await DbContext.Inventories.AnyAsync(x => x.InventoryId == entity.InventoryId);
        }
    }
}
