using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CookingRecipes.Domain.Common;
using EnsureThat;

namespace CookingRecipes.Domain.Entities;

[Table("Users")]
public class User : AuditableEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public DateTime DateOfBirth { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public byte[] PasswordHash { get; set; }
    [Required]
    public byte[] PasswordSalt { get; set; }
    public bool? IsAdmin { get; set; }
    public bool? IsDeleted { get; set; } = false;
    public ICollection<Recipe> Recipes { get; set; }


    private User() { }

    public User(string firstName, string lastName,
                DateTime dateOfBirth, string email)
    {
        Update(firstName, lastName, dateOfBirth, email);
    }

    public void Update(string firstName, string lastName,
                DateTime dateOfBirth, string email)
    {
        EnsureArg.IsNotNullOrWhiteSpace(firstName, nameof(firstName));
        EnsureArg.IsNotNullOrWhiteSpace(lastName, nameof(lastName));
        EnsureArg.IsNotNullOrWhiteSpace(email, nameof(email));

        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Email = email;
    }

    public void ChangeDeleteStatus()
    {
        IsDeleted = !IsDeleted;
    }

}

