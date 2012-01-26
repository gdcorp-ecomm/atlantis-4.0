using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.FastballGetOffersDetail.Interface
{
  public class ProductGroupAtribVal
  {
    //public string Id { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsDefault { get; set; }
    public string Name { get; set; }
    public List<Product> Products { get; set; }
  }
}
