using PAW2.Data.Models;
using PAW2.Repositories;
using PAW2.Core;
using PAW2.Models;
using PAW2.Core.Extensions;
using PAW2.Models.ViewModels;
using System.Linq.Expressions;

namespace PAW2.Business;
public interface IBusinessSupplier
{
    Task<IEnumerable<Supplier>> GetAllSuppliersAsync();
    Task<bool> SaveSupplierAsync(Supplier supplier);
    Task<bool> DeleteSupplierAsync(Supplier supplier);
    Task<Supplier> GetSupplierAsync(int id);
    Task<IEnumerable<SupplierViewModel>> Filter(Expression<Func<Supplier, bool>> predicate);
}

public class BusinessSupplier(IRepositorySupplier repositorySupplier) : IBusinessSupplier
{
    public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
    {
        return await repositorySupplier.ReadAsync();
    }

    public async Task<bool> SaveSupplierAsync(Supplier supplier)
    { 
       var user = "";
       supplier.AddAudit(user);
       supplier.AddLogging(supplier.SupplierId <= 0 ? Models.Enums.LoggingType.Create : Models.Enums.LoggingType.Update);

        return await repositorySupplier.CheckBeforeSavingAsync(supplier);
    }

    public async Task<bool> DeleteSupplierAsync(Supplier supplier)
    {
        return await repositorySupplier.DeleteAsync(supplier);
    }

    public async Task<Supplier> GetSupplierAsync(int id)
    {
        return await repositorySupplier.FindAsync(id);
    }

    public async Task<IEnumerable<SupplierViewModel>> Filter(Expression<Func<Supplier, bool>> predicate)
    {
        return await repositorySupplier.FilterAsync(predicate);
    }
}