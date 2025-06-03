using System;
using System.Collections.Generic;

namespace RecipeManagement.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? ProfilePicture { get; set; }

    public string? Bio { get; set; }

    public int? IsActive { get; set; }

    public int? IsAdmin { get; set; }

    public string? PasswordHash { get; set; }

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
