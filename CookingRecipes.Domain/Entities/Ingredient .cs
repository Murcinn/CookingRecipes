using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CookingRecipes.Domain.Common;
using EnsureThat;

namespace CookingRecipes.Domain.Entities;

[Table("Ingredients")]
public class Ingredient : AuditableEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public double Quantity { get; set; }
    public string Unit { get; set; }
    [ForeignKey("Recipe")]
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; }
    public bool? IsDeleted { get; set; } = false;

    private Ingredient() { }

    public Ingredient(string name, double quantity, string unit, int recipeId)
    {
        EnsureArg.IsNotNullOrWhiteSpace(name, nameof(name));
        Name = name;
        Quantity = quantity;
        Unit = unit;
        RecipeId = recipeId;
    }

    public void Update(string name, double quantity, string unit)
    {
        EnsureArg.IsNotNullOrWhiteSpace(name, nameof(name));
        Name = name;
        Quantity = quantity;
        Unit = unit;
    }

    public void ChangeDeleteStatus()
    {
        IsDeleted = !IsDeleted;
    }
}

