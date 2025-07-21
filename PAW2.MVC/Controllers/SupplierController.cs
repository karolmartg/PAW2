using Microsoft.AspNetCore.Mvc;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;
using PAW2.Services;
using System.Text.Json;

namespace PAW2.MVC.Controllers
{
    public class SupplierController(ISupplierService supplierService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            try
            {
                if (TempData["data"] != null)
                {
                    var jsonData = TempData["data"] as string;
                    var data = JsonSerializer.Deserialize<List<SupplierViewModel>>(jsonData);

                    if (data != null)
                    {
                        return View(data.Select(x => new Supplier()
                        {
                            SupplierId = x.SupplierId,
                            SupplierName = x.SupplierName,
                            ContactName = x.ContactName,
                            ContactTitle = x.ContactTitle,
                            Phone = x.Phone,
                            Address = x.Address,
                            City = x.City,
                            Country = x.Country,
                            LastModified = x.LastModified,
                            ModifiedBy = x.ModifiedBy,
                        }));
                    }
                }
                var suppliers = await supplierService.GetSuppliersAsync();
                return View(suppliers);
            }
            catch (Exception ex) 
            {
                TempData["ErrorMessage"] = $@"An unexpected error has occured. Double check with your IT admin. Detail: {ex.Message}";
                return View(Enumerable.Empty<Supplier>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Supplier supplier)
        {
            try
            {
                var result = await supplierService.SaveSuppliersAsync([supplier]);
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
            var found = await supplierService.GetSupplierAsync((int)id);

            if (found != null) { var result = await supplierService.DeleteSupplierAsync((int)id); }
            return RedirectToAction(nameof(Index));
        }
   
        [HttpPost]
        public async Task<IActionResult> Search(ConditionViewModel model)
        {
            var filterData = await supplierService.FilterSupplierAsync(model);
            TempData["data"] = JsonSerializer.Serialize(filterData);
            return RedirectToAction("Index");
        }
    }
}
