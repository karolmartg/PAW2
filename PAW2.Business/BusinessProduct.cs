using PAW2.Core.Extensions;
using PAW2.Models;
using PAW2.Models.ViewModels;
using PAW2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PAW2.Business;

public interface IBusinessProduct
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<bool> SaveProductsAsync(Product product);
    Task<bool> DeleteProductAsync(Product product);
    Task<Product> GetProductAsync(int id);
    Task<IEnumerable<ProductViewModel>> Filter(Expression<Func<Product, bool>> predicate);
    Task<IEnumerable<ProductViewModel>> FilterBusinessAsync(string filter);
}

public class BusinessProduct(IRepositoryProduct repositoryProduct) : IBusinessProduct
{
    public async Task<IEnumerable<Product>> GetAllProductsAsync() { return await repositoryProduct.ReadAsync(); }

    public async Task<bool> SaveProductsAsync(Product product)
    {
        var user = "";
        product.AddAudit(user);
        product.AddLogging(product.ProductId <= 0 ? Models.Enums.LoggingType.Create : Models.Enums.LoggingType.Update);

        return await repositoryProduct.CheckBeforeSavingAsync(product);
    }

    public async Task<bool> DeleteProductAsync(Product product) { return await repositoryProduct.DeleteAsync(product); }

    public async Task<Product> GetProductAsync(int id)
    {
        return await repositoryProduct.FindAsync(id);
    }


    public async Task<IEnumerable<ProductViewModel>> Filter(Expression<Func<Product, bool>> predicate)
    {
        return await repositoryProduct.FilterAsync(predicate);
    }

    // Get Products filter by Supplie ID, InventoryId, Highest Rating, Lowest Rating, Most common Rating
    public async Task<IEnumerable<ProductViewModel>> FilterBusinessAsync(string filter)
    {
        var all = await repositoryProduct.ReadAsync();

        if (!all.Any()) return Enumerable.Empty<ProductViewModel>();

        switch (filter)
        {
            case "no-supplierid":
                return await repositoryProduct.FilterAsync(c => c.SupplierId == null);
            case "no-inventoryid":
                return await repositoryProduct.FilterAsync(c => c.InventoryId == null);
            case "highest-rating":
                var max = all.Where(p => p.Rating != null).Max(p => p.Rating);
                return await repositoryProduct.FilterAsync(c => c.Rating == max);
            case "lowest-rating":
                var min = all.Where(p => p.Rating != null).Min(p => p.Rating);
                return await repositoryProduct.FilterAsync(c => c.Rating == min);
            case "most-common-rating":
                var mostCommon = all
                .Where(p => p.Rating != null)
                .GroupBy(p => p.Rating)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();
                return await repositoryProduct.FilterAsync(c => c.Rating == mostCommon);
            default:
                return Enumerable.Empty<ProductViewModel>();
        }

    }
}
