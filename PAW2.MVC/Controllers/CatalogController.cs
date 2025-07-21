using Microsoft.AspNetCore.Mvc;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;
using PAW2.Services;
using System.Text.Json;

namespace PAW2.MVC.Controllers
{
    public class CatalogController(ICatalogService catalogService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            try
            {
                if (TempData["data"] != null)
                {
                    var jsonData = TempData["data"] as string;
                    var data = JsonSerializer.Deserialize<List<CatalogViewModel>>(jsonData);

                    if (data != null)
                    {
                        return View(data.Select(x => new Catalog()
                        {
                            Identifier = x.Identifier,
                            Name = x.Name,
                            Description = x.Description,
                            Rating = x.Rating,
                            Sku = x.Sku,
                            CreatedBy = x.CreatedBy,
                            CreatedDate = x.CreatedDate,
                        }));
                    }
                }
                var catalogs = await catalogService.GetCatalogsAsync();
                return View(catalogs);
            }
            catch (Exception ex) 
            {
                TempData["ErrorMessage"] = $@"An unexpected error has occured. Double check with your IT admin. Detail: {ex.Message}";
                return View(Enumerable.Empty<Catalog>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Catalog catalog)
        {
            try
            {
                var result = await catalogService.SaveCatalogsAsync([catalog]);
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
            var found = await catalogService.GetCatalogAsync((int)id);

            if (found != null) { var result = await catalogService.DeleteCatalogAsync((int)id); }
            return RedirectToAction(nameof(Index));
        }
   
        [HttpPost]
        public async Task<IActionResult> Search(ConditionViewModel model)
        {
            var filterData = await catalogService.FilterCatalogAsync(model);
            TempData["data"] = JsonSerializer.Serialize(filterData);
            return RedirectToAction("Index");
        }
    }
}
