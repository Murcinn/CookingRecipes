using CookingRecipes.Domain.Entities;
using CookingRecipes.Domain.Interfaces;
using CookingRecipes.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CookingRecipes.Infrastructure.Repositories;

public class RecipeRepository : IRecipeRepository
{
    private readonly CookingRecipesContext _context;

    public RecipeRepository(CookingRecipesContext context)
    {
        _context = context;
    }

    public async Task<Recipe> Add(Recipe recipe)
    {
        _context.Recipes.Add(recipe);
        await _context.SaveChangesAsync();
        return recipe;
    }

    public async Task<Recipe?> GetById(int id)
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
            .Include(r => r.Steps)
            .Include(r => r.RecipeCategories)
                .ThenInclude(rc => rc.Category)
            .Include(r => r.Author)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<Recipe>> GetAllPublic()
    {
        return await _context.Recipes
            .Where(r => r.IsPublic)
            .Where(r => r.IsDeleted == false)
            .Include(r => r.RecipeCategories)
                .ThenInclude(rc => rc.Category)
            .ToListAsync();
    }


    public async Task<List<Recipe>> GetByAuthor(int authorId)
    {
        return await _context.Recipes
            .Where(r => r.AuthorId == authorId)
            .Include(r => r.RecipeCategories)
                .ThenInclude(rc => rc.Category)
            .Include(r => r.Steps)
            .Include(r => r.Ingredients)
            .ToListAsync();
    }

    public async Task Update(Recipe recipe)
    {
        _context.Recipes.Update(recipe);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var recipe = await _context.Recipes.FindAsync(id);
        if (recipe != null)
        {
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ChangeRecipesDeleteStatus(List<Recipe> recipes)
    {
        foreach (var recipe in recipes)
        {
            recipe.ChangeDeleteStatus();

            foreach (var step in recipe.Steps)
            {
                step.ChangeDeleteStatus();
            }

            foreach (var ingredient in recipe.Ingredients)
            {
                ingredient.ChangeDeleteStatus();
            }
        }
        await _context.SaveChangesAsync();
    }
}
