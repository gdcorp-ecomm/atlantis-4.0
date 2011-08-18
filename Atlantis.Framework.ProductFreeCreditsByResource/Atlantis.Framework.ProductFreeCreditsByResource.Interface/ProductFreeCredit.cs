using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.ProductFreeCreditsByResource.Interface
{
  public class ResourceFreeCredit
  {
    public int FreeProductPackageId { get; set; }
    public int UnifiedProductId { get; set; }
    public int Quantity { get; set; }
  }
}
