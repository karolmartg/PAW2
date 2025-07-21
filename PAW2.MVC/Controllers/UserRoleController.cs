using Microsoft.AspNetCore.Mvc;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;
using PAW2.Services;
using System.Text.Json;

namespace PAW2.MVC.Controllers
{
    public class UserRoleController(IUserRoleService userRoleService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            try
            {
                if (TempData["data"] != null)
                {
                    var jsonData = TempData["data"] as string;
                    var data = JsonSerializer.Deserialize<List<UserRoleViewModel>>(jsonData);

                    if (data != null)
                    {
                        return View(data.Select(x => new UserRole()
                        {
                            Id = x.Id,
                            RoldId = x.RoldId,
                            UserId = x.UserId,
                        }));
                    }
                }
                var userRoles = await userRoleService.GetUserRolesAsync();
                return View(userRoles);
            }
            catch (Exception ex) 
            {
                TempData["ErrorMessage"] = $@"An unexpected error has occured. Double check with your IT admin. Detail: {ex.Message}";
                return View(Enumerable.Empty<UserRole>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] UserRole userRole)
        {
            try
            {
                var result = await userRoleService.SaveUserRolesAsync([userRole]);
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
            var found = await userRoleService.GetUserRoleAsync((int)id);

            if (found != null) { var result = await userRoleService.DeleteUserRoleAsync((int)id); }
            return RedirectToAction(nameof(Index));
        }
   
        [HttpPost]
        public async Task<IActionResult> Search(ConditionViewModel model)
        {
            var filterData = await userRoleService.FilterUserRoleAsync(model);
            TempData["data"] = JsonSerializer.Serialize(filterData);
            return RedirectToAction("Index");
        }
    }
}
