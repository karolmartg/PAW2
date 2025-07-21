using Microsoft.EntityFrameworkCore;
using PAW2.Models;
using PAW2.Models.ViewModels;
using System.Linq.Expressions;

namespace PAW2.Repositories
{
    public interface IRepositorySupplier
    {
        Task<bool> UpsertAsync(Supplier entity, bool isUpdating);
        Task<bool> CreateAsync(Supplier entity);
        Task<bool> DeleteAsync(Supplier entity);
        Task<IEnumerable<Supplier>> ReadAsync();
        Task<Supplier> FindAsync(int id);
        Task<bool> UpdateAsync(Supplier entity);
        Task<bool> UpdateManyAsync(IEnumerable<Supplier> entities);
        Task<bool> ExistsAsync(Supplier entity);
        Task<bool> CheckBeforeSavingAsync(Supplier entity);
        Task<IEnumerable<SupplierViewModel>> FilterAsync(Expression<Func<Supplier, bool>> predicate);
    }

    public class RepositorySupplier : RepositoryBase<Supplier>, IRepositorySupplier
    {
        public async Task<bool> CheckBeforeSavingAsync(Supplier entity)
        {
            var exists = await ExistsAsync(entity);
            if (exists) { };
            
            return await UpsertAsync(entity, exists);
        }
    
        public async Task<IEnumerable<SupplierViewModel>> FilterAsync(Expression<Func<Supplier, bool>> predicate)
        {
            var random = new Random();

            return await DbContext.Suppliers.Where(predicate)
                .Select(x => new SupplierViewModel
                {
                    SupplierId = x.SupplierId,
                    SupplierName = x.SupplierName,
                    ContactName = x.ContactName,
                    ContactTitle = x.ContactTitle,
                    Phone = x.Phone,
                    Address = x.Address,
                    City = x.City,
                    Country = x.Country,
                    LastModified = x.LastModified,
                    ModifiedBy  = x.ModifiedBy,
                    TempID = random.Next(1, 1000) // Simulating a temporary ID for the view model
                }).ToListAsync();
        }
    
        public async new Task<bool> ExistsAsync (Supplier entity)
        {
            return await DbContext.Suppliers.AnyAsync(x => x.SupplierId == entity.SupplierId);
        }
    }
}
