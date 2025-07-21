using Microsoft.AspNetCore.Mvc;
using PAW.Models;
using PAW2.Business;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;

namespace PAW2.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SupplierController(IBusinessSupplier businessSupplier) : Controller
{
    [HttpGet(Name = "GetSuppliers")]
    public async Task<IEnumerable<Supplier>> GetAll()
    {
        return await businessSupplier.GetAllSuppliersAsync();
    }

    [HttpGet("{id:int}", Name = "GetSupplierById")]
    public async Task<ActionResult<Supplier>> GetById(int id)
    {
        var supplier = await businessSupplier.GetSupplierAsync(id);
        return supplier;
    }
   
    [HttpPost("filter", Name = "FilterSuppliers")]    
    public async Task<IEnumerable<SupplierViewModel>> Filter(ConditionViewModel condition)
    {
        var predicte = ConditionResolver.ResolveCondition<Supplier>(condition.Criteria, condition.Property, 
                                                          condition.Value, condition.Start, condition.End);
        var results = await businessSupplier.Filter(predicte);
        return results;
    }

    [HttpPost]
    public async Task<bool> Save([FromBody] IEnumerable<Supplier> suppliers)
    {
        foreach (var item in suppliers)
        {
            await businessSupplier.SaveSupplierAsync(item);
        }

        return true;
    }

    [HttpDelete]
    public async Task<bool> Delete(Supplier supplier)
    {
        return await businessSupplier.DeleteSupplierAsync(supplier);
    }
}
