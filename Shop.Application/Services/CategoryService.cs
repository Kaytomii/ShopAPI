using AutoMapper;
using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Interfaces.Services;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repo;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<int?> CreateCategoryAsync(CategoryCreateDTO dto)
    {
        var category = _mapper.Map<Category>(dto);
        return await _repo.AddCategoryAsync(category);
    }

    public async Task<IEnumerable<CategoryReadDTO>> GetAllCategoriesAsync()
    {
        var categories = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<CategoryReadDTO>>(categories);
    }

    public async Task<CategoryReadDTO?> GetCategoryByIdAsync(int id)
    {
        var category = await _repo.GetByIdAsync(id);
        return category == null ? null : _mapper.Map<CategoryReadDTO>(category);
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _repo.GetByIdAsync(id);
        if (category == null) return false;

        return await _repo.DeleteAsync(category);
    }

    public async Task<CategoryReadDTO?> UpdateCategoryAsync(int id, CategoryUpdateDTO dto)
    {
        var category = await _repo.GetByIdAsync(id);
        if (category == null) return null;

        _mapper.Map(dto, category);

        var updated = await _repo.UpdateAsync(category);
        return updated ? _mapper.Map<CategoryReadDTO>(category) : null;
    }
}