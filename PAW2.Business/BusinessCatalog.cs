using PAW2.Data.Models;
using PAW2.Repositories;
using PAW2.Core;
using PAW2.Models;
using PAW2.Core.Extensions;
using PAW2.Models.ViewModels;
using System.Linq.Expressions;

namespace PAW2.Business;
public interface IBusinessCatalog
{
    Task<IEnumerable<Catalog>> GetAllCatalogsAsync();
    Task<bool> SaveCatalogAsync(Catalog catalog);
    Task<bool> DeleteCatalogAsync(Catalog catalog);
    Task<Catalog> GetCatalogAsync(int id);
    Task<IEnumerable<CatalogViewModel>> Filter(Expression<Func<Catalog, bool>> predicate);
}

public class BusinessCatalog(IRepositoryCatalog repositoryCatalog) : IBusinessCatalog
{
    public async Task<IEnumerable<Catalog>> GetAllCatalogsAsync()
    {
        return await repositoryCatalog.ReadAsync();
    }

    public async Task<bool> SaveCatalogAsync(Catalog catalog)
    { 
       var user = "";
       catalog.AddAudit(user);
       catalog.AddLogging(catalog.Identifier <= 0 ? Models.Enums.LoggingType.Create : Models.Enums.LoggingType.Update);

        return await repositoryCatalog.CheckBeforeSavingAsync(catalog);
    }

    public async Task<bool> DeleteCatalogAsync (Catalog catalog)
    {
        return await repositoryCatalog.DeleteAsync(catalog);
    }

    public async Task<Catalog> GetCatalogAsync(int id)
    {
        return await repositoryCatalog.FindAsync(id);
    }

    public async Task<IEnumerable<CatalogViewModel>> Filter(Expression<Func<Catalog, bool>> predicate)
    {
        return await repositoryCatalog.FilterAsync(predicate);
    }
}