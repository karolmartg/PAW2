using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PAW2.Mvc.Helper.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW2.Mvc.Models;

public class InventoryViewModel
{

    [ValidateID]
    [JsonPropertyName("inventoryId")]
    [Display(Name = "Inventory Id")]
    public int InventoryId { get; set; }

    [JsonPropertyName("unitPrice")]
    [Display(Name = "Unit Price")]
    public decimal? UnitPrice { get; set; }

    [JsonPropertyName("unitsInStock")]
    [Display(Name = "Unit In Stock")]
    public int? UnitsInStock { get; set; }

    [JsonPropertyName("lastUpdated")]
    [Display(Name = "Last Updated")]
    public DateTime? LastUpdated { get; set; }

    [JsonPropertyName("dateAdded")]
    [Display(Name = "Date Added")]
    public DateTime? DateAdded { get; set; }

    [JsonPropertyName("modifyBy")]
    [Display(Name = "Modified By")]
    public string? ModifiedBy { get; set; }
}
