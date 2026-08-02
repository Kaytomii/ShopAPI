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

    public CategoryTreeService(ICategoryRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryReadDTO>> GetParentCategoriesAsync(int id)
    {
        var result = new List<Category>();
        var current = await _repo.GetByIdAsync(id);

        while (current?.ParentId != null)
        {
            var parent = await _repo.GetByIdAsync(current.ParentId.Value);
            if (parent == null) break;

            result.Add(parent);
            current = parent;
        }

        return _mapper.Map<IEnumerable<CategoryReadDTO>>(result);
    }

    public async Task<IEnumerable<CategoryReadDTO>> GetChildCategoriesAsync(int id)
    {
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

        return _mapper.Map<IEnumerable<CategoryReadDTO>>(result);
    }

    public async Task<IEnumerable<CategoryTreeDTO>> GetCategoryTreeAsync()
    {
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

        return BuildTree(null);
    }
}
