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

public interface IBusinessCategory
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<bool> SaveCategoryAsync(Category category);
    Task<bool> DeleteCategoryAsync(Category category);
    Task<Category> GetCategoryAsync(int id);
    Task<IEnumerable<CategoryViewModel>> Filter(Expression<Func<Category, bool>> predicate);
    Task<IEnumerable<CategoryViewModel>> FilterBusinessAsync(string filter);
}

public class BusinessCategory(IRespositoryCategory respositoryCategory) : IBusinessCategory
{
    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await respositoryCategory.ReadAsync();
    }

    public async Task<bool> SaveCategoryAsync(Category category)
    {
        var user = "";
        category.AddLogging(category.CategoryId <= 0 ? Models.Enums.LoggingType.Create : Models.Enums.LoggingType.Update);
        return await respositoryCategory.CheckBeforeSavingAsync(category);
    }

    public async Task<bool> DeleteCategoryAsync(Category category)
    {
        return await respositoryCategory.DeleteAsync(category);
    }

    public async Task<Category> GetCategoryAsync(int id)
    {
        return await respositoryCategory.FindAsync(id);
    }

    public async Task<IEnumerable<CategoryViewModel>> Filter(Expression<Func<Category, bool>> predicate)
    {
        return await respositoryCategory.FilterAsync(predicate);
    }

   
    // Get Categories filter by modified or lastmodified field
    public async Task<IEnumerable<CategoryViewModel>> FilterBusinessAsync(string filter)
    {
        return filter switch
        {
            "not-system" => await respositoryCategory.FilterAsync(c => c.ModifiedBy != "System"),
            "admin" => await respositoryCategory.FilterAsync(c => c.ModifiedBy == "Admin"),
            "null-lastmodified-not-admin" => await respositoryCategory.FilterAsync(c => c.LastModified == null && c.ModifiedBy != "Admin"),
            _ => Enumerable.Empty<CategoryViewModel>()
        };
    }

}
