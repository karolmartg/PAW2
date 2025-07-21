using Microsoft.AspNetCore.Mvc;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;
using PAW2.Services;
using System.Text.Json;

namespace PAW2.MVC.Controllers
{
    public class UserController(IUserService userService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            try
            {
                if (TempData["data"] != null)
                {
                    var jsonData = TempData["data"] as string;
                    var data = JsonSerializer.Deserialize<List<UserViewModel>>(jsonData);

                    if (data != null)
                    {
                        return View(data.Select(x => new User()
                        {
                            UserId = x.UserId,
                            Username = x.Username,
                            Email = x.Email,
                            PasswordHash = x.PasswordHash,
                            CreatedAt = x.CreatedAt,
                            IsActive = x.IsActive,
                            LastModified = x.LastModified,
                            ModifiedBy = x.ModifiedBy,
                        }));
                    }
                }
                var users = await userService.GetUsersAsync();
                return View(users);
            }
            catch (Exception ex) 
            {
                TempData["ErrorMessage"] = $@"An unexpected error has occured. Double check with your IT admin. Detail: {ex.Message}";
                return View(Enumerable.Empty<Catalog>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] User user)
        {
            try
            {
                var result = await userService.SaveUsersAsync([user]);
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
            var found = await userService.GetUserAsync((int)id);

            if (found != null) { var result = await userService.DeleteUserAsync((int)id); }
            return RedirectToAction(nameof(Index));
        }
   
        [HttpPost]
        public async Task<IActionResult> Search(ConditionViewModel model)
        {
            var filterData = await userService.FilterUserAsync(model);
            TempData["data"] = JsonSerializer.Serialize(filterData);
            return RedirectToAction("Index");
        }
    }
}
