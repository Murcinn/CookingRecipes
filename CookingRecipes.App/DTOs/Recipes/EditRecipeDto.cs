using CookingRecipes.App.DTOs.Ingredients;
using CookingRecipes.App.DTOs.Steps;
using Microsoft.AspNetCore.Http;

namespace CookingRecipes.App.DTOs.Recipes;

public class EditRecipeDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int PreparationTime { get; set; }
    public int CookingTime { get; set; }
    public int Servings { get; set; }
    public bool IsPublic { get; set; }
    public IFormFile? DishImage { get; set; }
    public List<CreateIngredientDto> Ingredients { get; set; } = new();
    public List<CreateStepDto> Steps { get; set; } = new();
    public List<int> CategoryIds { get; set; } = new();
    public string? CurrentImagePath { get; set; }
}
