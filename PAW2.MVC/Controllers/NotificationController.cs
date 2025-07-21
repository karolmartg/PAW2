using Microsoft.AspNetCore.Mvc;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;
using PAW2.Services;
using System.Text.Json;

namespace PAW2.MVC.Controllers
{
    public class NotificationController(INotificationService notificationService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            try
            {
                if (TempData["data"] != null)
                {
                    var jsonData = TempData["data"] as string;
                    var data = JsonSerializer.Deserialize<List<NotificationViewModel>>(jsonData);

                    if (data != null)
                    {
                        return View(data.Select(x => new Notification()
                        {
                            Id = x.Id,
                            UserId = x.UserId,
                            Message = x.Message,
                            IsRead = x.IsRead,
                            CreatedAt = x.CreatedAt
                        }));
                    }
                }
                var notifications = await notificationService.GetNotificationsAsync();
                return View(notifications);
            }
            catch (Exception ex) 
            {
                TempData["ErrorMessage"] = $@"An unexpected error has occured. Double check with your IT admin. Detail: {ex.Message}";
                return View(Enumerable.Empty<Catalog>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Notification notification)
        {
            try
            {
                var result = await notificationService.SaveNotificationsAsync([notification]);
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
            var found = await notificationService.GetNotificationAsync((int)id);

            if (found != null) { var result = await notificationService.DeleteNotificationAsync((int)id); }
            return RedirectToAction(nameof(Index));
        }
   
        [HttpPost]
        public async Task<IActionResult> Search(ConditionViewModel model)
        {
            var filterData = await notificationService.FilterNotificationAsync(model);
            TempData["data"] = JsonSerializer.Serialize(filterData);
            return RedirectToAction("Index");
        }
    }
}
