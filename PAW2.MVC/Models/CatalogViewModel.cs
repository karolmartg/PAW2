using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PAW2.Mvc.Helper.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW2.Mvc.Models;

public class CatalogViewModel
{
    [ValidateID]
    [JsonPropertyName("identifier")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [BindNever]
    public string Hash { get; set; }
}
