using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Shop.Application.DTOs.UserDTOs;
using Shop.Application.Interfaces.Helpers;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Interfaces.Services;
using Shop.Infrastructure.Configuration;
using ShopDomain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shop.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly IAuthRepository _authRepository;
    private readonly IAuthRepository _repository;
    private readonly IHashHelper _hashHelper;
    private readonly IJWTService _jwtService;
    private readonly IRefreshTokenRepository _refreshRepo;
    private readonly JwtSettings _jwtSettings;
    private readonly IQueueService _queueService;

    public AuthService(
        IMapper mapper,
        IAuthRepository repository,
        IHashHelper hashHelper,
        IJWTService jwtService,
        IRefreshTokenRepository refreshRepo,
        IOptions<JwtSettings> jwtOptions,
        IQueueService queueService,
        IAuthRepository authRepository)
    {
        _mapper = mapper;
        _repository = repository;
        _hashHelper = hashHelper;
        _jwtService = jwtService;
        _authRepository = authRepository;
        _refreshRepo = refreshRepo;
        _jwtSettings = jwtOptions.Value;
        _queueService = queueService;
    }

    public async Task<(UserReadDTO? User, string? AccessToken, string? RefreshToken)> RegisterAsync(UserCreateDTO dto)
    {
        var isExist = await _repository.IsEmailExistsAsync(dto.Email);
        if (isExist)
            return (null, null, null);

        var hash = _hashHelper.Hash(dto.Password);
        var user = _mapper.Map<User>(dto);

        var accessToken = _jwtService.GenerateAccessToken(
            _mapper.Map<UserLoginDTO>(user),
            user.Role.ToString()
        );

        var registerUser = await _repository.RegisterUserAsync(user, hash);

        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            UserId = registerUser.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.ExpiresRefreshTokenDay)
        };

        await _refreshRepo.AddAsync(refreshToken);

        var queueMessage = new UserQueueDto
        {
            Email = dto.Email,
            Password = dto.Password
        };

        await _queueService.PublishAsync("Users", queueMessage);

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

        var accessToken = _jwtService.GenerateAccessToken(
            _mapper.Map<UserLoginDTO>(user),
            user.Role.ToString()
        );

        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.ExpiresRefreshTokenDay)
        };

        await _refreshRepo.AddAsync(refreshToken);

        return (_mapper.Map<UserReadDTO>(user), accessToken, refreshToken.Token);
    }

    public async Task<string?> RefreshAsync(string refreshToken)
    {
        var tokenEntity = await _authRepository.GetRefreshTokenAsync(refreshToken);
        if (tokenEntity == null || tokenEntity.ExpiresAt < DateTime.UtcNow)
            return null;

        var user = await _authRepository.GetUserByIdAsync(tokenEntity.UserId);
        if (user == null)
            return null;

        var claims = new List<Claim>
        {
            new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresMinutes),
            signingCredentials: creds
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

        return accessToken;
    }

    public Task<User?> GetUserByEmailAsync(string email)
    {
        return _authRepository.GetUserByEmailAsync(email);
    }

    public async Task CreateExternalUserAsync(User user)
    {
        await _authRepository.RegisterUserAsync(user, _hashHelper.Hash(Guid.NewGuid().ToString()));
    }

    public async Task<string> GenerateRefreshTokenAsync(Guid userId)
    {
        var token = new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.ExpiresRefreshTokenDay)
        };

        await _refreshRepo.AddAsync(token);
        return token.Token;
    }
}
