using CookingRecipes.Domain.Entities;

namespace CookingRecipes.Domain.Interfaces;

public interface IRecipeRepository
{
    Task<Recipe> Add(Recipe recipe);
    Task<Recipe?> GetById(int id);
    Task<List<Recipe>> GetAllPublic();
    Task<List<Recipe>> GetByAuthor(int authorId);
    Task Update(Recipe recipe);
    Task Delete(int id);
    Task ChangeRecipesDeleteStatus(List<Recipe> recipes);
}
