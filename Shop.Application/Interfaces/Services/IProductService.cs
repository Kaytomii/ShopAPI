using Shop.Application.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Interfaces.Services;

public interface IProductService
{
    Task<int> CreateProductAsync(ProductCreateDTO dto);
    Task<IEnumerable<ProductReadDTO>> GetAllAsync();
    Task<ProductReadDTO?> GetByIdAsync(int id);
}
