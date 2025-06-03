using System;
using System.Collections.Generic;

namespace RecipeManagement.Entities;

public partial class Recipe
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Ingredients { get; set; } = null!;

    public string Instructions { get; set; } = null!;

    public int AuthorId { get; set; }

    public int CookingTimeInMins { get; set; }

    public int CategoryId { get; set; }

    public int? Active { get; set; }

    public virtual User Author { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<RecipeImage> RecipeImages { get; set; } = new List<RecipeImage>();
}
