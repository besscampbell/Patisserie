using System.Collections.Generic;

namespace Patisserie.Models
{
  public class Treat
  {
    public Treat()
    {
      this.Pastries = new HashSet<Pastry>();
    }
    public int TreatId { get; set; }
    public string Type { get; set; }
    public virtual ApplicationUser User { get; set; }

    public virtual ICollection<Pastry> Pastries { get; }
  }
}