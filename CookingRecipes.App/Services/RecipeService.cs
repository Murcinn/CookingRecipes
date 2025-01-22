using CookingRecipes.App.DTOs.Recipes;
using CookingRecipes.App.Interfaces;
using CookingRecipes.Domain.Entities;
using CookingRecipes.Domain.Interfaces;

namespace CookingRecipes.App.Services;

public class RecipeService : IRecipeService
{
    private readonly IRecipeRepository _recipeRepository;

    public RecipeService(IRecipeRepository recipeRepository)
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<Recipe> CreateRecipe(CreateRecipeDto dto, int authorId)
    {
        var recipe = new Recipe(dto.Title, authorId);
        recipe.Description = dto.Description;
        recipe.PreparationTime = dto.PreparationTime;
        recipe.CookingTime = dto.CookingTime;
        recipe.Servings = dto.Servings;
        recipe.IsPublic = dto.IsPublic;

        if (dto.DishImage != null && dto.DishImage.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + dto.DishImage.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await dto.DishImage.CopyToAsync(fileStream);
            }
            recipe.ImagePath = "/images/" + uniqueFileName;
        }

        foreach (var ingredientDto in dto.Ingredients)
        {
            var ingredient = new Ingredient(
                ingredientDto.Name,
                ingredientDto.Quantity,
                ingredientDto.Unit,
                recipe.Id
            );
            recipe.Ingredients.Add(ingredient);
        }

        foreach (var stepDto in dto.Steps)
        {
            var step = new Step(
                recipe.Id,
                stepDto.StepNumber,
                stepDto.Instruction
            );
            recipe.Steps.Add(step);
        }

        foreach (var categoryId in dto.CategoryIds)
        {
            var recipeCategory = new RecipeCategory(recipe.Id, categoryId);
            recipe.RecipeCategories.Add(recipeCategory);
        }

        return await _recipeRepository.Add(recipe);
    }


    public async Task<Recipe?> GetRecipeById(int id)
    {
        return await _recipeRepository.GetById(id);
    }

    public async Task<List<Recipe>> GetAllPublicRecipes()
    {
        return await _recipeRepository.GetAllPublic();
    }

    public async Task<List<Recipe>> GetRecipesByAuthor(int authorId)
    {
        return await _recipeRepository.GetByAuthor(authorId);
    }

    public async Task<bool> UpdateRecipe(EditRecipeDto dto, int userId)
    {
        var existingRecipe = await _recipeRepository.GetById(dto.Id);
        if (existingRecipe == null) return false;

        if (existingRecipe.AuthorId != userId) return false;

        existingRecipe.Title = dto.Title;
        existingRecipe.Description = dto.Description;
        existingRecipe.PreparationTime = dto.PreparationTime;
        existingRecipe.CookingTime = dto.CookingTime;
        existingRecipe.Servings = dto.Servings;
        existingRecipe.IsPublic = dto.IsPublic;

        if (dto.DishImage != null && dto.DishImage.Length > 0)
        {

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + dto.DishImage.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await dto.DishImage.CopyToAsync(fileStream);
            }

            existingRecipe.ImagePath = "/images/" + uniqueFileName;
        }


        existingRecipe.Ingredients.Clear();
        foreach (var ingDto in dto.Ingredients)
        {
            existingRecipe.Ingredients.Add(new Ingredient(ingDto.Name, ingDto.Quantity, ingDto.Unit, existingRecipe.Id));
        }

        existingRecipe.Steps.Clear();
        foreach (var stepDto in dto.Steps)
        {
            existingRecipe.Steps.Add(new Step(existingRecipe.Id, stepDto.StepNumber, stepDto.Instruction));
        }

        existingRecipe.RecipeCategories.Clear();
        foreach (var catId in dto.CategoryIds)
        {
            existingRecipe.RecipeCategories.Add(new RecipeCategory(existingRecipe.Id, catId));
        }

        await _recipeRepository.Update(existingRecipe);
        return true;
    }

    public async Task<bool> DeleteRecipe(int id, int userId)
    {
        var recipe = await _recipeRepository.GetById(id);
        if (recipe == null) return false;
        if (recipe.AuthorId != userId) return false;

        await _recipeRepository.Delete(id);
        return true;
    }
}
