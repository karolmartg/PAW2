using Microsoft.EntityFrameworkCore;
using PAW2.Models;
using PAW2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PAW2.Repositories
{
    public interface IRepositoryProduct
    {
        Task<bool> UpsertAsync(Product entity, bool isUpdating);
        Task<bool> CreateAsync(Product entity);
        Task<bool> DeleteAsync(Product entity);
        Task<IEnumerable<Product>> ReadAsync();
        Task<Product> FindAsync(int id);
        Task<bool> UpdateAsync(Product entity);
        Task<bool> UpdateManyAsync(IEnumerable<Product> entities);
        Task<bool> ExistsAsync(Product entity);
        Task<bool> CheckBeforeSavingAsync(Product entity);
        Task<IEnumerable<ProductViewModel>> FilterAsync(Expression<Func<Product, bool>> predicate);
    }

    public class RepositoryProduct : RepositoryBase<Product>, IRepositoryProduct
    {
        public async Task<bool> CheckBeforeSavingAsync(Product entity)
        {
            var exists = await ExistsAsync(entity);
            if (exists) { };

            return await UpsertAsync(entity, exists);
        }

        public async Task<IEnumerable<ProductViewModel>> FilterAsync(Expression<Func<Product, bool>> predicate)
        {
            var random = new Random();

            return await DbContext.Products.Where(predicate)
                .Select(x => new ProductViewModel
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    InventoryId = x.InventoryId,
                    SupplierId = x.SupplierId,
                    Description = x.Description,
                    Rating = x.Rating,
                    CategoryId = x.CategoryId,
                    LastModified = x.LastModified,
                    ModifiedBy = x.ModifiedBy,
                    Category = x.Category,
                    TempID = random.Next(1, 1000),
                }).ToListAsync();
        }
   
        public async new Task<bool> ExistsAsync (Product entity)
        {
            return await DbContext.Products.AnyAsync(x => x.ProductId == entity.ProductId);
        }
    }
}
