using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CookingRecipes.Domain.Common;
using EnsureThat;

namespace CookingRecipes.Domain.Entities;

[Table("Categories")]
public class Category : AuditableEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public ICollection<RecipeCategory> RecipeCategories { get; set; }

    private Category() { }

    public Category(string name)
    {
        EnsureArg.IsNotNullOrWhiteSpace(name, nameof(name));
        Name = name;
    }

    public void Update(string name)
    {
        EnsureArg.IsNotNullOrWhiteSpace(name, nameof(name));
        Name = name;
    }
}

