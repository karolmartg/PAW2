using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PAW2.Mvc.Helper.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW2.MVC.Models
{
    public class CategoryViewModel
    {
        [ValidateID]
        [JsonPropertyName("categoryId")]
        public int Id { get; set; }

        [JsonPropertyName("categoryName")]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
