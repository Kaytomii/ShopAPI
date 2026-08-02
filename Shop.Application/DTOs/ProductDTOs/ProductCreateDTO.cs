using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.DTOs.ProductDTOs;

public class ProductCreateDTO
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CategoryId { get; set; }

    public List<string> ImageUrls { get; set; } = new();
}
