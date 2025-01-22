using CookingRecipes.Domain.Entities;
using CookingRecipes.Domain.Interfaces;
using CookingRecipes.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CookingRecipes.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly CookingRecipesContext _context;

    public CategoryRepository(CookingRecipesContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAll()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task Add(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
