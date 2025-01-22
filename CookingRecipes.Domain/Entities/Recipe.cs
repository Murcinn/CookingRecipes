using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CookingRecipes.Domain.Common;
using EnsureThat;

namespace CookingRecipes.Domain.Entities;

[Table("Recipes")]
public class Recipe : AuditableEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
    public int PreparationTime { get; set; }
    public int CookingTime { get; set; }
    public int Servings { get; set; }
    public bool IsPublic { get; set; } = false;
    public string? ImagePath { get; set; }
    public int AuthorId { get; set; }
    public User Author { get; set; }
    public bool? IsDeleted { get; set; } = false;
    public ICollection<Ingredient> Ingredients { get; set; }
    public ICollection<RecipeCategory> RecipeCategories { get; set; }
    public ICollection<Step> Steps { get; set; }

    private Recipe() { }

    public Recipe(string title, int authorId)
    {
        EnsureArg.IsNotNullOrWhiteSpace(title, nameof(title));
        Title = title;
        AuthorId = authorId;
        Ingredients = new List<Ingredient>();
        RecipeCategories = new List<RecipeCategory>();
        Steps = new List<Step>();
    }

    public void Update(string title, string description, int prepTime, int cookingTime, int servings, bool isPublic)
    {
        EnsureArg.IsNotNullOrWhiteSpace(title, nameof(title));

        Title = title;
        Description = description;
        PreparationTime = prepTime;
        CookingTime = cookingTime;
        Servings = servings;
        IsPublic = isPublic;
    }

    public void ChangeDeleteStatus()
    {
        IsDeleted = !IsDeleted;
    }
}


