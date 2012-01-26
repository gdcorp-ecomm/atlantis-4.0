using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.FastballGetOffersDetail.Interface
{
  public class Product
  {
    public string Pfid { get; set; }
    public int Qty { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsDefault { get; set; }
    public List<PackageDetailLite> PackagesList { get; set; }
  }
}
