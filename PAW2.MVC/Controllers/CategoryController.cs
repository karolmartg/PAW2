using Microsoft.AspNetCore.Mvc;
using PAW2.Data.Models;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;
using PAW2.Services;
using System.Text.Json;

namespace PAW2.MVC.Controllers
{
    public class CategoryController(ICategoryService categoriesService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            try
            {
                if (TempData["data"] != null)
                {
                    var jsonData = TempData["data"] as string;
                    var data = JsonSerializer.Deserialize<List<CategoryViewModel>>(jsonData);

                    if (data != null)
                    {
                        return View(data.Select(x => new Category()
                        {
                            CategoryId = x.CategoryId,
                            CategoryName = x.CategoryName,
                            Description = x.Description,
                        }));
                    }
                }
                var categories = await categoriesService.GetCategoriesAsync();
                return View(categories);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $@"An unexpected error has occured. Double check with your IT admin. Details: {ex.Message}";
                return View(Enumerable.Empty<Category>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Category category)
        {
            try
            {
                var result = await categoriesService.SaveCategoryAsync([category]);
                if (result)
                {
                    TempData["ErrorMessage"] = $@"Item has been saved successfully";
                    return Json(new { success = true, message = "Catalog saved successfully" });
                }
            }
            catch
            {
                throw;
            }

            return await Index();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var found = await categoriesService.GetCategoryAsync((int)id);

            if (found != null) { var result = await categoriesService.DeleteCategoryAsync((int)id); }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Search(ConditionViewModel model)
        {
            var filterData = await categoriesService.FilterCategoryAsync(model);
            TempData["data"] = JsonSerializer.Serialize(filterData);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> filter (string filter)
        {
            var result = await categoriesService.FilterBusinessAsync(filter);
            return Json(result);
        }
    
    }
}
