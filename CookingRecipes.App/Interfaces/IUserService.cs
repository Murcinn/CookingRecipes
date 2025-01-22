using CookingRecipes.App.DTOs.Users;
using CookingRecipes.Domain.Entities;

namespace CookingRecipes.App.Interfaces;

public interface IUserService
{
    int GetCurrentUserId();
    Task<bool> Register(User user, string password);
    Task<string?> Login(string email, string password);
    Task<EditUserDto?> GetUserForEdit(int userId);
    Task<bool> UpdateUserProfile(int userId, EditUserDto dto);
    Task<List<User>> GetAllUsers();
    Task<bool> DeleteUser(int userId);
    Task<bool> RestoreUser(int userId);
}
