using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PAW2.Models.ViewModels
{
    public class ProductViewModel
    {
        [JsonPropertyName("tempID")]
        public int TempID { get; set; }

        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [Required]
        [JsonPropertyName("productName")]
        [Display(Name = "Name")]
        public string? ProductName { get; set; }

        [JsonPropertyName("inventoryId")]
        public int? InventoryId { get; set; }

        [JsonPropertyName("supplierId")]
        public int? SupplierId { get; set; }

        [JsonPropertyName("description")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [JsonPropertyName("rating")]
        [Display(Name = "Rating")]
        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public decimal? Rating { get; set; }

        [JsonPropertyName("categoryId")]
        public int? CategoryId { get; set; }

        [JsonPropertyName("lastModified")]
        [Display(Name = "Last Modified")]
        [DataType(DataType.Date)]
        public DateTime? LastModified { get; set; }

        [JsonPropertyName("modifiedBy")]
        [Display(Name = "Modified By")]
        public string? ModifiedBy { get; set; }

        [JsonPropertyName("category")]
        [Display(Name = "Category")]
        public virtual Category? Category { get; set; }
    }
}
