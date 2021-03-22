using System.Collections.Generic;

namespace RecipeBox.Models
{
  public class Tag
  {
    public Tag()
    {
      this.JoinEntities = new HashSet<TagRecipe>();
    }

    public int TagId { get; set; }
    public string Name { get; set; }
    public int Rating { get; set; }
    public virtual ICollection<TagRecipe> JoinEntities { get; set; }
  }
}