using Microsoft.AspNetCore.Mvc;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;
using PAW2.Services;
using System.Text.Json;

namespace PAW2.MVC.Controllers
{
    public class ProductController(IProductService productService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            try
            {
                if (TempData["data"] != null)
                {
                    var jsonData = TempData["data"] as string;
                    var data = JsonSerializer.Deserialize<List<ProductViewModel>>(jsonData);

                    if (data != null)
                    {
                        return View(data.Select(x => new Product
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
                        }));

                    }
                }
                var products = await productService.GetProductsAsync();
                return View(products);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $@"An unexpected error has occured. Double check with your IT admin. Details: {ex.Message}";
                return View(Enumerable.Empty<Product>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Product product)
        {
            try
            {
                var result = await productService.SaveProducts([product]);
                if (result)
                {
                    TempData["ErrorMessage"] = $@"Item has been saved successfully";
                    return Json(new { success = true, message = "Catalog saved successfully" });
                }
            }
            catch { throw; }

            return await Index();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var found = await productService.GetProductAsync((int)id);

            if (found != null) 
            {
                var result = await productService.DeleteProductAsync((int)id);
            }

            return RedirectToAction(nameof(Index));
        }
    
        public async Task<IActionResult> Search(ConditionViewModel model)
        {
            var filterData = await productService.FilterProductAsync(model);
            TempData["data"] = JsonSerializer.Serialize(filterData);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> filter(string filter)
        {
            var result = await productService.FilterBusinessAsync(filter);
            return Json(result);
        }
    }
}
