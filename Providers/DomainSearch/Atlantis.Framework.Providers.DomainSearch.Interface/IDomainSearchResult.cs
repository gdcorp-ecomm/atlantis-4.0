using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.DomainSearch.Interface;

namespace Atlantis.Framework.Providers.DomainSearch.Interface
{
  public interface IDomainSearchResult
  {
    List<IFindResponseDomain> AffixDomains { get; } 
    List<IFindResponseDomain> AllDomains { get; }
    List<IFindResponseDomain> AuctionDomains { get; }
    List<IFindResponseDomain> CountryCodeTopLevelDomains { get; } 
    List<IFindResponseDomain> PremiumDomains { get; } 
    List<IFindResponseDomain> PrivateDomains { get; }
  }
}
