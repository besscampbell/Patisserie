using System.Collections.Generic;

namespace Patisserie.Models
{
  public class Flavor
  {
    public Flavor()
    {
      this.Pastries = new HashSet<Pastry>();
    }
    public int FlavorId { get; set; }
    public string Type { get; set; }
    public virtual ICollection<Pastry> Pastries { get; }
  }
}