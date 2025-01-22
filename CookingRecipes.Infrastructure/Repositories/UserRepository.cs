using CookingRecipes.Domain.Entities;
using CookingRecipes.Domain.Interfaces;
using CookingRecipes.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CookingRecipes.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    CookingRecipesContext _context;

    public UserRepository(CookingRecipesContext context)
    {
        _context = context;
    }

    public async Task<User> Add(User user)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(r => r.Email == user.Email);
        if (existingUser != null)
            throw new Exception($"User with email '{user.Email}' already exists.");

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetById(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task Update(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task ChangeDeleteStatus(User user)
    {
        user.ChangeDeleteStatus();
        await _context.SaveChangesAsync();
    }
}
