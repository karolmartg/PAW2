using Microsoft.AspNetCore.Mvc;
using PAW.Models;
using PAW2.Business;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;

namespace PAW2.API.Controllers;

[ApiController]
[Route("[controller]")]
public class InventoryController (IBusinessInventory businessInventory) : Controller
{
    [HttpGet(Name = "GetInventories")]
    public async Task<IEnumerable<Inventory>> GetAll()
    {
        return await businessInventory.GetAllInventoriesAsync();
    }

    [HttpGet("{id:int}", Name = "GetInventoryById")]
    public async Task<ActionResult<Inventory>> GetById(int id)
    {
        var inventory = await businessInventory.GetInventoryAsync(id);
        return inventory;
    }
   
    [HttpPost("filter", Name = "FilterInventories")]    
    public async Task<IEnumerable<InventoryViewModel>> Filter(ConditionViewModel condition)
    {
        var predicte = ConditionResolver.ResolveCondition<Inventory>(condition.Criteria, condition.Property, 
                                                          condition.Value, condition.Start, condition.End);
        var results = await businessInventory.Filter(predicte);
        return results;
    }

    [HttpPost]
    public async Task<bool> Save([FromBody] IEnumerable<Inventory> inventories)
    {
        foreach (var item in inventories)
        {
            await businessInventory.SaveInventoryAsync(item);
        }

        return true;
    }

    [HttpDelete]
    public async Task<bool> Delete(Inventory inventory)
    {
        return await businessInventory.DeleteInventoryAsync(inventory);
    }
}
