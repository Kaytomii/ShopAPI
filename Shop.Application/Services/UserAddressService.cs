using AutoMapper;
using Shop.Application.DTOs.UserDTOs;
using Shop.Application.Interfaces.Repository;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Services;

public class UserAddressService
{
    private readonly IUserAddressRepository _repository;
    private readonly IMapper _mapper;

    public UserAddressService(IUserAddressRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task AddAddressAsync(UserAddressCreateDto dto, CancellationToken token)
    {
        var address = _mapper.Map<UserAddress>(dto);
        await _repository.AddAsync(address, token);
    }

    public async Task<IEnumerable<UserAddress>> GetUserAddressesAsync(Guid userId, CancellationToken token)
    {
        return await _repository.GetByUserIdAsync(userId, token);
    }
}