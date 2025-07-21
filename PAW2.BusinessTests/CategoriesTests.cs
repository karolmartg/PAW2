using Microsoft.VisualStudio.TestTools.UnitTesting;
using PAW2.Business;
using PAW2.Repositories;
using PAW2.Models;
using PAW2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAW2.BusinessTests;

[TestClass]
public sealed class CategoriesTest
{
    [TestMethod]
    public async Task GetAllCategories_ShouldNotThrowException()
    {
        try
        {
            var business = new BusinessCategory(new RepositoryCategory());
            var result = await business.GetAllCategoriesAsync();

            Assert.IsNotNull(result, "Result should not be null.");
        }
        catch
        {
            Assert.Fail("Exception thrown during GetAllCategoriesAsync.");
        }
    }

    [TestMethod]
    public async Task SaveCategory_ShouldNotThrowException()
    {
        try
        {
            var business = new BusinessCategory(new RepositoryCategory());

            var category = new Category
            {
                CategoryId = 0,
                CategoryName = "Test Category",
                Description = "Description",
                LastModified = DateTime.UtcNow,
                ModifiedBy = "Admin"
            };

            var result = await business.SaveCategoryAsync(category);

            Assert.IsTrue(result, "Saving the category should return true.");
        }
        catch
        {
            Assert.Fail("Exception thrown during SaveCategoryAsync.");
        }
    }

    [TestMethod]
    public async Task DeleteCategory_ShouldNotThrowException()
    {
        try
        {
            var business = new BusinessCategory(new RepositoryCategory());
            var category = new Category { CategoryId = 1 };

            var result = await business.DeleteCategoryAsync(category);

            Assert.IsTrue(result, "Deleting the category should return true.");
        }
        catch
        {
            Assert.Fail("Exception thrown during DeleteCategoryAsync.");
        }
    }

    [TestMethod]
    public async Task GetCategory_ShouldReturnCategory()
    {
        try
        {
            var business = new BusinessCategory(new RepositoryCategory());
            var result = await business.GetCategoryAsync(1);

            Assert.IsNotNull(result, "Should return a category object.");
        }
        catch
        {
            Assert.Fail("Exception thrown during GetCategoryAsync.");
        }
    }

    [TestMethod]
    public async Task FilterBusiness_ShouldNotThrowException()
    {
        try
        {
            var business = new BusinessCategory(new RepositoryCategory());
            var result = await business.FilterBusinessAsync("not-system");

            Assert.IsNotNull(result, "Should return filtered results.");
        }
        catch
        {
            Assert.Fail("Exception thrown during FilterBusinessAsync.");
        }
    }
}
