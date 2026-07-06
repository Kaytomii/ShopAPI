using Shop.Application.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<int?> CreateCategoryAsync(CategoryCreateDTO dto);
}
