using System;
using System.Collections.Generic;

namespace RecipeBox.Models
{
  public class Recipe
  {
    public Recipe()
    {
      this.JoinEntities = new HashSet<TagRecipe>();
    }
    public int RecipeId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Rating { get; set; }
    public string Ingredients { get; set; }
    public string Instructions { get; set; }
    //public bool HaveCookedIt 
    public virtual ApplicationUser User { get; set; }
    public virtual ICollection<TagRecipe> JoinEntities { get; }
  }
}