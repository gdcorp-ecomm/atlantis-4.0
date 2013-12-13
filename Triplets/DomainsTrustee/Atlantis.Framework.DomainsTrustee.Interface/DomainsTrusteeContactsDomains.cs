using System.Collections.Generic;
using Atlantis.Framework.Domains.Interface;

namespace Atlantis.Framework.DomainsTrustee.Interface
{
  public class DomainsTrusteeContactsDomains
  {
    public IList<DomainsTrusteeContact> Contacts { get; set; }
    public IList<Domain> Domains { get; set; }

    public DomainsTrusteeContactsDomains(IList<DomainsTrusteeContact> contacts, IList<Domain> domains)
    {
      Contacts = contacts;
      Domains = domains;
    }
  }
}
