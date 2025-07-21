using Microsoft.AspNetCore.Mvc;
using PAW.Models;
using PAW2.Business;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;

namespace PAW2.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController (IBusinessProduct businessProduct) : Controller
{
    [HttpGet(Name = "GetProducts")]
    public async Task<IEnumerable<Product>> GetAll()
    {
        return await businessProduct.GetAllProductsAsync();
    }

    [HttpGet("{id:int}", Name = "GetProductById")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await businessProduct.GetProductAsync(id);
        return product;
    }

    [HttpPost("filter", Name = "FilterProducts")]
    public async Task<IEnumerable<ProductViewModel>> Filter(ConditionViewModel condition)
    {
        var predicte = ConditionResolver.ResolveCondition<Product>(condition.Criteria, condition.Property,
                                                          condition.Value, condition.Start, condition.End);
        var results = await businessProduct.Filter(predicte);
        return results.ToList();
    }

    [HttpPost]
    public async Task<bool> Save([FromBody] IEnumerable<Product> products)
    {
        foreach (var item in products)
        {
            await businessProduct.SaveProductsAsync(item);
        }

        return true;
    }

    [HttpDelete]
    public async Task<bool> Delete(Product product)
    {
        return await businessProduct.DeleteProductAsync(product);
    }

    [HttpGet("filter")]
    public async Task<IActionResult> FilterByModified([FromQuery] string filter)
    {
        var result = await businessProduct.FilterBusinessAsync(filter);
        return Ok(result);
    }
}