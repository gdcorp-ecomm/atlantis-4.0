using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class ProductOption : ElementBase
  {
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int Duration { get; set; }
  }
}

