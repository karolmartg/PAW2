using Microsoft.EntityFrameworkCore;
using PAW2.Models;
using PAW2.Models.ViewModels;
using System.Linq.Expressions;

namespace PAW2.Repositories
{
    public interface IRepositoryCatalog
    {
        Task<bool> UpsertAsync(Catalog entity, bool isUpdating);
        Task<bool> CreateAsync(Catalog entity);
        Task<bool> DeleteAsync(Catalog entity);
        Task<IEnumerable<Catalog>> ReadAsync();
        Task<Catalog> FindAsync(int id);
        Task<bool> UpdateAsync(Catalog entity);
        Task<bool> UpdateManyAsync(IEnumerable<Catalog> entities);
        Task<bool> ExistsAsync(Catalog entity);
        Task<bool> CheckBeforeSavingAsync(Catalog entity);
        Task<IEnumerable<CatalogViewModel>> FilterAsync(Expression<Func<Catalog, bool>> predicate);
    }

    public class RepositoryCatalog : RepositoryBase<Catalog>, IRepositoryCatalog
    {
        public async Task<bool> CheckBeforeSavingAsync(Catalog entity)
        {
            var exists = await ExistsAsync(entity);
            if (exists) { };
            
            return await UpsertAsync(entity, exists);
        }
    
        public async Task<IEnumerable<CatalogViewModel>> FilterAsync(Expression<Func<Catalog, bool>> predicate)
        {
            var random = new Random();

            return await DbContext.Catalogs.Where(predicate)
                .Select(x => new CatalogViewModel
                {
                    Identifier = x.Identifier,
                    Name = x.Name,
                    Description = x.Description,
                    Rating = x.Rating,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    TempID = random.Next(1, 1000) // Simulating a temporary ID for the view model
                }).ToListAsync();
        }
    
        public async new Task<bool> ExistsAsync (Catalog entity)
        {
            return await DbContext.Catalogs.AnyAsync(x => x.Identifier == entity.Identifier);
        }
    }
}
