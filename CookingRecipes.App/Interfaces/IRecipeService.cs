using CookingRecipes.App.DTOs.Recipes;
using CookingRecipes.Domain.Entities;

namespace CookingRecipes.App.Interfaces;
public interface IRecipeService
{
    Task<Recipe> CreateRecipe(CreateRecipeDto dto, int authorId);
    Task<Recipe?> GetRecipeById(int id);
    Task<List<Recipe>> GetAllPublicRecipes();
    Task<List<Recipe>> GetRecipesByAuthor(int authorId);
    Task<bool> UpdateRecipe(EditRecipeDto dto, int userId);
    Task<bool> DeleteRecipe(int id, int userId);
}
