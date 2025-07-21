using Microsoft.AspNetCore.Mvc;
using PAW.Models;
using PAW2.Business;
using PAW2.Models;
using PAW2.Models.PAW2Models;
using PAW2.Models.ViewModels;

namespace PAW2.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController (IBusinessCategory businessCategory) : Controller
{
    [HttpGet(Name = "GetCategories")]
    public async Task<IEnumerable<Category>> GetAll()
    {
        return await businessCategory.GetAllCategoriesAsync();
    }

    [HttpGet("{id:int}", Name = "GetCategoryById")]
    public async Task<ActionResult<Category>> GetById (int id)
    {
        var category = await businessCategory.GetCategoryAsync(id);
        return category;
    }

    [HttpPost("filter", Name = "FilterCategories")]
    public async Task<IEnumerable<CategoryViewModel>> Filter(ConditionViewModel condition)
    {
        var predicate = ConditionResolver.ResolveCondition<Category>(condition.Criteria, condition.Property,
                                                                     condition.Value, condition.Start, condition.End);
        var results = await businessCategory.Filter(predicate);
        return results;
    }

    [HttpPost]
    public async Task<bool> Save([FromBody] IEnumerable<Category> categories)
    {
        foreach (var item in categories)
        {
            await businessCategory.SaveCategoryAsync(item);
        }

        return true;
    }

    [HttpDelete]
    public async Task<bool> Delete(Category category)
    {
        return await businessCategory.DeleteCategoryAsync(category);
    }

    // Filter for modifiedby and lastmodified
    [HttpGet("filter")]
    public async Task<IActionResult> FilterByModified([FromQuery] string filter)
    {
        var result = await businessCategory.FilterBusinessAsync(filter);
        return Ok(result);
    }
}
