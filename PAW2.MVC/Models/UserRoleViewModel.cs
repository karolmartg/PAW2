using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PAW2.Mvc.Helper.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW2.Mvc.Models;

public class UserRoleViewModel
{
    [ValidateID]
    [JsonPropertyName("identifier")]
    public int Id { get; set; }
}
