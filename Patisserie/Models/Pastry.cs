namespace Patisserie.Models
{
  public class Pastry
  {
    public int PastryId { get; set;}
    public int TreatId { get; set; }
    public int FlavorId { get; set; }
    public virtual Treat Treat { get; set; }
    public virtual Flavor Flavor { get; set; }
  }
}