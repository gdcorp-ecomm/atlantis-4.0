using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.RegVendorDomainSearch.Interface
{
  public enum RegVendorDomainSearchVendor : int
  {
    None = 0,
    FabulousDomains = 1,
    BuyDomains = 2,
    DomainsBot = 4,
    Auctions = 6    
  }

  [Obsolete("This class is obsolete.")]
  public class RegVendorDomainSearchVendorList : List<RegVendorDomainSearchVendor>
  {
  }
}
