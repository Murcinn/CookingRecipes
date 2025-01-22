using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CookingRecipes.Domain.Entities;

[Table("RecipeCategories")]
public class RecipeCategory
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Recipe")]
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; }

    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    private RecipeCategory() { }

    public RecipeCategory(int recipeId, int categoryId)
    {
        RecipeId = recipeId;
        CategoryId = categoryId;
    }
}
