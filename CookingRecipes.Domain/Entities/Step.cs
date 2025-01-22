using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CookingRecipes.Domain.Common;
using EnsureThat;

namespace CookingRecipes.Domain.Entities;

[Table("Steps")]
public class Step : AuditableEntity
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("Recipe")]
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; }
    public int StepNumber { get; set; }
    [Required]
    public string Instruction { get; set; }
    public bool? IsDeleted { get; set; } = false;

    private Step() { }

    public Step(int recipeId, int stepNumber, string instruction)
    {
        EnsureArg.IsNotNullOrWhiteSpace(instruction, nameof(instruction));
        RecipeId = recipeId;
        StepNumber = stepNumber;
        Instruction = instruction;
    }

    public void Update(int stepNumber, string instruction)
    {
        EnsureArg.IsNotNullOrWhiteSpace(instruction, nameof(instruction));
        StepNumber = stepNumber;
        Instruction = instruction;
    }

    public void ChangeDeleteStatus()
    {
        IsDeleted = !IsDeleted;
    }
}
