using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.DTOs.CategoryDTOs;

public class CategoryTreeDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int? ParentId { get; set; }

    public List<CategoryTreeDTO> Children { get; set; } = new();
}
