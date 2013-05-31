using System;
using System.Collections.Generic;

namespace Atlantis.Framework.RegVendorDomainSearch.Interface
{
  public enum RegVendorDomainSearchVendor : int
  {
    None = 0,
    FabulousDomains = 1,
    BuyDomains = 2,
    DomainsBot = 4,
    Auctions = 6,
    PrivateSale = 11
  }

  [Obsolete("This class is obsolete.")]
  public class RegVendorDomainSearchVendorList : List<RegVendorDomainSearchVendor>
  {
  }
}
