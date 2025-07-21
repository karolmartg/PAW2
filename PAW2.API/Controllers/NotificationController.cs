using Microsoft.AspNetCore.Mvc;
using PAW.Models;
using PAW2.Business;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;

namespace PAW2.API.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationController(IBusinessNotification businessNotification) : Controller
{
    [HttpGet(Name = "GetNotifications")]
    public async Task<IEnumerable<Notification>> GetAll()
    {
        return await businessNotification.GetAllNotificationsAsync();
    }

    [HttpGet("{id:int}", Name = "GetNotificationById")]
    public async Task<ActionResult<Notification>> GetById(int id)
    {
        var notification = await businessNotification.GetNotificationAsync(id);
        return notification;
    }
   
    [HttpPost("filter", Name = "FilterNotifications")]    
    public async Task<IEnumerable<NotificationViewModel>> Filter(ConditionViewModel condition)
    {
        var predicte = ConditionResolver.ResolveCondition<Notification>(condition.Criteria, condition.Property, 
                                                          condition.Value, condition.Start, condition.End);
        var results = await businessNotification.Filter(predicte);
        return results;
    }

    [HttpPost]
    public async Task<bool> Save([FromBody] IEnumerable<Notification> notifications)
    {
        foreach (var item in notifications)
        {
            await businessNotification.SaveNotificationAsync(item);
        }

        return true;
    }

    [HttpDelete]
    public async Task<bool> Delete(Notification notification)
    {
        return await businessNotification.DeleteNotificationAsync(notification);
    }
}
