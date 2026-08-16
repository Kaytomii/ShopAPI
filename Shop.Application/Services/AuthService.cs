using AutoMapper;
using Microsoft.Extensions.Options;
using Shop.Application.DTOs.UserDTOs;
using Shop.Application.Interfaces.Helpers;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Interfaces.Services;
using ShopDomain.Models;
using Shop.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Services;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly IAuthRepository _repository;
    private readonly IHashHelper _hashHelper;
    private readonly IJWTService _jwtService;
    private readonly IRefreshTokenRepository _refreshRepo;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        IMapper mapper,
        IAuthRepository repository,
        IHashHelper hashHelper,
        IJWTService jwtService,
        IRefreshTokenRepository refreshRepo,
        IOptions<JwtSettings> jwtOptions)
    {
        _mapper = mapper;
        _repository = repository;
        _hashHelper = hashHelper;
        _jwtService = jwtService;
        _refreshRepo = refreshRepo;
        _jwtSettings = jwtOptions.Value;
    }

    public async Task<(UserReadDTO? User, string? AccessToken, string? RefreshToken)> RegisterAsync(UserCreateDTO dto)
    {
        var isExist = await _repository.IsExistEmailAsync(dto.Email);
        if (isExist)
            return (null, null, null);

        var hash = _hashHelper.Hash(dto.Password);
        var user = _mapper.Map<User>(dto);

        var accessToken = _jwtService.GenerateAccessToken(_mapper.Map<UserLoginDTO>(user), user.Role.ToString());
        var registerUser = await _repository.RegisterUserAsync(user, hash);

        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            UserId = registerUser.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.ExpiresRefreshTokenDay)
        };

        await _refreshRepo.AddAsync(refreshToken);

        return (_mapper.Map<UserReadDTO>(registerUser), accessToken, refreshToken.Token);
    }

    public async Task<(UserReadDTO? User, string? AccessToken, string? RefreshToken)> LoginAsync(UserLoginDTO dto)
    {
        var user = await _repository.GetUserByEmailAsync(dto.Email);
        if (user == null)
            return (null, null, null);

        var isValidPassword = _hashHelper.IsValidPassword(dto.Password, user.PasswordHash);
        if (!isValidPassword)
            return (null, null, null);

        var accessToken = _jwtService.GenerateAccessToken(_mapper.Map<UserLoginDTO>(user), user.Role.ToString());

        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.ExpiresRefreshTokenDay)
        };

        await _refreshRepo.AddAsync(refreshToken);

        return (_mapper.Map<UserReadDTO>(user), accessToken, refreshToken.Token);
    }
}