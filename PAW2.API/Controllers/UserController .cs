using Microsoft.AspNetCore.Mvc;
using PAW.Models;
using PAW2.Business;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;

namespace PAW2.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController (IBusinessUser businessUser) : Controller
{
    [HttpGet(Name = "GetUsers")]
    public async Task<IEnumerable<User>> GetAll()
    {
        return await businessUser.GetAllUsersAsync();
    }

    [HttpGet("{id:int}", Name = "GetUserById")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        var user = await businessUser.GetUserAsync(id);
        return user;
    }
   
    [HttpPost("filter", Name = "FilterUsers")]    
    public async Task<IEnumerable<UserViewModel>> Filter(ConditionViewModel condition)
    {
        var predicte = ConditionResolver.ResolveCondition<User>(condition.Criteria, condition.Property, 
                                                          condition.Value, condition.Start, condition.End);
        var results = await businessUser.Filter(predicte);
        return results;
    }

    [HttpPost]
    public async Task<bool> Save([FromBody] IEnumerable<User> users)
    {
        foreach (var item in users)
        {
            await businessUser.SaveUserAsync(item);
        }

        return true;
    }

    [HttpDelete]
    public async Task<bool> Delete(User user)
    {
        return await businessUser.DeleteUserAsync(user);
    }
}
