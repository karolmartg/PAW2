using Microsoft.VisualStudio.TestTools.UnitTesting;
using PAW2.Business;
using PAW2.Repositories;
using PAW2.Models;
using PAW2.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAW2.BusinessTests;

[TestClass]
public sealed class ProductTests
{
    [TestMethod]
    public async Task GetAllProducts_ShouldNotThrowException()
    {
        try
        {
            var business = new BusinessProduct(new RepositoryProduct());
            var result = await business.GetAllProductsAsync();

            Assert.IsNotNull(result, "Result should not be null");
        }
        catch
        {
            Assert.Fail("Exception should not have been thrown.");
        }
    }

    [TestMethod]
    public async Task SaveProducts_ShouldNotThrowException()
    {
        try
        {
            var business = new BusinessProduct(new RepositoryProduct());

            var product = new Product
            {
                ProductId = 0,
                ProductName = "Test",
                Description = "Some Desc",
                Rating = 4.5m,
                SupplierId = 1,
                InventoryId = 1,
                CategoryId = 1
            };

            var result = await business.SaveProductsAsync(product);

            Assert.IsTrue(result, "Saving the product should return true.");
        }
        catch
        {
            Assert.Fail("Exception should not have been thrown during SaveProductsAsync.");
        }
    }

    [TestMethod]
    public async Task DeleteProduct_ShouldNotThrowException()
    {
        try
        {
            var business = new BusinessProduct(new RepositoryProduct());
            var product = new Product { ProductId = 1 };

            var result = await business.DeleteProductAsync(product);

            Assert.IsTrue(result, "Deleting the product should return true.");
        }
        catch
        {
            Assert.Fail("Exception should not have been thrown during DeleteProductAsync.");
        }
    }

    [TestMethod]
    public async Task GetProduct_ShouldReturnProduct()
    {
        try
        {
            var business = new BusinessProduct(new RepositoryProduct());
            var result = await business.GetProductAsync(1);

            Assert.IsNotNull(result, "Should return a product object.");
        }
        catch
        {
            Assert.Fail("Exception should not have been thrown during GetProductAsync.");
        }
    }

    [TestMethod]
    public async Task FilterBusiness_ShouldNotThrowException()
    {
        try
        {
            var business = new BusinessProduct(new RepositoryProduct());
            var result = await business.FilterBusinessAsync("highest-rating");

            Assert.IsNotNull(result, "Should return filtered results.");
        }
        catch
        {
            Assert.Fail("Exception should not have been thrown during FilterBusinessAsync.");
        }
    }
}
