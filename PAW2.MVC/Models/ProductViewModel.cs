using Microsoft.AspNetCore.Mvc;
using PAW2.Models;
using PAW2.Mvc.Helper.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW2.MVC.Models;

public class ProductViewModel
{
    [ValidateID]
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [Required]
    [JsonPropertyName("productName")]
    public string? ProductName { get; set; }

    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
