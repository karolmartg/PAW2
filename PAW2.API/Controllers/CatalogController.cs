using Microsoft.AspNetCore.Mvc;
using PAW.Models;
using PAW2.Business;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;

namespace PAW2.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogController (IBusinessCatalog businessCatalog) : Controller
{
    [HttpGet(Name = "GetCatalogs")]
    public async Task<IEnumerable<Catalog>> GetAll()
    {
        return await businessCatalog.GetAllCatalogsAsync();
    }

    [HttpGet("{id:int}", Name = "GetCatalogById")]
    public async Task<ActionResult<Catalog>> GetById(int id)
    {
        var catalog = await businessCatalog.GetCatalogAsync(id);
        return catalog;
    }
   
    [HttpPost("filter", Name = "FilterCatalogs")]    
    public async Task<IEnumerable<CatalogViewModel>> Filter(ConditionViewModel condition)
    {
        var predicte = ConditionResolver.ResolveCondition<Catalog>(condition.Criteria, condition.Property, 
                                                          condition.Value, condition.Start, condition.End);
        var results = await businessCatalog.Filter(predicte);
        return results;
    }

    [HttpPost]
    public async Task<bool> Save([FromBody] IEnumerable<Catalog> catalogs)
    {
        foreach (var item in catalogs)
        {
            await businessCatalog.SaveCatalogAsync(item);
        }

        return true;
    }

    [HttpDelete]
    public async Task<bool> Delete(Catalog catalog)
    {
        return await businessCatalog.DeleteCatalogAsync(catalog);
    }
}
