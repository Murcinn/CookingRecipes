using CookingRecipes.Domain.Entities;

namespace CookingRecipes.Api.Models;

public class HomeRecipesModel
{
    public List<Recipe> MyRecipes { get; set; } = new();
    public List<Recipe> AllRecipes { get; set; } = new();
    public string? SearchQuery { get; set; }
    public int? SelectedCategoryId { get; set; }
    public List<Category> Categories { get; set; } = new();
}
