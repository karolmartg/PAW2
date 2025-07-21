﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PAW2.Data.Models;

public partial class Component
{
    [JsonPropertyName("id")]
    public decimal Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
    
    [JsonPropertyName("content")]
    public string Content { get; set; } = null!;
}
