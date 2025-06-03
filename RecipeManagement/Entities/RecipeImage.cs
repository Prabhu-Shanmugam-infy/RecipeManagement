using System;
using System.Collections.Generic;

namespace RecipeManagement.Entities;

public partial class RecipeImage
{
    public int Id { get; set; }

    public string Path { get; set; } = null!;

    public int RecipeId { get; set; }

    public virtual Recipe Recipe { get; set; } = null!;
}
