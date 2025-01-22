using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using CookingRecipes.App.DTOs.Users;
using CookingRecipes.App.Interfaces;
using CookingRecipes.Domain.Entities;
using CookingRecipes.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace CookingRecipes.App.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly string _jwtKey = "My project CookingRecipes application secret token token";
    private readonly IRecipeRepository _recipeRepository;

    public UserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IRecipeRepository recipeRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _recipeRepository = recipeRepository;
    }

    public int GetCurrentUserId()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var token = httpContext?.Request.Cookies["AuthToken"];
        if (string.IsNullOrEmpty(token))
            return 0;

        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

        var jwtToken = handler.ReadJwtToken(token);
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
        {
            return userId;
        }

        return 0;
    }


    public async Task<bool> Register(User user, string password)
    {
        CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        await _userRepository.Add(user);
        return true;
    }

    public async Task<string?> Login(string email, string password)
    {
        var user = await _userRepository.GetByEmail(email);
        if (user == null || !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            return null;

        return GenerateJwtToken(user);
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using var hmac = new HMACSHA512(storedSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(storedHash);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString()),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email),
                new System.Security.Claims.Claim("IsAdmin", user.IsAdmin?.ToString() ?? "false")
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<EditUserDto?> GetUserForEdit(int userId)
    {
        var user = await _userRepository.GetById(userId);
        if (user == null)
            return null;

        return new EditUserDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            DateOfBirth = user.DateOfBirth
        };
    }

    public async Task<bool> UpdateUserProfile(int userId, EditUserDto dto)
    {
        var user = await _userRepository.GetById(userId);
        if (user == null)
            return false;

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.DateOfBirth = dto.DateOfBirth;

        await _userRepository.Update(user);
        return true;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _userRepository.GetAllUsers();
    }

    public async Task<bool> DeleteUser(int userId)
    {
        var user = await _userRepository.GetById(userId);
        if (user == null) return false;
        await _userRepository.ChangeDeleteStatus(user);

        var userRecipes = await _recipeRepository.GetByAuthor(userId);

        await _recipeRepository.ChangeRecipesDeleteStatus(userRecipes);

        return true;
    }

    public async Task<bool> RestoreUser(int userId)
    {
        var user = await _userRepository.GetById(userId);
        if (user == null) return false;
        await _userRepository.ChangeDeleteStatus(user);

        var userRecipes = await _recipeRepository.GetByAuthor(userId);

        await _recipeRepository.ChangeRecipesDeleteStatus(userRecipes);

        return true;
    }

}
