using AutoMapper;
using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Interfaces.Services;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Services;

public class CategoryTreeService : ICategoryTreeService
{
    private readonly ICategoryRepository _repo;
    private readonly IMapper _mapper;
    private readonly ICachingService _cache;

    public CategoryTreeService(ICategoryRepository repo, IMapper mapper, ICachingService cache)
    {
        _repo = repo;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<IEnumerable<CategoryReadDTO>> GetParentCategoriesAsync(int id)
    {
        var key = $"category:{id}:parents";

        var cached = await _cache.GetAsync<IEnumerable<CategoryReadDTO>>(key);
        if (cached != null)
            return cached;

        var result = new List<Category>();
        var current = await _repo.GetByIdAsync(id);

        while (current?.ParentId != null)
        {
            var parent = await _repo.GetByIdAsync(current.ParentId.Value);
            if (parent == null) break;

            result.Add(parent);
            current = parent;
        }

        var dto = _mapper.Map<IEnumerable<CategoryReadDTO>>(result);

        await _cache.SetAsync(key, dto);
        return dto;
    }

    public async Task<IEnumerable<CategoryReadDTO>> GetChildCategoriesAsync(int id)
    {
        var key = $"category:{id}:children";

        var cached = await _cache.GetAsync<IEnumerable<CategoryReadDTO>>(key);
        if (cached != null)
            return cached;

        var all = await _repo.GetAllAsync();
        var result = new List<Category>();

        void FindChildren(int parentId)
        {
            var children = all.Where(c => c.ParentId == parentId).ToList();
            foreach (var child in children)
            {
                result.Add(child);
                FindChildren(child.Id);
            }
        }

        FindChildren(id);

        var dto = _mapper.Map<IEnumerable<CategoryReadDTO>>(result);

        await _cache.SetAsync(key, dto);
        return dto;
    }

    public async Task<IEnumerable<CategoryTreeDTO>> GetCategoryTreeAsync()
    {
        var key = "categories:tree";

        var cached = await _cache.GetAsync<IEnumerable<CategoryTreeDTO>>(key);
        if (cached != null)
            return cached;

        var all = (await _repo.GetAllAsync()).ToList();

        List<CategoryTreeDTO> BuildTree(int? parentId)
        {
            return all
                .Where(c => c.ParentId == parentId)
                .Select(c => new CategoryTreeDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    ParentId = c.ParentId,
                    Children = BuildTree(c.Id)
                })
                .ToList();
        }

        var tree = BuildTree(null);

        await _cache.SetAsync(key, tree);
        return tree;
    }
}