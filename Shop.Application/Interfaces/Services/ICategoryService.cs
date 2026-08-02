using Shop.Application.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<int?> CreateCategoryAsync(CategoryCreateDTO dto);
    Task<IEnumerable<CategoryReadDTO>> GetAllCategoriesAsync();
    Task<CategoryReadDTO?> GetCategoryByIdAsync(int id);
    Task<bool> DeleteCategoryAsync(int id);
    Task<CategoryReadDTO?> UpdateCategoryAsync(int id, CategoryUpdateDTO dto);
}