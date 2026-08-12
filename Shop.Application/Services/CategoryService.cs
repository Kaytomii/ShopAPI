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
    private readonly ICachingService _cache;

    public CategoryService(ICategoryRepository repo, IMapper mapper, ICachingService cache)
    {
        _repo = repo;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<int?> CreateCategoryAsync(CategoryCreateDTO dto)
    {
        var category = _mapper.Map<Category>(dto);
        var id = await _repo.AddCategoryAsync(category);

        await _cache.RemoveAsync("categories:all");
        await _cache.RemoveAsync("categories:tree");

        return id;
    }

    public async Task<IEnumerable<CategoryReadDTO>> GetAllCategoriesAsync()
    {
        var cached = await _cache.GetAsync<IEnumerable<CategoryReadDTO>>("categories:all");
        if (cached != null)
            return cached;

        var categories = await _repo.GetAllAsync();
        var dto = _mapper.Map<IEnumerable<CategoryReadDTO>>(categories);

        await _cache.SetAsync("categories:all", dto);
        return dto;
    }

    public async Task<CategoryReadDTO?> GetCategoryByIdAsync(int id)
    {
        var key = $"category:{id}";
        var cached = await _cache.GetAsync<CategoryReadDTO>(key);
        if (cached != null)
            return cached;

        var category = await _repo.GetByIdAsync(id);
        if (category == null)
            return null;

        var dto = _mapper.Map<CategoryReadDTO>(category);
        await _cache.SetAsync(key, dto);
        return dto;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _repo.GetByIdAsync(id);
        if (category == null)
            return false;

        var deleted = await _repo.DeleteAsync(category);

        await _cache.RemoveAsync("categories:all");
        await _cache.RemoveAsync($"category:{id}");
        await _cache.RemoveAsync($"category:{id}:children");
        await _cache.RemoveAsync($"category:{id}:parents");
        await _cache.RemoveAsync("categories:tree");

        return deleted;
    }

    public async Task<CategoryReadDTO?> UpdateCategoryAsync(int id, CategoryUpdateDTO dto)
    {
        var category = await _repo.GetByIdAsync(id);
        if (category == null)
            return null;

        _mapper.Map(dto, category);

        var updated = await _repo.UpdateAsync(category);
        if (!updated)
            return null;

        var dtoResult = _mapper.Map<CategoryReadDTO>(category);

        await _cache.RemoveAsync("categories:all");
        await _cache.RemoveAsync($"category:{id}");
        await _cache.RemoveAsync($"category:{id}:children");
        await _cache.RemoveAsync($"category:{id}:parents");
        await _cache.RemoveAsync("categories:tree");

        return dtoResult;
    }
}