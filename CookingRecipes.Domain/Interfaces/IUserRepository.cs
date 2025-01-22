using CookingRecipes.Domain.Entities;

namespace CookingRecipes.Domain.Interfaces;

public interface IUserRepository
{
    Task<User> Add(User user);
    Task<User?> GetByEmail(string email);
    Task<User?> GetById(int id);
    Task Update(User user);
    Task<List<User>> GetAllUsers();
    Task ChangeDeleteStatus(User user);
}
