using APW.Architecture;
using PAW.Architecture.Providers;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAW2.Services
{
    public interface IProductService
    {
        Task<Product> GetProductAsync(int id);
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<bool> DeleteProductAsync(int id);
        Task<bool> SaveProducts(IEnumerable<Product> products);
        Task<IEnumerable<ProductViewModel>> FilterProductAsync(ConditionViewModel content);
        Task<IEnumerable<ProductViewModel>> FilterBusinessAsync(string filter);
    }

    public class ProductService(IRestProvider restProvider) : IProductService
    {
        // Get Product By ID
        public async Task<Product> GetProductAsync(int id)
        {
            var result = await restProvider.GetAsync("https://localhost:7197/Product/", "1");
            var product = await JsonProvider.DeserializeAsync<Product>(result);
            return product;
        }
    
        // Get All Products
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var results = await restProvider.GetAsync("https://localhost:7197/Product/", null);
            var products = await JsonProvider.DeserializeAsync<IEnumerable<Product>>(results);
            return products;
        }
    
        // Delete Product By ID
        public async Task<bool> DeleteProductAsync(int id)
        {
            var result = await restProvider.DeleteAsync("https://localhost:7197/Product/", $"{id}");
            return true;
        }
        
        // Save Products
        public async Task<bool> SaveProducts(IEnumerable<Product> products)
        {
            var content = JsonProvider.Serialize(products);
            var result = await restProvider.PostAsync("https://localhost:7197/Product/", content);
            return true;
        }
    
        // Filter Products
        public async Task<IEnumerable<ProductViewModel>> FilterProductAsync(ConditionViewModel content)
        {
            var result = await restProvider.PostAsync("https://localhost:7197/Product/filter", JsonProvider.Serialize(content));
            var prodcuts = await JsonProvider.DeserializeAsync<IEnumerable<ProductViewModel>> (result);
            return prodcuts;
        }

        public async Task<IEnumerable<ProductViewModel>> FilterBusinessAsync(string filter)
        {
            var result = await restProvider.GetAsync($"https://localhost:7197/Product/filter?filter={filter}", null);
            var prodcuts = await JsonProvider.DeserializeAsync<IEnumerable<ProductViewModel>>(result);
            return prodcuts;
        }
    }
}
