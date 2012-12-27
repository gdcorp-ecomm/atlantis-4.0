using System.ComponentModel.DataAnnotations;

namespace Atlantis.Framework.CDS.Entities.Shared
{
  public class ProductOption : ElementBase
  {
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int Duration { get; set; }

    [Range(0, 100)]
    public int DiscountPercent { get; set; }
  }
}
