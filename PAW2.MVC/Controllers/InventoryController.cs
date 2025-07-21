using Microsoft.AspNetCore.Mvc;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;
using PAW2.Services;
using System.Text.Json;

namespace PAW2.MVC.Controllers
{
    public class InventoryController(IInventoryService inventoryService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            try
            {
                if (TempData["data"] != null)
                {
                    var jsonData = TempData["data"] as string;
                    var data = JsonSerializer.Deserialize<List<InventoryViewModel>>(jsonData);

                    if (data != null)
                    {
                        return View(data.Select(x => new Inventory()
                        {
                            InventoryId = x.InventoryId,
                            UnitPrice = x.UnitPrice,
                            UnitsInStock = x.UnitsInStock,
                            LastUpdated = x.LastUpdated,
                            DateAdded = x.DateAdded,
                            ModifiedBy = x.ModifiedBy,
                        }));
                    }
                }
                var inventories = await inventoryService.GetInventoriesAsync();
                return View(inventories);
            }
            catch (Exception ex) 
            {
                TempData["ErrorMessage"] = $@"An unexpected error has occured. Double check with your IT admin. Detail: {ex.Message}";
                return View(Enumerable.Empty<Inventory>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Inventory inventory)
        {
            try
            {
                var result = await inventoryService.SaveInventoriesAsync([inventory]);
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
            var found = await inventoryService.GetInventoryAsync((int)id);

            if (found != null) { var result = await inventoryService.DeleteInventoryAsync((int)id); }
            return RedirectToAction(nameof(Index));
        }
   
        [HttpPost]
        public async Task<IActionResult> Search(ConditionViewModel model)
        {
            var filterData = await inventoryService.FilterInventoryAsync(model);
            TempData["data"] = JsonSerializer.Serialize(filterData);
            return RedirectToAction("Index");
        }
    }
}
