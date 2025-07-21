using Microsoft.AspNetCore.Mvc;
using PAW.Models;
using PAW2.Business;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;

namespace PAW2.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserRoleController (IBusinessUserRole businessUserRole) : Controller
{
    [HttpGet(Name = "GetUserRoles")]
    public async Task<IEnumerable<UserRole>> GetAll()
    {
        return await businessUserRole.GetAllUserRolesAsync();
    }

    [HttpGet("{id:int}", Name = "GetUserRoleById")]
    public async Task<ActionResult<UserRole>> GetById(int id)
    {
        var catalog = await businessUserRole.GetUserRoleAsync(id);
        return catalog;
    }
   
    [HttpPost("filter", Name = "FilterUserRoles")]    
    public async Task<IEnumerable<UserRoleViewModel>> Filter(ConditionViewModel condition)
    {
        var predicte = ConditionResolver.ResolveCondition<UserRole>(condition.Criteria, condition.Property, 
                                                          condition.Value, condition.Start, condition.End);
        var results = await businessUserRole.Filter(predicte);
        return results;
    }

    [HttpPost]
    public async Task<bool> Save([FromBody] IEnumerable<UserRole> userRoles)
    {
        foreach (var item in userRoles)
        {
            await businessUserRole.SaveUserRoleAsync(item);
        }

        return true;
    }

    [HttpDelete]
    public async Task<bool> Delete(UserRole userRole)
    {
        return await businessUserRole.DeleteUserRoleAsync(userRole);
    }
}
