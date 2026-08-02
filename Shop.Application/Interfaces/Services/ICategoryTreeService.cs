using Shop.Application.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Interfaces.Services;

public interface ICategoryTreeService
{
    Task<IEnumerable<CategoryReadDTO>> GetParentCategoriesAsync(int id);
    Task<IEnumerable<CategoryReadDTO>> GetChildCategoriesAsync(int id);
    Task<IEnumerable<CategoryTreeDTO>> GetCategoryTreeAsync();
}